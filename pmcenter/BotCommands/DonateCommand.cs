using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class DonateCommand : IBotCommand
    {
        public bool OwnerOnly => false;

        public string Prefix => "donate";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (string.IsNullOrEmpty(Vars.CurrentConf.DonateString))
            {
                return false;
            }
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentConf.DonateString,
                parseMode: ParseMode.Markdown,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyToMessageId: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
