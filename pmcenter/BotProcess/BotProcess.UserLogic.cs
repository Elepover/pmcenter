using pmcenter.CallbackActions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task UserLogic(Update update)
        {
            // is user

            if (await commandManager.Execute(Vars.Bot, update).ConfigureAwait(false))
            {
                return;
            }

            Log($"Received message from \"{update.Message.From.FirstName}\" (@{update.Message.From.Username} / {update.Message.From.Id}), forwarding...", "BOT");

            if (Vars.CurrentConf.ForwardingPaused)
            {
                Log("Stopped: forwarding is currently paused.", "BOT", LogLevel.INFO);
                _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_UserServicePaused,
                                                    ParseMode.Markdown,
                                                    false,
                                                    false,
                                                    update.Message.MessageId).ConfigureAwait(false);
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
            var ForwardedMessage = await Vars.Bot.ForwardMessageAsync(Vars.CurrentConf.OwnerUID,
                                                                          update.Message.From.Id,
                                                                          update.Message.MessageId,
                                                                          Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
            Vars.CurrentConf.Statistics.TotalForwardedToOwner += 1;
            // preprocess message link
            var link = new Conf.MessageIDLink()
            {
                OwnerSessionMessageID = ForwardedMessage.MessageId,
                UserSessionMessageID = update.Message.MessageId,
                TGUser = update.Message.From,
                IsFromOwner = false
            };
            // process actions
            if (Vars.CurrentConf.EnableActions && Vars.CurrentConf.EnableMsgLink)
            {
                var markup = GetKeyboardMarkup(update);
                link.OwnerSessionActionMessageID = (await Vars.Bot.SendTextMessageAsync(
                    Vars.CurrentConf.OwnerUID,
                    Vars.CurrentLang.Message_Action_ChooseAction,
                    ParseMode.Markdown,
                    false,
                    true,
                    ForwardedMessage.MessageId,
                    markup
                    ).ConfigureAwait(false)).MessageId;
            }
            // process message links
            if (Vars.CurrentConf.EnableMsgLink)
            {
                Log($"Recording message link: owner {ForwardedMessage.MessageId} <-> user {update.Message.MessageId} user: {update.Message.From.Id}", "BOT");
                Vars.CurrentConf.MessageLinks.Add(link);
                // Conf.SaveConf(false, true);
            }
            // check for real message sender
            // check if forwarded from channels
            if (update.Message.ForwardFrom == null && update.Message.ForwardFromChat != null)
            {
                // is forwarded from channel
                _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                    Vars.CurrentLang.Message_ForwarderNotReal
                                                        .Replace("$2", update.Message.From.Id.ToString())
                                                        .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                    ParseMode.Markdown,
                                                    false,
                                                    Vars.CurrentConf.DisableNotifications,
                                                    ForwardedMessage.MessageId).ConfigureAwait(false);
            }
            if (update.Message.ForwardFrom != null && update.Message.ForwardFromChat == null)
            {
                // is forwarded from chats
                if (update.Message.ForwardFrom.Id != update.Message.From.Id)
                {
                    _ = await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                        Vars.CurrentLang.Message_ForwarderNotReal
                                                            .Replace("$2", update.Message.From.Id.ToString())
                                                            .Replace("$1", "[" + update.Message.From.FirstName + " " + update.Message.From.LastName + "](tg://user?id=" + update.Message.From.Id + ")"),
                                                        ParseMode.Markdown,
                                                        false,
                                                        Vars.CurrentConf.DisableNotifications,
                                                        ForwardedMessage.MessageId).ConfigureAwait(false);
                }
            }

            // process cc
            if (Vars.CurrentConf.EnableCc)
            {
                await RunCc(update).ConfigureAwait(false);
            }
            if (Vars.CurrentConf.EnableForwardedConfirmation)
            {
                _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id,
                                                    Vars.CurrentLang.Message_ForwardedToOwner,
                                                    ParseMode.Markdown,
                                                    false,
                                                    false,
                                                    update.Message.MessageId).ConfigureAwait(false);
            }
            AddRateLimit(update.Message.From.Id);
        }
    }
}