using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Methods
    {
        public static async Task<TimeSpan> TestLatency(string target)
        {
            var reqSw = new Stopwatch();
            try
            {
                var req = WebRequest.CreateHttp(target);
                reqSw.Start();
                _ = await req.GetResponseAsync().ConfigureAwait(false);
                reqSw.Stop();
                return reqSw.Elapsed;
            }
            catch (WebException)
            {
                reqSw.Stop();
                return reqSw.Elapsed;
            }
            catch (Exception ex)
            {
                Log($"Latency test failed: {ex.Message}");
                reqSw.Reset();
                return new TimeSpan(0);
            }
        }
    }
}
