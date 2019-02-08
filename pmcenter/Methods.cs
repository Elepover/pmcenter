/*
// Methods.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Home of complicated processes of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types.Enums;
using static pmcenter.Conf;

namespace pmcenter
{
    public class Methods
    {
        public enum ThreadStatus
        {
            Unknown = 0,
            Standby = 1,
            Working = 2,
            Stopped = 3,
            Error = 4,
        }
        public enum LogLevel
        {
            INFO = 0,
            WARN = 1,
            ERROR = 2,
        }
        public static void Log(string Text,
                               string Module = "CORE",
                               LogLevel Type = LogLevel.INFO)
        {
            if (Vars.CurrentConf?.LowPerformanceMode == true)
            {
                return;
            }
            string Output = "[" + Vars.StartSW.Elapsed.TotalMilliseconds + "ms][" + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + "][" + Module + "]";
            Console.BackgroundColor = ConsoleColor.Black;
            switch (Type)
            {
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Output += "[INFO] "; // Spacebars between prefixes and contents were already added here.
                    break;
                case LogLevel.WARN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Output += "[WARN] ";
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Output += "[ERROR] ";
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Output += "[UKNN] ";
                    break;
            }
            Output += Text;
            Console.WriteLine(Output);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static async void ThrRateLimiter()
        {
            Log("Started!", "RATELIMIT");
            while (true)
            {
                Vars.RateLimiterStatus = ThreadStatus.Working;
                foreach (RateData Data in Vars.RateLimits)
                {
                    if (Data.MessageCount > Vars.CurrentConf.AutoBanThreshold && Vars.CurrentConf.AutoBan)
                    {
                        BanUser(Data.UID);
                        await Conf.SaveConf(false, true);
                        Log("Banning user: " + Data.UID, "RATELIMIT");
                    }
                    Data.MessageCount = 0;
                }
                Vars.RateLimiterStatus = ThreadStatus.Standby;
                Thread.Sleep(30000);
            }
        }
        public static async void ThrUpdateChecker()
        {
            Log("Started!", "UPDATER");
            while (true)
            {
                Vars.UpdateCheckerStatus = ThreadStatus.Working;
                try
                {
                    Update Latest = Conf.CheckForUpdates();
                    bool DisNotif = Vars.CurrentConf.DisableNotifications;
                    // Identical with BotProcess.cs, L206.
                    if (Conf.IsNewerVersionAvailable(Latest))
                    {
                        Vars.UpdatePending = true;
                        Vars.UpdateVersion = new Version(Latest.Latest);
                        Vars.UpdateLevel = Latest.UpdateLevel;
                        string UpdateString = Vars.CurrentLang.Message_UpdateAvailable
                            .Replace("$1", Latest.Latest)
                            .Replace("$2", Latest.Details);
                        await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID,
                                                            UpdateString,
                                                            ParseMode.Markdown,
                                                            false,
                                                            DisNotif);
                        return; // Since this thread wouldn't be useful any longer, exit.
                    }
                    else
                    {
                        Vars.UpdatePending = false;
                        // This part has been cut out.
                    }
                }
                catch (Exception ex)
                {
                    Log("Error during update check: " + ex.ToString(), "UPDATER", LogLevel.ERROR);
                }
                Vars.UpdateCheckerStatus = ThreadStatus.Standby;
                Thread.Sleep(60000);
            }
        }
        public static void ThrDoResetConfCount()
        {
            Vars.ConfResetTimerStatus = ThreadStatus.Working;
            if (Vars.IsResetConfAvailable) { return; }
            Vars.IsResetConfAvailable = true;
            Thread.Sleep(30000);
            Vars.IsResetConfAvailable = false;
            Vars.ConfResetTimerStatus = ThreadStatus.Stopped;
        }
        public static void ThrPerform()
        {
            if (Vars.IsPerformanceTestExecuting) { return; }
            Vars.IsPerformanceTestExecuting = true;
            Vars.PerformanceScore = 0;
            while (Vars.IsPerformanceTestEndRequested != true)
            {
                Vars.PerformanceScore += 1;
            }
            Vars.IsPerformanceTestExecuting = false;
            Vars.IsPerformanceTestEndRequested = false;
        }
        public static string GetThreadStatusString(ThreadStatus Status)
        {
            switch (Status)
            {
                case ThreadStatus.Working:
                    return Vars.CurrentLang.Message_ThreadStatus_Working;
                case ThreadStatus.Standby:
                    return Vars.CurrentLang.Message_ThreadStatus_Standby;
                case ThreadStatus.Stopped:
                    return Vars.CurrentLang.Message_ThreadStatus_Stopped;
                case ThreadStatus.Error:
                    return Vars.CurrentLang.Message_ThreadStatus_Error;
                default:
                    return Vars.CurrentLang.Message_ThreadStatus_Unknown;
            }
        }
        public static ThreadStatus GetThreadStatus(Thread Thread)
        {
            try
            {
                if (Thread.IsAlive)
                {
                    return ThreadStatus.Standby;
                }
                else
                {
                    return ThreadStatus.Stopped;
                }
            }
            catch
            {
                return ThreadStatus.Unknown;
            }

        }
        public static bool IsBanned(long UID)
        {
            foreach (BanObj Banned in Vars.CurrentConf.Banned)
            {
                if (Banned.UID == UID) { return true; }
            }
            return false;
        }
        public static bool IsKeywordBanned(string Sentence)
        {
            if (Vars.CurrentConf.KeywordBanning != true) { return false; }

            foreach (string Blocked in Vars.CurrentConf.BannedKeywords)
            {
                if (Vars.CurrentConf.EnableRegex)
                {
                    if (IsRegexMatch(Sentence, Blocked)) { return true; }
                }
                else
                {
                    if (Sentence.Contains(Blocked)) { return true; }
                }
            }
            return false;
        }
        public static void BanUser(long UID)
        {
            if (IsBanned(UID) != true)
            {
                BanObj Banned = new BanObj
                {
                    UID = UID
                };
                Vars.CurrentConf.Banned.Add(Banned);
            }
        }
        public static void UnbanUser(long UID)
        {
            if (IsBanned(UID))
            {
                Vars.CurrentConf.Banned.Remove(GetBanObjByID(UID));
            }
        }
        public static BanObj GetBanObjByID(long UID)
        {
            foreach (BanObj Banned in Vars.CurrentConf.Banned)
            {
                if (Banned.UID == UID) { return Banned; }
            }
            return null;
        }
        public static bool IsRateDataTracking(long UID)
        {
            foreach (RateData Data in Vars.RateLimits)
            {
                if (Data.UID == UID) { return true; }
            }
            return false;
        }
        public static int GetRateDataIDByID(long UID)
        {
            foreach (RateData Data in Vars.RateLimits)
            {
                if (Data.UID == UID) { return Vars.RateLimits.IndexOf(Data); }
            }
            return -1;
        }
        public static void AddRateLimit(long UID)
        {
            if (IsRateDataTracking(UID))
            {
                int DataID = GetRateDataIDByID(UID);
                Vars.RateLimits[DataID].MessageCount += 1;
            }
            else
            {
                RateData Data = new RateData
                {
                    UID = UID,
                    MessageCount = 1
                };
                Vars.RateLimits.Add(Data);
            }
        }
        public static string SerializeCurrentConf()
        {
            return JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
        }
        public static bool IsRegexMatch(string Source, string Expression)
        {
            try
            {
                if (Regex.IsMatch(Source, Expression, RegexOptions.None))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log("Regex match failed: " + ex.Message + ", did you use a wrong regex?", "BOT", LogLevel.ERROR);
                return false;
            }
        }
        public static string GetRandomString(int Length = 8)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(Chars, Length).Select(s => s[(new Random()).Next(s.Length)]).ToArray());
        }
        public static string GetUpdateLevel(UpdateLevel Level)
        {
            string Processed = Vars.CurrentLang.Message_SysStatus_UpdateLevel_Template;
            switch (Level)
            {
                case UpdateLevel.Optional:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Optional);
                case UpdateLevel.Recommended:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Recommended);
                case UpdateLevel.Important:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Important);
                case UpdateLevel.Urgent:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Urgent);
                default:
                    return Processed.Replace("$1", Vars.CurrentLang.Message_SysStatus_UpdateLevel_Unknown);
            }
        }
        public static async Task<int> ForwardMessageAnonymously(Telegram.Bot.Types.ChatId ToChatId,
                                                           bool DisableNotifications,
                                                           Telegram.Bot.Types.Message Msg)
        {
            switch (Msg.Type)
            {
                case MessageType.Text:
                    await Vars.Bot.SendTextMessageAsync(ToChatId, Msg.Text, ParseMode.Default, false, DisableNotifications);
                    return 0;
                default:
                    await Vars.Bot.SendTextMessageAsync(Vars.CurrentConf.OwnerUID, Vars.CurrentLang.Message_SupportTextMessagesOnly, ParseMode.Markdown, false, DisableNotifications, Msg.MessageId);
                    return 1;
            }
        }
        public static bool FlipBool(bool Input)
        {
            if (Input) { return false; } else { return true; }
        }
        public static string BoolStr(bool Input)
        {
            if (Input) { return "true"; } else { return "false"; }
        }
    }
}