using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
    {
        public class Update
        {
            public Update()
            {
                Details = "(Load failed.)";
                LangCode = new List<string>() { "en.integrated" };
            }
            public string Details { get; set; }
            public List<string> LangCode { get; set; }
        }
    }
}