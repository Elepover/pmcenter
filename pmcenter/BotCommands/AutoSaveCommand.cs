using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class AutoSaveCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "autosave";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                var text = update.Message.Text;
                var interval = 30000;
                if (text.Contains(" "))
                {
                    var command = text.Split(' ', 2)[1];
                    if (command.ToLower() == "off")
                    {
                        Vars.CurrentConf.ConfSyncInterval = 0;
                        _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                            Vars.CurrentLang.Message_AutoSaveDisabled,
                            ParseMode.Markdown,
                            false,
                            Vars.CurrentConf.DisableNotifications,
                            update.Message.MessageId).ConfigureAwait(false);
                        if (Vars.SyncConf != null) Vars.SyncConf.Interrupt();
                        return true;
                    }
                    interval = int.Parse(command);
                }
                Vars.CurrentConf.ConfSyncInterval = interval;
                _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                    Vars.CurrentLang.Message_AutoSaveEnabled.Replace("$1", (interval / 1000).ToString()),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                if (interval < 5000)
                    _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                    Vars.CurrentLang.Message_AutoSaveIntervalTooShort.Replace("$1", interval.ToString()),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                if ((Vars.SyncConf == null) || !Vars.SyncConf.IsAlive)
                {
                    Vars.SyncConf = new Thread(() => Methods.ThrSyncConf());
                    Vars.SyncConf.Start();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log($"Failed to process autosave command: {ex}", "BOT", LogLevel.Error);
                _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                    Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.ToString()),
                    ParseMode.Default,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
