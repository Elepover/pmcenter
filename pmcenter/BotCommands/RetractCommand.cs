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
                    var Link = Methods.GetLinkByOwnerMsgID(SelectedMsgID);
                    await botClient.DeleteMessageAsync(Link.TGUser.Id, Link.UserSessionMessageID).ConfigureAwait(false);
                }
                else
                {
                    _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                        Vars.CurrentLang.Message_FeatureNotAvailable,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                        return true;
                }
            }
            else // user retracting
            {
                if (Methods.IsUserRetractionAvailable(SelectedMsgID))
                {
                    var Link = Methods.GetLinkByUserMsgID(SelectedMsgID);
                    await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, Link.OwnerSessionMessageID).ConfigureAwait(false);
                    if (Vars.CurrentConf.EnableActions && Link.OwnerSessionActionMessageID != -1)
                        await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, Link.OwnerSessionActionMessageID).ConfigureAwait(false);
                }
                else
                {
                    _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                        Vars.CurrentLang.Message_FeatureNotAvailable,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    return true;
                }
            }
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_Retracted,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
