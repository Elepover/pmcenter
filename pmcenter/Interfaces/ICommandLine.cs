/*
// ICmdLine.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Commandline interface.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;

namespace pmcenter
{
    internal interface ICommandLine
    {
        string Prefix { get; }
        bool ExitAfterExecution { get; }
        Task<bool> Process();
    }
}
