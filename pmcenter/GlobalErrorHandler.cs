using System;
using System.Runtime.InteropServices;

namespace pmcenter
{
    public partial class Program
    {
        private static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs e)
        {
            L("[!] Critical error occurred, falling back to basic logging.");
            L("pmcenter's global error handler has captured a critical error.");
            L("If you believe that this is a bug, feel free to open an issue with the details below on pmcenter's GitHub repository at:");
            L("https://github.com/Elepover/pmcenter");
            L($"Is catastrophic? {(e.IsTerminating ? "yes" : "no")}.");
            var exception = (Exception) e.ExceptionObject;
            L($"Exception details: {exception.ToString()}");
            if (e.IsTerminating)
            {
                L("Since it's a catastrophic error, pmcenter will exit now.");
                Environment.Exit(Marshal.GetHRForException(exception));
            }
        }

        private static void L(string log)
        {
            Console.Error.WriteLine(log);
        }
    }
}