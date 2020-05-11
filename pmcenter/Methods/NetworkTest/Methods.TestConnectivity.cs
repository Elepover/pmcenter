using System;
using System.Net;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async Task<bool> TestConnectivity(string target, bool ignore45 = false)
        {
            try
            {
                var req = WebRequest.CreateHttp(target);
                req.ProtocolVersion = new Version(2, 0);
                req.Timeout = 10000;
                _ = await req.GetResponseAsync().ConfigureAwait(false);
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ignore45) return true;
                Log($"Connectivity to {target} is unavailable: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Log($"Connectivity test failed: {ex.Message}");
                return false;
            }
        }
    }
}
