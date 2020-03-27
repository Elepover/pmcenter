using System;
using static pmcenter.Methods;

namespace pmcenter
{
    public partial class Program
    {
        private static void CtrlCHandler(object sender, ConsoleCancelEventArgs e)
        {
            if (Vars.IsCtrlCHandled) return;
            if (Vars.CurrentConf.IgnoreKeyboardInterrupt)
            {
                Log("Keyboard interrupt is ignored. To change this behavior, set \"IgnoreKeyboardInterrupt\" key to \"false\" in pmcenter configurations.");
                e.Cancel = true;
                return;
            }
            Vars.IsCtrlCHandled = true;
            Log("Interrupt! pmcenter is exiting...");
            ExitApp(130);
        }
    }
}