using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using static pmcenter.Methods.H2Helper;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            /// <summary>
            /// Download update to filesystem and extract.
            /// You must run Conf.CheckForUpdatesAsync() in order to pass the latestUpdate argument.
            /// </summary>
            /// <param name="latestUpdate">The information of the latest update</param>
            /// <param name="localizationIndex">The target localization's index</param>
            /// <returns></returns>
            public static async Task DownloadUpdatesAsync(Update2 latestUpdate, int localizationIndex = 0)
            {
                Log("Starting update download... (pmcenter_update.zip)");
#pragma warning disable CA1062 // Validate arguments of public methods
                Log($"From address: {latestUpdate.UpdateCollection[localizationIndex].UpdateArchiveAddress}");
#pragma warning restore CA1062
                await DownloadFileAsync(
                    new Uri(latestUpdate.UpdateCollection[localizationIndex].UpdateArchiveAddress),
                    Path.Combine(Vars.AppDirectory, "pmcenter_update.zip")
                ).ConfigureAwait(false);
                Log("Download complete. Extracting...");
                using (var zip = ZipFile.OpenRead(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip")))
                {
                    foreach (var entry in zip.Entries)
                    {
                        Log($"Extracting: {Path.Combine(Vars.AppDirectory, entry.FullName)}");
                        entry.ExtractToFile(Path.Combine(Vars.AppDirectory, entry.FullName), true);
                    }
                }
                Log("Cleaning up temporary files...");
                File.Delete(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
            }
        }
    }
}
