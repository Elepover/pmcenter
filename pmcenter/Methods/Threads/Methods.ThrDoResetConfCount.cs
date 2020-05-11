using System.Threading;

namespace pmcenter
{
    public static partial class Methods
    {
        public static void ThrDoResetConfCount()
        {
            Vars.ConfResetTimerStatus = ThreadStatus.Working;
            if (Vars.IsResetConfAvailable) return;
            Vars.IsResetConfAvailable = true;
            Thread.Sleep(30000);
            Vars.IsResetConfAvailable = false;
            Vars.ConfResetTimerStatus = ThreadStatus.Stopped;
        }
    }
}
