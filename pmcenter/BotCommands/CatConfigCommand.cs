using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class CatConfigCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "catconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var text = SerializeCurrentConf();
            var texts = StrChunk(text, 1024);
            if (texts.Count == 1)
            {
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CurrentConf.Replace("$1", text),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                    return true;
            }
            else
            {
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CurrentConf.Replace("$1", texts[0]),
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId).ConfigureAwait(false);
                for (int i = 1; i < texts.Count; i++)
                {
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        ($"`{texts[i]}`"),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId).ConfigureAwait(false);
                }
                return true;
            }
        }
    }
}
