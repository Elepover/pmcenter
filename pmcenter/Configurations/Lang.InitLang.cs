using System;
using System.IO;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Lang
    {
        public static async Task InitLang()
        {
            Log("Checking language file's integrity...", "LANG");
            if (!File.Exists(Vars.LangFile))
            { // STEP 1, DETECT EXISTENCE.
                Log("Language file not found. Creating...", "LANG", LogLevel.Warning);
                Vars.CurrentLang = new Language();
                _ = await SaveLang(true).ConfigureAwait(false); // Then the app will exit, do nothing.
            }
            else
            { // STEP 2, READ TEST.
                try
                {
                    _ = await ReadLang(false).ConfigureAwait(false); // Read but don't apply.
                }
                catch (Exception ex)
                {
                    Log($"Error! {ex}", "LANG", LogLevel.Error);
                    Log("Moving old language file to \"pmcenter_locale.json.bak\"...", "LANG", LogLevel.Warning);
                    File.Move(Vars.LangFile, Vars.LangFile + ".bak");
                    Vars.CurrentLang = new Language();
                    _ = await SaveLang(true).ConfigureAwait(false); // Then the app will exit, do nothing.
                }
            }
            Log("Integrity test finished!", "LANG");
        }
    }
}
