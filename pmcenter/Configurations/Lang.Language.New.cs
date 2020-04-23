namespace pmcenter
{
    public static partial class Lang
    {
        public sealed partial class Language
        {
            public Language()
            {
                TargetVersion = Vars.AppVer.ToString();
                LangCode = "en.integrated";
                LanguageNameInEnglish = "English";
                LanguageNameInNative = "English";
                Message_CommandNotReplying = "😶 Don't talk to me, spend time chatting with those who love you.";
                Message_CommandNotReplyingValidMessage = "😐 Speaking to me makes no sense.";
                Message_Help = "❓ `pmcenter` *Bot Help*\n/start - Display welcome message.\n/info - Display the message's info.\n/ban - Restrict the user from contacting you.\n/banid <ID> - Restrict a user from contacting you with his/her ID.\n/pardon - Pardon the user.\n/pardonid <ID> - Pardon a user with his/her ID.\n/ping - Test if the bot is working.\n/switchfw - Pause/Resume message forwarding.\n/switchbw - Enable/Disable keyword banning.\n/switchnf - Enable/Disable notifications.\n/switchlang <URL> - Switch language file.\n/switchlangcode [Code] - Switch language with language code.\n/detectperm - Detect permissions.\n/backup - Backup configurations.\n/editconf <CONF> - Manually edit settings w/ JSON-formatted text.\n/saveconf - Manually save all settings and translations. Especially useful after upgrades.\n/readconf - Reload configurations without restarting bot.\n/resetconf - Reset configurations.\n/uptime - Check system uptime information.\n/update - Check for updates and update bot.\n/chkupdate - Only check for updates.\n/catconf - Get your current configurations.\n/restart - Restart bot.\n/status - Get host device's status information.\n/perform - Run performance test.\n/testnetwork - Test latency to servers used by pmcenter.\n/chat - Enter Continued Conversation mode.\n/stopchat - Leave Continued Conversation mode.\n/retract - Retract a message.\n/clearmessagelinks - Clear message links.\n/getstats - Get statistics data.\n/autosave [off/interval] - Switch autosave status.\n/help - Display this message.\n\nThank you for using `pmcenter`!";
                Message_OwnerStart = "😊 *Hi!* I'm your `pmcenter` bot, and I work just for you.\nThis message means that you've set up the bot successfully.\nTo reply to any forwarded messages, just directly reply to them here.\n\nThank you for using the `pmcenter` bot!";
                Message_ReplySuccessful = "✅ Successfully replied to user $1!";
                Message_ForwardedToOwner = "✅ Your message has been forwarded to my owner!";
                Message_UserBanned = "🚫 The user has been banned permanently.";
                Message_UserPardoned = "✅ You've pardoned the user.";
                Message_UserStartDefault = "📨 *Hi!* To send anything to my owner, just send it here.\n⚠ To be informed: I'll *automatically* ban flooding users.";
                Message_PingReply = "📶 Pong!";
                Message_ServicePaused = "📴 Message forwarding has been *paused*.";
                Message_ServiceResumed = "📲 Messsage forwarding has been *resumed*.";
                Message_UserServicePaused = "🚧 *Sorry...*\nYour messages won't be forwarded to my lord, currently.";
                Message_BotStarted = "🚀 Bot has started successfully in `$1`!";
                Message_MessageBlockEnabled = "📴 Keyword blocking is now *enabled*.";
                Message_MessageBlockDisabled = "📲 Keyword blocking is now *disabled*.";
                Message_ConfigUpdated = "🔄 Settings have been updated!";
                Message_ConfigReloaded = "🔄 Settings reloaded!";
                Message_UptimeInfo = "🚀 *Uptime Information*:\nSystem uptime: `$1`\nBot uptime: `$2`";
                Message_UpdateAvailable = "🔄 *Update available!*\n\n$3\nNew version: `$1`\nWhat's new:\n`$2`\n\nExecute /update to perform automatic update.";
                Message_UpdateProcessing = "💠 Preparing to update...";
                Message_UpdateCheckFailed = "⚠ Update failed: `$1`";
                Message_AlreadyUpToDate = "✅ *Already up to date*!\nLatest version: `$1`\nCurrently installed: `$2`\nUpdate details:\n`$3`";
                Message_UpdateExtracting = "📤 Extracting update files...";
                Message_UpdateFinalizing = "✅ Files patching complete! Trying to restart...";
                Message_CurrentConf = "💾 *Your current configurations*: \n`$1`";
                Message_SysStatus_Header = "💻 *System Status*";
                Message_SysStatus_NoOperationRequired = "🚀 *Good job, No action needed!*";
                Message_SysStatus_PendingUpdate = "🔄 *Update available to*: `$1`";
                Message_SysStatus_UpdateLevel_Template = "🚨 *Update level*: `$1`";
                Message_SysStatus_UpdateLevel_Optional = "💡 Optional";
                Message_SysStatus_UpdateLevel_Recommended = "💠 Recommended";
                Message_SysStatus_UpdateLevel_Important = "❗ Important";
                Message_SysStatus_UpdateLevel_Urgent = "⚠ Urgent";
                Message_SysStatus_UpdateLevel_Unknown = "❓ Unknown";
                Message_SysStatus_RestartRequired = "🔄 *Bot restart required to apply changes.*";
                Message_SysStatus_Summary = "📝 *Device name*: `$1`\n💿 *Operating System*: `$2`\nℹ *OS description*: `$3`\n⌛ *Server uptime*: `$4`\n🕓 *Bot uptime*: `$5`\n📅 *Server time (UTC)*: `$6`\n📐 *Runtime version*: `$7`\nℹ *Runtime description*: `$8`\n📏 *Application version*: `$9`\n💠 *Processor count*: `$a`\n📖 *Language code*: `$b`\n🛫 *Update channel (current)*: `$f`\n🛬 *Update channel (target)*: `$g`\n🔄 *Update checker*: `$c`\n🔄 *Rate limit processor*: `$d`\n🔄 *Configuration reset verifier*: `$e`";
                Message_Restarting = "🔄 Restarting...\n\n_It only works with systemd-like daemons._";
                Message_NotificationsOff = "📳 Notifications are *OFF*.";
                Message_NotificationsOn = "📲 Notifications are *ON*.";
                Message_SupportTextMessagesOnly = "📋 Sorry... Only text messages can be forwarded in Anonymous Forward mode.";
                Message_ForwarderNotReal = "ℹ The actual sender of this message is $1, whose UID is `$2`.\n\nYou can also ban this user by sending this following command:\n\n`/banid $2`\n\nTo undo this, send this command:\n\n`/pardonid $2`";
                Message_GeneralFailure = "✖ Error processing request: $1";
                Message_LangVerMismatch = "⚠ Language file (`$1`) is not for current version (`$2`), consider updating language file?";
                Message_SwitchingLang = "💠 Switching language...";
                Message_LangSwitched = "🚀 Language switched!";
                Message_ThreadStatus_Unknown = "Unknown";
                Message_ThreadStatus_Standby = "Standby";
                Message_ThreadStatus_Working = "Working";
                Message_ThreadStatus_Stopped = "Stopped";
                Message_ThreadStatus_Error = "Error";
                Message_ConfReset_Inited = "❓ *Are you sure you want to reset configurations?*\n\nThis will reset everything to default value (except API Key and Owner ID) and is irrevertable!\nBot will restart after resetting configurations.\n\nSend /resetconf again in 30s to continue.";
                Message_ConfReset_Started = "💠 Resetting...";
                Message_ConfReset_Done = "🔄 Configurations have been reset! Restarting...";
                Message_Performance_Inited = "🔄 Performance test started.";
                Message_Performance_Results = "✅ *Performance test complete*\n\nScore: `$1`.";
                Message_BackupComplete = "✅ Backup complete! File name: `$1`";
                Message_ConfAccess = "ℹ *Access Info*\n\nConfigurations: `$1`\nLanguage: `$2`";
                Message_APIKeyChanged = "⚠ We've detected an API Key change. Please restart pmcenter to apply this change.";
                Message_Connectivity = "📡 *Connectivity Information*\n\nLatency to GitHub: $1\nLatency to Telegram API: $2\nLatency to CI (updates): $3";
                Message_ContinuedChatEnabled = "💬 *Continued Conversation* mode is now `active`! All your messages (except commands and replys) will be forwarded to $1";
                Message_ContinuedChatDisabled = "✅ *Continued Conversation* is now `disabled`.";
                Message_FeatureNotAvailable = "⚠ *This feature is unavailable or disabled*.";
                Message_Stats = "📝 *Statistics*\n\n💬 Received messages: `$1`\n🔄 Forwarded to owner: `$2`\n🔄 Forwarded from owner: `$3`\n🚀 Commands received: `$4`";
                Message_Retracted = "✅ This message has been retracted.";
                Message_MsgLinksCleared = "✅ All message links have been cleared.";
                Message_AvailableLang = "ℹ *Available languages*\n\n`$1`";
                Message_NetCore31Required = "⚠ You need `.NET Core 3.1` (runtime) installed in order to receive pmcenter v2 and further updates.\n\nLatest .NET Core runtime version detected on your device: `$1`\n\nThis warning will only show once.";
                Message_MsgLinkTip = "ℹ Tip: You need to set `EnableMsgLink` option to `true` in pmcenter configurations in order to reply to anonymously forwarded messages.\nThis also happens when the message link for the message couldn't be found.\nDue to Telegram API's restrictions, it's impossible now to reply to that message.\nAfter you set `EnableMsgLink` to `true`, you'll be able to reply to this kind of messages.\n\nThis tip will only prompt once.";
                Message_AutoSaveEnabled = "✅ Autosave *enabled*, interval: `$1s`.";
                Message_AutoSaveIntervalTooShort = "⚠ The current autosave interval (`$1ms`) is *too short*! It may cause high CPU and disk usage as a result. *Disable it if you didn't intend to do that!*";
                Message_AutoSaveDisabled = "✅ Autosave *disabled*.";
                Message_Action_Banned = "✅ User $1 has been banned!";
                Message_Action_Pardoned = "✅ User $1 has been pardoned!";
                Message_Action_ContChatEnabled = "✅ You're now chatting with $1!";
                Message_Action_ContChatDisabled = "✅ Continued chat disabled!";
                Message_Action_Error = "✖ Action failed. Check logs.";
                Message_Action_ErrorWithDetails = "✖ Action failed: $1";
                Message_Action_ChooseAction = "❓ *What do you want to do with this message?*";
                Message_Action_Ban = "✖ Ban the user";
                Message_Action_Pardon = "✅ Pardon the user";
                Message_Action_Chat = "💬 Enter continued conversation";
                Message_Action_StopChat = "💬 Stop continued conversation";
                Message_Action_LinkNotFound = "✖ Cannot find the corresponding message link, did you just clear the message links, or was the message links feature disabled?";
            }
        }
    }
}
