using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class ChatCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chat";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Log("Enabling Continued Conversation...", "BOT");

                long RealTarget;
                bool IsArgumentMode;
                // try to use argument
                if (update.Message.Text.Contains(" "))
                {
                    IsArgumentMode = true;
                    Log("Resolving arguments...", "BOT");
                    RealTarget = long.Parse(update.Message.Text.Split(" ", 2)[1]);
                }
                else
                {
                    IsArgumentMode = false;
                    // no argument detected / use reply message instead
                    if (update.Message.ReplyToMessage.ForwardFrom == null)
                    {
                        throw (new ArgumentException("Cannot initiate Continued Conversation by channel posts."));
                    }
                    RealTarget = update.Message.ReplyToMessage.ForwardFrom.Id;
                }

                Log("Continued Conversation enabled, target: " + RealTarget, "BOT");
                Vars.CurrentConf.ContChatTarget = RealTarget;
                _ = await Conf.SaveConf(false, true).ConfigureAwait(false);

                string ReplaceText;
                if (IsArgumentMode)
                {
                    ReplaceText = "[" + RealTarget + "](tg://user?id=" + RealTarget + ")";
                }
                else
                {
                    ReplaceText = "[" + update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + update.Message.ReplyToMessage.ForwardFrom.Username + ")](tg://user?id=" + RealTarget + ")";
                }

                _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_ContinuedChatEnabled.Replace("$1", ReplaceText),
                                                    ParseMode.Markdown,
                                                    false,
                                                    Vars.CurrentConf.DisableNotifications,
                                                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log("Failed to enable Continued Conversation: " + ex.ToString(), "BOT", LogLevel.ERROR);
                _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                    Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.ToString()),
                    ParseMode.Default,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
