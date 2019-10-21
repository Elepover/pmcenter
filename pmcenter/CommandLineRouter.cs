/*
// CommandLineRouter.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Command line routing manager.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static pmcenter.Methods;

namespace pmcenter
{
    internal class CommandLineRouter
    {
        private const string globalPrefix = "--";

        private readonly List<ICmdLine> commands = new List<ICmdLine>();

        public CommandLineRouter()
        {
        }

        public ICmdLine this[string prefix] => commands.FirstOrDefault(command => command.Prefix == prefix);

        /// <summary>
        /// Add a command to manager.
        /// Throws <see cref="ArgumentException"/> when a command with same prefix exists.
        /// </summary>
        /// <param name="command">the command</param>
        /// <exception cref="ArgumentException"/>
        public void RegisterCommand(ICmdLine command)
        {
            if (commands.Any(x => x.Prefix == command.Prefix))
            { throw new ArgumentException($@"A commandline with prefix ""{ command.Prefix }"" already exists.", nameof(command)); }

            commands.Add(command);
        }

        /// <summary>
        /// Try to find matching commandline and execute it.
        /// </summary>
        /// <param name="cmdLine">Original full commandline.</param>
        /// <returns></returns>
        public async Task<bool> Execute(string cmdLine)
        {
            Log("Processing commandlines...", "CMD");
            foreach (ICmdLine cmdProcess in commands)
            {
                if (cmdLine.Contains(globalPrefix + cmdProcess.Prefix))
                {
                    Log("HIT: executing: " + globalPrefix + cmdProcess.Prefix, "CMD");
                    try
                    {
                        _ = await cmdProcess.Process().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Log("Exception while executing commandline: " + ex.ToString(), "CMD", LogLevel.ERROR);
                        Environment.Exit(1);
                    }
                    Log("Command finished.", "CMD");
                    if (cmdProcess.ExitAfterExecution)
                    {
                        Log("I am leaving...", "CMD");
                        Environment.Exit(0);
                    }
                    return true;
                }
            }
            Log("No matching commandline found, or no commandline specified.", "CMD");
            return false;
        }
    }
}