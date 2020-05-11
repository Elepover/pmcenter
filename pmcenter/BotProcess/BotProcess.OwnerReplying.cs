using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task OwnerReplying(Update update)
        {
            // check anonymous forward (5.5.0 new feature compatibility fix)
            var link = Methods.GetLinkByOwnerMsgID(update.Message.ReplyToMessage.MessageId);
            if (link != null && !link.IsFromOwner)
            {
                Log($"Found corresponding message link for message #{update.Message.ReplyToMessage.MessageId}, which was actually forwarded from {link.TGUser.Id}, patching user information from database...", "BOT");
                update.Message.ReplyToMessage.ForwardFrom = link.TGUser;
            }
            if ((update.Message.ReplyToMessage.ForwardFrom == null) && (update.Message.Text.ToLowerInvariant() != "/retract"))
            {
                // The owner is replying to bot messages. (no forwardfrom)
                _ = await Vars.Bot.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CommandNotReplyingValidMessage,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                // The message is forwarded anonymously
                if (!string.IsNullOrEmpty(update.Message.ReplyToMessage.ForwardSenderName) && !Vars.CurrentConf.DisableMessageLinkTip)
                {
                    _ = await Vars.Bot.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_MsgLinkTip,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    Vars.CurrentConf.DisableMessageLinkTip = true;
                }
                return;
            }

            if (await commandManager.Execute(Vars.Bot, update).ConfigureAwait(false))
            {
                Vars.CurrentConf.Statistics.TotalCommandsReceived += 1;
                return;
            }

            // Is replying, replying to forwarded message AND not command.
            var forwarded = await Vars.Bot.ForwardMessageAsync(
                    update.Message.ReplyToMessage.ForwardFrom.Id,
                    update.Message.Chat.Id,
                    update.Message.MessageId,
                    Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
            if (Vars.CurrentConf.EnableMsgLink)
            {
                Log($"Recording message link: user {forwarded.MessageId} <-> owner {update.Message.MessageId}, user: {update.Message.ReplyToMessage.ForwardFrom.Id}", "BOT");
                Vars.CurrentConf.MessageLinks.Add(
                    new Conf.MessageIDLink()
                    { OwnerSessionMessageID = update.Message.MessageId, UserSessionMessageID = forwarded.MessageId, TGUser = update.Message.ReplyToMessage.ForwardFrom, IsFromOwner = true }
                );
                // Conf.SaveConf(false, true);
            }
            Vars.CurrentConf.Statistics.TotalForwardedFromOwner += 1;
            // Process locale.
            if (Vars.CurrentConf.EnableRepliedConfirmation)
            {
                var replyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                replyToMessage = replyToMessage.Replace("$1", $"[{Methods.GetComposedUsername(update.Message.ReplyToMessage.ForwardFrom)}](tg://user?id={update.Message.ReplyToMessage.ForwardFrom.Id})");
                _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id, replyToMessage, ParseMode.Markdown, false, false, update.Message.MessageId).ConfigureAwait(false);
            }
            Log($"Successfully passed owner's reply to {Methods.GetComposedUsername(update.Message.ReplyToMessage.ForwardFrom, true, true)}", "BOT");
        }
    }
}
