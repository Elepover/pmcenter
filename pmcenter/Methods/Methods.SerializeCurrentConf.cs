using Newtonsoft.Json;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public static string SerializeCurrentConf()
        {
            return JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
        }
    }
}
