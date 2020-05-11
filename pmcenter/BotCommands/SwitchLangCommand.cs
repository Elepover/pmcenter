using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.H2Helper;

namespace pmcenter.Commands
{
    internal class SwitchLangCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchlang";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            // errors are handled by global error handler
            // notify user
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_SwitchingLang,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
                
            // download file
            var langUrl = update.Message.Text.Split(" ")[1];
            Vars.CurrentConf.LangURL = langUrl;
            // save conf
            _ = await Conf.SaveConf(isAutoSave: true);
            await DownloadFileAsync(
                new Uri(langUrl),
                Path.Combine(Vars.AppDirectory, "pmcenter_locale.json")
            ).ConfigureAwait(false);

            // reload configurations
            _ = await Conf.ReadConf().ConfigureAwait(false);
            _ = await Lang.ReadLang().ConfigureAwait(false);

            // notify user
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_LangSwitched,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);

            return true;
        }
    }
}
