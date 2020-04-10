using System;
using System.Threading.Tasks;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public sealed partial class H2Helper
        {
            public static async Task<string> GetStringAsync(Uri uri)
            {
                using var content = await GetHttpContentAsync(uri).ConfigureAwait(false);
                return await content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}
