using System;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class Logging
        {
            public class LogMode
            {
                public ConsoleColor Color;
                public string Prefix;
                public Action<string> Func;
            }
        }
    }
}
