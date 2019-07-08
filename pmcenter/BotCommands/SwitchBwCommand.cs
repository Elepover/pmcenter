using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchBwCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchbw";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var IsEnabledNow = Conf.SwitchBlocking();
            await Conf.SaveConf(false, true);
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                IsEnabledNow ?
                    Vars.CurrentLang.Message_MessageBlockEnabled :
                    Vars.CurrentLang.Message_MessageBlockDisabled,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);

            return true;
        }
    }
}
