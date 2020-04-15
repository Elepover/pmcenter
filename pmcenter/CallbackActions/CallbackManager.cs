using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace pmcenter.CallbackActions
{
    internal class CallbackManager
    {
        private readonly List<ICallbackAction> callbacks = new List<ICallbackAction>();

        public CallbackManager()
        { }

        public ICallbackAction this[string prefix] => callbacks.FirstOrDefault(x => x.Name.StartsWith(prefix));

        public void RegisterAction(ICallbackAction action)
        {
            if (callbacks.Any(x => x.Name == action.Name))
                throw new ArgumentException($"An action named \"{action.Name}\" is already registered.", nameof(action));
            callbacks.Add(action);
        }

        public async Task<string> Execute(string actionName, User user, Message msg)
        {
            foreach (var action in callbacks)
            {
                if (action.Name == actionName)
                {
                    return await action.Action(user, msg);
                }
            }
            return null;
        }

        public List<List<InlineKeyboardButton>> GetAvailableButtons(Update update)
        {
            var result = new List<List<InlineKeyboardButton>>();
            foreach (var action in callbacks)
                if (action.IsAvailable(update))
                {
                    var oneLineKeyboard = new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton()
                        {
                            CallbackData = action.Name,
                            Text = action.ButtonName
                        }
                    };
                    result.Add(oneLineKeyboard);
                }
            return result;
        }
    }
}
