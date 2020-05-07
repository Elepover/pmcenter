using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class ReadConfigCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "readconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await Conf.ReadConf().ConfigureAwait(false);
            _ = await Lang.ReadLang().ConfigureAwait(false);
            Log("Configurations and locale files have been reloaded from disk", "BOT");
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfigReloaded,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
