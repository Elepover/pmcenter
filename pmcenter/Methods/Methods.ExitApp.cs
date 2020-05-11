using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async Task ExitApp(int code)
        {
            Log("Attempting to exit gracefully...");
            Log("Stopping bot message receiving...");
            Vars.Bot.StopReceiving();
            Log("Waiting for background workers to exit (timeout: 10s)...");
            var sw = new Stopwatch();
            sw.Start();

            Vars.IsShuttingDown = true;
            Log("Saving configurations...");
            await Conf.SaveConf();
            while (Vars.IsPerformanceTestExecuting)
            {
                if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                Thread.Sleep(50);
            }
            Log("[OK] Shut down performance tester.");

            Thread[] threads =
            {
                Vars.ConfValidator,
                Vars.UpdateChecker,
                Vars.RateLimiter,
                Vars.BannedSweeper,
                Vars.SyncConf
            };

            string[] threadNames =
            {
                "reset timer",
                "update checker",
                "rate limiter",
                "banned sweeper",
                "configurations autosaver"
            };

            for (int i = 0; i < threads.Length; i++)
            {
                var thread = threads[i];
                if (thread != null && thread.IsAlive)
                {
                    thread.Interrupt();
                    while (thread.IsAlive)
                    {
                        if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                        Thread.Sleep(50);
                    }
                }
                Log($"[OK] Shut down {threadNames[i]}.");
            }

            sw.Stop();
            Log($"pmcenter has stopped in {Math.Round(sw.Elapsed.TotalSeconds, 2)}s.");
            Environment.Exit(code);
        }
    }
}
