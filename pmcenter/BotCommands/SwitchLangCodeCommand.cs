using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.H2Helper;

namespace pmcenter.Commands
{
    internal class SwitchLangCodeCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchlangcode";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            string reply;
            string listJsonString;
            Conf.LocaleList localeList;
            // try to get locale list
            listJsonString = await GetStringAsync(new Uri(Vars.LocaleMapURL.Replace("$channel", Vars.CurrentConf.UpdateChannel))).ConfigureAwait(false);
            localeList = JsonConvert.DeserializeObject<Conf.LocaleList>(listJsonString);
            if (!update.Message.Text.Contains(" "))
            {
                var listString = "";
                foreach (var mirror in localeList.Locales)
                {
                    listString += $"{mirror.LocaleCode} - {mirror.LocaleNameNative} ({mirror.LocaleNameEng})\n";
                }
                reply = Vars.CurrentLang.Message_AvailableLang.Replace("$1", listString);
            }
            else
            {
                var targetCode = update.Message.Text.Split(" ")[1];
                foreach (var mirror in localeList.Locales)
                {
                    if (mirror.LocaleCode == targetCode)
                    {
                        // update configurations
                        Vars.CurrentConf.LangURL = mirror.LocaleFileURL.Replace("$channel", Vars.CompileChannel);
                        // start downloading
                        _ = await Conf.SaveConf(isAutoSave: true).ConfigureAwait(false);
                        await DownloadFileAsync(
                            new Uri(Vars.CurrentConf.LangURL),
                            Path.Combine(
                                Vars.AppDirectory,
                                "pmcenter_locale.json"
                            )
                        ).ConfigureAwait(false);
                        // reload configurations
                        _ = await Conf.ReadConf().ConfigureAwait(false);
                        _ = await Lang.ReadLang().ConfigureAwait(false);
                        reply = Vars.CurrentLang.Message_LangSwitched;
                        goto SendMsg;
                    }
                }
                throw new ArgumentException("Language not found.");
            }
SendMsg:
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                reply,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
