using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace pmcenter
{
    public sealed partial class Methods
    {
        public sealed partial class Logging
        {
            public static void Log(string text, LogLevel type)
            {
                Log(text, "CORE", type);
            }
            public static void Log(string text,
                                   string module = "CORE",
                                   LogLevel type = LogLevel.INFO,
                                   [CallerFilePath] string filePath = "file?",
                                   [CallerMemberName] string callerName = "method?",
                                   [CallerLineNumber] int lineNumber = 0)
            {
                if (Vars.CurrentConf?.LowPerformanceMode == true) return;

                var file = $"/{(Path.GetFileName((Environment.OSVersion.Platform == PlatformID.Unix) ? filePath.Replace(@"\", "/") : filePath))}/{callerName}()@L{lineNumber}";
                var Output = Vars.CurrentConf?.DisableTimeDisplay != true
                           ? $"[{DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}]"
                           : "";
                Output += $"[{module}{(Vars.CurrentConf?.AdvancedLogging == true ? file : "")}]";
                Output += LogTable[type].Prefix;
                Output += text;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = LogTable[type].Color;
                LogTable[type].Func(Output);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
