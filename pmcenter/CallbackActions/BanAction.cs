using System.Threading.Tasks;
using Telegram.Bot.Types;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    internal class BanAction : ICallbackAction
    {
        public string Name => "ban";
        public ICallbackAction Replacement => new PardonAction();

#pragma warning disable CS1998
        public async Task<string> Action(User user, Message msg)
#pragma warning restore CS1998
        {
            // attempt to ban the user
            Log($"Attempting to ban user {user.Id} via callback actions...", "BOT");
            BanUser(user.Id);
            return Vars.CurrentLang.Message_Action_Banned.Replace("$1", GetComposedUsername(user, false));
        }
    }
}
