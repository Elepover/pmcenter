using System;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public static bool IsNewerVersionAvailable(Update2 CurrentUpdate)
            {
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
