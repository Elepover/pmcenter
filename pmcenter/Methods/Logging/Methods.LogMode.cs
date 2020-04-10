using System;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public sealed partial class Logging
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
