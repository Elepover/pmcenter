using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using static pmcenter.Conf;

namespace pmcenter {
    public class Methods {
        public enum LogLevel {
            INFO = 0,
            WARN = 1,
            ERROR = 2
        }
        public static void Log(string Text, string Module = "CORE", LogLevel Type = LogLevel.INFO) {
            string Output = "[" + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + "][" + Module + "]";
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
        public static void ThrBannedSweeper() {
            Log("Started!", "SWEEPPER");
            while (true) {
                int SweepCount = 0;
                List<BanObj> RemoveList = new List<BanObj>();
                if (Vars.CurrentConf.Banned.Count > 0) {
                    for (int i = 0; i <= Vars.CurrentConf.Banned.Count; i++) {
                        if (Vars.CurrentConf.Banned[i].BanUntil < DateTime.UtcNow) {
                            RemoveList.Add(Vars.CurrentConf.Banned[i]);
                            SweepCount += 1;
                        }
                    }
                    foreach (BanObj Banned in RemoveList) {
                        Vars.CurrentConf.Banned.Remove(Banned);
                    }
                }
                if (SweepCount != 0) {
                    Log(SweepCount + " banned users have been pardoned.", "SWEEPER");
                }
                Thread.Sleep(5000);
            }
        }
        public static void ThrRateLimiter() {
            Log("Started!", "RATELIMIT");
            while (true) {
                foreach (RateData Data in Vars.RateLimits) {
                    if (Data.MessageCount > 60) {
                        BanUser(Data.UID);
                        Log("Banning user: " + Data.UID, "RATELIMIT");
                    }
                    Data.MessageCount = 0;
                }
                Thread.Sleep(60000);
            }
        }
        public static bool IsBanned(long UID) {
            foreach (BanObj Banned in Vars.CurrentConf.Banned) {
                if (Banned.UID == UID) { return true; }
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
    }
}