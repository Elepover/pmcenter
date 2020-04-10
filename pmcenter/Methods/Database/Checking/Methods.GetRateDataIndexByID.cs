using static pmcenter.Conf;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public static int GetRateDataIndexByID(long UID)
        {
            foreach (RateData Data in Vars.RateLimits)
            {
                if (Data.UID == UID) { return Vars.RateLimits.IndexOf(Data); }
            }
            return -1;
        }
    }
}
