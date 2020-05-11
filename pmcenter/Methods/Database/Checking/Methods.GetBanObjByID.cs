using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static BanObj GetBanObjByID(long uid)
        {
            foreach (var banned in Vars.CurrentConf.Banned)
            {
                if (banned.UID == uid) return banned;
            }
            return null;
        }
    }
}
