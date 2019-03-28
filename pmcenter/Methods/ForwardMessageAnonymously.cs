using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace pmcenter
{
    public partial class Methods
    {
        public static async Task<int> ForwardMessageAnonymously(Telegram.Bot.Types.ChatId ToChatId,
                                                           bool DisableNotifications,
                                                           Telegram.Bot.Types.Message Msg)
        {
            switch (Msg.Type)
            {
                case MessageType.Text:
                    await Vars.Bot.SendTextMessageAsync(ToChatId, Msg.Text, ParseMode.Default, false, DisableNotifications);
                    return 0;
                default:
                    await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_SupportTextMessagesOnly, ParseMode.Markdown, false, DisableNotifications, Msg.MessageId);
                    return 1;
            }
        }
    }
}