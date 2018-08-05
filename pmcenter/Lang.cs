using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using static pmcenter.Methods;

namespace pmcenter {
    public class Lang {
        public class Language {
            public Language() {
                Message_CommandNotReplying = "😶 Don't talk to me, spend time chatting with those who love you.";
                Message_CommandNotReplyingValidMessage = "😐 Speaking to me makes no sense.";
                Message_Help = "❓ `pmcenter` *Bot Help*\n/start - Display welcome message.\n/info - Display the message's info.\n/ban - Restrict the user from contacting you.\n/pardon - Pardon the user.\n/help - Display this message.";
                Message_OwnerStart = "😊 *Hi!* I'm your `pmcenter` bot, and I work just for you.\nThis message means that you've set up the bot successfully.\nTo reply to any forwarded messages, just directly reply to them here.\n\nThank you for using the `pmcenter` bot!";
                Message_ReplySuccessful = "✅ Successfully replied to user $1!";
                Message_UserBanned = "🚫 The user has been banned permanently.";
                Message_UserPardoned = "✅ You've pardoned the user.";
                Message_UserStartDefault = "📨 *Hi!* To send anything to my owner, just send it here.\n⚠ To be informed: I'll *automatically* ban flooding users.";
            }
            public string Message_OwnerStart {get; set;}
            public string Message_UserStartDefault {get; set;}
            public string Message_ReplySuccessful {get; set;}
            public string Message_Help {get; set;}
            public string Message_UserBanned {get; set;}
            public string Message_UserPardoned {get; set;}
            public string Message_CommandNotReplying {get; set;}
            public string Message_CommandNotReplyingValidMessage {get; set;}
        }
        public static string KillIllegalChars(string Input) {
            return Input.Replace("/", "-").Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("\"", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-");
        }
        public static async Task<bool> SaveLang(bool IsInvalid = false) { // DO NOT HANDLE ERRORS HERE.
            string Text = JsonConvert.SerializeObject(Vars.CurrentLang, Formatting.Indented);
            StreamWriter Writer = new StreamWriter(File.Create(Vars.LangFile), System.Text.Encoding.UTF8);
            await Writer.WriteAsync(Text);
            await Writer.FlushAsync();
            Writer.Close();
            if (IsInvalid) {
                Log("We've detected an invalid language file and have reset it.", "LANG", LogLevel.WARN);
                Log("Please reconfigure it and try to start pmcenter again.", "LANG", LogLevel.WARN);
                Environment.Exit(1);
            }
            return true;
        }
        public static async Task<bool> ReadLang(bool Apply = true) { // DO NOT HANDLE ERRORS HERE. THE CALLING METHOD WILL HANDLE THEM.
            string SettingsText = await File.ReadAllTextAsync(Vars.LangFile);
            Language Temp = JsonConvert.DeserializeObject<Language>(SettingsText);
            if (Apply) { Vars.CurrentLang = Temp; }
            return true;
        }
        public static async void InitLang() {
            Log("Checking language file's integrity...", "LANG");
            if (File.Exists(Vars.LangFile) != true) { // STEP 1, DETECT EXISTENCE.
                Vars.CurrentLang = new Language();
			    await SaveLang(true); // Then the app will exit, do nothing.
            } else { // STEP 2, READ TEST.
                try {
                    await ReadLang(false); // Read but don't apply.
                } catch {
                    Log("Moving old language file to \"pmcenter_locale.json.bak\"...", "LANG", LogLevel.WARN);
                    File.Move(Vars.LangFile, Vars.LangFile + ".bak");
                    Vars.CurrentLang = new Language();
                    await SaveLang(true); // Then the app will exit, do nothing.
                }
            }
            Log("Integrity test PASSED!", "LANG");
        }
    }
}