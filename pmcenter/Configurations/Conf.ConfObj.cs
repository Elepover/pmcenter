using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
    {
        public partial class ConfObj
        {
            public string APIKey;
            public long OwnerUID;
            public bool EnableCc;
            public List<long> Cc;
            public bool AutoBan;
            public int AutoBanThreshold;
            public bool ForwardingPaused;
            public bool KeywordBanning;
            public bool KeywordAutoBan;
            public bool EnableRegex;
            public bool AutoLangUpdate;
            public string LangURL;
            public bool DisableNotifications;
            public bool EnableRepliedConfirmation;
            public bool EnableForwardedConfirmation;
            public bool EnableAutoUpdateCheck;
            public bool UseUsernameInMsgInfo;
            public string DonateString;
            public bool LowPerformanceMode;
            public bool DetailedMsgLogging;
            public bool UseProxy;
            public bool ResolveHostnamesLocally;
            public bool CatchAllExceptions;
            public bool NoStartupMessage;
            public long ContChatTarget;
            public bool EnableMsgLink;
            public bool AllowUserRetraction;
            public int ConfSyncInterval;
            public bool AdvancedLogging;
            public bool DisableTimeDisplay;
            public Stats Statistics;
            public List<Socks5Proxy> Socks5Proxies;
            public List<string> BannedKeywords;
            public List<BanObj> Banned;
            public List<MessageIDLink> MessageLinks;
        }
    }
}