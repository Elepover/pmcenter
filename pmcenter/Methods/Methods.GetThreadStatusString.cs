namespace pmcenter
{
    public partial class Methods
    {
        public static string GetThreadStatusString(ThreadStatus Status)
        {
            switch (Status)
            {
                case ThreadStatus.Working:
                    return Vars.CurrentLang.Message_ThreadStatus_Working;
                case ThreadStatus.Standby:
                    return Vars.CurrentLang.Message_ThreadStatus_Standby;
                case ThreadStatus.Stopped:
                    return Vars.CurrentLang.Message_ThreadStatus_Stopped;
                case ThreadStatus.Error:
                    return Vars.CurrentLang.Message_ThreadStatus_Error;
                default:
                    return Vars.CurrentLang.Message_ThreadStatus_Unknown;
            }
        }
    }
}