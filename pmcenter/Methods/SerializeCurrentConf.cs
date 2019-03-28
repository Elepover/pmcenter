using Newtonsoft.Json;

namespace pmcenter
{
    public partial class Methods
    {
        public static string SerializeCurrentConf()
        {
            return JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
        }
    }
}