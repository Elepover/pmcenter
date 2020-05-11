using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class GetStatsCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "getstats";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Stats.Replace("$1", Vars.CurrentConf.Statistics.TotalMessagesReceived.ToString())
                                              .Replace("$2", Vars.CurrentConf.Statistics.TotalForwardedToOwner.ToString())
                                              .Replace("$3", Vars.CurrentConf.Statistics.TotalForwardedFromOwner.ToString())
                                              .Replace("$4", Vars.CurrentConf.Statistics.TotalCommandsReceived.ToString()),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
