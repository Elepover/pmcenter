using System;
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
                string ConfStr = update.Message.Text.Split(" ", 2)[1];
                Conf.ConfObj Temp = JsonConvert.DeserializeObject<Conf.ConfObj>(ConfStr);
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
