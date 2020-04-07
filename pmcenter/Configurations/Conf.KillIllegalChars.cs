namespace pmcenter
{
    public partial class Conf
    {
        public static string KillIllegalChars(string Input)
        {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
    }
}
