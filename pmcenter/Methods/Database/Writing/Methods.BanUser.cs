using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static void BanUser(long UID)
        {
            if (!IsBanned(UID))
            {
                var Banned = new BanObj
                {
                    UID = UID
                };
                Vars.CurrentConf.Banned.Add(Banned);
            }
        }
    }
}
