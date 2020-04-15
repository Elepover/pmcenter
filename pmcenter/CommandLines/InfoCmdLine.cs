using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.CommandLines
{
    internal class InfoCmdLine : ICommandLine
    {
        public string Prefix => "info";
        public bool ExitAfterExecution => true;
        public async Task<bool> Process()
        {
            Log("Gathering application information... This may take a while for there's some network activities.", "CMD");
            var isTelegramApiAccessible = await TestConnectivity("https://api.telegram.org/bot/", true).ConfigureAwait(false);
            var isGitHubAccessible = await TestConnectivity("https://raw.githubusercontent.com/", true).ConfigureAwait(false);
            var isCiAvailable = await TestConnectivity("https://ci.appveyor.com/", true).ConfigureAwait(false);
            Log("Application information", "CMD");
            Log($"CLR version: {Environment.Version}", "CMD");
            Log($"Framework description: {RuntimeInformation.FrameworkDescription}", "CMD");
            Log($"Application version: {Vars.AppVer}", "CMD");
            Log($"Configurations filename: {Vars.ConfFile}", "CMD");
            Log($"Language filename: {Vars.LangFile}", "CMD");
            Log($"Is Telegram API accessible? {(isTelegramApiAccessible ? "yes" : "no")}", "CMD");
            Log($"Is GitHub accessible? {(isGitHubAccessible ? "yes" : "no")}", "CMD");
            Log($"Is CI accessible? {(isCiAvailable ? "yes" : "no")}", "CMD");

            return true;
        }
    }
}
