using System;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public sealed partial class EventHandlers
    {
        public static void CtrlCHandler(object sender, ConsoleCancelEventArgs e)
        {
            Vars.CtrlCCounter++;
            if (Vars.CtrlCCounter > 3)
            {
                Log("More than 3 interrupts has received, terminating...", LogLevel.WARN);
                Environment.Exit(137);
            }
            if (Vars.IsCtrlCHandled) return;
            if (Vars.CurrentConf.IgnoreKeyboardInterrupt)
            {
                Log("Keyboard interrupt is currently being ignored. To change this behavior, set \"IgnoreKeyboardInterrupt\" key to \"false\" in pmcenter configurations.", LogLevel.WARN);
                if (e != null) e.Cancel = true;
                return;
            }
            Vars.IsCtrlCHandled = true;
            Log("Interrupt! pmcenter is exiting...", LogLevel.WARN);
            Log("If pmcenter was unresponsive, press Ctrl-C 3 more times.");
            ExitApp(130);
        }
    }
}
