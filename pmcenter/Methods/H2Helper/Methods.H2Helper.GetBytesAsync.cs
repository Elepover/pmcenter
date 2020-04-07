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
                using var content = await GetHttpContentAsync(uri).ConfigureAwait(false);
                return await content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }
    }
}
