using System;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public static bool IsNewerVersionAvailable(Update2 CurrentUpdate)
            {
                var CurrentVersion = Vars.AppVer;
                var CurrentLatest = new Version(CurrentUpdate.Latest);
                if (CurrentLatest > CurrentVersion)
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
