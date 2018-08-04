using System;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using static pmcenter.Methods;

namespace pmcenter {
    public class BotProcess {
        public static async void OnUpdate(object sender, UpdateEventArgs e) {
            if (e.Update.Type != UpdateType.Message) { return; }
            if (String.IsNullOrEmpty(e.Update.Message.Text)) { return; }
            if (e.Update.Message.From.IsBot) { return; }
            string Username = e.Update.Message.From.Username;
            string FirstName = e.Update.Message.From.FirstName;
            long UID = e.Update.Message.From.Id;
            if (IsBanned(e.Update.Message.From.Id)) {
                Log("Restricting banned user from sending messages: " + FirstName + " (@" + Username + " / " + UID + ")", "BOT");
                return;
            }
            // Decide if the sender is the owner or not.
            if (e.Update.Message.From.Id == Vars.CurrentConf.OwnerUID) {
                if (e.Update.Message.Text.ToLower() == "/start") {
                    await Vars.Bot.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(Vars.CurrentConf.OwnerUID), Vars.OwnerStartMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                } else {
                    // Process replying.
                    if (e.Update.Message.ReplyToMessage.ForwardFromChat != null) {
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.ReplyToMessage.ForwardFromChat, e.Update.Message.Text, ParseMode.Markdown, false, false);
                        await Vars.Bot.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Update.Message.From.Id), "✅ Successfully replied to user [" + FirstName + " (@" + Username + ")](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")!", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    } else {
                        await Vars.Bot.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Update.Message.From.Id), "😶 Don't talk to me, spend time chatting with those who love you.", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    }
                }
            } else {
                try {
                    if (e.Update.Message.Text.ToLower() == "/start") {
                        await Vars.Bot.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Update.Message.From.Id), Vars.CurrentConf.StartMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    } else {
                        Log("Message from \"" + FirstName + "\" (@" + Username + " / " + UID + "), forwarding...", "BOT");
                        await Vars.Bot.ForwardMessageAsync(new Telegram.Bot.Types.ChatId(Vars.CurrentConf.OwnerUID), e.Update.Message.From.Id, e.Update.Message.MessageId, false);
                    }
                } catch (Exception ex) {
                    Log("Unable to forward the message to owner (" + ex.Message + "). Have you ever /start-ed the bot?", "BOT", LogLevel.ERROR);
                }
            }
        }
    }
}