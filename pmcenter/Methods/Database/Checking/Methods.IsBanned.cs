using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsBanned(long uid)
        {
            foreach (var banned in Vars.CurrentConf.Banned)
            {
                if (banned.UID == uid) return true;
            }
            return false;
        }

        public static bool IsBanned(Update update)
        {
            bool isBanned = true;
            if (update.Type == UpdateType.CallbackQuery) isBanned = IsBanned(GetLinkByOwnerMsgID(update.CallbackQuery.Message.ReplyToMessage.MessageId).TGUser.Id);
            if (update.Type == UpdateType.Message) isBanned = IsBanned(update.Message.From.Id);
            return isBanned;
        }
    }
}
