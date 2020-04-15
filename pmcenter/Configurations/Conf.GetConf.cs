using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task<ConfObj> GetConf(string filename)
        {
            var settingsText = await System.IO.File.ReadAllTextAsync(filename).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ConfObj>(settingsText);
        }
    }
}
