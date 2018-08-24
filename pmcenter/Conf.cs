/*
// Conf.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Configuration system of pmcenter.
// Copyright (C) 2018 Elepover. Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods;

namespace pmcenter {
    public class Conf {
        /*
         * CLASSES. DO NOT PUT ANY METHODS OR FUNCTIONS HERE.
         */
        public class ConfObj {
            public ConfObj() {
                APIKey = "";
                OwnerUID = -1;
                EnableCc = false;
                Cc = new long[]{};
                AutoBan = true;
                AutoBanThreshold = 20;
                Banned = new List<BanObj>();
                ForwardingPaused = false;
                BannedKeywords = new List<string>();
                KeywordBanning = true;
                KeywordAutoBan = false;
                EnableRegex = false;
                RestartCommand = "/bin/systemctl";
                RestartArgs = "restart pmcenter";
                AutoLangUpdate = true;
                LangURL = "https://raw.githubusercontent.com/Elepover/pmcenter/master/locales/pmcenter_locale_en.json";
                DisableNotifications = false;
            }
            public string APIKey {get; set;}
            public long OwnerUID {get; set;}
            public bool EnableCc {get; set;}
            public long[] Cc {get; set;}
            public bool AutoBan {get; set;}
            public int AutoBanThreshold {get; set;}
            public List<BanObj> Banned {get; set;}
            public bool ForwardingPaused {get; set;}
            public List<string> BannedKeywords {get; set;}
            public bool KeywordBanning {get; set;}
            public bool KeywordAutoBan {get; set;}
            public bool EnableRegex {get; set;}
            public string RestartCommand {get; set;}
            public string RestartArgs {get; set;}
            public bool AutoLangUpdate {get; set;}
            public string LangURL {get; set;}
            public bool DisableNotifications {get; set;}
        }
        public class BanObj {
            public BanObj() {
                UID = -1;
            }
            public long UID {get; set;}
        }
        public class RateData {
            public RateData() {
                UID = -1;
                MessageCount = 0;
            }
            public long UID {get; set;}
            public int MessageCount {get; set;}
        }
        public class Update {
            public Update() {
                Latest = "0.0.0.0";
                Details = "(Load failed.)";
            }
            public string Latest {get; set;}
            public string Details {get; set;}
        }
        /*
         * FUNCTIONS & METHODS. DO NOT PUT ANY CLASSES HERE.
         */
        public static string KillIllegalChars(string Input) {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
        public static async Task<bool> SaveConf(bool IsInvalid = false, bool IsAutoSave = false) { // DO NOT HANDLE ERRORS HERE.
            string Text = JsonConvert.SerializeObject(Vars.CurrentConf, Formatting.Indented);
            await File.WriteAllTextAsync(Vars.ConfFile, Text);
            if (IsAutoSave) {
                Log("Autosave complete.", "CONF");
            }
            if (IsInvalid) {
                Log("We've detected an invalid configurations file and have reset it.", "CONF", LogLevel.WARN);
                Log("Please reconfigure it and try to start pmcenter again.", "CONF", LogLevel.WARN);
                Vars.RestartRequired = true;
            }
            return true;
        }
        public static async Task<bool> ReadConf(bool Apply = true) { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            string SettingsText = await File.ReadAllTextAsync(Vars.ConfFile);
            ConfObj Temp = JsonConvert.DeserializeObject<ConfObj>(SettingsText);
            if (Apply) { Vars.CurrentConf = Temp; }
            return true;
        }
        public static async void InitConf() {
            Log("Checking configurations file's integrity...", "CONF");
            if (File.Exists(Vars.ConfFile) != true) { // STEP 1, DETECT EXISTENCE.
                Log("Configurations file not found. Creating...", "CONF", LogLevel.WARN);
                Vars.CurrentConf = new ConfObj();
			    await SaveConf(true); // Then the app will exit, do nothing.
            } else { // STEP 2, READ TEST.
                try {
                    await ReadConf(false); // Read but don't apply.
                } catch (Exception ex) {
                    Log("Error! " + ex.ToString(), "CONF", LogLevel.ERROR);
                    Log("Moving old configurations file to \"pmcenter.json.bak\"...", "CONF", LogLevel.WARN);
                    File.Move(Vars.ConfFile, Vars.ConfFile + ".bak");
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
        public static bool SwitchPaused() {
            if (Vars.CurrentConf.ForwardingPaused) {
                Vars.CurrentConf.ForwardingPaused = false;
                return false;
            } else {
                Vars.CurrentConf.ForwardingPaused = true;
                return true;
            }
        }
        /// <summary>
        /// Switch 'blocking' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchBlocking() {
            if (Vars.CurrentConf.KeywordBanning) {
                Vars.CurrentConf.KeywordBanning = false;
                return false;
            } else {
                Vars.CurrentConf.KeywordBanning = true;
                return true;
            }
        }
        /// <summary>
        /// Switch 'disablenotifications' status, returning current status.
        /// </summary>
        /// <returns></returns>
        public static bool SwitchNotifications() {
            if (Vars.CurrentConf.DisableNotifications) {
                Vars.CurrentConf.DisableNotifications = false;
                return false;
            } else {
                Vars.CurrentConf.DisableNotifications = true;
                return true;
            }
        }
        public static Update CheckForUpdates() {
            WebClient Downloader = new WebClient();
            string Response = Downloader.DownloadString(new Uri(Vars.UpdateInfoURL));
            return JsonConvert.DeserializeObject<Update>(Response);
        }
        public static bool IsNewerVersionAvailable(Update CurrentUpdate) {
            Version CurrentVersion = Vars.AppVer;
            Version CurrentLatest = new Version(CurrentUpdate.Latest);
            if (CurrentLatest > CurrentVersion) {
                return true;
            } else {
                return false;
            }
        }
    }
}