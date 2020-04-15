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
            var selectedMsgId = update.Message.ReplyToMessage.MessageId;
            if (update.Message.From.Id == Vars.CurrentConf.OwnerUID)
            { // owner retracting
                if (Methods.IsOwnerRetractionAvailable(selectedMsgId))
                {
                    var link = Methods.GetLinkByOwnerMsgID(selectedMsgId);
                    await botClient.DeleteMessageAsync(link.TGUser.Id, link.UserSessionMessageID).ConfigureAwait(false);
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
                if (Methods.IsUserRetractionAvailable(selectedMsgId))
                {
                    var link = Methods.GetLinkByUserMsgID(selectedMsgId);
                    await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, link.OwnerSessionMessageID).ConfigureAwait(false);
                    if (Vars.CurrentConf.EnableActions && link.OwnerSessionActionMessageID != -1)
                        await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, link.OwnerSessionActionMessageID).ConfigureAwait(false);
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
