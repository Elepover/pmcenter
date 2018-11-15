using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace pmcenter
{
    internal interface ICommand
    {
        bool OwnerOnly { get; }
        string Prefix { get; }
        Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update);
    }
}
