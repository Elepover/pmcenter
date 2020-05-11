using System.Threading.Tasks;
using static pmcenter.Methods.Logging;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter.CommandLines
{
    internal class UpdateCmdLine : ICommandLine
    {
        public string Prefix => "update";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log($"Application version: {Vars.AppVer}", "CMD");
            Log("Checking for updates...", "CMD");
            Log("Custom update channels and languages are currently unsupported in command line mode, will use \"master\" channel with English.", "CMD");
            var latest = await CheckForUpdatesAsync().ConfigureAwait(false);
            if (IsNewerVersionAvailable(latest))
            {
                Log($"Newer version found: {latest.Latest}, main changes:\n{latest.UpdateCollection[0].Details}", "CMD");
                Log("Updating...", "CMD");
                await DownloadUpdatesAsync(latest).ConfigureAwait(false);
                await DownloadLangAsync().ConfigureAwait(false);
                Log("Update complete.", "CMD");
            }
            else
            {
                Log($"No newer version found.\nCurrently installed version: {Vars.AppVer}\nThe latest version is: {latest.Latest}", "CMD");
            }
            return true;
        }
    }
}
