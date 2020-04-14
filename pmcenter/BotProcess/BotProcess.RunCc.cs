using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task RunCc(Update update)
        {
            Log("Cc enabled, forwarding...", "BOT");
            foreach (long Id in Vars.CurrentConf.Cc)
            {
                Log($"Forwarding message to cc: {Id}", "BOT");
                try
                {
                    var ForwardedMessageCc = await Vars.Bot.ForwardMessageAsync(Id,
                                                                                                       update.Message.From.Id,
                                                                                                       update.Message.MessageId,
                                                                                                       Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
                    // check if forwarded from channels
                    if (update.Message.ForwardFrom == null && update.Message.ForwardFromChat != null)
                    {
                        // is forwarded from channel
                        _ = await Vars.Bot.SendTextMessageAsync(Id,
                                                            Vars.CurrentLang.Message_ForwarderNotReal
                                                                .Replace("$2", update.Message.From.Id.ToString())
                                                                .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                            ParseMode.Markdown,
                                                            false,
                                                            Vars.CurrentConf.DisableNotifications,
                                                            ForwardedMessageCc.MessageId).ConfigureAwait(false);
                    }
                    if (update.Message.ForwardFrom != null && update.Message.ForwardFromChat == null)
                    {
                        // is forwarded from chats
                        // check real message sender
                        if (update.Message.ForwardFrom.Id != update.Message.From.Id)
                        {
                            _ = await Vars.Bot.SendTextMessageAsync(Id,
                                                                Vars.CurrentLang.Message_ForwarderNotReal
                                                                    .Replace("$2", update.Message.From.Id.ToString())
                                                                    .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                                ParseMode.Markdown,
                                                                false,
                                                                Vars.CurrentConf.DisableNotifications,
                                                                ForwardedMessageCc.MessageId).ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Unable to forward message to cc: {Id}, reason: {ex.Message}", "BOT", LogLevel.ERROR);
                }
            }
        }
    }
}