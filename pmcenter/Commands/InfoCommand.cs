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

            string MessageInfo = "ℹ *Message Info*\n\n📩 *Sender*: [";
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
                + update.Message.MessageId
                + "`";
                
                MessageInfo += "\n\n➕ *Additional Information*"
                + "\n📼 Message Type: " + update.Message.Type.ToString();
                if (update.Message.Type == MessageType.Document)
                {
                    MessageInfo += "\n📛 File Name: `"
                    + update.Message.Document.FileName
                    + "`\n📄 File ID: `"
                    + update.Message.Document.FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.Document.FileSize
                    + "`\n📖 MIME Type: `"
                    + update.Message.Document.MimeType
                    + "`";
                }
                else if (update.Message.Type == MessageType.Location)
                {
                    MessageInfo += "\n🌐 Latitude: `"
                    + update.Message.Location.Latitude
                    + "`\n🌐 Longitude: `"
                    + update.Message.Location.Longitude
                    + "`";
                }
                else if (update.Message.Type == MessageType.Sticker)
                {
                    MessageInfo += "\n😶 Emoji: `"
                    + update.Message.Sticker.Emoji
                    + "`\n 📄 File ID: `"
                    + update.Message.Sticker.FileId
                    + "`";
                }
                else if (update.Message.Type == MessageType.Audio)
                {
                    MessageInfo += "\n📄 File ID: `"
                    + update.Message.Audio.FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.Audio.FileSize
                    + "`\n📖 MIME Type: `"
                    + update.Message.Audio.MimeType
                    + "`\n⏳ Duration(secs): `"
                    + update.Message.Audio.Duration
                    + "`";
                }
                else if (update.Message.Type == MessageType.Photo)
                {
                    MessageInfo += "\n📄 File ID: `"
                    + update.Message.Photo[0].FileId
                    + "`\n🗜 File Size: `"
                    + update.Message.Photo[0].FileSize
                    + "`";
                }
                MessageInfo += "\n\n_Additional information is available for a limited set of message types, including: Audios, Documents(Files), Locations, Photos and Stickers._";
                
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
