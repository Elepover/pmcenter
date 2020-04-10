namespace pmcenter
{
    public sealed partial class Methods
    {
        public static void UnbanUser(long UID)
        {
            if (IsBanned(UID))
            {
                _ = Vars.CurrentConf.Banned.Remove(GetBanObjByID(UID));
            }
        }
    }
}
