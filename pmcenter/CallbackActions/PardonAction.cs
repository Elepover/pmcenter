using System.Threading.Tasks;
using Telegram.Bot.Types;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    internal class PardonAction : ICallbackAction
    {
        public string Name => "pardon";
        public string ButtonName => Vars.CurrentLang.Message_Action_Pardon;
        public ICallbackAction Replacement => new BanAction();

#pragma warning disable CS1998
        public async Task<string> Action(User user, Message msg)
#pragma warning restore CS1998
        {
            // attempt to pardon the user
            Log($"Attempting to pardon {user.Id} via callback actions...", "BOT");
            UnbanUser(user.Id);
            Log($"User {user.Id} pardoned via callback actions...", "BOT");
            return Vars.CurrentLang.Message_Action_Pardoned.Replace("$1", GetComposedUsername(user, false));
        }

        public bool IsAvailable(Update update) => IsBanned(update);
    }
}
