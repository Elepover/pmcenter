using System.Threading.Tasks;
using static pmcenter.Methods.UpdateHelper;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class UpdateCmdLine : ICmdLine
    {
        public string Prefix => "update";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log($"Application version: {Vars.AppVer}", "CMD");
            Log("Checking for updates...", "CMD");
            Log("Custom update channels and languages are currently unsupported in command line mode, will use \"master\" channel with English.", "CMD");
            var Latest = await CheckForUpdatesAsync().ConfigureAwait(false);
            if (IsNewerVersionAvailable(Latest))
            {
                Log($"Newer version found: {Latest.Latest}, main changes:\n{Latest.UpdateCollection[0].Details}", "CMD");
                Log("Updating...", "CMD");
                await DownloadUpdatesAsync(Latest).ConfigureAwait(false);
                await DownloadLangAsync().ConfigureAwait(false);
                Log("Update complete.", "CMD");
            }
            else
            {
                Log($"No newer version found.\nCurrently installed version: {Vars.AppVer}\nThe latest version is: {Latest.Latest}", "CMD");
            }
            return true;
        }
    }
}
