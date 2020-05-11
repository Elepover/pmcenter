using Telegram.Bot.Types;

namespace pmcenter
{
    public static partial class Conf
    {
        public class MessageIDLink
        {
            public MessageIDLink()
            {
                TGUser = null;
                OwnerSessionMessageID = -1;
                OwnerSessionActionMessageID = -1;
                UserSessionMessageID = -1;
                IsFromOwner = false;
            }
            public User TGUser { get; set; }
            /// <summary>
            /// Message ID of the message in owner's session
            /// </summary>
            public int OwnerSessionMessageID { get; set; }
            public int OwnerSessionActionMessageID { get; set; }
            public int UserSessionMessageID { get; set; }
            public bool IsFromOwner { get; set; }
        }
    }
}
