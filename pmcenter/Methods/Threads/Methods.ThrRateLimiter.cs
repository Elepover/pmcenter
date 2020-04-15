using System.Threading;
using static pmcenter.Conf;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async void ThrRateLimiter()
        {
            Log("Started!", "RATELIMIT");
            while (!Vars.IsShuttingDown)
            {
                Vars.RateLimiterStatus = ThreadStatus.Working;
                foreach (var data in Vars.RateLimits)
                {
                    if (data.MessageCount > Vars.CurrentConf.AutoBanThreshold && Vars.CurrentConf.AutoBan)
                    {
                        BanUser(data.UID);
                        _ = await SaveConf(false, true).ConfigureAwait(false);
                        Log($"Banning user: {data.UID}", "RATELIMIT");
                    }
                    data.MessageCount = 0;
                }
                Vars.RateLimiterStatus = ThreadStatus.Standby;
                try { Thread.Sleep(30000); } catch { }
            }
        }
    }
}
