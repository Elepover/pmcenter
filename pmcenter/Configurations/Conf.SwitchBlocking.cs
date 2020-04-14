namespace pmcenter
{
    public static partial class Conf
    {
        /// <summary>
        /// Switch 'blocking' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchBlocking()
        {
            if (Vars.CurrentConf.KeywordBanning)
            {
                Vars.CurrentConf.KeywordBanning = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.KeywordBanning = true;
                return true;
            }
        }
    }
}
