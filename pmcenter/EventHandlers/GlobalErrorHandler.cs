using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using static pmcenter.Methods;

namespace pmcenter
{
    public partial class EventHandlers
    {
        public static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs e)
        {
            L("[!] Critical error occurred, falling back to basic logging.");
            L("pmcenter's global error handler has captured a critical error.");
            L("If you believe that this is a bug, feel free to open an issue with the details below on pmcenter's GitHub repository at:");
            L("https://github.com/Elepover/pmcenter");
            L(e.IsTerminating ? "" : "This is not a catastrophic. pmcenter will keep running.\nIf you encounter more errors, please consider restarting pmcenter.");
            var exception = (Exception) e.ExceptionObject;
            var fileName = Path.Combine(Environment.CurrentDirectory, $"pmcenter-error-{GetDateTimeString(true)}.log");
            L($"Exception details: {exception.ToString()}");
            L($"Attempting to write to {fileName}");
            try
            {
                var writer = new StreamWriter(fileName, false, Encoding.UTF8) { AutoFlush = true };
                writer.WriteLine($"pmcenter critical error log @{GetDateTimeString()} (local time)");
                writer.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}\nSystem: {RuntimeInformation.OSDescription}");
                writer.WriteLine("If you believe that this is a bug, feel free to open an issue with the details below on pmcenter's GitHub repository at https://github.com/Elepover/pmcenter");
                writer.WriteLine($"==> HRESULT 0x{exception.HResult.ToString("x")} error details:");
                writer.WriteLine(exception.ToString());
                writer.Close();
            }
            catch (Exception ex)
            {
                L($"Unable to save error logs: {ex.ToString()}");
            }
            if (e.IsTerminating)
            {
                L("Since it's a catastrophic error, pmcenter will exit now.");
                Environment.Exit(Marshal.GetHRForException(exception));
            }
        }

        private static void L(string log)
        {
            if (string.IsNullOrEmpty(log)) return;
            Console.Error.WriteLine(log);
        }
    }
}
