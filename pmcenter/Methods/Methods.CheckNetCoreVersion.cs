using System;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static bool CheckNetCoreVersion(Version version)
        {
            if (version.Major != 3 || version.Minor != 1)
            {
                Log("pmcenter v2 or up wouldn't run on devices without .NET Core 3.1 (runtime) installed.", LogLevel.Warning);
                Log($"pmcenter has detected that the latest version installed on your device is .NET Core runtime {version}.", LogLevel.Warning);
                Log("Consider updating your .NET Core runtime in order to run pmcenter v2 and receive further updates.", LogLevel.Warning);
                return false;
            }
            return true;
        }
    }
}
