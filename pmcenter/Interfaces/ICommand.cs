/*
// ICommand.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Command interface.
// Copyright (C) 2018 Genteure. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface ICommand
    {
        bool OwnerOnly { get; }
        string Prefix { get; }
        Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update);
    }
}
