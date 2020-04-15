using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsBanned(long UID)
        {
            foreach (BanObj Banned in Vars.CurrentConf.Banned)
            {
                if (Banned.UID == UID) { return true; }
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
