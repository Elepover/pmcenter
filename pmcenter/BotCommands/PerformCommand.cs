using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class PerformCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "perform";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Performance_Inited,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            var performanceChecker = new Thread(() => Methods.ThrPerform());
            performanceChecker.Start();
            Thread.Sleep(1000);
            Vars.IsPerformanceTestEndRequested = true;
            while (Vars.IsPerformanceTestExecuting)
            {
                Thread.Sleep(500);
            }
            Vars.PerformanceScore /= 1000000;
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Performance_Results.Replace("$1", (Vars.PerformanceScore / 5) + "Mop/s"),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
