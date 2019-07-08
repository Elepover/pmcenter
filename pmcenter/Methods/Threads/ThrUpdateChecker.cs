using System;
using System.Threading;
using Telegram.Bot.Types.Enums;
using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static async void ThrUpdateChecker()
        {
            Log("Started!", "UPDATER");
            while (true)
            {
                Vars.UpdateCheckerStatus = ThreadStatus.Working;
                try
                {
                    var Latest = Conf.CheckForUpdates();
                    var DisNotif = Vars.CurrentConf.DisableNotifications;
                    // Identical with BotProcess.cs, L206.
                    if (Conf.IsNewerVersionAvailable(Latest))
                    {
                        Vars.UpdatePending = true;
                        Vars.UpdateVersion = new Version(Latest.Latest);
                        Vars.UpdateLevel = Latest.UpdateLevel;
                        var UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Latest.Details)
                            .Replace("$3", GetUpdateLevel(Latest.UpdateLevel));
                        await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                            UpdateString,
                                                            ParseMode.Markdown,
                                                            false,
                                                            DisNotif);
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
                    Log("Error during update check: " + ex.ToString(), "UPDATER", LogLevel.ERROR);
                }
                Vars.UpdateCheckerStatus = ThreadStatus.Standby;
                Thread.Sleep(60000);
            }
        }
    }
}