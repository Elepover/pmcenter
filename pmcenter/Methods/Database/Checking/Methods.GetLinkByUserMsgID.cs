using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static MessageIDLink GetLinkByUserMsgID(long userSessionMsgId)
        {
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (var link in Vars.CurrentConf.MessageLinks)
                {
                    if (link.UserSessionMessageID == userSessionMsgId) { return link; }
                }
            }
            return null;
        }
    }
}
