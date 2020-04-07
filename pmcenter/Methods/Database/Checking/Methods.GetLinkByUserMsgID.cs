using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static MessageIDLink GetLinkByUserMsgID(long UserSessionMsgID)
        {
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (MessageIDLink Link in Vars.CurrentConf.MessageLinks)
                {
                    if (Link.UserSessionMessageID == UserSessionMsgID) { return Link; }
                }
            }
            return null;
        }
    }
}
