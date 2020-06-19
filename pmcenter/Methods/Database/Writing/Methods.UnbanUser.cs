namespace pmcenter
{
    public static partial class Methods
    {
        public static void UnbanUser(long uid)
        {
            if (IsBanned(uid))
            {
                var banObj = GetBanObjByID(uid);
                if (banObj != null) _ = Vars.CurrentConf.Banned.Remove(banObj);
            }
        }
    }
}
