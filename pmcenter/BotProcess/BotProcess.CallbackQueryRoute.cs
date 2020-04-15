using pmcenter.CallbackActions;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task CallbackQueryRoute(object sender, UpdateEventArgs e)
        {
            var update = e.Update;
            if (update.CallbackQuery == null) return;
            if (update.CallbackQuery.From.IsBot) return;
            try
            {
                var result = await CallbackProcess.DoCallback(update.CallbackQuery.Data, GetLinkByOwnerMsgID(update.CallbackQuery.Message.ReplyToMessage.MessageId).TGUser, update.CallbackQuery.Message);
                // prompt
                await Vars.Bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, result.Status);
                // update existing buttons
                if (result.Succeeded)
                    _ = await Vars.Bot.EditMessageReplyMarkupAsync(
                        update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        new InlineKeyboardMarkup(CallbackProcess.GetAvailableButtons(update)));
            }
            catch (Exception ex)
            {
                Log($"Unable to process action {update.CallbackQuery.Data}: {ex}", "BOT", LogLevel.Error);
            }
        }
    }
}
