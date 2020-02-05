using System.Text;
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
            var sb = new StringBuilder("ℹ *Message Info*\n📩 *Sender*: [");
            if (Vars.CurrentConf.UseUsernameInMsgInfo)
            {
                sb.Append(update.Message.ReplyToMessage.ForwardFrom.FirstName);
                sb.Append(" ");
                sb.Append(update.Message.ReplyToMessage.ForwardFrom.LastName);
            }
            else
            {
                sb.Append("Here");
            }
            sb.Append("](tg://user?id=");
            sb.Append(update.Message.ReplyToMessage.ForwardFrom.Id);
            sb.Append(")\n👤 User ID: `");
            sb.Append(update.Message.ReplyToMessage.ForwardFrom.Id);
            sb.Append("`\n🌐 Language: `");
            sb.Append(update.Message.ReplyToMessage.ForwardFrom.LanguageCode);
            sb.Append("`\n⌚ Forward Time: `");
            sb.Append(update.Message.ReplyToMessage.ForwardDate.ToString());
            sb.Append("`\n🆔 Message ID: `");
            sb.Append(update.Message.ReplyToMessage.MessageId);
            sb.Append("`");
                
            sb.Append("\n\n➕ *Additional Info*");
            sb.Append("\n📼 Message Type: " + update.Message.ReplyToMessage.Type.ToString());
            if (update.Message.ReplyToMessage.Document != null)
            {
                sb.Append("\n📛 File Name: `");
                sb.Append(update.Message.ReplyToMessage.Document.FileName);
                sb.Append("`\n📄 File ID: `");
                sb.Append(update.Message.ReplyToMessage.Document.FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(update.Message.ReplyToMessage.Document.FileSize);
                sb.Append("`\n📖 MIME Type: `");
                sb.Append(update.Message.ReplyToMessage.Document.MimeType);
                sb.Append("`");
            }
            else if (update.Message.ReplyToMessage.Location != null)
            {
                sb.Append("\n🌐 Latitude: `");
                sb.Append(update.Message.ReplyToMessage.Location.Latitude);
                sb.Append("`\n🌐 Longitude: `");
                sb.Append(update.Message.ReplyToMessage.Location.Longitude);
                sb.Append("`");
            }
            else if (update.Message.ReplyToMessage.Sticker != null)
            {
                sb.Append("\n😶 Emoji: `");
                sb.Append(update.Message.ReplyToMessage.Sticker.Emoji);
                sb.Append("`\n📄 File ID: `");
                sb.Append(update.Message.ReplyToMessage.Sticker.FileId);
                sb.Append("`");
            }
            else if (update.Message.ReplyToMessage.Audio != null)
            {
                sb.Append("\n📄 File ID: `");
                sb.Append(update.Message.ReplyToMessage.Audio.FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(update.Message.ReplyToMessage.Audio.FileSize);
                sb.Append("`\n📖 MIME Type: `");
                sb.Append(update.Message.ReplyToMessage.Audio.MimeType);
                sb.Append("`\n⏳ Duration(secs): `");
                sb.Append(update.Message.ReplyToMessage.Audio.Duration);
                sb.Append("`");
            }
            else if (update.Message.ReplyToMessage.Photo != null)
            {
                sb.Append("\n📄 File ID: `");
                sb.Append(update.Message.ReplyToMessage.Photo[0].FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(update.Message.ReplyToMessage.Photo[0].FileSize);
                sb.Append("`");
            }
            sb.Append("\n\n_Additional information is available for a limited set of message types, including: Audios, Documents(Files), Locations, Photos and Stickers._");

            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                sb.ToString(),
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
