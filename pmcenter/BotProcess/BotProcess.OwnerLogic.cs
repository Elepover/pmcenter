using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task OwnerLogic(Update update)
        {
            if (update.Message.ReplyToMessage != null)
            {
                await OwnerReplying(update).ConfigureAwait(false);
            }
            else
            {
                await OwnerCommand(update).ConfigureAwait(false);
            }
        }
    }
}
