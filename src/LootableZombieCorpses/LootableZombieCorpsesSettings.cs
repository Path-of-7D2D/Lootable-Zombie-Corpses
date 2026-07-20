using System;
using System.IO;
using System.Xml.Linq;

namespace LootableZombieCorpses
{
    internal static class LootableZombieCorpsesSettings
    {
        private const string SettingsFileName = "LootableZombieCorpses.xml";

        internal static bool EnableCorpseLooting { get; private set; } = true;

        internal static void Load(Mod mod)
        {
            EnableCorpseLooting = true;

            string modPath = mod?.Path;
            if (string.IsNullOrEmpty(modPath))
            {
                Log.Warning("[LootableZombieCorpses] Mod path unavailable; corpse looting remains enabled.");
                return;
            }

            string settingsPath = Path.Combine(modPath, SettingsFileName);
            if (!File.Exists(settingsPath))
            {
                Log.Warning("[LootableZombieCorpses] " + SettingsFileName
                    + " not found; corpse looting remains enabled.");
                return;
            }

            try
            {
                XDocument document = XDocument.Load(settingsPath);
                string rawValue = document.Root?
                    .Element("EnableCorpseLooting")?
                    .Attribute("value")?
                    .Value;

                if (bool.TryParse(rawValue, out bool enabled))
                {
                    EnableCorpseLooting = enabled;
                }
                else if (rawValue == "1" || rawValue == "0")
                {
                    EnableCorpseLooting = rawValue == "1";
                }
                else
                {
                    Log.Warning("[LootableZombieCorpses] Invalid EnableCorpseLooting value in "
                        + SettingsFileName + "; corpse looting remains enabled.");
                }
            }
            catch (Exception ex)
            {
                Log.Warning("[LootableZombieCorpses] Failed to read " + SettingsFileName
                    + "; corpse looting remains enabled. " + ex.Message);
            }
        }
    }
}
