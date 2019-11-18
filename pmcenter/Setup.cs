/*
// Setup.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Methods & functions for Setup Wizard.
// Copyright (C) 2018 Elepover. Licensed under the Apache License (Version 2.0).
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace pmcenter
{
    public class Setup
    {
        private static Conf.ConfObj NewConf = new Conf.ConfObj();
        private static TelegramBotClient TestBot;
        private static bool IsUIDReceived = false;
        private static long ReceivedUID = -1;
        private static string Nickname = "";
        private static void OnUpdate(object sender, UpdateEventArgs e)
        {
            Say("Update received.");
            Say(".. Processing...");
            if (!IsUIDReceived)
            {
                IsUIDReceived = true;
                ReceivedUID = e.Update.Message.From.Id;
                Nickname = e.Update.Message.From.FirstName + " " + e.Update.Message.From.LastName;
                TestBot.StopReceiving();
            }
        }

        private static void Say(string Input)
        {
            Console.WriteLine(Input);
        }
        private static void SIn(string Input)
        {
            Console.Write(Input);
        }
        public static async Task SetupWizard()
        {
            Say(":) Welcome!");
            Say("   This is the pmcenter setup wizard.");
            Say("   App version: " + Vars.AppVer.ToString());
            Say("   Here to guide you through some *important* configurations of pmcenter.");
            SIn("=> Continue? [y/N]: ");
            if (Console.ReadLine().ToLower() != "y")
            {
                Environment.Exit(0);
            }

            Say("");
            await SetAPIKey().ConfigureAwait(false);
            Say("");
            SetUID();
            Say("");
            SetNotifPrefs();
            Say("");
            SetAutoBanPrefs();

            // finalization
            Say("");
            Say(">> Complete!");
            Say("   All major configurations have been set!");
            Say("");
            SIn("=> Save configurations? [Y/n]: ");
            string Choice = Console.ReadLine();
            if (Choice.ToLower() != "n")
            {
                if (File.Exists(Vars.ConfFile))
                {
                    Say("Warning: pmcenter.json already exists.");
                    SIn("..       Moving the existing one to pmcenter.json.bak...");
                    if (File.Exists(Vars.ConfFile + ".bak"))
                    {
                        SIn(" File exists, deleting...");
                        File.Delete(Vars.ConfFile + ".bak");
                    }
                    File.Move(Vars.ConfFile, Vars.ConfFile + ".bak");
                    Say(" Done!");
                }
                SIn("Saving configurations to " + Vars.ConfFile + "...");
                Vars.CurrentConf = NewConf;
                _ = await Conf.SaveConf().ConfigureAwait(false);
                Say(" Done!");
                if (File.Exists(Vars.LangFile))
                {
                    Say("Warning: pmcenter_locale.json already exists.");
                    SIn("..       Moving the existing one to pmcenter_locale.json.bak...");
                    if (File.Exists(Vars.LangFile + ".bak"))
                    {
                        SIn(" File exists, deleting...");
                        File.Delete(Vars.LangFile + ".bak");
                    }
                    File.Move(Vars.LangFile, Vars.LangFile + ".bak");
                    Say(" Done!");
                }
                SIn("Saving language file to " + Vars.LangFile + "...");
                Vars.CurrentLang = new Lang.Language();
                _ = await Lang.SaveLang().ConfigureAwait(false);
                Say(" Done!");
                
                Say(">> Setup complete!");
                Say("   Thanks for using pmcenter!");
                Say("   Check out pmcenter's GitHub repository at:");
                Say("     https://github.com/Elepover/pmcenter");
                Say("   Program will now exit.");
                Say("   You can start it right away by typing this command:");
                Say("     dotnet pmcenter.dll");
                Say("   To run setup wizard again:");
                Say("     dotnet pmcenter.dll --setup");
            }
            else
            {
                Say("OK. Come back later!");
            }
            Environment.Exit(0);
        }
        public static async Task SetAPIKey()
        {
            Say("1> API Key");
            Say("   API Key is necessary for any Telegram bot to contact with Telegram servers.");
            Say("   You can always get one for free at @BotFather");
            Say("");
        EnterKey:
            SIn("=> Enter your API Key: ");
            string Key = Console.ReadLine();
            SIn(".. Testing API Key: " + Key + " ...");
            try
            {
                TestBot = new TelegramBotClient(Key);
                if (!await TestBot.TestApiAsync().ConfigureAwait(false))
                {
                    throw (new ArgumentException("API Key is not valid."));
                }
            }
            catch (Exception ex)
            {
                Methods.CheckOpenSSLComp(ex);
                Say(" Invalid API Key: " + ex.Message);
                goto EnterKey;
            }
            NewConf.APIKey = Key;
            Say(" Done!");
        }
        public static void SetUID()
        {
            Say("2> Owner ID");
            Say("   Your Telegram UID is your unique and permanent identifier.");
            Say("   It is required to enable your pmcenter instance to contact with you.");
            Say("");
        EnterUID:
            SIn("=> Enter your UID, or \"auto\" for automatic setup: ");
            string UID = Console.ReadLine();
            if (UID.ToLower() == "auto")
            {
                Say(".. Preparing for automatic UID detection...");
                TestBot.OnUpdate += OnUpdate;
                TestBot.StartReceiving(new UpdateType[] { UpdateType.Message });
                Say("Say something to your bot on Telegram. We'll detect your UID automatically.");
                while (!IsUIDReceived)
                {
                    Thread.Sleep(200);
                }
                Say("Hello, " + Nickname + "! Your UID has been detected as " + ReceivedUID + ".");
                SIn(".. Saving UID: " + ReceivedUID + " ...");
                NewConf.OwnerUID = ReceivedUID;
                Say(" Done!");
            }
            else
            {
                try
                {
                    long NewUID = long.Parse(UID);
                    SIn(".. Saving UID: " + NewUID + " ...");
                    NewConf.OwnerUID = NewUID;
                    Say(" Done!");
                }
                catch (Exception ex)
                {
                    Say("Error parsing UID: " + ex.Message);
                    goto EnterUID;
                }
            }
        }
        public static void SetNotifPrefs()
        {
            Say("3> Notification preferences");
            Say("   Sometimes you just want everyone to shhhh...");
            Say("");
            SIn("=> Mute notifications? [y/N]: ");
            string MuteNotif = Console.ReadLine();
            SIn(".. Saving...");
            if (MuteNotif.ToLower() != "y")
            {
                NewConf.DisableNotifications = true;
            }
            else
            {
                NewConf.DisableNotifications = false;
            }
            Say(" Done!");
        }
        public static void SetAutoBanPrefs()
        {
            Say("4> Anti-flood preferences");
            Say("   Your pmcenter is absolutely not a spambox.");
            Say("");
            SIn("=> Auto ban flooding users? [Y/n]: ");
            string AutoBan = Console.ReadLine();
            SIn(".. Saving...");
            if (AutoBan.ToLower() != "n")
            {
                NewConf.AutoBan = true;
            }
            else
            {
                NewConf.AutoBan = false;
            }
            Say(" Done!");
        }
    }
}