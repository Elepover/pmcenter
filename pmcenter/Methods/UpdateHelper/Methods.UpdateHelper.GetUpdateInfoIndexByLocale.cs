using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public static int GetUpdateInfoIndexByLocale(Update2 Update, string Locale)
            {
                var indexes = new List<int>();
                for (int i = 0; i < Update.UpdateCollection.Count; i++)
                {
                    if (Update.UpdateCollection[i].LangCode.Contains(Locale)) indexes.Add(i);
                }
                if (indexes.Count == 0) return 0;
                foreach (var i in indexes)
                {
                    if (Update.UpdateCollection[i].UpdateChannel == Vars.CurrentConf.UpdateChannel) return i;
                }
                return indexes[0];
            }
        }
    }
}
