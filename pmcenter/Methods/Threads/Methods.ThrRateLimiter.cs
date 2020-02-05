using System.Threading;
using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static async void ThrRateLimiter()
        {
            Log("Started!", "RATELIMIT");
            while (true)
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
                Thread.Sleep(30000);
            }
        }
    }
}