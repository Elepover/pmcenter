/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry to pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Conf;
using static pmcenter.Lang;
using static pmcenter.Methods;
using static pmcenter.Vars;

namespace pmcenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartSW.Start();
            Console.WriteLine(ASCII);
            Log("Main delegator activated!", "DELEGATOR");
            Log("Starting pmcenter, version " + AppVer.ToString() + ".", "DELEGATOR");
            Task MainAsyncTask = MainAsync(args);
            MainAsyncTask.Wait();
            Log("Main worker accidentally exited. Stopping...", "DELEGATOR", LogLevel.ERROR);
            Environment.Exit(1);
        }
        public static async Task MainAsync(string[] args)
        {
            try
            {
                Log("==> Running pre-start operations...");
                await CmdLineProcess.RunCommand(Environment.CommandLine);
                // everything (exits and/or errors) are handled above, please do not process.
                Log("==> Running start operations...");
                Log("==> Initializing module - CONF"); // BY DEFAULT CONF & LANG ARE NULL! PROCEED BEFORE DOING ANYTHING.
                await InitConf();
                await ReadConf();
                await InitLang();
                await ReadLang();
                if (RestartRequired)
                {
                    Log("This may be the first time that you use the pmcenter bot.", "CORE");
                    Log("Configuration guide could be found at https://see.wtf/feEJJ", "CORE");
                    Log("Received restart requirement from settings system. Exiting...", "CORE", LogLevel.ERROR);
                    Log("You may need to check your settings and try again.", "CORE", LogLevel.INFO);
                    Environment.Exit(1);
                }

                Log("==> Initializing module - THREADS");
                Log("Starting RateLimiter...");
                RateLimiter = new Thread(() => ThrRateLimiter());
                RateLimiter.Start();
                Log("Waiting...");
                while (RateLimiter.IsAlive != true)
                {
                    Thread.Sleep(100);
                }
                Log("Starting UpdateChecker");
                if (Vars.CurrentConf.EnableAutoUpdateCheck)
                {
                    UpdateChecker = new Thread(() => ThrUpdateChecker());
                    UpdateChecker.Start();
                    Log("Waiting...");
                    while (UpdateChecker.IsAlive != true)
                    {
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Vars.UpdateCheckerStatus = ThreadStatus.Stopped;
                    Log("Skipped.");
                }
                Thread.Sleep(500);

                Log("==> Initializing module - BOT");
                Log("Initializing bot instance...");
                if (CurrentConf.UseProxy)
                {
                    Log("Activating SOCKS5 proxy...");
                    List<ProxyInfo> ProxyInfoList = new List<ProxyInfo>();
                    foreach (Socks5Proxy Info in CurrentConf.Socks5Proxies)
                    {
                        ProxyInfo ProxyInfo = new ProxyInfo(Info.ServerName,
                                                            Info.ServerPort,
                                                            Info.Username,
                                                            Info.ProxyPass);
                        ProxyInfoList.Add(ProxyInfo);
                    }
                    HttpToSocks5Proxy Proxy = new HttpToSocks5Proxy(ProxyInfoList.ToArray())
                    {
                        ResolveHostnamesLocally = CurrentConf.ResolveHostnamesLocally
                    };
                    Log("SOCKS5 proxy is enabled.");
                    Bot = new TelegramBotClient(CurrentConf.APIKey, Proxy);
                }
                else
                {
                    Bot = new TelegramBotClient(CurrentConf.APIKey);
                }
                await Bot.TestApiAsync();
                Log("Hooking event processors...");
                Bot.OnUpdate += BotProcess.OnUpdate;
                Log("Starting receiving...");
                Bot.StartReceiving(new[] { UpdateType.Message });
                Log("==> Startup complete!");
                Log("==> Running post-start operations...");
                await Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                               Vars.CurrentLang.Message_BotStarted
                                                   .Replace("$1", StartSW.Elapsed.TotalMilliseconds + "ms"),
                                               Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                               false,
                                               false);
                if (Vars.CurrentLang.TargetVersion != Vars.AppVer.ToString())
                {
                    Log("Language version mismatch detected.", "CORE", LogLevel.WARN);
                    await Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                   Vars.CurrentLang.Message_LangVerMismatch
                                                       .Replace("$1", Vars.CurrentLang.TargetVersion)
                                                       .Replace("$2", Vars.AppVer.ToString()),
                                                   Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                                   false,
                                                   false);
                }
                Log("==> All finished!");
                while (true)
                {
                    Thread.Sleep(60000);
                }
            }
            catch (Exception ex)
            {
                Log("Unexpected error during startup: " + ex.ToString(), "CORE", LogLevel.ERROR);
                Environment.Exit(1);
            }
        }
    }
}
