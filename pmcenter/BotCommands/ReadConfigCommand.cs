using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class ReadConfigCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "readconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            await Conf.ReadConf();
            await Lang.ReadLang();
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfigReloaded,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
