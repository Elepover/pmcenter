using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace pmcenter
{
    public partial class Methods
    {
        public static void Log(string Text,
                               string Module = "CORE",
                               LogLevel Type = LogLevel.INFO,
                               [CallerFilePath] string filePath = "file?",
                               [CallerMemberName] string callerName = "method?",
                               [CallerLineNumber] int lineNumber = 0)
        {
            if (Vars.CurrentConf?.LowPerformanceMode == true) return;
            string file = $"/{((Environment.OSVersion.Platform == PlatformID.Unix) ? Path.GetFileName(filePath.Replace(@"\", "/")) : Path.GetFileName(filePath))}/{callerName}()@L{lineNumber}";
            var Output = 
                (Vars.CurrentConf?.DisableTimeDisplay != true ?
                        $"[{DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}]"
                        :
                        ""
                )
                + "["
                + Module
                + 
                (Vars.CurrentConf?.AdvancedLogging == true ?
                    file
                    :
                    ""
                )
                + "]";
            Console.BackgroundColor = ConsoleColor.Black;
            switch (Type)
            {
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Output += "[INFO] "; // Spacebars between prefixes and contents were already added here.
                    break;
                case LogLevel.WARN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Output += "[WARN] ";
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Output += "[ERROR] ";
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Output += "[UKNN] ";
                    break;
            }
            Output += Text;
            if (Type == LogLevel.ERROR)
            {
                Console.Error.WriteLine(Output);
            }
            else
            {
                Console.WriteLine(Output);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}