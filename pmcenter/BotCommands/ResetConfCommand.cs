using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class ResetConfCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "resetconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (Vars.IsResetConfAvailable)
            {
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfReset_Started,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                long OwnerID = Vars.CurrentConf.OwnerUID;
                string APIKey = Vars.CurrentConf.APIKey;
                Vars.CurrentConf = new Conf.ConfObj();
                Vars.CurrentConf.OwnerUID = OwnerID;
                Vars.CurrentConf.APIKey = APIKey;
                await Conf.SaveConf(false, true);
                Vars.CurrentLang = new Lang.Language();
                await Lang.SaveLang();
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfReset_Done,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                Environment.Exit(0);
                return true;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfReset_Inited,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                Thread ConfValidator = new Thread(() => Methods.ThrDoResetConfCount());
                ConfValidator.Start();
                return true;
            }
        }
    }
}
