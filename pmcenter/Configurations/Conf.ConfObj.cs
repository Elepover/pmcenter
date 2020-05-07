using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Conf
    {
        public sealed partial class ConfObj
        {
            public bool Minify { get; set; }
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
            public string UpdateChannel { get; set; }
            public bool IgnoreKeyboardInterrupt { get; set; }
            public bool DisableNetCore3Check { get; set; }
            public bool DisableMessageLinkTip { get; set; }
            public bool AnalyzeStartupTime { get; set; }
            public bool SkipAPIKeyVerification { get; set; }
            public bool EnableActions { get; set; }
            public bool CheckLangVersionMismatch { get; set; }
            public Stats Statistics { get; set; }
            public List<string> IgnoredLogModules { get; set; }
            public List<Socks5Proxy> Socks5Proxies { get; set; }
            public List<string> BannedKeywords { get; set; }
            public List<BanObj> Banned { get; set; }
            public List<MessageIDLink> MessageLinks { get; set; }
        }
    }
}
