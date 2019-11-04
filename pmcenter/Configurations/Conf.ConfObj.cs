using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
    {
        public class ConfObj
        {
            public ConfObj()
            {
                APIKey = "";
                OwnerUID = -1;
                EnableCc = false;
                Cc = new List<long>();
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
                ConfSyncInterval = 30000;
                AdvancedLogging = false;
                DisableTimeDisplay = false;
                Statistics = new Stats();
                Socks5Proxies = new List<Socks5Proxy>();
                BannedKeywords = new List<string>();
                Banned = new List<BanObj>();
                MessageLinks = new List<MessageIDLink>();
            }
            public string APIKey { get; set; }
            public long OwnerUID { get; set; }
            public bool EnableCc { get; set; }
            public List<long> Cc { get; set; }
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
            public int ConfSyncInterval { get; set; }
            public bool AdvancedLogging { get; set; }
            public bool DisableTimeDisplay { get; set; }
            public Stats Statistics { get; set; }
            public List<Socks5Proxy> Socks5Proxies { get; set; }
            public List<string> BannedKeywords { get; set; }
            public List<BanObj> Banned { get; set; }
            public List<MessageIDLink> MessageLinks { get; set; }
        }
    }
}