using System;

namespace pmcenter
{
    public partial class Methods
    {
        public static bool CheckNetCoreVersion(Version version)
        {
            if (version.Major != 3 || version.Minor != 1)
            {
                Log("pmcenter v2 or up wouldn't run on devices without .NET Core 3.1 (runtime) installed.", Type: LogLevel.WARN);
                Log($"pmcenter has detected that the latest version installed on your device is .NET Core runtime {version.ToString()}.", Type: LogLevel.WARN);
                Log("Consider updating your .NET Core runtime in order to run pmcenter v2 and receive further updates.", Type: LogLevel.WARN);
                return false;
            }
            return true;
        }
    }
}