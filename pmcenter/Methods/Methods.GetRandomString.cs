using System;
using System.Linq;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public static string GetRandomString(int length = 8)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(Chars, length).Select(s => s[(new Random()).Next(s.Length)]).ToArray());
        }
    }
}
