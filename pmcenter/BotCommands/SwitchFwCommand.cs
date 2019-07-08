using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchFwCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchfw";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var IsPausedNow = Conf.SwitchPaused();
            await Conf.SaveConf(false, true);
            await botClient.SendTextMessageAsync(update.Message.From.Id,
            IsPausedNow ?
                Vars.CurrentLang.Message_ServicePaused :
                Vars.CurrentLang.Message_ServiceResumed,
            ParseMode.Markdown,
            false,
            Vars.CurrentConf.DisableNotifications,
            update.Message.MessageId);

            return true;
        }
    }
}
