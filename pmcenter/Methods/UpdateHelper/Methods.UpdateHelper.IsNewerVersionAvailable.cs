using System;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public static bool IsNewerVersionAvailable(Update2 CurrentUpdate)
            {
                if (Vars.GitHubReleases)
                {
                    Log("This distribution of pmcenter is released via GitHub releases. Automatic updates are not supported yet.", LogLevel.Warning);
                    return false;
                }
                var currentVersion = Vars.AppVer;
                var currentLatest = new Version(CurrentUpdate.Latest);
                if (currentLatest > currentVersion)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
