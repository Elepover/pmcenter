/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using static pmcenter.Methods;
using static pmcenter.Vars;
using static pmcenter.Conf;
using static pmcenter.Lang;
using Telegram.Bot;

namespace pmcenter {
    public class Program {
        public static void Main(string[] args) {
            StartSW.Start();
            Console.WriteLine(Vars.ASCII);
            Log("Starting pmcenter, version " + AppVer.ToString() + ".", "DELEGATOR");
            Task MainAsyncTask = MainAsync(args);
            MainAsyncTask.Wait();
            Log("Main worker accidentally exited. Stopping...", "DELEGATOR", LogLevel.ERROR);
            Environment.Exit(1);
        }
        public static async Task MainAsync(string[] args) {
            try {
                Log("==> Running pre-start operations...");
                // Nothing hahah.
                Log("==> Running start operations...");
                Log("==> Initializing module - CONF"); // BY DEFAULT CONF & LANG ARE NULL! PROCEED BEFORE DOING ANYTHING.
                InitConf();
                await ReadConf();
                InitLang();
                await ReadLang();
                if (RestartRequired) {
                    Log("Received restart requirement from settings system. Exiting...", "CORE", LogLevel.ERROR);
                    Log("You may need to check your settings and try again.", "CORE", LogLevel.INFO);
                    Environment.Exit(1);
                }

                Log(Vars.CurrentLang.CLI_InitThreads);
                Log(Vars.CurrentLang.CLI_StartRateLimiter);
                RateLimiter = new Thread(() => ThrRateLimiter());
                RateLimiter.Start();
                Thread.Sleep(500);

                Log(Vars.CurrentLang.CLI_InitBotHeader);
                Log(Vars.CurrentLang.CLI_InitBot);
                Bot = new TelegramBotClient(CurrentConf.APIKey);
                await Bot.TestApiAsync();
                Log(Vars.CurrentLang.CLI_HookEvents);
                Bot.OnUpdate += BotProcess.OnUpdate;
                Log(Vars.CurrentLang.CLI_StartReceiving);
                Bot.StartReceiving(new []{Telegram.Bot.Types.Enums.UpdateType.Message});
                Log(Vars.CurrentLang.CLI_StartupComplete);
                Log(Vars.CurrentLang.CLI_PostStartup);
                await Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_BotStarted.Replace("$1", StartSW.Elapsed.TotalMilliseconds + "ms"), Telegram.Bot.Types.Enums.ParseMode.Markdown, false, false);
                Log(Vars.CurrentLang.CLI_Finished);
                while (true) {
                    Thread.Sleep(60000);
                }
            } catch (Exception ex) {
                Log("Error during startup: " + ex.ToString(), "CORE", LogLevel.ERROR);
                Environment.Exit(1);
            }
        }
    }
}
