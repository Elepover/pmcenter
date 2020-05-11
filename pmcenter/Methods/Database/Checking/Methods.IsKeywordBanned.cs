namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsKeywordBanned(string sentence)
        {
            if (!Vars.CurrentConf.KeywordBanning) return false;

            foreach (var blocked in Vars.CurrentConf.BannedKeywords)
            {
                if (Vars.CurrentConf.EnableRegex)
                {
                    if (IsRegexMatch(sentence, blocked)) return true;
                }
                else
                {
                    if (sentence.Contains(blocked)) return true;
                }
            }
            return false;
        }
    }
}
