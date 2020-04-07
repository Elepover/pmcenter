using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods;

namespace pmcenter
{
    public partial class Lang
    {
        public static async Task<bool> SaveLang(bool isInvalid = false)
        { // DO NOT HANDLE ERRORS HERE.
            var Text = JsonConvert.SerializeObject(Vars.CurrentLang, Formatting.Indented);
            var Writer = new StreamWriter(File.Create(Vars.LangFile), System.Text.Encoding.UTF8);
            await Writer.WriteAsync(Text).ConfigureAwait(false);
            await Writer.FlushAsync().ConfigureAwait(false);
            Writer.Close();
            if (isInvalid)
            {
                Log("We've detected an invalid language file and have reset it.", "LANG", LogLevel.WARN);
                Log("Please reconfigure it and try to start pmcenter again.", "LANG", LogLevel.WARN);
                Vars.RestartRequired = true;
            }
            return true;
        }
    }
}