using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using static pmcenter.Methods;

namespace pmcenter {
    public class BotProcess {
        public static async void OnUpdate(object sender, UpdateEventArgs e) {
            if (e.Update.Type != UpdateType.Message) { return; }
            if (e.Update.Message.From.IsBot) { return; }
            if (e.Update.Message.Chat.Type != ChatType.Private) { return; }
            string Username = e.Update.Message.From.Username;
            string FirstName = e.Update.Message.From.FirstName;
            long UID = e.Update.Message.From.Id;
            if (IsBanned(e.Update.Message.From.Id)) {
                Log("Restricting banned user from sending messages: " + FirstName + " (@" + Username + " / " + UID + ")", "BOT");
                return;
            }
            // Reworked processing logic.
            if (e.Update.Message.From.Id == Vars.CurrentConf.OwnerUID) {
                // Commands?
                if (e.Update.Message.ReplyToMessage != null) {
                    if (e.Update.Message.ReplyToMessage.ForwardFrom != null) {
                        if (e.Update.Message.Type == MessageType.Text) {
                            if (e.Update.Message.Text.ToLower() == "/info") {
                                string MessageInfo = "ℹ *Message Info*\n📩 *Sender*: [Here](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")\n🔢 *User ID*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + "`\n🌐 *Language*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.LanguageCode + "`\n⌚ *Forward Time*: `" + e.Update.Message.ReplyToMessage.ForwardDate.ToString() + "`";
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, MessageInfo, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else if (e.Update.Message.Text.ToLower() == "/ban") {
                                BanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserBanned, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else if (e.Update.Message.Text.ToLower() == "/pardon") {
                                UnbanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserPardoned, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } // not a recogized command.
                        }
                        // Is replying, replying to forwarded message AND not command.
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.ReplyToMessage.ForwardFrom.Id, e.Update.Message.Text, ParseMode.Markdown, false, false);
                        // Process locale.
                        string ReplyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                        ReplyToMessage = ReplyToMessage.Replace("$1", "[" + e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + ")](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")");
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, ReplyToMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        Log("Successfully passed owner's reply to " + e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + " / " + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")", "BOT");
                        return;
                    } else {
                        // The owner is replying to bot messages. (no forwardfrom)
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_CommandNotReplyingValidMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        return;
                    }
                } else {
                    // The owner is not even replying.
                    // start or help command?
                    if (e.Update.Message.Type == MessageType.Text) {
                        if (e.Update.Message.Text.ToLower() == "/start") {
                            await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_OwnerStart, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/help") {
                            await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_Help, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        }
                        // not a command.
                    }
                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_CommandNotReplying, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    return;
                }
            } else {
                // is user
                if (e.Update.Message.Type == MessageType.Text) {
                    // is command?
                    if (e.Update.Message.Text.ToLower() == "/start") {
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserStartDefault, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        return;
                    }
                }
                Log("Message from \"" + FirstName + "\" (@" + Username + " / " + UID + "), forwarding...", "BOT");
                await Vars.Bot.ForwardMessageAsync(Vars.CurrentConf.OwnerUID, e.Update.Message.From.Id, e.Update.Message.MessageId, false);
                AddRateLimit(e.Update.Message.From.Id);
                return;
            }
        }
    }
}