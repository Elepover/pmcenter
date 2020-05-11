using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter.Commands
{
    internal class CheckUpdateCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chkupdate";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Telegram.Bot.Types.Update update)
        {
            try
            {
                var latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                var currentLocalizedIndex = GetUpdateInfoIndexByLocale(latest, Vars.CurrentLang.LangCode);
                if (IsNewerVersionAvailable(latest))
                {
                    Vars.UpdatePending = true;
                    Vars.UpdateVersion = new Version(latest.Latest);
                    Vars.UpdateLevel = latest.UpdateLevel;
                    var updateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", latest.Latest)
                        .Replace("$2", latest.UpdateCollection[currentLocalizedIndex].Details)
                        .Replace("$3", Methods.GetUpdateLevel(latest.UpdateLevel));
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        updateString,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                }
                else
                {
                    Vars.UpdatePending = false;
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_AlreadyUpToDate
                            .Replace("$1", latest.Latest)
                            .Replace("$2", Vars.AppVer.ToString())
                            .Replace("$3", latest.UpdateCollection[currentLocalizedIndex].Details),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                }
                return true;
            }
            catch (Exception ex)
            {
                var errorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.Message);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    errorString,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
