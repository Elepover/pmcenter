using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class Logging
        {
            public static void Log(string text, LogLevel type)
            {
                Log(text, "CORE", type);
            }
            public static void Log(string text,
                                   string module = "CORE",
                                   LogLevel type = LogLevel.Info,
                                   [CallerFilePath] string filePath = "file?",
                                   [CallerMemberName] string callerName = "method?",
                                   [CallerLineNumber] int lineNumber = 0)
            {
                if (Vars.CurrentConf?.IgnoredLogModules.Contains(module) == true) return;
                if (Vars.CurrentConf?.LowPerformanceMode == true) return;

                var file = $"/{(Path.GetFileName((Environment.OSVersion.Platform == PlatformID.Unix) ? filePath.Replace(@"\", "/") : filePath))}/{callerName}()@L{lineNumber}";
                var output = Vars.CurrentConf?.DisableTimeDisplay != true
                           ? $"[{DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}]"
                           : "";
                output += $"[{module}{(Vars.CurrentConf?.AdvancedLogging == true ? file : "")}]";
                output += LogTable[type].Prefix;
                output += text;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = LogTable[type].Color;
                LogTable[type].Func(output);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
