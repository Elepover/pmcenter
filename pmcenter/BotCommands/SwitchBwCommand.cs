using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchBwCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchbw";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var isEnabledNow = Conf.SwitchBlocking();
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                isEnabledNow ?
                    Vars.CurrentLang.Message_MessageBlockEnabled :
                    Vars.CurrentLang.Message_MessageBlockDisabled,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);

            return true;
        }
    }
}
