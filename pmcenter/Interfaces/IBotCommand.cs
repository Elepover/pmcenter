/*
// ICommand.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Command interface.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface IBotCommand
    {
        bool OwnerOnly { get; }
        string Prefix { get; }
        Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update);
    }
}
