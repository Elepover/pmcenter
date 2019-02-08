using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class InfoCmdLine : ICmdLine
    {
        public string Prefix => "info";
        public bool ExitAfterExecution => true;
        public Task<bool> Process()
        {
            Log("Application information", "CMD");
            Log("CLR version: " + Environment.Version.ToString(), "CMD");
            Log("Framework description: " + RuntimeInformation.FrameworkDescription, "CMD");
            Log("Application version: " + Vars.AppVer.ToString(), "CMD");
            return Task.FromResult(true);
        }
    }
}