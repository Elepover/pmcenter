using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class SetupWizardCmdLine : ICmdLine
    {
        public string Prefix => "setup";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log("Launching setup wizard...", "CMD");
            // use the global error handler.
            await Setup.SetupWizard();
            return true;
        }
    }
}