using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class DetectPermissionCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "detectperm";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            var confWritable = FlipBool((new FileInfo(Vars.ConfFile)).IsReadOnly);
            var langWritable = FlipBool((new FileInfo(Vars.LangFile)).IsReadOnly);
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfAccess
                    .Replace("$1", BoolStr(confWritable))
                    .Replace("$2", BoolStr(langWritable))
                ,
                ParseMode.Markdown,
                false,
                Vars.CurrentConf.DisableNotifications,
                update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
