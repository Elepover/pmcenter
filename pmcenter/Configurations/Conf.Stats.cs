namespace pmcenter
{
    public static partial class Conf
    {
        public class Stats
        {
            public Stats()
            {
                TotalMessagesReceived = 0;
                TotalCommandsReceived = 0;
                TotalForwardedToOwner = 0;
                TotalForwardedFromOwner = 0;
            }
            public int TotalMessagesReceived { get; set; }
            public int TotalCommandsReceived { get; set; }
            public int TotalForwardedToOwner { get; set; }
            public int TotalForwardedFromOwner { get; set; }
        }
    }
}
