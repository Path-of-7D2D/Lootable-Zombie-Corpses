# Lootable Zombie Corpses

Lootable Zombie Corpses is a 7 Days To Die V3.0 XML port of CHAR0NTR's Lootable Zombie Corpse mod.

The mod keeps zombie corpses in the world long enough to be looted, adds corpse loot lists for vanilla zombie archetypes, and keeps zombie harvest drops focused on rotting flesh and bones.

## Features

- Makes standard zombie corpses persist for 600 seconds.
- Adds `LootListOnDeath` entries to normal, feral, and radiated zombie variants.
- Provides dedicated loot containers for individual zombie archetypes.
- Keeps harvest drops for rotting flesh and bones on patched zombie corpses.
- Preserves the source mod's easy, normal, and hard loot table variants in `loot.xml`.
- Runs as an XML-only modlet with no DLL.

## V3 Port Notes

- Updated the deployable folder to `1A-LootableZombieCorpses`.
- Updated `ModInfo.xml` metadata for 7 Days To Die V3.0.
- Ported legacy `loot_prob` item attributes to the V3 `prob` attribute.
- Retained the source mod's custom zombie probability templates under `lootprobtemplates`.
- Tightened the old Arlene XPath from broad `contains(...)` matches to exact entity targets so variants are not patched twice.

## Requirements

- 7 Days To Die V3.0.
- No DLL required.

Easy Anti-Cheat can stay enabled because this is an XML-only modlet.

## Installation

1. Download the latest `LootableZombieCorpses-*.zip` from the [GitHub Releases](https://github.com/Path-of-7D2D/Lootable-Zombie-Corpses-Port/releases) page.
2. Extract the release zip.
3. Copy the `1A-LootableZombieCorpses` folder into your `Mods` folder:

```text
7 Days To Die/Mods/1A-LootableZombieCorpses/
```

The folder is installed correctly when this file exists:

```text
7 Days To Die/Mods/1A-LootableZombieCorpses/ModInfo.xml
```

## Multiplayer

Install the mod on the server and every connecting client so corpse loot behavior and loot table definitions stay consistent.

## Compatibility

This mod patches `entityclasses.xml` and appends loot containers in `loot.xml`.

It may conflict with another mod that changes zombie corpse lifetime, zombie harvest drops, zombie `LootListOnDeath` properties, or loot containers with the same names.

## Testing

1. Install `1A-LootableZombieCorpses` into the game's `Mods` folder.
2. Start a test world.
3. Spawn or find several vanilla zombie archetypes.
4. Kill each zombie and confirm the corpse remains long enough to interact with.
5. Loot the corpse and confirm a zombie-specific loot container opens.
6. Harvest the corpse with a butcher-capable tool and confirm rotting flesh and bones can drop.

## Releasing

The release workflow is manual. Run the `Release` GitHub Action with a `version_tag` such as `v3.0.0` or `3.0.0`.

The workflow validates the deployable `1A-LootableZombieCorpses` folder, creates a `LootableZombieCorpses-<version>.zip`, generates release notes with `Path-of-7D2D/Changelog-Generator`, and publishes the GitHub release.
