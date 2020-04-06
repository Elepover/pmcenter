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
    internal class SwitchLangCodeCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchlangcode";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            string Reply;
            string ListJSONString;
            Conf.LocaleList LocaleList;
            // try to get locale list
            ListJSONString = await GetStringAsync(new Uri(Vars.LocaleMapURL.Replace("$channel", Vars.CurrentConf.UpdateChannel))).ConfigureAwait(false);
            LocaleList = JsonConvert.DeserializeObject<Conf.LocaleList>(ListJSONString);
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
                        // update configurations
                        Vars.CurrentConf.LangURL = Mirror.LocaleFileURL.Replace("$channel", Vars.CompileChannel);
                        // start downloading
                        _ = await Conf.SaveConf(IsAutoSave: true).ConfigureAwait(false);
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
