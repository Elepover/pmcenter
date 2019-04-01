/*
// Conf.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Configuration system of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using static pmcenter.Methods;

namespace pmcenter
{
    public class Conf
    {
        /*
         * CLASSES. DO NOT PUT ANY METHODS OR FUNCTIONS HERE.
         */
        public class ConfObj
        {
            public ConfObj()
            {
                APIKey = "";
                OwnerUID = -1;
                EnableCc = false;
                Cc = new long[] { };
                AutoBan = true;
                AutoBanThreshold = 20;
                ForwardingPaused = false;
                KeywordBanning = true;
                KeywordAutoBan = false;
                EnableRegex = false;
                AutoLangUpdate = true;
                LangURL = "https://raw.githubusercontent.com/Elepover/pmcenter/master/locales/pmcenter_locale_en.json";
                DisableNotifications = false;
                EnableRepliedConfirmation = true;
                EnableForwardedConfirmation = false;
                EnableAutoUpdateCheck = false;
                UseUsernameInMsgInfo = true;
                DonateString = "";
                LowPerformanceMode = false;
                DetailedMsgLogging = false;
                UseProxy = false;
                ResolveHostnamesLocally = true;
                CatchAllExceptions = false;
                NoStartupMessage = false;
                ContChatTarget = -1;
                EnableMsgLink = false;
                AllowUserRetraction = false;
                Statistics = new Stats();
                Socks5Proxies = new List<Socks5Proxy>();
                BannedKeywords = new List<string>();
                Banned = new List<BanObj>();
                MessageLinks = new List<MessageIDLink>();
            }
            public string APIKey { get; set; }
            public long OwnerUID { get; set; }
            public bool EnableCc { get; set; }
            public long[] Cc { get; set; }
            public bool AutoBan { get; set; }
            public int AutoBanThreshold { get; set; }
            public bool ForwardingPaused { get; set; }
            public bool KeywordBanning { get; set; }
            public bool KeywordAutoBan { get; set; }
            public bool EnableRegex { get; set; }
            public bool AutoLangUpdate { get; set; }
            public string LangURL { get; set; }
            public bool DisableNotifications { get; set; }
            public bool EnableRepliedConfirmation { get; set; }
            public bool EnableForwardedConfirmation { get; set; }
            public bool EnableAutoUpdateCheck { get; set; }
            public bool UseUsernameInMsgInfo { get; set; }
            public string DonateString { get; set; }
            public bool LowPerformanceMode { get; set; }
            public bool DetailedMsgLogging { get; set; }
            public bool UseProxy { get; set; }
            public bool ResolveHostnamesLocally { get; set; }
            public bool CatchAllExceptions { get; set; }
            public bool NoStartupMessage { get; set; }
            public long ContChatTarget { get; set; }
            public bool EnableMsgLink { get; set; }
            public bool AllowUserRetraction { get; set; }
            public Stats Statistics { get; set; }
            public List<Socks5Proxy> Socks5Proxies { get; set; }
            public List<string> BannedKeywords { get; set; }
            public List<BanObj> Banned { get; set; }
            public List<MessageIDLink> MessageLinks { get; set; }
        }
        public class BanObj
        {
            public BanObj()
            {
                UID = -1;
            }
            public long UID { get; set; }
        }
        public class RateData
        {
            public RateData()
            {
                UID = -1;
                MessageCount = 0;
            }
            public long UID { get; set; }
            public int MessageCount { get; set; }
        }
        public class Update
        {
            public Update()
            {
                Latest = "0.0.0.0";
                Details = "(Load failed.)";
                UpdateLevel = UpdateLevel.Optional;
            }
            public string Latest { get; set; }
            public string Details { get; set; }
            public UpdateLevel UpdateLevel { get; set; }
        }
        public class Socks5Proxy
        {
            public Socks5Proxy()
            {
                ServerName = "example.com";
                ServerPort = 1080;
                Username = "username";
                ProxyPass = "password";
            }
            public string ServerName { get; set; }
            public int ServerPort { get; set; }
            public string Username { get; set; }
            public string ProxyPass { get; set; }
        }
        public class MessageIDLink
        {
            public MessageIDLink()
            {
                TGUser = null;
                OwnerSessionMessageID = -1;
                UserSessionMessageID = -1;
                IsFromOwner = false;
            }
            public User TGUser { get; set; }
            /// <summary>
            /// Message ID of the message in owner's session
            /// </summary>
            public int OwnerSessionMessageID { get; set; }
            public int UserSessionMessageID { get; set; }
            public bool IsFromOwner { get; set; }
        }
        public enum UpdateLevel
        {
            Optional = 0,
            Recommended = 1,
            Important = 2,
            Urgent = 3,
        }
        public class Stats
        {
            public Stats()
            {
                TotalMessagesReceived = 0;
                TotalCommandsReceived = 0;
                TotalForwardedToOwner = 0;
                TotalForwardedFromOwner = 0;
            }
            public int TotalMessagesReceived { get; set; }
            public int TotalCommandsReceived { get; set; }
            public int TotalForwardedToOwner { get; set; }
            public int TotalForwardedFromOwner { get; set; }
        }
        /*
         * FUNCTIONS & METHODS. DO NOT PUT ANY CLASSES HERE.
         */
        public static string KillIllegalChars(string Input)
        {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
        public static async Task<bool> SaveConf(bool IsInvalid = false, bool IsAutoSave = false)
        { // DO NOT HANDLE ERRORS HERE.
            string Text = JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(Vars.ConfFile, Text);
            if (IsAutoSave)
            {
                Log("Autosave complete.", "CONF");
            }
            if (IsInvalid)
            {
                Log("We've detected an invalid configurations file and have reset it.", "CONF", LogLevel.WARN);
                Log("Please reconfigure it and try to start pmcenter again.", "CONF", LogLevel.WARN);
                Vars.RestartRequired = true;
            }
            return true;
        }
        public static async Task<bool> ReadConf(bool Apply = true)
        { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            ConfObj Temp = await GetConf(Vars.ConfFile);
            if (Apply) { Vars.CurrentConf = Temp; }
            return true;
        }
        public static async Task<ConfObj> GetConf(string Filename)
        {
            string SettingsText = await System.IO.File.ReadAllTextAsync(Filename);
            return JsonConvert.DeserializeObject<ConfObj>(SettingsText);
        }
        public static async Task InitConf()
        {
            Log("Checking configurations file's integrity...", "CONF");
            if (!System.IO.File.Exists(Vars.ConfFile))
            { // STEP 1, DETECT EXISTENCE.
                Log("Configurations file not found. Creating...", "CONF", LogLevel.WARN);
                Vars.CurrentConf = new ConfObj();
                await SaveConf(true); // Then the app will exit, do nothing.
            }
            else
            { // STEP 2, READ TEST.
                try
                {
                    await ReadConf(false); // Read but don't apply.
                }
                catch (Exception ex)
                {
                    Log("Error! " + ex.ToString(), "CONF", LogLevel.ERROR);
                    Log("Moving old configurations file to \"pmcenter.json.bak\"...", "CONF", LogLevel.WARN);
                    System.IO.File.Move(Vars.ConfFile, Vars.ConfFile + ".bak");
                    Vars.CurrentConf = new ConfObj();
                    await SaveConf(true); // Then the app will exit, do nothing.
                }
            }
            Log("Integrity test finished!", "CONF");
        }
        /// <summary>
        /// Switch 'forwarding' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchPaused()
        {
            if (Vars.CurrentConf.ForwardingPaused)
            {
                Vars.CurrentConf.ForwardingPaused = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.ForwardingPaused = true;
                return true;
            }
        }
        /// <summary>
        /// Switch 'blocking' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchBlocking()
        {
            if (Vars.CurrentConf.KeywordBanning)
            {
                Vars.CurrentConf.KeywordBanning = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.KeywordBanning = true;
                return true;
            }
        }
        /// <summary>
        /// Switch 'disablenotifications' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchNotifications()
        {
            if (Vars.CurrentConf.DisableNotifications)
            {
                Vars.CurrentConf.DisableNotifications = false;
                return false;
            }
            else
            {
                Vars.CurrentConf.DisableNotifications = true;
                return true;
            }
        }
        public static Update CheckForUpdates()
        {
            WebClient Downloader = new WebClient();
            string Response = Downloader.DownloadString(new Uri(Vars.UpdateInfoURL));
            return JsonConvert.DeserializeObject<Update>(Response);
        }
        public static bool IsNewerVersionAvailable(Update CurrentUpdate)
        {
            Version CurrentVersion = Vars.AppVer;
            Version CurrentLatest = new Version(CurrentUpdate.Latest);
            if (CurrentLatest > CurrentVersion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}