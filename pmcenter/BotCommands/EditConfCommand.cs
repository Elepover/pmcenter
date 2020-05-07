using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class EditConfCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "editconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Log("Configurations received, applying...", "BOT", LogLevel.Info);
                var confStr = update.Message.Text.Split(" ", 2)[1];
                var temp = JsonConvert.DeserializeObject<Conf.ConfObj>(confStr);
                if (temp.APIKey != Vars.CurrentConf.APIKey)
                {
                    Log("API Key has changed! Please restart pmcenter to apply the change.", "BOT", LogLevel.Warning);
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_APIKeyChanged,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                    Vars.RestartRequired = true;
                }
                if (temp.ConfSyncInterval == 0)
                {
                    Log("ConfSync has been disabled, the worker thread will exit soon.", "BOT", LogLevel.Warning);
                }
                else if (Vars.CurrentConf.ConfSyncInterval == 0)
                {
                    Log("The ConfSync feature has been enabled via configurations. Restarting thread...", "BOT");
                    while (Vars.SyncConf.IsAlive) Thread.Sleep(100);
                    Vars.SyncConf = new Thread(() => ThrSyncConf());
                    Vars.SyncConf.Start();
                }
                Vars.CurrentConf = temp;
                Log("Applied! Saving to local disk...", "BOT", LogLevel.Info);
                _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfigUpdated,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.Message),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
            }
            return true;
        }
    }
}
