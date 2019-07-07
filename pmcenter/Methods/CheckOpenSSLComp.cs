using System;
using System.Net.Http;
using System.Security.Authentication;

namespace pmcenter
{
    public partial class Methods
    {
        public static void CheckOpenSSLComp(Exception ex)
        {
            try
            {
                if (ex.GetType() == typeof(HttpRequestException))
                {
                    if (ex.InnerException?.GetType() == typeof(AuthenticationException))
                    {
                        if (ex.InnerException?.InnerException?.GetType() == typeof(TypeInitializationException))
                        {
                            Log("\n\n[!] pmcenter has detected a known problem.\n\nIt appears that your version of .NET Core runtime is incompatible with OpenSSL 1.1+ and therefore pmcenter will not be able to make TLS connections to necessary servers.\n\nTo learn more, open:\nhttps://github.com/Elepover/pmcenter#openssl-compatibility-problem\n\npmcenter will now quit.", "CORE", LogLevel.ERROR);
                            Environment.Exit(1);
                        }
                    }
                }
            }
            catch {}
        }
    }
}