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
        private static readonly CommandLineRouter CmdLineRouter = new CommandLineRouter();

        static CmdLineProcess()
        {
            CmdLineRouter.RegisterCommand(new CommandLines.HelpCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.InfoCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.NonServiceModeCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.SetupWizardCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.ResetCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.BackupCmdLine());
            CmdLineRouter.RegisterCommand(new CommandLines.UpdateCmdLine());
        }

        public static async Task RunCommand(string CommandLine)
        {
            _ = await CmdLineRouter.Execute(CommandLine).ConfigureAwait(false);
        }
    }
}
