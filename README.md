# Lootable Zombie Corpses

Butcher zombies for bones and rotting flesh

The mod keeps zombie corpses in the world long enough to be looted, adds corpse loot lists for vanilla zombie archetypes, and keeps zombie harvest drops focused on rotting flesh and bones.

## Features

- Makes standard zombie corpses persist for 600 seconds.
- You can now get bones and rotting flesh from zombie corpses by butchering them

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
