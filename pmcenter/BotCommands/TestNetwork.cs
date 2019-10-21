using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class TestNetworkCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "testnetwork";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            Log("Starting network test...", "BOT");
            var LatencyToGH = Math.Round((await TestLatency("https://github.com").ConfigureAwait(false)).TotalMilliseconds, 2);
            var LatencyToTG = Math.Round((await TestLatency("https://api.telegram.org/bot").ConfigureAwait(false)).TotalMilliseconds, 2);
            var LatencyToCI = Math.Round((await TestLatency("https://ci.appveyor.com").ConfigureAwait(false)).TotalMilliseconds, 2);
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_Connectivity
                    .Replace("$1", LatencyToGH + "ms")
                    .Replace("$2", LatencyToTG + "ms")
                    .Replace("$3", LatencyToCI + "ms"),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
