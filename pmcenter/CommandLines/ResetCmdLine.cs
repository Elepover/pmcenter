using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter.CommandLines
{
    internal class ResetCmdLine : ICommandLine
    {
        public string Prefix => "reset";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log("Resetting configurations...", "CMD");
            Vars.CurrentConf = new Conf.ConfObj();
            _ = await Conf.SaveConf().ConfigureAwait(false);
            Vars.CurrentLang = new Lang.Language();
            _ = await Lang.SaveLang().ConfigureAwait(false);
            Log("Reset complete.");
            return true;
        }
    }
}
