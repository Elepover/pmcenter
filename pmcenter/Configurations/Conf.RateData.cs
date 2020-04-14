namespace pmcenter
{
    public static partial class Conf
    {
        public class RateData
        {
            public RateData()
            {
                UID = -1;
                MessageCount = 0;
            }
            public long UID { get; set; }
            public int MessageCount { get; set; }
        }
    }
}
