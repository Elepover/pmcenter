namespace pmcenter
{
    public static partial class Conf
    {
        public static string KillIllegalChars(string Input)
        {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
    }
}
