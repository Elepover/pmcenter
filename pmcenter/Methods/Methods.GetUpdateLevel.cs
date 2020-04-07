using static pmcenter.Methods.UpdateHelper;

namespace pmcenter
{
    public partial class Methods
    {
        public static string GetUpdateLevel(UpdateLevel Level)
        {
            string Processed = Vars.CurrentLang.Message_SysStatus_UpdateLevel_Template;
            switch (Level)
            {
                case UpdateLevel.Optional:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Optional);
                case UpdateLevel.Recommended:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Recommended);
                case UpdateLevel.Important:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Important);
                case UpdateLevel.Urgent:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Urgent);
                default:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Unknown);
            }
        }
    }
}