/*
// Program.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main entry to pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
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
            Console.WriteLine(Vars.ASCII);
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
                if (Environment.CommandLine.ToLower().Contains("--setup"))
                {
                    try
                    {
                        await Setup.SetupWizard();
                    }
                    catch (Exception ex)
                    {
                        Log("Setup wizard has exited accidentally: " + ex.ToString() + "\n\nProgram will now exit.", "DELEGATOR", LogLevel.ERROR);
                    }
                    // after the method above, program will exit.
                }
                // Nothing hahah.
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
                    Log("Skipped.");
                }
                Thread.Sleep(500);

                Log("==> Initializing module - BOT");
                Log("Initializing bot instance...");
                Bot = new TelegramBotClient(CurrentConf.APIKey);
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
