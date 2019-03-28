using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class HelpCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "help";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(
                Vars.CurrentConf.OwnerUID,
                Vars.CurrentLang.Message_Help,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
