using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pmcenter
{
    public static partial class Lang
    {
        public static async Task<bool> ReadLang(bool apply = true)
        { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            var SettingsText = await File.ReadAllTextAsync(Vars.LangFile).ConfigureAwait(false);
            var Temp = JsonConvert.DeserializeObject<Language>(SettingsText);
            if (apply) { Vars.CurrentLang = Temp; }
            return true;
        }
    }
}
