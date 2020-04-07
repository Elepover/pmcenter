using System;

namespace pmcenter
{
    public partial class Methods
    {
        public class LogMode
        {
            public ConsoleColor Color;
            public string Prefix;
            public Action<string> Func;
        }
    }
}
