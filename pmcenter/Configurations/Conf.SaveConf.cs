using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods;

namespace pmcenter
{
    public partial class Conf
    {
        public static async Task<bool> SaveConf(bool isInvalid = false, bool isAutoSave = false)
        { // DO NOT HANDLE ERRORS HERE.
            string Text = JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(Vars.ConfFile, Text).ConfigureAwait(false);
            if (isAutoSave)
            {
                Log("Autosave complete.", "CONF");
            }
            if (isInvalid)
            {
                Log("We've detected an invalid configurations file and have reset it.", "CONF", LogLevel.WARN);
                Log("Please reconfigure it and try to start pmcenter again.", "CONF", LogLevel.WARN);
                Vars.RestartRequired = true;
            }
            return true;
        }
    }
}
