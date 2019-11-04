using System.Threading.Tasks;

namespace pmcenter
{
    public partial class Conf
    {
        public static async Task<bool> ReadConf(bool Apply = true)
        { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            var Temp = await GetConf(Vars.ConfFile).ConfigureAwait(false);
            if (Apply) { Vars.CurrentConf = Temp; }
            return true;
        }
    }
}