using static pmcenter.Methods.UpdateHelper;

namespace pmcenter
{
    public static partial class Methods
    {
        public static string GetUpdateLevel(UpdateLevel level)
        {
            string processed = Vars.CurrentLang.Message_SysStatus_UpdateLevel_Template;
            return level switch
            {
                UpdateLevel.Optional => processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Optional),
                UpdateLevel.Recommended => processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Recommended),
                UpdateLevel.Important => processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Important),
                UpdateLevel.Urgent => processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Urgent),
                _ => processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Unknown),
            };
        }
    }
}
