using System;
using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Methods
    {
        public partial class H2Helper
        {
            public static async Task<byte[]> GetBytesAsync(Uri uri)
            {
                return await (await GetHttpContentAsync(uri).ConfigureAwait(false)).ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }
    }
}
