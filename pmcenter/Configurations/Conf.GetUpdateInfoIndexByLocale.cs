namespace pmcenter
{
    public partial class Conf
    {
        public static int GetUpdateInfoIndexByLocale(Update2 Update, string Locale)
        {
            for (int i = 0; i < Update.UpdateCollection.Count; i++)
            {
                if (Update.UpdateCollection[i].LangCode.Contains(Locale)) return i;
            }
            return 0;
        }
    }
}