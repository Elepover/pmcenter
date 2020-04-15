/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry to pmcenter.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
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
using static pmcenter.EventHandlers;
using static pmcenter.Lang;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public sealed partial class Program
    {
        public static void Main(string[] args)
        {
            Vars.StartSW.Start();
            Console.WriteLine(Vars.ASCII);
            Log("Main delegator activated!", "DELEGATOR");
            Log($"Starting pmcenter, version {Vars.AppVer}. Channel: \"{Vars.CompileChannel}\"", "DELEGATOR");
            if (Vars.GitHubReleases)
                Log("This image of pmcenter is built for GitHub releases. Will use a different updating mechanism.", "DELEGATOR");
            var mainAsyncTask = MainAsync(args);
            mainAsyncTask.Wait();
            Log("Main worker accidentally exited. Stopping...", "DELEGATOR", LogLevel.Error);
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
                // including:
                // $pmcenter_conf
                // $pmcenter_lang
                try
                {
                    var confByEnvironmentVar = Environment.GetEnvironmentVariable("pmcenter_conf");
                    var langByEnvironmentVar = Environment.GetEnvironmentVariable("pmcenter_lang");
                    if (confByEnvironmentVar != null)
                    {
                        if (File.Exists(confByEnvironmentVar))
                        {
                            Vars.ConfFile = confByEnvironmentVar;
                        }
                        else
                        {
                            Log($"==> The following file was not found: {confByEnvironmentVar}", "CORE", LogLevel.Info);
                        }
                    }
                    if (langByEnvironmentVar != null)
                    {
                        if (File.Exists(langByEnvironmentVar))
                        {
                            Vars.LangFile = langByEnvironmentVar;
                        }
                        else
                        {
                            Log($"==> The following file was not found: {langByEnvironmentVar}", "CORE", LogLevel.Info);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to read environment variables: {ex}", "CORE", LogLevel.Warning);
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
                    Log("Received restart requirement from settings system. Exiting...", "CORE", LogLevel.Error);
                    Log("You may need to check your settings and try again.", "CORE", LogLevel.Info);
                    Environment.Exit(1);
                }

                // check if logs are being omitted
                if (Vars.CurrentConf.IgnoredLogModules.Count > 0)
                {
                    var tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("!!!!!!!!!! SOME LOG ENTRIES ARE HIDDEN ACCORDING TO CURRENT SETTINGS !!!!!!!!!!");
                    Console.WriteLine("To revert this, clear the \"IgnoredLogModules\" field in pmcenter.json.");
                    Console.WriteLine("To disable all log output, turn on \"LowPerformanceMode\".");
                    Console.WriteLine("This warning will appear every time pmcenter starts up with \"IgnoredLogModules\" set.");
                    Console.ForegroundColor = tmp;
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
                    List<ProxyInfo> proxyInfoList = new List<ProxyInfo>();
                    foreach (var proxyInfo in Vars.CurrentConf.Socks5Proxies)
                    {
                        ProxyInfo ProxyInfo = new ProxyInfo(proxyInfo.ServerName,
                                                            proxyInfo.ServerPort,
                                                            proxyInfo.Username,
                                                            proxyInfo.ProxyPass);
                        proxyInfoList.Add(ProxyInfo);
                    }
                    var proxy = new HttpToSocks5Proxy(proxyInfoList.ToArray())
                    {
                        ResolveHostnamesLocally = Vars.CurrentConf.ResolveHostnamesLocally
                    };
                    Log("SOCKS5 proxy is enabled.");
                    Vars.Bot = new TelegramBotClient(Vars.CurrentConf.APIKey, proxy);
                }
                else
                {
                    Vars.Bot = new TelegramBotClient(Vars.CurrentConf.APIKey);
                }
                Log("Validating API Key...");
                _ = await Vars.Bot.TestApiAsync().ConfigureAwait(false);
                Log("Hooking event processors...");
                Vars.Bot.OnUpdate += BotProcess.OnUpdate;
                Log("Starting receiving...");
                Vars.Bot.StartReceiving(new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                });
                Log("==> Startup complete!");
                Log("==> Running post-start operations...");
                try
                {
                    // prompt startup success
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
                    Log($"Failed to send startup message to owner.\nDid you set the \"OwnerID\" key correctly? Otherwise pmcenter could not work properly.\nYou can try to use setup wizard to update/get your OwnerID automatically, just run \"dotnet pmcenter.dll --setup\".\n\nError details: {ex}", "BOT", LogLevel.Warning);
                }
                try
                {
                    // check .net core runtime version
                    var netCoreVersion = GetNetCoreVersion();
                    if (!CheckNetCoreVersion(netCoreVersion) && !Vars.CurrentConf.DisableNetCore3Check)
                    {
                        _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                                Vars.CurrentLang.Message_NetCore31Required
                                                                    .Replace("$1", netCoreVersion.ToString()),
                                                                ParseMode.Markdown,
                                                                false,
                                                                false).ConfigureAwait(false);
                        Vars.CurrentConf.DisableNetCore3Check = true;
                    }
                }
                catch (Exception ex)
                {
                    Log($".NET Core runtime version warning wasn't delivered to the owner: {ex.Message}, did you set the \"OwnerID\" key correctly?", "BOT", LogLevel.Warning);
                }
                // check language mismatch
                if (Vars.CurrentLang.TargetVersion != Vars.AppVer.ToString())
                {
                    Log("Language version mismatch detected.", "CORE", LogLevel.Warning);
                    _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                   Vars.CurrentLang.Message_LangVerMismatch
                                                       .Replace("$1", Vars.CurrentLang.TargetVersion)
                                                       .Replace("$2", Vars.AppVer.ToString()),
                                                   ParseMode.Markdown,
                                                   false,
                                                   false).ConfigureAwait(false);
                }
                Log("==> All finished!");
                if (Vars.ServiceMode)
                    while (true)
                        Thread.Sleep(int.MaxValue);
                else
                    while (true)
                        Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                CheckOpenSSLComp(ex);
                Log($"Unexpected error during startup: {ex}", "CORE", LogLevel.Error);
                Environment.Exit(1);
            }
        }
    }
}
