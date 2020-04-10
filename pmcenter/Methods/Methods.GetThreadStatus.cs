using System.Threading;

namespace pmcenter
{
    public sealed partial class Methods
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
