using System;
using System.IO;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class H2Helper
        {
            public static async Task DownloadFileAsync(Uri uri, string filename)
            {
                var bytes = await GetBytesAsync(uri).ConfigureAwait(false);
                var fileStream = File.Create(filename);
                await fileStream.WriteAsync(bytes).ConfigureAwait(false);
                await fileStream.FlushAsync();
                fileStream.Close();
            }
        }
    }
}
