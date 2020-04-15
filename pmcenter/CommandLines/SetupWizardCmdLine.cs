using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter.CommandLines
{
    internal class SetupWizardCmdLine : ICommandLine
    {
        public string Prefix => "setup";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log("Launching setup wizard...", "CMD");
            // use the global error handler.
            await Setup.SetupWizard().ConfigureAwait(false);
            return true;
        }
    }
}
