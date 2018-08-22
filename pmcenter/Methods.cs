/*
// Methods.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Home of complicated processes of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Conf;

namespace pmcenter {
    public class Methods {
        public enum LogLevel {
            INFO = 0,
            WARN = 1,
            ERROR = 2
        }
        public static void Log(string Text,
                               string Module = "CORE",
                               LogLevel Type = LogLevel.INFO) {
            string Output = "[" + Vars.StartSW.Elapsed.TotalMilliseconds + "ms][" + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + "][" + Module + "]";
            Console.BackgroundColor = ConsoleColor.Black;
            switch (Type) {
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
        public static async void ThrRateLimiter() {
            Log(Vars.CurrentLang.CLI_ThreadStarted, "RATELIMIT");
            while (true) {
                foreach (RateData Data in Vars.RateLimits) {
                    if (Data.MessageCount > Vars.CurrentConf.AutoBanThreshold && Vars.CurrentConf.AutoBan) {
                        BanUser(Data.UID);
                        await Conf.SaveConf(false, true);
                        Log("Banning user: " + Data.UID, "RATELIMIT");
                    }
                    Data.MessageCount = 0;
                }
                Thread.Sleep(30000);
            }
        }
        public static bool IsBanned(long UID) {
            foreach (BanObj Banned in Vars.CurrentConf.Banned) {
                if (Banned.UID == UID) { return true; }
            }
            return false;
        }
        public static bool IsKeywordBanned(string Sentence) {
            if (Vars.CurrentConf.KeywordBanning != true) { return false; }
            
            foreach (string Blocked in Vars.CurrentConf.BannedKeywords) {
                if (Vars.CurrentConf.EnableRegex) {
                    if (IsRegexMatch(Sentence, Blocked)) { return true; }
                } else {
                    if (Sentence.Contains(Blocked)) { return true; }
                }
            }
            return false;
        }
        public static void BanUser(long UID) {
            if (IsBanned(UID) != true) {
                BanObj Banned = new BanObj();
                Banned.UID = UID;
                Vars.CurrentConf.Banned.Add(Banned);
            }
        }
        public static void UnbanUser(long UID) {
            if (IsBanned(UID)) {
                Vars.CurrentConf.Banned.Remove(GetBanObjByID(UID));
            } 
        }
        public static BanObj GetBanObjByID(long UID) {
            foreach (BanObj Banned in Vars.CurrentConf.Banned) {
                if (Banned.UID == UID) { return Banned; }
            }
            return null;
        }
        public static bool IsRateDataTracking(long UID) {
            foreach (RateData Data in Vars.RateLimits) {
                if (Data.UID == UID) { return true; }
            }
            return false;
        }
        public static int GetRateDataIDByID(long UID) {
            foreach (RateData Data in Vars.RateLimits) {
                if (Data.UID == UID) { return Vars.RateLimits.IndexOf(Data); }
            }
            return -1;
        }
        public static void AddRateLimit(long UID) {
            if (IsRateDataTracking(UID)) {
                int DataID = GetRateDataIDByID(UID);
                Vars.RateLimits[DataID].MessageCount += 1;
            } else {
                RateData Data = new RateData();
                Data.UID = UID;
                Data.MessageCount = 1;
                Vars.RateLimits.Add(Data);
            }
        }
        public static string SerializeCurrentConf() {
            return JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
        }
        public static bool IsRegexMatch(string Source, string Expression) {
            try {
                if (Regex.IsMatch(Source, Expression, RegexOptions.None)) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) {
                Log("Regex match failed: " + ex.Message + ", did you use a wrong regex?", "BOT", LogLevel.ERROR);
                return false;
            }
        }
    }
}