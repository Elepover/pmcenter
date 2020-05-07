using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Lang
    {
        public static async Task<bool> ReadLang(bool apply = true)
        { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            var langText = await File.ReadAllTextAsync(Vars.LangFile).ConfigureAwait(false);
            var temp = JsonConvert.DeserializeObject<Language>(langText);
            if (apply) { Vars.CurrentLang = temp; }
            return true;
        }
    }
}
