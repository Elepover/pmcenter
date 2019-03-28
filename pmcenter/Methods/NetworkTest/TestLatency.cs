using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Methods
    {
        public static async Task<TimeSpan> TestLatency(string Target)
        {
            Stopwatch ReqSW = new Stopwatch();
            try
            {
                HttpWebRequest Req = WebRequest.CreateHttp(Target);
                ReqSW.Start();
                await Req.GetResponseAsync();
                ReqSW.Stop();
                return ReqSW.Elapsed;
            }
            catch (WebException)
            {
                ReqSW.Stop();
                return ReqSW.Elapsed;
            }
            catch (Exception ex)
            {
                Log("Latency test failed: " + ex.Message);
                return new TimeSpan(0);
            }
        }
    }
}