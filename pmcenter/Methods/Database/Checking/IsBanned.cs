using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static bool IsBanned(long UID)
        {
            foreach (BanObj Banned in Vars.CurrentConf.Banned)
            {
                if (Banned.UID == UID) { return true; }
            }
            return false;
        }
    }
}