namespace pmcenter
{
    public static partial class Conf
    {
        /// <summary>
        /// Switch 'forwarding' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchPaused()
        {
            if (Vars.CurrentConf.ForwardingPaused)
            {
                Vars.CurrentConf.ForwardingPaused = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.ForwardingPaused = true;
                return true;
            }
        }
    }
}
