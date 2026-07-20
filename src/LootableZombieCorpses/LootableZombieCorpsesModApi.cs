using System;
using HarmonyLib;
using UnityEngine.Scripting;

namespace LootableZombieCorpses
{
    [Preserve]
    public class LootableZombieCorpsesModApi : IModApi
    {
        public void InitMod(Mod _modInstance)
        {
            LootableZombieCorpsesSettings.Load(_modInstance);

            if (LootableZombieCorpsesSettings.EnableCorpseLooting
                && !CorpseInteraction.IsRuntimeReady)
            {
                throw new MissingMethodException(
                    "Could not find the V3 entity lock method required for corpse search.");
            }

            new Harmony("com.pathof7d2d.lootablezombiecorpses")
                .PatchAll(typeof(LootableZombieCorpsesModApi).Assembly);

            Log.Out("[LootableZombieCorpses] V3 corpse search interaction loaded. Corpse looting: "
                + (LootableZombieCorpsesSettings.EnableCorpseLooting ? "enabled" : "disabled") + ".");
        }
    }
}
