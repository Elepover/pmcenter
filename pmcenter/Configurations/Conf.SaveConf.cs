using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task<bool> SaveConf(bool isInvalid = false, bool isAutoSave = false)
        { // DO NOT HANDLE ERRORS HERE.
            string text = JsonConvert.SerializeObject(Vars.CurrentConf, Vars.CurrentConf.Minify ? Formatting.None : Formatting.Indented);
            await File.WriteAllTextAsync(Vars.ConfFile, text).ConfigureAwait(false);
            if (isAutoSave)
            {
                Log("Autosave complete.", "CONF");
            }
            if (isInvalid)
            {
                Log("We've detected an invalid configurations file and have reset it.", "CONF", LogLevel.Warning);
                Log("Please reconfigure it and try to start pmcenter again.", "CONF", LogLevel.Warning);
                Vars.RestartRequired = true;
            }
            return true;
        }
    }
}
