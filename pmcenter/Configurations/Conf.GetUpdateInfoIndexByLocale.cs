using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
    {
        public static int GetUpdateInfoIndexByLocale(Update2 Update, string Locale)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < Update.UpdateCollection.Count; i++)
            {
                if (Update.UpdateCollection[i].LangCode.Contains(Locale)) indexes.Add(i);
            }
            if (indexes.Count == 0) return 0;
            foreach (int i in indexes)
            {
                if (Update.UpdateCollection[i].UpdateChannel == Vars.CurrentConf.UpdateChannel) return i;
            }
            return indexes[0];
        }
    }
}