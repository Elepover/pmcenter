/*
// CommandLines.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Commandlines are processed here.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;

namespace pmcenter
{
    public static class CmdLineProcess
    {
        private static readonly CommandLineRouter cmdLineRouter = new CommandLineRouter();

        static CmdLineProcess()
        {
            cmdLineRouter.RegisterCommand(new CommandLines.HelpCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.InfoCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.NonServiceModeCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.SetupWizardCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.ResetCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.BackupCmdLine());
            cmdLineRouter.RegisterCommand(new CommandLines.UpdateCmdLine());
        }

        public static async Task RunCommand(string CommandLine)
        {
            _ = await cmdLineRouter.Execute(CommandLine).ConfigureAwait(false);
        }
    }
}
