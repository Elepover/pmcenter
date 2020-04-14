using System;
using System.Threading;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async void ThrSyncConf()
        {
            while (!Vars.IsShuttingDown)
            {
                try
                {
                    _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
                    Log("Configurations saved.", "CONFSYNC");
                }
                catch (Exception ex)
                {
                    Log($"Failed to write configurations to local disk: {ex.Message}", "CONFSYNC", LogLevel.ERROR);
                }
                if (Vars.CurrentConf.ConfSyncInterval == 0)
                {
                    Log("ConfSync disabled, stopping...", "CONFSYNC", LogLevel.WARN);
                    return;
                }
                try { Thread.Sleep(Vars.CurrentConf.ConfSyncInterval); } catch { }
            }
        }
    }
}
