using System;
using System.Diagnostics;
using System.Threading;

namespace pmcenter
{
    public partial class Methods
    {
        public static void ExitApp(int code)
        {
            Log("Attempting to exit gracefully...");
            Log("Stopping bot message receiving...");
            Vars.Bot.StopReceiving();
            Log("Waiting for background workers to exit (timeout: 10s)...");
            var sw = new Stopwatch();
            sw.Start();
            Vars.IsShuttingDown = true;
            if (Vars.IsPerformanceTestExecuting)
            {
                while (!Vars.IsPerformanceTestExecuting)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down performance tester.");

            if (Vars.ConfValidator != null && Vars.ConfValidator.IsAlive)
            {
                Vars.ConfValidator.Interrupt();
                while (Vars.ConfValidator.IsAlive)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down reset timer.");

            if (Vars.UpdateChecker != null && Vars.UpdateChecker.IsAlive)
            {
                Vars.UpdateChecker.Interrupt();
                while (Vars.UpdateChecker.IsAlive)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down update checker.");


            if (Vars.RateLimiter != null && Vars.RateLimiter.IsAlive)
            {
                Vars.RateLimiter.Interrupt();
                while (Vars.RateLimiter.IsAlive)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down rate limiter.");

            if (Vars.BannedSweepper != null && Vars.BannedSweepper.IsAlive)
            {
                Vars.BannedSweepper.Interrupt();
                while (Vars.BannedSweepper.IsAlive)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down banned sweeper.");

            if (Vars.SyncConf != null && Vars.SyncConf.IsAlive)
            {
                Vars.SyncConf.Interrupt();
                while (Vars.SyncConf.IsAlive)
                {
                    if (sw.ElapsedMilliseconds >= 10000) Environment.Exit(16);
                    Thread.Sleep(50);
                }
            }
            Log("[OK] Shut down configurations autosaver.");
            sw.Stop();
            Log($"pmcenter has stopped in {Math.Round(sw.Elapsed.TotalSeconds, 2)}s.");
            Environment.Exit(code);
        }
    }
}