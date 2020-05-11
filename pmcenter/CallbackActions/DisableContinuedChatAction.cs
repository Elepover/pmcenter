using System.Threading.Tasks;
using Telegram.Bot.Types;
using static pmcenter.Methods.Logging;

namespace pmcenter.CallbackActions
{
    internal class DisableContinuedChatAction : ICallbackAction
    {
        public string Name => "disablechat";
        public string ButtonName => Vars.CurrentLang.Message_Action_StopChat;
        public ICallbackAction Replacement => new ContinuedChatAction();

#pragma warning disable CS1998
        public async Task<string> Action(User user, Message msg)
#pragma warning restore CS1998
        {
            Log("Disabling continued conversation via actions...", "BOT");
            Vars.CurrentConf.ContChatTarget = -1;
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            Log("Continued conversation disabled via actions.", "BOT");
            return Vars.CurrentLang.Message_Action_ContChatDisabled;
        }

        public bool IsAvailable(Update update) => Vars.CurrentConf.ContChatTarget != -1;
    }
}
