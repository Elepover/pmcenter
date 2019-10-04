using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class EditConfCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "editconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Log("Configurations received, applying...", "BOT", LogLevel.INFO);
                var ConfStr = update.Message.Text.Split(" ", 2)[1];
                var Temp = JsonConvert.DeserializeObject<Conf.ConfObj>(ConfStr);
                if (Temp.APIKey != Vars.CurrentConf.APIKey)
                {
                    Log("API Key has changed! Please restart pmcenter to apply the change.", "BOT", LogLevel.WARN);
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_APIKeyChanged,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    Vars.RestartRequired = true;
                }
                if (Temp.ConfSyncInterval == 0)
                {
                    Log("ConfSync has been disabled, the worker thread will exit soon.", "BOT", LogLevel.WARN);
                }
                else if (Vars.CurrentConf.ConfSyncInterval == 0)
                {
                    Log("The ConfSync feature has been enabled via configurations. Restarting thread...", "BOT");
                    while (Vars.SyncConf.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                    Vars.SyncConf = new Thread(() => ThrSyncConf());
                    Vars.SyncConf.Start();
                }
                Vars.CurrentConf = Temp;
                Log("Applied! Saving to local disk...", "BOT", LogLevel.INFO);
                await Conf.SaveConf(false, true);
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfigUpdated,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.Message),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
            }
            return true;
        }
    }
}
