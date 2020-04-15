using System.Threading;

namespace pmcenter
{
    public static partial class Methods
    {
        public static ThreadStatus GetThreadStatus(Thread thread)
        {
            try
            {
                return thread.IsAlive ? ThreadStatus.Standby : ThreadStatus.Stopped;
            }
            catch
            {
                return ThreadStatus.Unknown;
            }
        }
    }
}
