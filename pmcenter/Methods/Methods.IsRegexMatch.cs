using System;
using System.Text.RegularExpressions;

namespace pmcenter
{
    public partial class Methods
    {
        public static bool IsRegexMatch(string Source, string Expression)
        {
            try
            {
                if (Regex.IsMatch(Source, Expression, RegexOptions.None))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"Regex match failed: {ex.Message}, did you use a wrong regex?", "BOT", LogLevel.ERROR);
                return false;
            }
        }
    }
}