using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Telegram.Bot;

namespace pmcenter {
    public class Vars {
        public readonly static Version AppVer = new Version("1.0.16.61");
        public readonly static string AppExecutable = Assembly.GetExecutingAssembly().Location;
        public readonly static string AppDirectory = (new FileInfo(AppExecutable)).DirectoryName;
        public readonly static string ConfFile = Path.Combine(AppDirectory, "pmcenter.json");
        public readonly static string LangFile = Path.Combine(AppDirectory, "pmcenter_locale.json");

        public static Stopwatch StartSW = new Stopwatch();
        public static List<Conf.RateData> RateLimits = new List<Conf.RateData>();
        public static Conf.ConfObj CurrentConf;
        public static Lang.Language CurrentLang;
        public static Telegram.Bot.Types.User Owner;
        public static TelegramBotClient Bot;
        public static Thread BannedSweepper;
        public static Thread RateLimiter;
    }
}