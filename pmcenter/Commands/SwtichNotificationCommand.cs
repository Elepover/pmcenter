using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwtichNotificationCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchnf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            bool IsDisabledNow = Conf.SwitchNotifications();
            await Conf.SaveConf(false, true);
            await botClient.SendTextMessageAsync(update.Message.From.Id,
                IsDisabledNow ?
                    Vars.CurrentLang.Message_NotificationsOff :
                    Vars.CurrentLang.Message_NotificationsOn,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);

            return true;
        }
    }
}
