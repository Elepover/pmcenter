using System.Threading;

namespace pmcenter
{
    public partial class Methods
    {
        public static ThreadStatus GetThreadStatus(Thread Thread)
        {
            try
            {
                if (Thread.IsAlive)
                {
                    return ThreadStatus.Standby;
                }
                else
                {
                    return ThreadStatus.Stopped;
                }
            }
            catch
            {
                return ThreadStatus.Unknown;
            }

        }
    }
}