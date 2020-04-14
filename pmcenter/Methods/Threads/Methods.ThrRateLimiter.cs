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
                foreach (RateData Data in Vars.RateLimits)
                {
                    if (Data.MessageCount > Vars.CurrentConf.AutoBanThreshold && Vars.CurrentConf.AutoBan)
                    {
                        BanUser(Data.UID);
                        _ = await SaveConf(false, true).ConfigureAwait(false);
                        Log($"Banning user: {Data.UID}", "RATELIMIT");
                    }
                    Data.MessageCount = 0;
                }
                Vars.RateLimiterStatus = ThreadStatus.Standby;
                try { Thread.Sleep(30000); } catch { }
            }
        }
    }
}
