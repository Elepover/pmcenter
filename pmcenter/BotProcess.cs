/*
// BotProcess.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Main processing logic of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using Telegram.Bot.Args;
using static pmcenter.Methods;

namespace pmcenter {
    public class BotProcess {
        public static async void OnUpdate(object sender, UpdateEventArgs e) {
            if (e.Update.Type != UpdateType.Message) { return; }
            if (e.Update.Message.From.IsBot) { return; }
            if (e.Update.Message.Chat.Type != ChatType.Private) { return; }
            
            string Username = e.Update.Message.From.Username;
            string FirstName = e.Update.Message.From.FirstName;
            long UID = e.Update.Message.From.Id;
            if (IsBanned(e.Update.Message.From.Id)) {
                string WaitToProcess = Vars.CurrentLang.CLI_UserBlocked;
                WaitToProcess = WaitToProcess.Replace("$1", FirstName + " (@" + Username + " / " + UID + ")");
                Log(WaitToProcess, "BOT");
                return;
            }
            // Reworked processing logic.
            if (e.Update.Message.From.Id == Vars.CurrentConf.OwnerUID) {
                // Commands?
                if (e.Update.Message.ReplyToMessage != null) {
                    if (e.Update.Message.ReplyToMessage.ForwardFrom != null) {
                        if (e.Update.Message.Type == MessageType.Text) {
                            if (e.Update.Message.Text.ToLower() == "/info") {
                                string MessageInfo = "ℹ *Message Info*\n📩 *Sender*: [Here](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")\n🔢 *User ID*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + "`\n🌐 *Language*: `" + e.Update.Message.ReplyToMessage.ForwardFrom.LanguageCode + "`\n⌚ *Forward Time*: `" + e.Update.Message.ReplyToMessage.ForwardDate.ToString() + "`\n🆔 *Message ID*: `" + e.Update.Message.MessageId + "`";
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, MessageInfo, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else if (e.Update.Message.Text.ToLower() == "/ban") {
                                BanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Conf.SaveConf(false, true);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserBanned, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else if (e.Update.Message.Text.ToLower() == "/pardon") {
                                UnbanUser(e.Update.Message.ReplyToMessage.ForwardFrom.Id);
                                await Conf.SaveConf(false, true);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserPardoned, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } // not a recogized command.
                        }
                        // Is replying, replying to forwarded message AND not command.
                        await Vars.Bot.ForwardMessageAsync(e.Update.Message.ReplyToMessage.ForwardFrom.Id, e.Update.Message.Chat.Id, e.Update.Message.MessageId, false);
                        // Process locale.
                        string ReplyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                        ReplyToMessage = ReplyToMessage.Replace("$1", "[" + e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + ")](tg://user?id=" + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")");
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, ReplyToMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        string WaitToProcess = Vars.CurrentLang.CLI_OwnerMessageForwarded;
                        WaitToProcess = WaitToProcess.Replace("$1", e.Update.Message.ReplyToMessage.ForwardFrom.FirstName + " (@" + e.Update.Message.ReplyToMessage.ForwardFrom.Username + " / " + e.Update.Message.ReplyToMessage.ForwardFrom.Id + ")");
                        Log(WaitToProcess, "BOT");
                        return;
                    } else {
                        // The owner is replying to bot messages. (no forwardfrom)
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_CommandNotReplyingValidMessage, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        return;
                    }
                } else {
                    // The owner is not even replying.
                    // start or help command?
                    if (e.Update.Message.Type == MessageType.Text) {
                        if (e.Update.Message.Text.ToLower() == "/start") {
                            await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_OwnerStart, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/help") {
                            await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_Help, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/ping") {
                            await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_PingReply, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/switchfw") {
                            bool IsPausedNow = Conf.SwitchPaused();
                            await Conf.SaveConf(false, true);
                            if (IsPausedNow) {
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_ServicePaused, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else {
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_ServiceResumed, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            }
                        } else if (e.Update.Message.Text.ToLower() == "/switchbw") {
                            bool IsEnabledNow = Conf.SwitchBlocking();
                            await Conf.SaveConf(false, true);
                            if (IsEnabledNow) {
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_MessageBlockEnabled, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } else {
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_MessageBlockDisabled, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            }
                        } else if (e.Update.Message.Text.ToLower() == "/saveconf") {
                            await Conf.SaveConf();
                            await Lang.SaveLang();
                            await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_ConfigUpdated, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/readconf") {
                            await Conf.ReadConf();
                            await Lang.ReadLang();
                            await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_ConfigReloaded, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/uptime") {
                            string UptimeString = Vars.CurrentLang.Message_UptimeInfo;
                            UptimeString = UptimeString.Replace("$1", (new TimeSpan(0, 0, 0, 0, Environment.TickCount)).ToString()).Replace("$2", Vars.StartSW.Elapsed.ToString());
                            await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, UptimeString, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                            return;
                        } else if (e.Update.Message.Text.ToLower() == "/chkupdate") {
                            try {
                                Conf.Update Latest = Conf.CheckForUpdates();
                                if (Conf.IsNewerVersionAvailable(Latest)) {
                                    string UpdateString = Vars.CurrentLang.Message_UpdateAvailable.Replace("$1", Latest.Latest).Replace("$2", Latest.Details);
                                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, UpdateString, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                    return;
                                } else {
                                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_AlreadyUpToDate, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                    return;
                                }
                            } catch (Exception ex) {
                                string ErrorString = Vars.CurrentLang.Message_AlreadyUpToDate.Replace("$1", ex.Message);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, ErrorString, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            }
                        } else if (e.Update.Message.Text.ToLower() == "/update") {
                            // Copied code starting at L115
                            try {
                                Conf.Update Latest = Conf.CheckForUpdates();
                                if (Conf.IsNewerVersionAvailable(Latest)) {
                                    string UpdateString = Vars.CurrentLang.Message_UpdateAvailable.Replace("$1", Latest.Latest).Replace("$2", Latest.Details);
                                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, UpdateString, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                    // where difference begins
                                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UpdateProcessing, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                    // download compiled package
                                    Log("Starting update download...", "BOT");
                                    WebClient Downloader = new WebClient();
                                    Downloader.DownloadFileAsync(new Uri(Vars.UpdateArchiveURL), Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
                                    Log("Download complete. Extracting...", "BOT");
                                    ZipFile.ExtractToDirectory(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"), Vars.AppDirectory);
                                    Log("Cleaning up temporary files...", "BOT");
                                    File.Delete(Path.Combine(Vars.AppDirectory, "pmcenter_update.zip"));
                                    Log("Trying to execute restart command...", "BOT");
                                    try {
                                        Process.Start(Vars.CurrentConf.RestartCommand);
                                        Log("Executed.", "BOT");
                                        return;
                                    } catch (Exception ex) {
                                        Log("Failed to execute restart command: " + ex.ToString(), "BOT", LogLevel.ERROR);
                                        return;
                                    }
                                    // end of difference
                                } else {
                                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_AlreadyUpToDate, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                    return;
                                }
                            } catch (Exception ex) {
                                string ErrorString = Vars.CurrentLang.Message_AlreadyUpToDate.Replace("$1", ex.Message);
                                await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, ErrorString, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                                return;
                            } // end of try, not end if
                        } // not a command.
                    }
                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_CommandNotReplying, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                    return;
                }
            } else {
                // is user
                if (e.Update.Message.Type == MessageType.Text) {
                    // is command?
                    if (e.Update.Message.Text.ToLower() == "/start") {
                        await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserStartDefault, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                        return;
                    }
                }
                string WaitToProcess = Vars.CurrentLang.CLI_UserMessageReceived;
                WaitToProcess = WaitToProcess.Replace("$1", "\"" + FirstName + "\" (@" + Username + " / " + UID + ")");
                Log(WaitToProcess, "BOT");
                if (Vars.CurrentConf.ForwardingPaused) {
                    Log(Vars.CurrentLang.CLI_ForwardingPaused, "BOT", LogLevel.INFO);
                    await Vars.Bot.SendTextMessageAsync(e.Update.Message.From.Id, Vars.CurrentLang.Message_UserServicePaused, ParseMode.Markdown, false, false, e.Update.Message.MessageId);
                } else {
                    // test text blacklist
                    if (string.IsNullOrEmpty(e.Update.Message.Text) != true) {
                        if (IsKeywordBanned(e.Update.Message.Text)) {
                            Log(Vars.CurrentLang.CLI_SentenceBlocked, "BOT", LogLevel.INFO);
                            return;
                        }
                    }
                    await Vars.Bot.ForwardMessageAsync(Vars.CurrentConf.OwnerUID, e.Update.Message.From.Id, e.Update.Message.MessageId, false);
                    AddRateLimit(e.Update.Message.From.Id);
                }
                return;
            }
        }
    }
}