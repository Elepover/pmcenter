using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class StatusCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "status";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var MessageStr = Vars.CurrentLang.Message_SysStatus_Header + "\n\n";
            // process other headers
            var NoActionRequired = true;
            if (Vars.UpdatePending)
            {
                MessageStr += Vars.CurrentLang.Message_SysStatus_PendingUpdate.Replace("$1", Vars.UpdateVersion.ToString()) + "\n";
                MessageStr += GetUpdateLevel(Vars.UpdateLevel) + "\n";
                NoActionRequired = false;
            }
            if (Vars.NonEmergRestartRequired)
            {
                MessageStr += Vars.CurrentLang.Message_SysStatus_RestartRequired + "\n";
                NoActionRequired = false;
            }
            if (NoActionRequired)
            {
                MessageStr += Vars.CurrentLang.Message_SysStatus_NoOperationRequired + "\n";
            }
            MessageStr += "\n";
            // process summary
            MessageStr += Vars.CurrentLang.Message_SysStatus_Summary
                .Replace("$1", Environment.MachineName)
                .Replace("$2", Environment.OSVersion.ToString())
                .Replace("$3", RuntimeInformation.OSDescription)
                .Replace("$4", (new TimeSpan(0, 0, 0, 0, Environment.TickCount)).ToString())
                .Replace("$5", Vars.StartSW.Elapsed.ToString())
                .Replace("$6", DateTime.UtcNow.ToShortDateString() + " / " + DateTime.UtcNow.ToShortTimeString())
                .Replace("$7", Environment.Version.ToString())
                .Replace("$8", RuntimeInformation.FrameworkDescription)
                .Replace("$9", Vars.AppVer.ToString())
                .Replace("$a", Environment.ProcessorCount.ToString())
                .Replace("$b", Vars.CurrentLang.LangCode)
                .Replace("$c", GetThreadStatusString(Vars.UpdateCheckerStatus))
                .Replace("$d", GetThreadStatusString(Vars.RateLimiterStatus))
                .Replace("$e", GetThreadStatusString(Vars.ConfResetTimerStatus));

            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                MessageStr,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
