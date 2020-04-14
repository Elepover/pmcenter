using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static MessageIDLink GetLinkByOwnerMsgID(long OwnerSessionMsgID)
        {
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (MessageIDLink Link in Vars.CurrentConf.MessageLinks)
                {
                    if (Link.OwnerSessionMessageID == OwnerSessionMsgID) { return Link; }
                }
            }
            return null;
        }
    }
}
