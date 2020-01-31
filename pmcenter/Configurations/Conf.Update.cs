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
                UpdateChannel = "master";
                UpdateArchiveAddress = "https://see.wtf/pmcenter-update";
            }
            public string Details;
            public List<string> LangCode;
            public string UpdateChannel;
            public string UpdateArchiveAddress;
        }
    }
}