using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class StatusCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "status";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var messageStr = Vars.CurrentLang.Message_SysStatus_Header + "\n\n";
            // process other headers
            var noActionRequired = true;
            if (Vars.UpdatePending)
            {
                messageStr += Vars.CurrentLang.Message_SysStatus_PendingUpdate.Replace("$1", Vars.UpdateVersion.ToString()) + "\n";
                messageStr += GetUpdateLevel(Vars.UpdateLevel) + "\n";
                noActionRequired = false;
            }
            if (Vars.NonEmergRestartRequired)
            {
                messageStr += Vars.CurrentLang.Message_SysStatus_RestartRequired + "\n";
                noActionRequired = false;
            }
            if (noActionRequired)
            {
                messageStr += Vars.CurrentLang.Message_SysStatus_NoOperationRequired + "\n";
            }
            messageStr += "\n";
            // process summary
            messageStr += Vars.CurrentLang.Message_SysStatus_Summary
                .Replace("$1", Environment.MachineName)
                .Replace("$2", Environment.OSVersion.ToString())
                .Replace("$3", RuntimeInformation.OSDescription)
                .Replace("$4", (new TimeSpan(0, 0, 0, 0, Environment.TickCount)).ToString())
                .Replace("$5", Vars.StartSW.Elapsed.ToString())
                .Replace("$6", $"{DateTime.UtcNow.ToShortDateString()} / {DateTime.UtcNow.ToShortTimeString()}")
                .Replace("$7", Environment.Version.ToString())
                .Replace("$8", RuntimeInformation.FrameworkDescription)
                .Replace("$9", Vars.AppVer.ToString())
                .Replace("$a", Environment.ProcessorCount.ToString())
                .Replace("$b", Vars.CurrentLang.LangCode)
                .Replace("$c", GetThreadStatusString(Vars.UpdateCheckerStatus))
                .Replace("$d", GetThreadStatusString(Vars.RateLimiterStatus))
                .Replace("$e", GetThreadStatusString(Vars.ConfResetTimerStatus))
                .Replace("$f", Vars.CompileChannel)
                .Replace("$g", Vars.CurrentConf.UpdateChannel);

            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                messageStr,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
