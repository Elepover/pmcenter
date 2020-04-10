using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class BackupConfCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "backup";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var RandomFilename = $"pmcenter.{DateTime.Now:yyyy-dd-M-HH-mm-ss}#{GetRandomString(6)}.json";
            Log($"Backing up configurations, filename: {Path.Combine(Vars.AppDirectory, RandomFilename)}", "BOT");
            System.IO.File.Copy(Vars.ConfFile, RandomFilename);
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_BackupComplete.Replace("$1", RandomFilename),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
