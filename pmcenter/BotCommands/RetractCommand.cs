using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class RetractCommand : ICommand
    {
        public bool OwnerOnly => false;

        public string Prefix => "retract";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (update.Message.ReplyToMessage == null || update.Message.ReplyToMessage.ForwardFrom != null)
            {
                return false;
            }
            var SelectedMsgID = update.Message.ReplyToMessage.MessageId;
            if (update.Message.From.Id == Vars.CurrentConf.OwnerUID)
            { // owner retracting
                if (Methods.IsOwnerRetractionAvailable(SelectedMsgID))
                {
                    Conf.MessageIDLink Link = Methods.GetLinkByOwnerMsgID(SelectedMsgID);
                    await botClient.DeleteMessageAsync(Link.TGUser.Id, Link.UserSessionMessageID);
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.From.Id,
                        Vars.CurrentLang.Message_FeatureNotAvailable,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                        return true;
                }
            }
            else // user retracting
            {
                if (Methods.IsUserRetractionAvailable(SelectedMsgID))
                {
                    Conf.MessageIDLink Link = Methods.GetLinkByUserMsgID(SelectedMsgID);
                    await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, Link.OwnerSessionMessageID);
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.From.Id,
                        Vars.CurrentLang.Message_FeatureNotAvailable,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    return true;
                }
            }
            await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_Retracted,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
