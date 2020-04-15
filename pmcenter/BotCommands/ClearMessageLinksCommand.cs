using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class ClearMessageLinksCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "clearmessagelinks";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            Log("Clearing message links...", "BOT");
            Vars.CurrentConf.MessageLinks = new List<Conf.MessageIDLink>();
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_MsgLinksCleared,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
