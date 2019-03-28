using System.Linq;

namespace pmcenter
{
    public partial class Methods
    {
        public static string GetRandomString(int Length = 8)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(Chars, Length).Select(s => s[(new Random()).Next(s.Length)]).ToArray());
        }
    }
}