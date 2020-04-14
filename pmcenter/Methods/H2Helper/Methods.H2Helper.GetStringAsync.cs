using System;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class H2Helper
        {
            public static async Task<string> GetStringAsync(Uri uri)
            {
                using var content = await GetHttpContentAsync(uri).ConfigureAwait(false);
                return await content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}
