namespace pmcenter
{
    public partial class Methods
    {
        public static bool IsKeywordBanned(string Sentence)
        {
            if (Vars.CurrentConf.KeywordBanning != true) { return false; }

            foreach (string Blocked in Vars.CurrentConf.BannedKeywords)
            {
                if (Vars.CurrentConf.EnableRegex)
                {
                    if (IsRegexMatch(Sentence, Blocked)) { return true; }
                }
                else
                {
                    if (Sentence.Contains(Blocked)) { return true; }
                }
            }
            return false;
        }
    }
}