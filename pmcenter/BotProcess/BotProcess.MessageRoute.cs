using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task MessageRoute(object sender, UpdateEventArgs e)
        {
            var update = e.Update;
            if (update.Message == null) return;
            if (update.Message.From.IsBot) return;
            if (update.Message.Chat.Type != ChatType.Private) return;

            Vars.CurrentConf.Statistics.TotalMessagesReceived += 1;
            if (update.Message.From.Id == Vars.CurrentConf.OwnerUID)
            {
                await OwnerLogic(update).ConfigureAwait(false);
            }
            else
            {
                await UserLogic(update).ConfigureAwait(false);
            }
        }
    }
}
