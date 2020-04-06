using System;
using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Methods
    {
        public partial class H2Helper
        {
            public static async Task<string> GetStringAsync(Uri uri)
            {
                return await (await GetHttpContentAsync(uri).ConfigureAwait(false)).ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}
