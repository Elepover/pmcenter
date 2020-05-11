using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter.CommandLines
{
    internal class NonServiceModeCmdLine : ICommandLine
    {
        public string Prefix => "noservice";
        public bool ExitAfterExecution => false;
#pragma warning disable CS1998
        public async Task<bool> Process()
#pragma warning restore CS1998
        {
            Vars.ServiceMode = false;
            Log("Service mode disabled.");
            return true;
        }
    }
}
