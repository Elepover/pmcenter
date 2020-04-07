using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace pmcenter
{
    public partial class Methods
    {
        public static Dictionary<LogLevel, LogMode> LogTable = new Dictionary<LogLevel, LogMode>()
        {
            {LogLevel.INFO, new LogMode() {Color = ConsoleColor.White, Prefix = "[INFO] ", Func = Console.WriteLine}},
            {LogLevel.WARN, new LogMode() {Color = ConsoleColor.Yellow, Prefix = "[WARN] ", Func = Console.WriteLine}},
            {LogLevel.ERROR, new LogMode() {Color = ConsoleColor.Red, Prefix = "[ERROR] ", Func = Console.Error.WriteLine}}
        }; // table-driven

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
            Console.ForegroundColor = LogTable[type].Color;
            Console.BackgroundColor = ConsoleColor.Black;
            LogTable[type].Func(Output);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
