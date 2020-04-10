using static pmcenter.Methods.UpdateHelper;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public static string GetUpdateLevel(UpdateLevel Level)
        {
            string Processed = Vars.CurrentLang.Message_SysStatus_UpdateLevel_Template;
            return Level switch
            {
                UpdateLevel.Optional => Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Optional),
                UpdateLevel.Recommended => Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Recommended),
                UpdateLevel.Important => Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Important),
                UpdateLevel.Urgent => Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Urgent),
                _ => Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Unknown),
            };
        }
    }
}
