using System;
using System.Net;
using Newtonsoft.Json;

namespace pmcenter
{
    public partial class Conf
    {
        public static Update2 CheckForUpdates()
        {
            using (var Downloader = new WebClient())
            {
                var Response = Downloader.DownloadString(new Uri(Vars.UpdateInfo2URL));
                return JsonConvert.DeserializeObject<Update2>(Response);
            }
        }
    }
}