namespace pmcenter
{
    public static partial class Methods
    {
        public static string GetThreadStatusString(ThreadStatus status)
        {
            return status switch
            {
                ThreadStatus.Working => Vars.CurrentLang.Message_ThreadStatus_Working,
                ThreadStatus.Standby => Vars.CurrentLang.Message_ThreadStatus_Standby,
                ThreadStatus.Stopped => Vars.CurrentLang.Message_ThreadStatus_Stopped,
                ThreadStatus.Error => Vars.CurrentLang.Message_ThreadStatus_Error,
                _ => Vars.CurrentLang.Message_ThreadStatus_Unknown,
            };
        }
    }
}
