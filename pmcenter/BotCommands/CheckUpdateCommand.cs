using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter.Commands
{
    internal class CheckUpdateCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chkupdate";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Telegram.Bot.Types.Update update)
        {
            try
            {
                var latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                var CurrentLocalizedIndex = GetUpdateInfoIndexByLocale(latest, Vars.CurrentLang.LangCode);
                if (IsNewerVersionAvailable(latest))
                {
                    Vars.UpdatePending = true;
                    Vars.UpdateVersion = new Version(latest.Latest);
                    Vars.UpdateLevel = latest.UpdateLevel;
                    var UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", latest.Latest)
                        .Replace("$2", latest.UpdateCollection[CurrentLocalizedIndex].Details)
                        .Replace("$3", Methods.GetUpdateLevel(latest.UpdateLevel));
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        UpdateString,
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
                            .Replace("$3", latest.UpdateCollection[CurrentLocalizedIndex].Details),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                }
                return true;
            }
            catch (Exception ex)
            {
                var ErrorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.Message);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    ErrorString, ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
