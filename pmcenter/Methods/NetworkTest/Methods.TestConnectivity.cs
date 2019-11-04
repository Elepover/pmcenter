using System;
using System.Net;
using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Methods
    {
        public static async Task<bool> TestConnectivity(string Target, bool Ignore45 = false)
        {
            try
            {
                var Req = WebRequest.CreateHttp(Target);
                Req.Timeout = 10000;
                using (_ = await Req.GetResponseAsync().ConfigureAwait(false)) { };
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && Ignore45) { return true; }
                Log("Connectivity to " + Target + " is unavailable: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Log("Connectivity test failed: " + ex.Message);
                return false;
            }
        }
    }
}
