using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class UptimeCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "uptime";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var uptimeString =
                Vars.CurrentLang.Message_UptimeInfo
                .Replace("$1", (new TimeSpan(0, 0, 0, 0, Environment.TickCount)).ToString())
                .Replace("$2", Vars.StartSW.Elapsed.ToString());
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                uptimeString,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
