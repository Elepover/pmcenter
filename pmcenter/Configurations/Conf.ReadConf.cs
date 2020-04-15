using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Conf
    {
        public static async Task<bool> ReadConf(bool apply = true)
        { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            var temp = await GetConf(Vars.ConfFile).ConfigureAwait(false);
            if (apply) { Vars.CurrentConf = temp; }
            return true;
        }
    }
}
