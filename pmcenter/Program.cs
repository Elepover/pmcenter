/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry to pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using static pmcenter.Conf;
using static pmcenter.Lang;
using static pmcenter.Methods;
using static pmcenter.Vars;

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
                    Log("This may be the first time that you use the pmcenter bot.");
                    Log("Configuration guide could be found at https://see.wtf/feEJJ");
                    Log("Received restart requirement from settings system. Exiting...", "CORE", LogLevel.ERROR);
                    Log("You may need to check your settings and try again.", "CORE", LogLevel.INFO);
                    Environment.Exit(1);
                }

                Log("==> Initializing module - THREADS");
                Log("Starting RateLimiter...");
                RateLimiter = new Thread(() => ThrRateLimiter());
                RateLimiter.Start();
                Thread.Sleep(500);

                Log("==> Initializing module - BOT");
                Log("Initializing bot instance...");
                Bot = new TelegramBotClient(CurrentConf.APIKey);
                await Bot.TestApiAsync();
                Log("Hooking event processors...");
                Bot.OnUpdate += BotProcess.OnUpdate;
                Log("Starting receiving...");
                Bot.StartReceiving(new []{Telegram.Bot.Types.Enums.UpdateType.Message});
                Log("==> Startup complete!");
                Log("==> Running post-start operations...");
                await Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_BotStarted.Replace("$1", StartSW.Elapsed.TotalMilliseconds + "ms"), Telegram.Bot.Types.Enums.ParseMode.Markdown, false, false);
                Log("==> All finished!");
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
