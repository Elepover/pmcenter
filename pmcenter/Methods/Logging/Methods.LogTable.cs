using System;
using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class Logging
        {
            public static Dictionary<LogLevel, LogMode> LogTable = new Dictionary<LogLevel, LogMode>()
            {
                {LogLevel.INFO, new LogMode() {Color = ConsoleColor.White, Prefix = "[INFO] ", Func = Console.WriteLine}},
                {LogLevel.WARN, new LogMode() {Color = ConsoleColor.Yellow, Prefix = "[WARN] ", Func = Console.WriteLine}},
                {LogLevel.ERROR, new LogMode() {Color = ConsoleColor.Red, Prefix = "[ERROR] ", Func = Console.Error.WriteLine}}
            };
        }
    }
}
