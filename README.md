# Lootable Zombie Corpses

Butcher zombies for bones and rotting flesh

The mod keeps zombie corpses in the world long enough to be searched, adds corpse loot lists for vanilla zombie archetypes, and keeps zombie harvest drops focused on rotting flesh and bones.

## Features

- Makes standard zombie corpses persist for 600 seconds.
- Adds an interact/search prompt to supported zombie corpses.
- You can get bones and rotting flesh from zombie corpses by butchering them.

## Configuration

Edit `LootableZombieCorpses.xml` in the installed mod folder and restart the game or server after changing it.

```xml
<EnableCorpseLooting value="false" />
```

Setting this to `false` removes the corpse search interaction. Zombie corpses can still be harvested for bones and rotting flesh. The default is `true`.

## Requirements

- 7 Days To Die V3.0.
- The game's bundled `0_TFP_Harmony` mod.

This mod uses Harmony, so Easy Anti-Cheat must be off. The mod is marked with `SkipWithAntiCheat`.

## Installation

1. Download the latest `LootableZombieCorpses-*.zip` from the [GitHub Releases](https://github.com/Path-of-7D2D/Lootable-Zombie-Corpses/releases) page.
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

Use the same `EnableCorpseLooting` value on the server and every client.

Players seeing `This item is in use` on every corpse are usually running a stale client or server build. Update the server and all clients to the same current release.

## Compatibility

This mod patches `entityclasses.xml`, appends loot containers in `loot.xml`, and uses Harmony to expose the V3 search interaction on marked dead zombies.

It may conflict with another mod that changes zombie corpse lifetime, zombie harvest drops, zombie `LootList` properties, entity activation behavior, or loot containers with the same names.
