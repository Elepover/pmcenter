using static pmcenter.Conf;

namespace pmcenter
{
    public partial class Methods
    {
        public static BanObj GetBanObjByID(long UID)
        {
            foreach (BanObj Banned in Vars.CurrentConf.Banned)
            {
                if (Banned.UID == UID) { return Banned; }
            }
            return null;
        }
    }
}