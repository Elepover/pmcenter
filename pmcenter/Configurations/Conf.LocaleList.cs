using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Conf
    {
        public class LocaleList
        {
            public LocaleList()
            {
                Locales = new List<LocaleMirror>();
            }
            public List<LocaleMirror> Locales { get; set; }
        }
    }
}
