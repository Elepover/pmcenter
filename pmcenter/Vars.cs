/*
// Vars.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Storage of variables for easier calling.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Telegram.Bot;

namespace pmcenter {
    public class Vars {
        public readonly static string ASCII = "                                     __           \n    ____  ____ ___  ________  ____  / /____  _____\n   / __ \\/ __ `__ \\/ ___/ _ \\/ __ \\/ __/ _ \\/ ___/\n  / /_/ / / / / / / /__/  __/ / / / /_/  __/ /    \n / .___/_/ /_/ /_/\\___/\\___/_/ /_/\\__/\\___/_/     \n/_/                                               ";
        public readonly static Version AppVer = new Version("1.1.37.107");
        public readonly static string AppExecutable = Assembly.GetExecutingAssembly().Location;
        public readonly static string AppDirectory = (new FileInfo(AppExecutable)).DirectoryName;
        public readonly static string ConfFile = Path.Combine(AppDirectory, "pmcenter.json");
        public readonly static string LangFile = Path.Combine(AppDirectory, "pmcenter_locale.json");
        public readonly static string UpdateArchiveURL = "https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip";
        public readonly static string UpdateInfoURL = "https://raw.githubusercontent.com/Elepover/pmcenter/master/updateinfo.json";

        public static bool RestartRequired = false;
        public static Stopwatch StartSW = new Stopwatch();
        public static List<Conf.RateData> RateLimits = new List<Conf.RateData>();
        public static double TotalForwarded = 0;
        public static Conf.ConfObj CurrentConf;
        public static Lang.Language CurrentLang;
        public static TelegramBotClient Bot;
        
        public static Thread BannedSweepper;
        public static Thread RateLimiter;
    }
}