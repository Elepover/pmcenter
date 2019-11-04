using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
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