using System;
using System.IO;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task InitConf()
        {
            Log("Checking configurations file's integrity...", "CONF");
            if (!File.Exists(Vars.ConfFile))
            { // STEP 1, DETECT EXISTENCE.
                Log("Configurations file not found. Creating...", "CONF", LogLevel.Warning);
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
                    Log($"Error! {ex}", "CONF", LogLevel.Error);
                    Log("Moving old configurations file to \"pmcenter.json.bak\"...", "CONF", LogLevel.Warning);
                    File.Move(Vars.ConfFile, Vars.ConfFile + ".bak");
                    Vars.CurrentConf = new ConfObj();
                    _ = await SaveConf(true).ConfigureAwait(false); // Then the app will exit, do nothing.
                }
            }
            Log("Integrity test finished!", "CONF");
        }
    }
}
