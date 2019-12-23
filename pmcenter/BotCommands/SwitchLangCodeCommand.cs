using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchLangCodeCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchlangcode";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            string Reply;
            string ListJSONString;
            // try to get locale list
            using (var ListDownloader = new WebClient())
            {
                ListJSONString = await ListDownloader.DownloadStringTaskAsync(Vars.LocaleMapURL).ConfigureAwait(false);
            }
            Conf.LocaleList LocaleList = JsonConvert.DeserializeObject<Conf.LocaleList>(ListJSONString);
            if (!update.Message.Text.Contains(" "))
            {
                var ListString = "";
                foreach (Conf.LocaleMirror Mirror in LocaleList.Locales)
                {
                    ListString += $"{Mirror.LocaleCode} - {Mirror.LocaleNameNative} ({Mirror.LocaleNameEng})\n";
                }
                Reply = Vars.CurrentLang.Message_AvailableLang.Replace("$1", ListString);
            }
            else
            {
                var TargetCode = update.Message.Text.Split(" ")[1];
                foreach (Conf.LocaleMirror Mirror in LocaleList.Locales)
                {
                    if (Mirror.LocaleCode == TargetCode)
                    {
                        // start downloading
                        using (var Downloader = new WebClient())
                        {
                            await Downloader.DownloadFileTaskAsync(
                                            new Uri(Mirror.LocaleFileURL),
                                            Path.Combine(Vars.AppDirectory, "pmcenter_locale.json")
                                            ).ConfigureAwait(false);
                        }
                        // reload configurations
                        _ = await Conf.ReadConf().ConfigureAwait(false);
                            _ = await Lang.ReadLang().ConfigureAwait(false);
                            Reply = Vars.CurrentLang.Message_LangSwitched;
                            goto SendMsg;
                    }
                }
                throw new ArgumentException("Language not found.");
            }
SendMsg:
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Reply,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
