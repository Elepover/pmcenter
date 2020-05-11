using System;
using System.Diagnostics;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static Version GetNetCoreVersion()
        {
            var newVersionInDotnet3 = Environment.Version;
            if (newVersionInDotnet3.Major == 3) return newVersionInDotnet3;
            try
            {
                var searchPattern = "Microsoft.NETCore.App ";
                var proc = new Process();
                proc.StartInfo.FileName = "dotnet";
                proc.StartInfo.Arguments = "--list-runtimes";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                var output = proc.StandardOutput.ReadToEnd();
                // search pattern: Microsoft.NETCore.App 2.1.7 [/home/user/dotnet/shared/Microsoft.NETCore.App]
                int index = output.IndexOf(searchPattern);
                Version version;
                while (true)
                {
                    int indexOfStartingSpace = index + searchPattern.Length - 1;
                    int indexOfEndingSpace = output.IndexOf(" ", index + searchPattern.Length);
                    var versionString = output.Substring(indexOfStartingSpace + 1, indexOfEndingSpace - indexOfStartingSpace - 1);
                    version = new Version(versionString);
                    if (version.Major == 3 && version.Minor == 1) return version;
                    index = output.IndexOf(searchPattern, indexOfEndingSpace);
                    if (index == -1) break;
                }
                return version;
            }
            catch (Exception ex)
            {
                Log($"pmcenter is unable to detect your .NET Core installation: {ex.Message}, you need to have .NET Core 3.1 (runtime) installed in order to run pmcenter v2 or up.", LogLevel.Warning);
                return new Version("0.0.0.0");
            }
        }
    }
}
