using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task OwnerCommand(Update update)
        {
            // The owner is not even replying.
            // start or help command?
            if (await commandManager.Execute(Vars.Bot, update).ConfigureAwait(false))
            {
                Vars.CurrentConf.Statistics.TotalCommandsReceived += 1;
                return;
            }
            // command mismatch
            if (Vars.CurrentConf.ContChatTarget != -1)
            {
                // Is replying, replying to forwarded message AND not command.
                var Forwarded = await Vars.Bot.ForwardMessageAsync(
                                                                       Vars.CurrentConf.ContChatTarget,
                                                                       update.Message.Chat.Id,
                                                                       update.Message.MessageId,
                                                                       Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
                if (Vars.CurrentConf.EnableMsgLink)
                {
                    Log($"Recording message link: {Forwarded.MessageId} -> {update.Message.MessageId} in {update.Message.From.Id}", "BOT");
                    Vars.CurrentConf.MessageLinks.Add(
                        new Conf.MessageIDLink()
                        { OwnerSessionMessageID = Forwarded.MessageId, UserSessionMessageID = update.Message.MessageId, TGUser = update.Message.From, IsFromOwner = true }
                    );
                    // Conf.SaveConf(false, true);
                }
                // Process locale.
                if (Vars.CurrentConf.EnableRepliedConfirmation)
                {
                    var ReplyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                    ReplyToMessage = ReplyToMessage.Replace("$1", $"[{Vars.CurrentConf.ContChatTarget}](tg://user?id={Vars.CurrentConf.ContChatTarget})");
                    _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id, ReplyToMessage, ParseMode.Markdown, false, false, update.Message.MessageId).ConfigureAwait(false);
                }
                Log($"Successfully passed owner's reply to UID: {Vars.CurrentConf.ContChatTarget}", "BOT");
                return;
            }

            _ = await Vars.Bot.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_CommandNotReplying,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
        }
    }
}