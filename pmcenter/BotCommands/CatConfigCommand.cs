using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class CatConfigCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "catconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var text = SerializeCurrentConf();
            List<string> texts = (from Match match in Regex.Matches(text, @"\d{1,4096}") select match.Value).ToList();
            if (texts.Count == 1)
            {
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CurrentConf.Replace("$1", text),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                    return true;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CurrentConf.Replace("$1", texts[0]),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                for (int i = 1; i < texts.Count; i++)
                {
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        ("`" + texts[i] + "`"),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                }
                return true;
            }
        }
    }
}
