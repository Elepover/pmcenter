using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class CheckUpdateCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chkupdate";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Conf.Update Latest = Conf.CheckForUpdates();
                if (Conf.IsNewerVersionAvailable(Latest))
                {
                    Vars.UpdatePending = true;
                    Vars.UpdateVersion = new Version(Latest.Latest);
                    Vars.UpdateLevel = Latest.UpdateLevel;
                    string UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", Latest.Latest)
                        .Replace("$2", Latest.Details);
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        UpdateString,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);

                }
                else
                {
                    Vars.UpdatePending = false;
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_AlreadyUpToDate
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Vars.AppVer.ToString())
                            .Replace("$3", Latest.Details),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                }
                return true;
            }
            catch (Exception ex)
            {
                string ErrorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.Message);
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    ErrorString, ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                return true;
            }
        }
    }
}
