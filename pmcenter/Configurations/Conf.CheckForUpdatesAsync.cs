using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods.H2Helper;

namespace pmcenter
{
    public partial class Conf
    {
        public static async Task<Update2> CheckForUpdatesAsync()
        {
            var response = await GetStringAsync(
                new Uri(
                    Vars.UpdateInfo2URL.Replace("$channel", Vars.CurrentConf == null ? "master" : Vars.CurrentConf.UpdateChannel)
                )
            );
            return JsonConvert.DeserializeObject<Update2>(response);
        }
    }
}