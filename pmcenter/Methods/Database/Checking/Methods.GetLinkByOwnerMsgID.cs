using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static MessageIDLink GetLinkByOwnerMsgID(long ownerSessionMsgId)
        {
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (var link in Vars.CurrentConf.MessageLinks)
                {
                    if (link.OwnerSessionMessageID == ownerSessionMsgId) return link;
                }
            }
            return null;
        }
    }
}
