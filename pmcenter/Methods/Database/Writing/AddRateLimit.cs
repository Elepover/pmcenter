using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static void AddRateLimit(long UID)
        {
            if (IsRateDataTracking(UID))
            {
                int DataID = GetRateDataIndexByID(UID);
                Vars.RateLimits[DataID].MessageCount += 1;
            }
            else
            {
                RateData Data = new RateData
                {
                    UID = UID,
                    MessageCount = 1
                };
                Vars.RateLimits.Add(Data);
            }
        }
    }
}