using System.Threading;

namespace pmcenter
{
    public partial class Methods
    {
        public static ThreadStatus GetThreadStatus(Thread Thread)
        {
            try
            {
                return Thread.IsAlive ? ThreadStatus.Standby : ThreadStatus.Stopped;
            }
            catch
            {
                return ThreadStatus.Unknown;
            }
        }
    }
}