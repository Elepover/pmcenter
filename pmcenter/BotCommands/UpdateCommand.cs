using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.UpdateHelper;

namespace pmcenter.Commands
{
    internal class UpdateCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "update";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Telegram.Bot.Types.Update update)
        {
            try
            {
                var Latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                var CurrentLocalizedIndex = GetUpdateInfoIndexByLocale(Latest, Vars.CurrentLang.LangCode);
                if (IsNewerVersionAvailable(Latest))
                {
                    var UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", Latest.Latest)
                        .Replace("$2", Latest.UpdateCollection[CurrentLocalizedIndex].Details)
                        .Replace("$3", GetUpdateLevel(Latest.UpdateLevel));
                    // \ update available! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        UpdateString, ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    // \ updating! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateProcessing,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    // download compiled package
                    await DownloadUpdatesAsync(Latest, CurrentLocalizedIndex).ConfigureAwait(false);
                    // update languages
                    if (Vars.CurrentConf.AutoLangUpdate) await DownloadLangAsync().ConfigureAwait(false);

                    // \ see you! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateFinalizing,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    Log("Exiting program... (Let the daemon do the restart job)", "BOT");
                    ExitApp(0);
                    return true;
                    // end of difference
                }
                else
                {
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_AlreadyUpToDate
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Vars.AppVer.ToString())
                            .Replace("$3", Latest.UpdateCollection[CurrentLocalizedIndex].Details),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string ErrorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.ToString());
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    ErrorString,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
