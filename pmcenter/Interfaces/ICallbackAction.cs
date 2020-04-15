using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface ICallbackAction
    {
        string Name { get; }
        string ButtonName { get; }
        ICallbackAction Replacement { get; }
        Task<string> Action(User user, Message msg);
        bool IsAvailable(Update update);
    }
}
