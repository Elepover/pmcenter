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
            if (e.Update.Message.Chat.Type != ChatType.Private) { return; }
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
                    await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.OwnerStartMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                } else {
                    // Process replying.
                    if (e.Update.Message.ReplyToMessage != null) {
                        if (e.Update.Message.ReplyToMessage.ForwardFrom != null) {
                            if (e.Update.Message.Text.ToLower() == "/info") {
                                string MessageInfo = "ℹ *Message Info*\n📩 *Sender*: [Here](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")\n🔢 *User ID*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + "`\n🌐 *Language*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.LanguageCode + "`\n⌚ *Forward Time*: `" + e.Update.Message.ReplyToMessage.ForwardDate.ToString() + "`";
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, MessageInfo, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            } else if (e.Update.Message.Text.ToLower() == "/ban") {
                                BanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, "🚫 Banned the user for 30 minutes.", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            } else if (e.Update.Message.Text.ToLower() == "/pardon") {
                                UnbanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, "✅ You've pardoned the user.", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            } else {
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.ReplyToMessage.ForwardFrom.Id, e.Update.Message.Text, ParseMode.Markdown, false, false);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, "✅ Successfully replied to user [" + e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + ")](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")!", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                Log("Successfully passed owner's reply to " + e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + " / " + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")", "BOT");
                            }
                        } else {
                            await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, "😐 Speaking to me makes no sense.", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        }
                    } else {
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, "😶 Don't talk to me, spend time chatting with those who love you.", ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    }
                }
            } else {
                try {
                    if (e.Update.Message.Text.ToLower() == "/start") {
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentConf.StartMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    } else {
                        Log("Message from \"" + FirstName + "\" (@" + Username + " / " + UID + "), forwarding...", "BOT");
                        await Vars.Bot.ForwardMessageAsync(Vars.CurrentConf.OwnerUID, e.Update.Message.From.Id, e.Update.Message.MessageId, false);
                        AddRateLimit(e.Update.Message.From.Id);
                    }
                } catch (Exception ex) {
                    Log("Unable to forward the message to owner (" + ex.Message + "). Have you ever /start-ed the bot?", "BOT", LogLevel.ERROR);
                }
            }
        }
    }
}