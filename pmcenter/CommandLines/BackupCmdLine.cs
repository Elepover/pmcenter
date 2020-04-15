using System;
using System.IO;
using System.Threading.Tasks;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.CommandLines
{
    internal class BackupCmdLine : ICommandLine
    {
        public string Prefix => "backup";
        public bool ExitAfterExecution => true;
        public Task<bool> Process()
        {
            Log("Backing up...", "CMD");
            var randomFilename = $"pmcenter.{DateTime.Now:yyyy-dd-M-HH-mm-ss}#{GetRandomString(6)}.json";
            randomFilename = Path.Combine(Vars.AppDirectory, randomFilename);
            File.Copy(Vars.ConfFile, randomFilename);
            Log($"Backup complete. Filename: {randomFilename}", "CMD");
            return Task.FromResult(true);
        }
    }
}
