namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsOwnerRetractionAvailable(int ownerSessionMsgId)
        {
            if (!Vars.CurrentConf.EnableMsgLink) return false;
            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (var link in Vars.CurrentConf.MessageLinks)
                {
                    if (link.OwnerSessionMessageID == ownerSessionMsgId) return true;
                }
            }
            return false;
        }
    }
}
