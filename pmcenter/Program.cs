using System;
using System.Threading;
using System.Threading.Tasks;
using static pmcenter.Methods;
using static pmcenter.Vars;
using static pmcenter.Conf;
using Telegram.Bot;

namespace pmcenter {
    public class Program {
        public static void Main(string[] args) {
            Log("Starting PMCenter, version " + AppVer.ToString() + ".", "DELEGATOR");
            StartSW.Start();
            Task MainAsyncTask = MainAsync(args);
            MainAsyncTask.Wait();
            Log("Main worker accidentally exited. Stopping...", "CORE", LogLevel.ERROR);
            Environment.Exit(1);
        }
        public static async Task MainAsync(string[] args) {
            try {
                Log("==> Initializing module - CONF");
                InitConf();
                await ReadConf();
                
                Log("==> Initializing module - THREADS");
                Log("Starting BannedSweeper...");
                BannedSweepper = new Thread(() => ThrBannedSweeper());
                BannedSweepper.Start();
                while (BannedSweepper.IsAlive != true) {
                    Thread.Sleep(500);
                }
                Log("Starting RateLimiter...");
                RateLimiter = new Thread(() => ThrRateLimiter());
                RateLimiter.Start();
                while (RateLimiter.IsAlive != true) {
                    Thread.Sleep(500);
                }

                Log("==> Initializing module - BOT");
                Log("Initializing bot instance...");
                Bot = new TelegramBotClient(CurrentConf.APIKey);
                await Bot.TestApiAsync();
                Log("Hooking event processors...");
                Bot.OnUpdate += BotProcess.OnUpdate;
                Log("Starting receiving...");
                Bot.StartReceiving(new []{Telegram.Bot.Types.Enums.UpdateType.Message});
                Log("==> Startup complete! (" + StartSW.Elapsed.TotalMilliseconds + "ms)");
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
