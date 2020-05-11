using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class ChatCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chat";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Log("Enabling Continued Conversation...", "BOT");

                long realTarget;
                bool isArgumentMode;
                // try to use argument
                if (update.Message.Text.Contains(" "))
                {
                    isArgumentMode = true;
                    Log("Resolving arguments...", "BOT");
                    realTarget = long.Parse(update.Message.Text.Split(" ", 2)[1]);
                }
                else
                {
                    isArgumentMode = false;
                    // no argument detected / use reply message instead
                    if (update.Message.ReplyToMessage.ForwardFrom == null)
                    {
                        throw (new ArgumentException("Cannot initiate Continued Conversation by channel posts."));
                    }
                    realTarget = update.Message.ReplyToMessage.ForwardFrom.Id;
                }

                Log($"Continued Conversation enabled, target: {realTarget}", "BOT");
                Vars.CurrentConf.ContChatTarget = realTarget;
                _ = await Conf.SaveConf(false, true).ConfigureAwait(false);

                string replacementText;
                if (isArgumentMode)
                {
                    replacementText = $"[{realTarget}](tg://user?id={realTarget})";
                }
                else
                {
                    replacementText = $"[{update.Message.ReplyToMessage.ForwardFrom.FirstName} (@{update.Message.ReplyToMessage.ForwardFrom.Username})](tg://user?id={realTarget})";
                }

                _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_ContinuedChatEnabled.Replace("$1", replacementText),
                                                    ParseMode.Markdown,
                                                    false,
                                                    Vars.CurrentConf.DisableNotifications,
                                                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log($"Failed to enable Continued Conversation: {ex}", "BOT", LogLevel.Error);
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
