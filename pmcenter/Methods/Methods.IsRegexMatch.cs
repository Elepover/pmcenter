using System;
using System.Text.RegularExpressions;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsRegexMatch(string source, string expression)
        {
            try
            {
                if (Regex.IsMatch(source, expression, RegexOptions.None)) return true;
                return false;
            }
            catch (Exception ex)
            {
                Log($"Regex match failed: {ex.Message}, did you use a wrong regex?", "BOT", LogLevel.Error);
                return false;
            }
        }
    }
}
