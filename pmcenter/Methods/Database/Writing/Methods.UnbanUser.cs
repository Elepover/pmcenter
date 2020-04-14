namespace pmcenter
{
    public static partial class Methods
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
