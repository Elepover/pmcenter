using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using static pmcenter.Methods;

namespace pmcenter
{
    public static partial class EventHandlers
    {
        public static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs e)
        {
            void L(string log)
            {
                if (string.IsNullOrEmpty(log)) return;
                Console.Error.WriteLine(log);
            }
            L("[!] Critical error occurred, falling back to basic logging.");
            L("pmcenter's global error handler has captured a critical error.");
            L("If you believe that this is a bug, feel free to open an issue with the details below on pmcenter's GitHub repository at:");
            L("https://github.com/Elepover/pmcenter");
            var exception = e != null ? (Exception)e.ExceptionObject : new Exception("unknown error");
            var fileName = Path.Combine(Environment.CurrentDirectory, $"pmcenter-error-{GetDateTimeString(true)}.log");
            L($"Exception details: {exception}");
            L($"Attempting to write to {fileName}");
            try
            {
                var writer = new StreamWriter(fileName, false, Encoding.UTF8) { AutoFlush = true };
                writer.WriteLine($"pmcenter critical error log @{GetDateTimeString()} (local time)");
                writer.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}\nSystem: {RuntimeInformation.OSDescription}");
                writer.WriteLine("If you believe that this is a bug, feel free to open an issue with the details below on pmcenter's GitHub repository at https://github.com/Elepover/pmcenter");
                writer.WriteLine($"==> HRESULT 0x{exception.HResult:x} error details:");
                writer.WriteLine(exception.ToString());
                writer.Close();
            }
            catch (Exception ex)
            {
                L($"Unable to save error logs: {ex}");
            }
            Environment.Exit(Marshal.GetHRForException(exception));
        }
    }
}
