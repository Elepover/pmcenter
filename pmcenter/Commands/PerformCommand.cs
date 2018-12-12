using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class PerformCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "perform";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Performance_Inited,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            Thread PerformanceChecker = new Thread(() => Methods.ThrPerform());
            PerformanceChecker.Start();
            Thread.Sleep(1000);
            Vars.IsPerformanceTestEndRequested = true;
            while (Vars.IsPerformanceTestExecuting)
            {
                Thread.Sleep(500);
            }
            Vars.PerformanceScore /= 1000000000;
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Performance_Results.Replace("$1", Vars.PerformanceScore / 5 + "Gop/s"),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
