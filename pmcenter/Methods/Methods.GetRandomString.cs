using System;
using System.Linq;

namespace pmcenter
{
    public static partial class Methods
    {
        public static string GetRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[(new Random()).Next(s.Length)]).ToArray());
        }
    }
}
