using System.Collections.Generic;
using System.Linq;

namespace pmcenter
{
    public static partial class Methods
    {
        public static List<string> StrChunk(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }
    }
}
