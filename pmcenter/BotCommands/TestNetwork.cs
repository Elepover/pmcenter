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
            double LatencyToGH = Math.Round((await TestLatency("https://github.com")).TotalMilliseconds, 2);
            double LatencyToTG = Math.Round((await TestLatency("https://api.telegram.org/bot")).TotalMilliseconds, 2);
            double LatencyToCI = Math.Round((await TestLatency("https://ci.appveyor.com")).TotalMilliseconds, 2);
            await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_Connectivity
                    .Replace("$1", LatencyToGH + "ms")
                    .Replace("$2", LatencyToTG + "ms")
                    .Replace("$3", LatencyToCI + "ms"),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
