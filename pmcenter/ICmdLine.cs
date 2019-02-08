/*
// ICmdLine.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Commandline interface.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface ICmdLine
    {
        string Prefix { get; }
        bool ExitAfterExecution { get; }
        Task<bool> Process();
    }
}
