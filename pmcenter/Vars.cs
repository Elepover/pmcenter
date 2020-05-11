/*
// Vars.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Storage of variables for easier calling.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

//#define BUILT_FOR_GITHUB_RELEASES
//#define SELFCONTAINED

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Telegram.Bot;

namespace pmcenter
{
    public static class Vars
    {
        public readonly static string AsciiArt = "                                     __           \n    ____  ____ ___  ________  ____  / /____  _____\n   / __ \\/ __ `__ \\/ ___/ _ \\/ __ \\/ __/ _ \\/ ___/\n  / /_/ / / / / / / /__/  __/ / / / /_/  __/ /    \n / .___/_/ /_/ /_/\\___/\\___/_/ /_/\\__/\\___/_/     \n/_/                                               ";
        public readonly static Version AppVer = new Version("2.0.2.0");
        public readonly static string AppExecutable = Assembly.GetExecutingAssembly().Location;
        public readonly static string AppDirectory = Path.GetDirectoryName(AppExecutable);
        public static string ConfFile = Path.Combine(AppDirectory, "pmcenter.json");
        public static string LangFile = Path.Combine(AppDirectory, "pmcenter_locale.json");
        public readonly static string UpdateArchiveURL = "https://see.wtf/pmcenter-update";
        public readonly static string UpdateInfoURL = "https://raw.githubusercontent.com/Elepover/pmcenter/$channel/updateinfo.json";
        public readonly static string UpdateInfo2URL = "https://raw.githubusercontent.com/Elepover/pmcenter/$channel/updateinfo2.json";
        public readonly static string LocaleMapURL = "https://raw.githubusercontent.com/Elepover/pmcenter/$channel/locales/locale_map.json";
#if MASTER
        public readonly static string CompileChannel = "master";
#else
        public readonly static string CompileChannel = "pmcenter-lazer";
#endif
#if BUILT_FOR_GITHUB_RELEASES
        public readonly static bool GitHubReleases = true;
#else
        public readonly static bool GitHubReleases = false;
#endif
#if SELFCONTAINED
        public readonly static bool SelfContained = true;
#else
        public readonly static bool SelfContained = false;
#endif
        // public readonly static long AnonymousChannelId = -1001228946795;
        // public readonly static string AnonymousChannelTitle = "a user";
        // public readonly static string AnonymousChannelUsername = "HiddenSender";

        public static bool RestartRequired = false;
        public static bool NonEmergRestartRequired = false;
        public static bool UpdatePending = false;
        public static bool IsResetConfAvailable = false;
        public static bool IsCtrlCHandled = false;
        public static int CtrlCCounter = 0;
        public static bool IsShuttingDown = false;
        public static bool ServiceMode = true;
        public static Methods.UpdateHelper.UpdateLevel UpdateLevel;
        public static Version UpdateVersion;
        public static Stopwatch StartSW = new Stopwatch();
        public static List<Conf.RateData> RateLimits = new List<Conf.RateData>();
        public static double TotalForwarded = 0;
        public static Conf.ConfObj CurrentConf;
        public static Lang.Language CurrentLang;
        public static TelegramBotClient Bot;

        public static bool IsPerformanceTestExecuting = false;
        public static bool IsPerformanceTestEndRequested = false;
        public static double PerformanceScore = 0;

        public static Thread BannedSweeper;
        public static Thread ConfValidator;
        public static Methods.ThreadStatus ConfResetTimerStatus = Methods.ThreadStatus.Stopped;
        public static Thread RateLimiter;
        public static Methods.ThreadStatus RateLimiterStatus = Methods.ThreadStatus.Stopped;
        public static Thread UpdateChecker;
        public static Methods.ThreadStatus UpdateCheckerStatus = Methods.ThreadStatus.Stopped;
        public static Thread SyncConf;
    }
}
