using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class StopChatCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "stopchat";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            Log($"Disabling Continued Conversation with: {Vars.CurrentConf.ContChatTarget}", "BOT");
            Vars.CurrentConf.ContChatTarget = -1;
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_ContinuedChatDisabled,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
