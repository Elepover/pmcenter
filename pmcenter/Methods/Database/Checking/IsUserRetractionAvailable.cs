using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static bool IsUserRetractionAvailable(int UserSessionMsgID)
        {
            if (!Vars.CurrentConf.EnableMsgLink) { return false; }
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (MessageIDLink Link in Vars.CurrentConf.MessageLinks)
                {
                    if (Link.UserSessionMessageID == UserSessionMsgID) { return true; }
                }
            }
            return false;
        }
    }
}