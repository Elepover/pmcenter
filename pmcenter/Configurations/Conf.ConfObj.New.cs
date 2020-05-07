using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Conf
    {
        public sealed partial class ConfObj
        {
            public ConfObj()
            {
                Minify = false;
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
                EnableMsgLink = true;
                AllowUserRetraction = false;
                ConfSyncInterval = 30000;
                AdvancedLogging = false;
                DisableTimeDisplay = false;
                UpdateChannel = "master";
                IgnoreKeyboardInterrupt = false;
                DisableNetCore3Check = false;
                DisableMessageLinkTip = false;
                AnalyzeStartupTime = false;
                SkipAPIKeyVerification = false;
                EnableActions = false;
                CheckLangVersionMismatch = true;
                Statistics = new Stats();
                IgnoredLogModules = new List<string>();
                Socks5Proxies = new List<Socks5Proxy>();
                BannedKeywords = new List<string>();
                Banned = new List<BanObj>();
                MessageLinks = new List<MessageIDLink>();
            }
        }
    }
}
