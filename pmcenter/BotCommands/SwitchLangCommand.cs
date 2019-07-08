using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class SwitchLangCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchlang";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            // errors are handled by global error handler
            // notify user
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_SwitchingLang,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
                
            // download file
            var LangURL = update.Message.Text.Split(" ")[1];
            var Downloader = new WebClient();
            await Downloader.DownloadFileTaskAsync(
                new Uri(LangURL),
                Path.Combine(Vars.AppDirectory, "pmcenter_locale.json")
            );

            // reload configurations
            await Conf.ReadConf();
            await Lang.ReadLang();

            // notify user
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_LangSwitched,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);

            return true;
        }
    }
}
