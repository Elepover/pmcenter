using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class CatConfigCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "catconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_CurrentConf.Replace("$1", SerializeCurrentConf()),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
