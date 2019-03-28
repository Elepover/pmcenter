using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static MessageIDLink GetLinkByMsgID(long OwnerSessionMsgID)
        {
            foreach (MessageIDLink Link in Vars.CurrentConf.MessageLinks)
            {
                if (Link.OwnerSessionMessageID == OwnerSessionMsgID) { return Link; }
            }
            return null;
        }
    }
}