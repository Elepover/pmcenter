/*
// BotProcess.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main processing logic of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using pmcenter.Commands;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter
{
    public static class BotProcess
    {
        private static readonly CommandManager commandManager = new CommandManager();

        static BotProcess()
        {
            commandManager.RegisterCommand(new StartCommand());
            commandManager.RegisterCommand(new HelpCommand());

            commandManager.RegisterCommand(new InfoCommand());
            commandManager.RegisterCommand(new BanCommand());
            commandManager.RegisterCommand(new PardonCommand());

            commandManager.RegisterCommand(new BackupConfCommand());
            commandManager.RegisterCommand(new BanIdCommand());
            commandManager.RegisterCommand(new CatConfigCommand());
            commandManager.RegisterCommand(new CheckUpdateCommand());
            commandManager.RegisterCommand(new DetectPermissionCommand());
            commandManager.RegisterCommand(new DonateCommand());
            commandManager.RegisterCommand(new EditConfCommand());
            commandManager.RegisterCommand(new PardonIdCommand());
            commandManager.RegisterCommand(new PerformCommand());
            commandManager.RegisterCommand(new PingCommand());
            commandManager.RegisterCommand(new ReadConfigCommand());
            commandManager.RegisterCommand(new ResetConfCommand());
            commandManager.RegisterCommand(new RestartCommand());
            commandManager.RegisterCommand(new SaveConfigCommand());
            commandManager.RegisterCommand(new StatusCommand());
            commandManager.RegisterCommand(new SwitchBwCommand());
            commandManager.RegisterCommand(new SwitchFwCommand());
            commandManager.RegisterCommand(new SwitchLangCommand());
            commandManager.RegisterCommand(new SwitchNotificationCommand());
            commandManager.RegisterCommand(new UpdateCommand());
            commandManager.RegisterCommand(new UptimeCommand());
        }

        public static async void OnUpdate(object sender, UpdateEventArgs e)
        {
            try
            {
                if (Vars.CurrentConf.DetailedMsgLogging)
                {
                    Log("OnUpdate() triggered: UpdType: " + e.Update.Type.ToString()
                        + " UpdID: " + e.Update.Id
                        + " ChatId: " + e.Update.Message.Chat.Id
                        + " Username: " + e.Update.Message.Chat.Username
                        + " FromID: " + e.Update.Message.From.Id
                        + " FromUsername: " + e.Update.Message.From.Username
                        , "BOT-DETAILED", LogLevel.INFO);
                }
                Update update = e.Update;
                if (update.Type != UpdateType.Message) { return; }
                if (update.Message.From.IsBot) { return; }
                if (update.Message.Chat.Type != ChatType.Private) { return; }

                if (IsBanned(update.Message.From.Id))
                {
                    Log("Restricting banned user from sending messages: " + update.Message.From.FirstName + " (@" + update.Message.From.Username + " / " + (long)update.Message.From.Id + ")", "BOT");
                    return;
                }

                if (update.Message.From.Id == Vars.CurrentConf.OwnerUID)
                {
                    await OwnerLogic(update);
                }
                else
                {
                    await UserLogic(update);
                }

            }
            catch (Exception ex)
            {
                Log("General error while processing incoming update: " + ex.ToString(), "BOT", LogLevel.ERROR);
                if (Vars.CurrentConf.CatchAllExceptions)
                {
                    try
                    {
                        await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, 
                                                        Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.ToString()),
                                                        ParseMode.Default,
                                                        false,
                                                        Vars.CurrentConf.DisableNotifications);
                    }
                    catch (Exception iEx)
                    {
                        Log("Failed to catch exception to owner: "+ iEx.ToString(), "BOT", LogLevel.ERROR);
                    }
                }
            }
        }

        private static async Task UserLogic(Update update)
        {
            // is user

            if (await commandManager.Execute(Vars.Bot, update))
            {
                return;
            }

            Log("Received message from " + "\"" + update.Message.From.FirstName + "\" (@" + update.Message.From.Username + " / " + (long)update.Message.From.Id + ")" + ", forwarding...", "BOT");

            if (Vars.CurrentConf.ForwardingPaused)
            {
                Log("Stopped: forwarding is currently paused.", "BOT", LogLevel.INFO);
                await Vars.Bot.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_UserServicePaused,
                                                    ParseMode.Markdown,
                                                    false,
                                                    false,
                                                    update.Message.MessageId);
                return;
            }

            // test text blacklist
            if (!string.IsNullOrEmpty(update.Message.Text) && IsKeywordBanned(update.Message.Text))
            {
                Log("Stopped: sentence contains blocked words.", "BOT", LogLevel.INFO);
                if (Vars.CurrentConf.KeywordAutoBan)
                {
                    BanUser(update.Message.From.Id);
                }
                return;
            }

            // process owner
            Log("Forwarding message to owner...", "BOT");
            Message ForwardedMessage = await Vars.Bot.ForwardMessageAsync(Vars.CurrentConf.OwnerUID,
                                                                          update.Message.From.Id,
                                                                          update.Message.MessageId,
                                                                          Vars.CurrentConf.DisableNotifications);
            // check for real message sender
            // check if forwarded from channels
            if (update.Message.ForwardFrom == null && update.Message.ForwardFromChat != null)
            {
                // is forwarded from channel
                await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                    Vars.CurrentLang.Message_ForwarderNotReal
                                                        .Replace("$2", update.Message.From.Id.ToString())
                                                        .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                    ParseMode.Markdown,
                                                    false,
                                                    Vars.CurrentConf.DisableNotifications,
                                                    ForwardedMessage.MessageId);
            }
            if (update.Message.ForwardFrom != null && update.Message.ForwardFromChat == null)
            {
                // is forwarded from chats
                if (update.Message.ForwardFrom.Id != update.Message.From.Id)
                {
                    await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                        Vars.CurrentLang.Message_ForwarderNotReal
                                                            .Replace("$2", update.Message.From.Id.ToString())
                                                            .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                        ParseMode.Markdown,
                                                        false,
                                                        Vars.CurrentConf.DisableNotifications,
                                                        ForwardedMessage.MessageId);
                }
            }
            
            // process cc
            if (Vars.CurrentConf.EnableCc)
            {
                await RunCC(update);
            }
            if (Vars.CurrentConf.EnableForwardedConfirmation)
            {
                await Vars.Bot.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_ForwardedToOwner,
                                                    ParseMode.Markdown,
                                                    false,
                                                    false,
                                                    update.Message.MessageId);
            }
            AddRateLimit(update.Message.From.Id);
        }

        private static async Task RunCC(Update update)
        {
            Log("Cc enabled, forwarding...", "BOT");
            foreach (long Id in Vars.CurrentConf.Cc)
            {
                Log("Forwarding message to cc: " + Id, "BOT");
                try
                {
                    Message ForwardedMessageCc = await Vars.Bot.ForwardMessageAsync(Id,
                                                                                                       update.Message.From.Id,
                                                                                                       update.Message.MessageId,
                                                                                                       Vars.CurrentConf.DisableNotifications);
                    // check if forwarded from channels
                    if (update.Message.ForwardFrom == null && update.Message.ForwardFromChat != null)
                    {
                        // is forwarded from channel
                        await Vars.Bot.SendTextMessageAsync(Id,
                                                            Vars.CurrentLang.Message_ForwarderNotReal
                                                                .Replace("$2", update.Message.From.Id.ToString())
                                                                .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                            ParseMode.Markdown,
                                                            false,
                                                            Vars.CurrentConf.DisableNotifications,
                                                            ForwardedMessageCc.MessageId);
                    }
                    if (update.Message.ForwardFrom != null && update.Message.ForwardFromChat == null)
                    {
                        // is forwarded from chats
                        // check real message sender
                        if (update.Message.ForwardFrom.Id != update.Message.From.Id)
                        {
                            await Vars.Bot.SendTextMessageAsync(Id,
                                                                Vars.CurrentLang.Message_ForwarderNotReal
                                                                    .Replace("$2", update.Message.From.Id.ToString())
                                                                    .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                                ParseMode.Markdown,
                                                                false,
                                                                Vars.CurrentConf.DisableNotifications,
                                                                ForwardedMessageCc.MessageId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Unable to forward message to cc: " + Id + ", reason: " + ex.Message, "BOT", LogLevel.ERROR);
                }
            }
        }

        private static async Task OwnerLogic(Update update)
        {
            if (update.Message.ReplyToMessage != null)
            {
                await OwnerReplying(update);
            }
            else
            {
                await OwnerCommand(update);
            }
        }

        private static async Task OwnerCommand(Update update)
        {
            // The owner is not even replying.
            // start or help command?
            if (await commandManager.Execute(Vars.Bot, update))
            {
                return;
            }

            await Vars.Bot.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_CommandNotReplying,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId);

        }

        private static async Task OwnerReplying(Update update)
        {
            if (update.Message.ReplyToMessage.ForwardFrom == null)
            {
                // The owner is replying to bot messages. (no forwardfrom)
                await Vars.Bot.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CommandNotReplyingValidMessage,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                return;
            }

            if (await commandManager.Execute(Vars.Bot, update))
            {
                return;
            }

            // Is replying, replying to forwarded message AND not command.
            if (Vars.CurrentConf.AnonymousForward)
            {
                await ForwardMessageAnonymously(update.Message.ReplyToMessage.ForwardFrom.Id, Vars.CurrentConf.DisableNotifications, update.Message);
            }
            else
            {
                await Vars.Bot.ForwardMessageAsync(
                    update.Message.ReplyToMessage.ForwardFrom.Id,
                    update.Message.Chat.Id,
                    update.Message.MessageId,
                    Vars.CurrentConf.DisableNotifications);
            }
            // Process locale.
            if (Vars.CurrentConf.EnableRepliedConfirmation)
            {
                string ReplyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                ReplyToMessage = ReplyToMessage.Replace("$1", "[" + update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + update.Message.ReplyToMessage.ForwardFrom.Username + ")](tg://user?id=" + update.Message.ReplyToMessage.ForwardFrom.Id + ")");
                await Vars.Bot.SendTextMessageAsync(update.Message.From.Id, ReplyToMessage, ParseMode.Markdown, false, false, update.Message.MessageId);
            }
            Log("Successfully passed owner's reply to " + update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + update.Message.ReplyToMessage.ForwardFrom.Username + " / " + update.Message.ReplyToMessage.ForwardFrom.Id + ")", "BOT");

        }
    }
}