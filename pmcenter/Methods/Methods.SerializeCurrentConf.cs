using Newtonsoft.Json;

namespace pmcenter
{
    public static partial class Methods
    {
        public static string SerializeCurrentConf()
        {
            return JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
        }
    }
}
