namespace pmcenter
{
    public static partial class Conf
    {
        public static string KillIllegalChars(string input)
        {
            return input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
    }
}
