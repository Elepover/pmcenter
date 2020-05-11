using System;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class EventHandlers
    {
        public static async void CtrlCHandler(object sender, ConsoleCancelEventArgs e)
        {
            if (e != null) e.Cancel = true;
            Vars.CtrlCCounter++;
            if (Vars.CtrlCCounter > 3)
            {
                Log("More than 3 interrupts has received, terminating...", LogLevel.Warning);
                Environment.Exit(137);
            }
            if (Vars.IsCtrlCHandled) return;
            if (Vars.CurrentConf.IgnoreKeyboardInterrupt)
            {
                Log("Keyboard interrupt is currently being ignored. To change this behavior, set \"IgnoreKeyboardInterrupt\" key to \"false\" in pmcenter configurations.", LogLevel.Warning);
                return;
            }
            Vars.IsCtrlCHandled = true;
            Log("Interrupt! pmcenter is exiting...", LogLevel.Warning);
            Log("If pmcenter was unresponsive, press Ctrl-C 3 more times.");
            await ExitApp(130);
        }
    }
}
