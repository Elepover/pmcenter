using System;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task InitConf()
        {
            Log("Checking configurations file's integrity...", "CONF");
            if (!System.IO.File.Exists(Vars.ConfFile))
            { // STEP 1, DETECT EXISTENCE.
                Log("Configurations file not found. Creating...", "CONF", LogLevel.WARN);
                Vars.CurrentConf = new ConfObj();
                _ = await SaveConf(true).ConfigureAwait(false); // Then the app will exit, do nothing.
            }
            else
            { // STEP 2, READ TEST.
                try
                {
                    _ = await ReadConf(false).ConfigureAwait(false); // Read but don't apply.
                }
                catch (Exception ex)
                {
                    Log($"Error! {ex}", "CONF", LogLevel.ERROR);
                    Log("Moving old configurations file to \"pmcenter.json.bak\"...", "CONF", LogLevel.WARN);
                    System.IO.File.Move(Vars.ConfFile, Vars.ConfFile + ".bak");
                    Vars.CurrentConf = new ConfObj();
                    _ = await SaveConf(true).ConfigureAwait(false); // Then the app will exit, do nothing.
                }
            }
            Log("Integrity test finished!", "CONF");
        }
    }
}
