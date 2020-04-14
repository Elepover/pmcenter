namespace pmcenter
{
    public static partial class Conf
    {
        /// <summary>
        /// Switch 'disablenotifications' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchNotifications()
        {
            if (Vars.CurrentConf.DisableNotifications)
            {
                Vars.CurrentConf.DisableNotifications = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.DisableNotifications = true;
                return true;
            }
        }
    }
}
