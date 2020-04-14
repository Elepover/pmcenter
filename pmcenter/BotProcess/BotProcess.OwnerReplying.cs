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
            var Link = Methods.GetLinkByOwnerMsgID(update.Message.ReplyToMessage.MessageId);
            if (Link != null && !Link.IsFromOwner)
            {
                Log($"Selected message is forwarded anonymously from {Link.TGUser.Id}, fixing user information from database.", "BOT");
                update.Message.ReplyToMessage.ForwardFrom = Link.TGUser;
            }
            if (update.Message.ReplyToMessage.ForwardFrom == null && update.Message.Text.ToLower() != "/retract")
            {
                // The owner is replying to bot messages. (no forwardfrom)
                _ = await Vars.Bot.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CommandNotReplyingValidMessage,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return;
            }

            if (await commandManager.Execute(Vars.Bot, update).ConfigureAwait(false))
            {
                Vars.CurrentConf.Statistics.TotalCommandsReceived += 1;
                return;
            }

            // Is replying, replying to forwarded message AND not command.
            var Forwarded = await Vars.Bot.ForwardMessageAsync(
                    update.Message.ReplyToMessage.ForwardFrom.Id,
                    update.Message.Chat.Id,
                    update.Message.MessageId,
                    Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
            if (Vars.CurrentConf.EnableMsgLink)
            {
                Log($"Recording message link: user {Forwarded.MessageId} <-> owner {update.Message.MessageId}, user: {update.Message.ReplyToMessage.ForwardFrom.Id}", "BOT");
                Vars.CurrentConf.MessageLinks.Add(
                    new Conf.MessageIDLink()
                    { OwnerSessionMessageID = update.Message.MessageId, UserSessionMessageID = Forwarded.MessageId, TGUser = update.Message.ReplyToMessage.ForwardFrom, IsFromOwner = true }
                );
                // Conf.SaveConf(false, true);
            }
            Vars.CurrentConf.Statistics.TotalForwardedFromOwner += 1;
            // Process locale.
            if (Vars.CurrentConf.EnableRepliedConfirmation)
            {
                var ReplyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                ReplyToMessage = ReplyToMessage.Replace("$1", $"[{update.Message.ReplyToMessage.ForwardFrom.FirstName} (@{update.Message.ReplyToMessage.ForwardFrom.Username})](tg://user?id={update.Message.ReplyToMessage.ForwardFrom.Id})");
                _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id, ReplyToMessage, ParseMode.Markdown, false, false, update.Message.MessageId).ConfigureAwait(false);
            }
            Log($"Successfully passed owner's reply to {update.Message.ReplyToMessage.ForwardFrom.FirstName} (@{update.Message.ReplyToMessage.ForwardFrom.Username} / {update.Message.ReplyToMessage.ForwardFrom.Id})", "BOT");
        }
    }
}