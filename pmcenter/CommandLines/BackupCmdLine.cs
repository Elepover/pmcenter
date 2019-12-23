using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter.CommandLines
{
    internal class BackupCmdLine : ICmdLine
    {
        public string Prefix => "backup";
        public bool ExitAfterExecution => true;
        public Task<bool> Process()
        {
            Log("Backing up...", "CMD");
            var RandomFilename = $"pmcenter.{DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss")}#{GetRandomString(6)}.json";
            RandomFilename = Path.Combine(Vars.AppDirectory, RandomFilename);
            File.Copy(Vars.ConfFile, RandomFilename);
            Log($"Backup complete. Filename: {RandomFilename}", "CMD");
            return Task.FromResult(true);
        }
    }
}