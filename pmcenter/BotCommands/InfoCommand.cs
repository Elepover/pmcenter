using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class InfoCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "info";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (update.Message.ReplyToMessage == null || update.Message.ReplyToMessage.ForwardFrom == null)
            {
                return false;
            }
            var targetMessage = update.Message.ReplyToMessage;
            var sb = new StringBuilder("ℹ *Message Info*\n📩 *Sender*: [");
            if (Vars.CurrentConf.UseUsernameInMsgInfo)
            {
                sb.Append(targetMessage.ForwardFrom.FirstName);
                sb.Append(" ");
                sb.Append(targetMessage.ForwardFrom.LastName);
            }
            else
            {
                sb.Append("Here");
            }
            sb.Append("](tg://user?id=");
            sb.Append(targetMessage.ForwardFrom.Id);
            sb.Append(")\n👤 User ID: `");
            sb.Append(targetMessage.ForwardFrom.Id);
            sb.Append("`\n🌐 Language: `");
            sb.Append(targetMessage.ForwardFrom.LanguageCode);
            sb.Append("`\n⌚ Forward Time: `");
            sb.Append(targetMessage.ForwardDate.ToString());
            sb.Append("`\n🆔 Message ID: `");
            sb.Append(targetMessage.MessageId);
            sb.Append("`");
                
            sb.Append("\n\n➕ *Additional Info*");
            sb.Append("\n📼 Message Type: " + targetMessage.Type.ToString());
            if (targetMessage.Document != null)
            {
                sb.Append("\n📛 File Name: `");
                sb.Append(targetMessage.Document.FileName);
                sb.Append("`\n📄 File ID: `");
                sb.Append(targetMessage.Document.FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(targetMessage.Document.FileSize);
                sb.Append("`\n📖 MIME Type: `");
                sb.Append(targetMessage.Document.MimeType);
                sb.Append("`");
            }
            else if (targetMessage.Location != null)
            {
                sb.Append("\n🌐 Latitude: `");
                sb.Append(targetMessage.Location.Latitude);
                sb.Append("`\n🌐 Longitude: `");
                sb.Append(targetMessage.Location.Longitude);
                sb.Append("`");
            }
            else if (targetMessage.Sticker != null)
            {
                sb.Append("\n😶 Emoji: `");
                sb.Append(targetMessage.Sticker.Emoji);
                sb.Append("`\n📄 File ID: `");
                sb.Append(targetMessage.Sticker.FileId);
                sb.Append("`");
            }
            else if (targetMessage.Audio != null)
            {
                sb.Append("\n📄 File ID: `");
                sb.Append(targetMessage.Audio.FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(targetMessage.Audio.FileSize);
                sb.Append("`\n📖 MIME Type: `");
                sb.Append(targetMessage.Audio.MimeType);
                sb.Append("`\n⏳ Duration(secs): `");
                sb.Append(targetMessage.Audio.Duration);
                sb.Append("`");
            }
            else if (targetMessage.Photo != null)
            {
                sb.Append("\n📄 File ID: `");
                sb.Append(targetMessage.Photo[0].FileId);
                sb.Append("`\n🗜 File Size: `");
                sb.Append(targetMessage.Photo[0].FileSize);
                sb.Append("`");
            }
            else if (targetMessage.Dice != null)
            {
                sb.Append("\n🎲 Dice/Dart: `");
                sb.Append(targetMessage.Dice.Value);
                sb.Append("`");
            }
            sb.Append("\n\n_Additional information is available for a limited set of message types, including: Audios, Documents(Files), Dices, Locations, Photos and Stickers._");

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
