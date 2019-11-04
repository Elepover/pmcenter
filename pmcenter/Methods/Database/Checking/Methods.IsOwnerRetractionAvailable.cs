using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static bool IsOwnerRetractionAvailable(int OwnerSessionMsgID)
        {
            if (!Vars.CurrentConf.EnableMsgLink) { return false; }
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (MessageIDLink Link in Vars.CurrentConf.MessageLinks)
                {
                    if (Link.OwnerSessionMessageID == OwnerSessionMsgID) { return true; }
                }
            }
            return false;
        }
    }
}