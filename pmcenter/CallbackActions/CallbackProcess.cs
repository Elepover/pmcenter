using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    public static class CallbackProcess
    {
        private static readonly CallbackManager callbackManager = new CallbackManager();

        static CallbackProcess()
        {
            callbackManager.RegisterAction(new BanAction());
            callbackManager.RegisterAction(new PardonAction());
            callbackManager.RegisterAction(new ContinuedChatAction());
            callbackManager.RegisterAction(new DisableContinuedChatAction());
        }

        /// <summary>
        /// Do callback with given arguments
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <param name="user">The user who initiated the action</param>
        /// <param name="msg">The corresponding message</param>
        /// <returns></returns>
        public static async Task<CallbackActionResult> DoCallback(string actionName, User user, Message msg)
        {
            try
            {
                var result = await callbackManager.Execute(actionName, user, msg);
                if (result == null)
                    Log($"Callback {actionName} from user {user.Id} cannot be found.", "BOT", LogLevel.WARN);
                return new CallbackActionResult(result, result != null);
            }
            catch (Exception ex)
            {
                Log($"Callback {actionName} could not be executed: {ex}", "BOT", LogLevel.ERROR);
                return new CallbackActionResult(Vars.CurrentLang.Message_Action_Error, false);
            }
        }

        public static List<List<InlineKeyboardButton>> GetAvailableButtons(Update update) => callbackManager.GetAvailableButtons(update);
    }
}
