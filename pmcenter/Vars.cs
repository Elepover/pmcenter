using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Telegram.Bot;

namespace pmcenter {
    public class Vars {
        public readonly static Version AppVer = new Version("1.0.5.23");
        public readonly static string AppExecutable = Assembly.GetExecutingAssembly().Location;
        public readonly static string AppDirectory = (new FileInfo(AppExecutable)).DirectoryName;
        public readonly static string ConfFile = Path.Combine(AppDirectory, "pmcenter.json");
        public readonly static string OwnerStartMessage = "😊 *Hi!* I'm your `pmcenter` bot, and I work just for you.\nThis message means that you've set up the bot successfully.\nTo reply to any forwarded messages, just directly reply to them here.\n\nThank you for using the `pmcenter` bot!";

        public static Stopwatch StartSW = new Stopwatch();
        public static List<Conf.RateData> RateLimits = new List<Conf.RateData>();
        public static Conf.ConfObj CurrentConf;
        public static Telegram.Bot.Types.User Owner;
        public static TelegramBotClient Bot;
        public static Thread BannedSweepper;
        public static Thread RateLimiter;
    }
}