using System.Threading.Tasks;
using Telegram.Bot.Types;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    internal class DisableContinuedChatAction : ICallbackAction
    {
        public string Name => "disablechat";
        public ICallbackAction Replacement => new ContinuedChatAction();

#pragma warning disable CS1998
        public async Task<string> Action(User user, Message msg)
#pragma warning restore CS1998
        {
            Log("Disabling continued conversation via actions...", "BOT");
            Vars.CurrentConf.ContChatTarget = -1;
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            return Vars.CurrentLang.Message_Action_ContChatDisabled;
        }
    }
}
