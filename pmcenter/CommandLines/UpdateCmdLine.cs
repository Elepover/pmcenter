using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class UpdateCmdLine : ICmdLine
    {
        public string Prefix => "update";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log("Application version: " + Vars.AppVer.ToString(), "CMD");
            Log("Checking for updates...", "CMD");
            var Latest = Conf.CheckForUpdates();
            if (Conf.IsNewerVersionAvailable(Latest))
            {
                Log("Newer version found: " + Latest.Latest + ", main changes:\n" + Latest.UpdateCollection[0].Details, "CMD");
                Log("Updating...", "CMD");
                Log("Starting update download... (pmcenter_update.zip)", "CMD");
                using (var Downloader = new WebClient())
                {
                    await Downloader.DownloadFileTaskAsync(
                        new Uri(Vars.UpdateArchiveURL),
                        Path.Combine(Vars.AppDirectory, "pmcenter_update.zip")).ConfigureAwait(false);
                    Log("Download complete. Extracting...", "CMD");
                    using (ZipArchive Zip = ZipFile.OpenRead(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip")))
                    {
                        foreach (ZipArchiveEntry Entry in Zip.Entries)
                        {
                            Log("Extracting: " + Path.Combine(Vars.AppDirectory, Entry.FullName), "CMD");
                            Entry.ExtractToFile(Path.Combine(Vars.AppDirectory, Entry.FullName), true);
                        }
                    }
                    Log("Starting language file update...", "CMD");
                    await Downloader.DownloadFileTaskAsync(
                        new Uri(Vars.CurrentConf.LangURL),
                        Path.Combine(Vars.AppDirectory, "pmcenter_locale.json")
                        ).ConfigureAwait(false);
                }
                Log("Cleaning up temporary files...", "CMD");
                File.Delete(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
                Log("Update complete.", "CMD");
            }
            else
            {
                Log("No newer version found.\nCurrently installed version: " + Vars.AppVer.ToString() + "\nThe latest version is: " + Latest.Latest, "CMD");
            }
            return true;
        }
    }
}