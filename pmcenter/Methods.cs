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
                for (int i = 0; i <= Vars.CurrentConf.Banned.Count; i++) {
                    if (Vars.CurrentConf.Banned[i].BanUntil < DateTime.UtcNow) {
                        RemoveList.Add(Vars.CurrentConf.Banned[i]);
                        SweepCount += 1;
                    }
                }
                foreach (BanObj Banned in RemoveList) {
                    Vars.CurrentConf.Banned.Remove(Banned);
                }
                if (SweepCount != 0) {
                    Log(SweepCount + " banned users have been pardoned.", "SWEEPER");
                }
                Thread.Sleep(5000);
            }
        }
        public static bool IsBanned(long UID) {
            foreach (BanObj Banned in Vars.CurrentConf.Banned) {
                if (Banned.UID == UID) { return true; }
            }
            return false;
        }
    }
}