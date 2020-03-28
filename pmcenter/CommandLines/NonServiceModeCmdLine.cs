using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class NonServiceModeCmdLine : ICmdLine
    {
        public string Prefix => "noservice";
        public bool ExitAfterExecution => false;
        public async Task<bool> Process()
        {
            Vars.ServiceMode = false;
            Log("Service mode disabled.");
            return true;
        }
    }
}