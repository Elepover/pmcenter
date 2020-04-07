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
        public async Task<bool> Process()
        {
            Log("Gathering application information... This may take a while for there's some network activities.", "CMD");
            var IsTelegramAPIAccessible = await TestConnectivity("https://api.telegram.org/bot/", true).ConfigureAwait(false);
            var IsGitHubAccessible = await TestConnectivity("https://raw.githubusercontent.com/", true).ConfigureAwait(false);
            var IsCIAvailable = await TestConnectivity("https://ci.appveyor.com/", true).ConfigureAwait(false);
            Log("Application information", "CMD");
            Log($"CLR version: {Environment.Version.ToString()}", "CMD");
            Log($"Framework description: {RuntimeInformation.FrameworkDescription}", "CMD");
            Log($"Application version: {Vars.AppVer.ToString()}", "CMD");
            Log($"Configurations filename: {Vars.ConfFile}", "CMD");
            Log($"Language filename: {Vars.LangFile}", "CMD");
            Log($"Is Telegram API accessible? {(IsTelegramAPIAccessible ? "yes" : "no")}", "CMD");
            Log($"Is GitHub accessible? {(IsGitHubAccessible ? "yes" : "no")}", "CMD");
            Log($"Is CI accessible? {(IsCIAvailable ? "yes" : "no")}", "CMD");

            return true;
        }
    }
}
