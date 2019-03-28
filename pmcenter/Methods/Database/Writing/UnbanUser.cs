namespace pmcenter
{
    public partial class Methods
    {
        public static void UnbanUser(long UID)
        {
            if (IsBanned(UID))
            {
                Vars.CurrentConf.Banned.Remove(GetBanObjByID(UID));
            }
        }
    }
}