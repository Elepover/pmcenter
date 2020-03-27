/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry to pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Conf;
using static pmcenter.Lang;
using static pmcenter.Methods;

namespace pmcenter
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Vars.StartSW.Start();
            Console.WriteLine(Vars.ASCII);
            Log("Main delegator activated!", "DELEGATOR");
            Log($"Starting pmcenter, version {Vars.AppVer.ToString()}. Channel: \"{Vars.CompileChannel}\"", "DELEGATOR");
            var MainAsyncTask = MainAsync(args);
            MainAsyncTask.Wait();
            Log("Main worker accidentally exited. Stopping...", "DELEGATOR", LogLevel.ERROR);
            Environment.Exit(1);
        }
        public static async Task MainAsync(string[] args)
        {
            try
            {
                Log("==> Running pre-start operations...");
                // hook global errors (final failsafe)
                AppDomain.CurrentDomain.UnhandledException += GlobalErrorHandler;
                Log("Global error handler is armed and ready!");
                // hook ctrl-c events
                Console.CancelKeyPress += CtrlCHandler;
                // process commandlines
                await CmdLineProcess.RunCommand(Environment.CommandLine).ConfigureAwait(false);
                // everything (exits and/or errors) are handled above, please do not process.
                // detect environment variables
                // including: $pmcenter_conf, $pmcenter_lang
                try
                {
                    var ConfByEnviVar = Environment.GetEnvironmentVariable("pmcenter_conf");
                    var LangByEnviVar = Environment.GetEnvironmentVariable("pmcenter_lang");
                    if (ConfByEnviVar != null)
                    {
                        if (File.Exists(ConfByEnviVar))
                        {
                            Vars.ConfFile = ConfByEnviVar;
                        }
                        else
                        {
                            Log($"==> The following file was not found: {ConfByEnviVar}", "CORE", LogLevel.INFO);
                        }
                    }
                    if (LangByEnviVar != null)
                    {
                        if (File.Exists(LangByEnviVar))
                        {
                            Vars.LangFile = LangByEnviVar;
                        }
                        else
                        {
                            Log($"==> The following file was not found: {LangByEnviVar}", "CORE", LogLevel.INFO);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to read environment variables: {ex.ToString()}", "CORE", LogLevel.WARN);
                }
                
                Log($"==> Using configurations file: {Vars.ConfFile}");
                Log($"==> Using language file: {Vars.LangFile}");
                
                Log("==> Running start operations...");
                Log("==> Initializing module - CONF"); // BY DEFAULT CONF & LANG ARE NULL! PROCEED BEFORE DOING ANYTHING. <- well anyway we have default values
                await InitConf().ConfigureAwait(false);
                _ = await ReadConf().ConfigureAwait(false);
                await InitLang().ConfigureAwait(false);
                _ = await ReadLang().ConfigureAwait(false);

                if (Vars.CurrentLang == null)
                {
                    throw new InvalidOperationException("Language file is empty.");
                }

                if (Vars.RestartRequired)
                {
                    Log("This may be the first time that you use the pmcenter bot.", "CORE");
                    Log("Configuration guide could be found at https://see.wtf/feEJJ", "CORE");
                    Log("Received restart requirement from settings system. Exiting...", "CORE", LogLevel.ERROR);
                    Log("You may need to check your settings and try again.", "CORE", LogLevel.INFO);
                    Environment.Exit(1);
                }

                Log("==> Initializing module - THREADS");
                Log("Starting RateLimiter...");
                Vars.RateLimiter = new Thread(() => ThrRateLimiter());
                Vars.RateLimiter.Start();
                Log("Waiting...");
                while (!Vars.RateLimiter.IsAlive)
                {
                    Thread.Sleep(100);
                }
                Log("Starting UpdateChecker...");
                if (Vars.CurrentConf.EnableAutoUpdateCheck)
                {
                    Vars.UpdateChecker = new Thread(() => ThrUpdateChecker());
                    Vars.UpdateChecker.Start();
                    Log("Waiting...");
                    while (!Vars.UpdateChecker.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Vars.UpdateCheckerStatus = ThreadStatus.Stopped;
                    Log("Skipped.");
                }
                Log("Starting SyncConf...");
                Vars.SyncConf = new Thread(() => ThrSyncConf());
                Vars.SyncConf.Start();
                Log("Waiting...");
                while (!Vars.SyncConf.IsAlive)
                {
                    Thread.Sleep(100);
                }

                Log("==> Initializing module - BOT");
                Log("Initializing bot instance...");
                if (Vars.CurrentConf.UseProxy)
                {
                    Log("Activating SOCKS5 proxy...");
                    List<ProxyInfo> ProxyInfoList = new List<ProxyInfo>();
                    foreach (Socks5Proxy Info in Vars.CurrentConf.Socks5Proxies)
                    {
                        ProxyInfo ProxyInfo = new ProxyInfo(Info.ServerName,
                                                            Info.ServerPort,
                                                            Info.Username,
                                                            Info.ProxyPass);
                        ProxyInfoList.Add(ProxyInfo);
                    }
                    HttpToSocks5Proxy Proxy = new HttpToSocks5Proxy(ProxyInfoList.ToArray())
                    {
                        ResolveHostnamesLocally = Vars.CurrentConf.ResolveHostnamesLocally
                    };
                    Log("SOCKS5 proxy is enabled.");
                    Vars.Bot = new TelegramBotClient(Vars.CurrentConf.APIKey, Proxy);
                }
                else
                {
                    Vars.Bot = new TelegramBotClient(Vars.CurrentConf.APIKey);
                }
                _ = await Vars.Bot.TestApiAsync().ConfigureAwait(false);
                Log("Hooking event processors...");
                Vars.Bot.OnUpdate += BotProcess.OnUpdate;
                Log("Starting receiving...");
                Vars.Bot.StartReceiving(new[] { UpdateType.Message });
                Log("==> Startup complete!");
                Log("==> Running post-start operations...");
                try
                {
                    if (!Vars.CurrentConf.NoStartupMessage)
                    {
                        _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                            Vars.CurrentLang.Message_BotStarted
                                                                .Replace("$1", Math.Round(Vars.StartSW.Elapsed.TotalSeconds, 2) + "s"),
                                                            ParseMode.Markdown,
                                                            false,
                                                            false).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to send startup message to owner.\nDid you set the \"OwnerID\" key correctly? Otherwise pmcenter could not work properly.\nYou can try to use setup wizard to update/get your OwnerID automatically, just run \"dotnet pmcenter.dll --setup\".\n\nError details: {ex.ToString()}", "BOT", LogLevel.WARN);
                }
                try
                {
                    var netCoreVersion = GetNetCoreVersion();
                    if (!CheckNetCoreVersion(netCoreVersion))
                    {
                        _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                                Vars.CurrentLang.Message_NetCore31Required
                                                                    .Replace("$1", netCoreVersion.ToString()),
                                                                ParseMode.Markdown,
                                                                false,
                                                                false).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Log($".NET Core runtime version warning wasn't delivered to the owner: {ex.Message}, did you set the \"OwnerID\" key correctly?", "BOT", LogLevel.WARN);
                }
                if (Vars.CurrentLang.TargetVersion != Vars.AppVer.ToString())
                {
                    Log("Language version mismatch detected.", "CORE", LogLevel.WARN);
                    _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                   Vars.CurrentLang.Message_LangVerMismatch
                                                       .Replace("$1", Vars.CurrentLang.TargetVersion)
                                                       .Replace("$2", Vars.AppVer.ToString()),
                                                   ParseMode.Markdown,
                                                   false,
                                                   false).ConfigureAwait(false);
                }
                Log("==> All finished!");
                while (true)
                {
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                CheckOpenSSLComp(ex);
                Log($"Unexpected error during startup: {ex.ToString()}", "CORE", LogLevel.ERROR);
                Environment.Exit(1);
            }
        }
    }
}
