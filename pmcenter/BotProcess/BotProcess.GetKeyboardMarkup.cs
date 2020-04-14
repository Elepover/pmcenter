using pmcenter.CallbackActions;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static pmcenter.Methods;

namespace pmcenter
{
    public static partial class BotProcess
    {
        public static InlineKeyboardMarkup GetKeyboardMarkup(Update update)
        {
            var buttons = new List<InlineKeyboardButton>();
            bool isBanned = true;
            if (update.Type == UpdateType.CallbackQuery) isBanned = IsBanned(GetLinkByOwnerMsgID(update.CallbackQuery.Message.ReplyToMessage.MessageId).TGUser.Id);
            if (update.Type == UpdateType.Message) isBanned = IsBanned(update.Message.From.Id);
            if (isBanned)
                buttons.Add(new InlineKeyboardButton() { CallbackData = new PardonAction().Name, Text = Vars.CurrentLang.Message_Action_Pardon });
            else
                buttons.Add(new InlineKeyboardButton() { CallbackData = new BanAction().Name, Text = Vars.CurrentLang.Message_Action_Ban });
            if (Vars.CurrentConf.ContChatTarget == -1)
                buttons.Add(new InlineKeyboardButton() { CallbackData = new ContinuedChatAction().Name, Text = Vars.CurrentLang.Message_Action_Chat });
            else
                buttons.Add(new InlineKeyboardButton() { CallbackData = new DisableContinuedChatAction().Name, Text = Vars.CurrentLang.Message_Action_StopChat });

            return new InlineKeyboardMarkup(buttons);
        }
    }
}