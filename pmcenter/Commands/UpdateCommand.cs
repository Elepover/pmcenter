using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class UpdateCommand : ICommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "update";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Conf.Update Latest = Conf.CheckForUpdates();
                if (Conf.IsNewerVersionAvailable(Latest))
                {
                    string UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", Latest.Latest)
                        .Replace("$2", Latest.Details);
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        UpdateString, ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    // where difference begins
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateProcessing,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    // download compiled package
                    Log("Starting update download... (pmcenter_update.zip)", "BOT");
                    WebClient Downloader = new WebClient();
                    Downloader.DownloadFile(
                        new Uri(Vars.UpdateArchiveURL),
                        Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
                    Log("Download complete. Extracting...", "BOT");
                    using (ZipArchive Zip = ZipFile.OpenRead(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip")))
                    {
                        foreach (ZipArchiveEntry Entry in Zip.Entries)
                        {
                            Log("Extracting: " + Path.Combine(Vars.AppDirectory, Entry.FullName), "BOT");
                            Entry.ExtractToFile(Path.Combine(Vars.AppDirectory, Entry.FullName), true);
                        }
                    }
                    Log("Resting for a while...", "BOT");
                    Thread.Sleep(3000);
                    if (Vars.CurrentConf.AutoLangUpdate)
                    {
                        Log("Starting automatic language file update...", "BOT");
                        Downloader.DownloadFile(
                            new Uri(Vars.CurrentConf.LangURL),
                            Path.Combine(Vars.AppDirectory, "pmcenter_locale.json")
                        );
                    }
                    Log("Cleaning up temporary files...", "BOT");
                    System.IO.File.Delete(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateFinalizing,
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    Log("Resting for a while...", "BOT");
                    Thread.Sleep(3000);
                    Log("Trying to execute restart command...", "BOT");
                    try
                    {
                        Process.Start(Vars.CurrentConf.RestartCommand, Vars.CurrentConf.RestartArgs);
                        Log("Executed.", "BOT");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log("Failed to execute restart command: " + ex.ToString(), "BOT", LogLevel.ERROR);
                        return true;
                    }
                    // end of difference
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_AlreadyUpToDate
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Vars.AppVer.ToString())
                            .Replace("$3", Latest.Details),
                        ParseMode.Markdown,
                        false,
                        Vars.CurrentConf.DisableNotifications,
                        update.Message.MessageId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string ErrorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.Message);
                await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    ErrorString,
                    ParseMode.Markdown,
                    false,
                    Vars.CurrentConf.DisableNotifications,
                    update.Message.MessageId);
                return true;
            }
        }
    }
}
