namespace pmcenter
{
    public static partial class Methods
    {
        public static int GetRateDataIndexByID(long uid)
        {
            foreach (var data in Vars.RateLimits)
            {
                if (data.UID == uid) { return Vars.RateLimits.IndexOf(data); }
            }
            return -1;
        }
    }
}
