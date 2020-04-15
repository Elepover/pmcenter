using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchFwCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchfw";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var isPausedNow = Conf.SwitchPaused();
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
            isPausedNow ?
                Vars.CurrentLang.Message_ServicePaused :
                Vars.CurrentLang.Message_ServiceResumed,
            ParseMode.Markdown,
            false,
            Vars.CurrentConf.DisableNotifications,
            update.Message.MessageId).ConfigureAwait(false);

            return true;
        }
    }
}
