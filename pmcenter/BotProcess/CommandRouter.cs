/*
// CommandManager.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Command routing processor.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter
{
    internal class CommandRouter
    {
        private const char globalPrefix = '/';

        private readonly List<IBotCommand> commands = new List<IBotCommand>();

        public CommandRouter()
        {
        }

        public IBotCommand this[string prefix] => commands.FirstOrDefault(command => command.Prefix == prefix);

        /// <summary>
        /// Add a command to manager.
        /// Throws <see cref="ArgumentException"/> when a command with same prefix exists.
        /// </summary>
        /// <param name="command">the command</param>
        /// <exception cref="ArgumentException"/>
        public void RegisterCommand(IBotCommand command)
        {
            if (commands.Any(x => x.Prefix == command.Prefix))
            { throw new ArgumentException($"A command with prefix \"{ command.Prefix }\" already exists.", nameof(command)); }

            commands.Add(command);
        }

        /// <summary>
        /// Try run a message as command
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <returns>processed by one command</returns>
        public async Task<bool> Execute(TelegramBotClient botClient, Update update)
        {
            if (update.Message.Type != MessageType.Text) return false;
            if (!update.Message.Text.StartsWith(globalPrefix)) return false;

            var command = commands.FirstOrDefault(cmd =>
            {
                // can run = cmd is not owner only command, OR sender id equals owner id
                bool adminRunnable = !cmd.OwnerOnly || (update.Message.From.Id == Vars.CurrentConf.OwnerUID);

                // can run = remove global prefix fist, then if starts with cmd.Prefix
                bool prefixMatch = update.Message.Text.TrimStart(globalPrefix).StartsWith(cmd.Prefix, StringComparison.InvariantCultureIgnoreCase);

                return adminRunnable && prefixMatch;
            });

            if (command == null)
            {
                return false;
            }
            else
            {
                return await command.ExecuteAsync(botClient, update).ConfigureAwait(false);
            }
        }
    }
}
