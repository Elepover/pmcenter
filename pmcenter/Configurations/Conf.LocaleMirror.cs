namespace pmcenter
{
    public partial class Conf
    {
        public class LocaleMirror
        {
            public LocaleMirror()
            {
                LocaleFileURL = "";
                LocaleCode = "";
                LocaleNameEng = "";
                LocaleNameNative = "";
            }
            public string LocaleFileURL { get; set; }
            public string LocaleCode { get; set; }
            public string LocaleNameEng { get; set; }
            public string LocaleNameNative { get; set; }
        }
    }
}
