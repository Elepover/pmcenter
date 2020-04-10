namespace pmcenter
{
    public sealed partial class Conf
    {
        public static string KillIllegalChars(string Input)
        {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
    }
}
