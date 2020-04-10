using System;
using System.Threading;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public static async void ThrUpdateChecker()
        {
            Log("Started!", "UPDATER");
            while (!Vars.IsShuttingDown)
            {
                Vars.UpdateCheckerStatus = ThreadStatus.Working;
                try
                {
                    var Latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                    var CurrentLocalizedIndex = GetUpdateInfoIndexByLocale(Latest, Vars.CurrentLang.LangCode);
                    var DisNotif = Vars.CurrentConf.DisableNotifications;
                    // Identical with BotProcess.cs, L206.
                    if (IsNewerVersionAvailable(Latest))
                    {
                        Vars.UpdatePending = true;
                        Vars.UpdateVersion = new Version(Latest.Latest);
                        Vars.UpdateLevel = Latest.UpdateLevel;
                        var UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Latest.UpdateCollection[CurrentLocalizedIndex].Details)
                            .Replace("$3", GetUpdateLevel(Latest.UpdateLevel));
                        _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                            UpdateString,
                                                            ParseMode.Markdown,
                                                            false,
                                                            DisNotif).ConfigureAwait(false);
                        return; // Since this thread wouldn't be useful any longer, exit.
                    }
                    else
                    {
                        Vars.UpdatePending = false;
                        // This part has been cut out.
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error during update check: {ex}", "UPDATER", LogLevel.ERROR);
                }
                Vars.UpdateCheckerStatus = ThreadStatus.Standby;
                try { Thread.Sleep(60000); } catch { }
            }
        }
    }
}
