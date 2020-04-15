using System;
using System.Threading;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async void ThrUpdateChecker()
        {
            Log("Started!", "UPDATER");
            while (!Vars.IsShuttingDown)
            {
                Vars.UpdateCheckerStatus = ThreadStatus.Working;
                try
                {
                    var latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                    var currentLocalizedIndex = GetUpdateInfoIndexByLocale(latest, Vars.CurrentLang.LangCode);
                    var isNotificationDisabled = Vars.CurrentConf.DisableNotifications;
                    // Identical with BotProcess.cs, L206.
                    if (IsNewerVersionAvailable(latest))
                    {
                        Vars.UpdatePending = true;
                        Vars.UpdateVersion = new Version(latest.Latest);
                        Vars.UpdateLevel = latest.UpdateLevel;
                        var updateString = Vars.CurrentLang.Message_UpdateAvailable
                            .Replace("$1", latest.Latest)
                            .Replace("$2", latest.UpdateCollection[currentLocalizedIndex].Details)
                            .Replace("$3", GetUpdateLevel(latest.UpdateLevel));
                        _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                            updateString,
                                                            ParseMode.Markdown,
                                                            false,
                                                            isNotificationDisabled).ConfigureAwait(false);
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
                    Log($"Error during update check: {ex}", "UPDATER", LogLevel.Error);
                }
                Vars.UpdateCheckerStatus = ThreadStatus.Standby;
                try { Thread.Sleep(60000); } catch { }
            }
        }
    }
}
