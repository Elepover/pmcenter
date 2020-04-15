namespace pmcenter
{
    public static partial class Methods
    {
        public static void UnbanUser(long uid)
        {
            if (IsBanned(uid))
            {
                _ = Vars.CurrentConf.Banned.Remove(GetBanObjByID(uid));
            }
        }
    }
}
