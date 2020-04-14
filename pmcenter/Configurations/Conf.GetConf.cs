using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task<ConfObj> GetConf(string Filename)
        {
            var SettingsText = await System.IO.File.ReadAllTextAsync(Filename).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ConfObj>(SettingsText);
        }
    }
}
