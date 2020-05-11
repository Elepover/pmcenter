using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class RetractCommand : IBotCommand
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
            Log($"Retracting message for user {update.Message.From.Id}", "BOT");
            if (update.Message.From.Id == Vars.CurrentConf.OwnerUID)
            { // owner retracting
                if (IsOwnerRetractionAvailable(selectedMsgId))
                {
                    var link = GetLinkByOwnerMsgID(selectedMsgId);
                    await botClient.DeleteMessageAsync(link.TGUser.Id, link.UserSessionMessageID).ConfigureAwait(false);
                    Log($"Successfully retracted message from {GetComposedUsername(link.TGUser, true, true)}.", "BOT");
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
                if (IsUserRetractionAvailable(selectedMsgId))
                {
                    var link = GetLinkByUserMsgID(selectedMsgId);
                    await botClient.DeleteMessageAsync(Vars.CurrentConf.OwnerUID, link.OwnerSessionMessageID).ConfigureAwait(false);
                    Log($"Successfully retracted message from owner.", "BOT");
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
