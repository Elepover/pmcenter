using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class InfoCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "info";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (update.Message.ReplyToMessage == null || update.Message.ReplyToMessage.ForwardFrom == null)
            {
                return false;
            }

            string MessageInfo = "ℹ *Message Info*\n📩 *Sender*: [";
            if (Vars.CurrentConf.UseUsernameInMsgInfo)
            {
                MessageInfo += update.Message.ReplyToMessage.ForwardFrom.FirstName + " " + update.Message.ReplyToMessage.ForwardFrom.LastName;
            }
            else
            {
                MessageInfo += "Here";
            }
            MessageInfo += "](tg://user?id="
                + update.Message.ReplyToMessage.ForwardFrom.Id
                + ")\n🔢 *User ID*: `"
                + update.Message.ReplyToMessage.ForwardFrom.Id
                + "`\n🌐 *Language*: `"
                + update.Message.ReplyToMessage.ForwardFrom.LanguageCode
                + "`\n⌚ *Forward Time*: `"
                + update.Message.ReplyToMessage.ForwardDate.ToString()
                + "`\n🆔 *Message ID*: `"
                + update.Message.MessageId
                + "`";
            await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                MessageInfo,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);
            return true;
        }
    }
}
