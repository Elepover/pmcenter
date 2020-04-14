using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface ICallbackAction
    {
        string Name { get; }
        ICallbackAction Replacement { get; }
        Task<string> Action(User user, Message msg);
    }
}
