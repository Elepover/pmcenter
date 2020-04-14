using System.Threading.Tasks;
using Telegram.Bot.Types;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    internal class ContinuedChatAction : ICallbackAction
    {
        public string Name => "chat";
        public ICallbackAction Replacement => new DisableContinuedChatAction();

#pragma warning disable CS1998
        public async Task<string> Action(User user, Message msg)
#pragma warning restore CS1998
        {
            Log("Enabling continued conversation via actions...", "BOT");
            Vars.CurrentConf.ContChatTarget = user.Id;
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            return Vars.CurrentLang.Message_Action_ContChatEnabled.Replace("$1", GetComposedUsername(user, false));
        }
    }
}
