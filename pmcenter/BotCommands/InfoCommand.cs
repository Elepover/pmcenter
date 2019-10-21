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

            var MessageInfo = "ℹ *Message Info*\n📩 *Sender*: [";
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
                + ")\n👤 User ID: `"
                + update.Message.ReplyToMessage.ForwardFrom.Id
                + "`\n🌐 Language: `"
                + update.Message.ReplyToMessage.ForwardFrom.LanguageCode
                + "`\n⌚ Forward Time: `"
                + update.Message.ReplyToMessage.ForwardDate.ToString()
                + "`\n🆔 Message ID: `"
                + update.Message.ReplyToMessage.MessageId
                + "`";
                
                MessageInfo += "\n\n➕ *Additional Info*"
                + "\n📼 Message Type: " + update.Message.ReplyToMessage.Type.ToString();
                if (update.Message.ReplyToMessage.Document != null)
                {
                    MessageInfo += "\n📛 File Name: `"
                    + update.Message.ReplyToMessage.Document.FileName
                    + "`\n📄 File ID: `"
                    + update.Message.ReplyToMessage.Document.FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.ReplyToMessage.Document.FileSize
                    + "`\n📖 MIME Type: `"
                    + update.Message.ReplyToMessage.Document.MimeType
                    + "`";
                }
                else if (update.Message.ReplyToMessage.Location != null)
                {
                    MessageInfo += "\n🌐 Latitude: `"
                    + update.Message.ReplyToMessage.Location.Latitude
                    + "`\n🌐 Longitude: `"
                    + update.Message.ReplyToMessage.Location.Longitude
                    + "`";
                }
                else if (update.Message.ReplyToMessage.Sticker != null)
                {
                    MessageInfo += "\n😶 Emoji: `"
                    + update.Message.ReplyToMessage.Sticker.Emoji
                    + "`\n📄 File ID: `"
                    + update.Message.ReplyToMessage.Sticker.FileId
                    + "`";
                }
                else if (update.Message.ReplyToMessage.Audio != null)
                {
                    MessageInfo += "\n📄 File ID: `"
                    + update.Message.ReplyToMessage.Audio.FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.ReplyToMessage.Audio.FileSize
                    + "`\n📖 MIME Type: `"
                    + update.Message.ReplyToMessage.Audio.MimeType
                    + "`\n⏳ Duration(secs): `"
                    + update.Message.ReplyToMessage.Audio.Duration
                    + "`";
                }
                else if (update.Message.ReplyToMessage.Photo != null)
                {
                    MessageInfo += "\n📄 File ID: `"
                    + update.Message.ReplyToMessage.Photo[0].FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.ReplyToMessage.Photo[0].FileSize
                    + "`";
                }
                MessageInfo += "\n\n_Additional information is available for a limited set of message types, including: Audios, Documents(Files), Locations, Photos and Stickers._";

            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                MessageInfo,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
