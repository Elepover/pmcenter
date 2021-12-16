using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class BanCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "ban";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (update.Message.ReplyToMessage == null || update.Message.ReplyToMessage.ForwardFrom == null) return false;

            BanUser(update.Message.ReplyToMessage.ForwardFrom.Id);
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_UserBanned,
                parseMode: ParseMode.Markdown,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyToMessageId: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
