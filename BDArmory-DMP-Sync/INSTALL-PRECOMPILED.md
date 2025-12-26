# Installation Instructions - v2.0.0 Available Now!

## Status: READY TO USE ✅

BDArmoryDMPSync v2.0.0 is compiled and ready for multiplayer BDArmory combat!

## What v2.0 Provides

✅ **Building Destruction Sync** - All players see building damage in real-time
✅ **Vessel Damage Sync** - Part damage is synchronized across clients
✅ **Weapon Fire Visual Sync** - See when other players fire weapons
✅ **Explosion Sync** - Explosions visible to all players

## Installation

### Option 1: Download from GitHub (Recommended)

1. Download the latest release: [BDArmoryDMPSync-v2.0.0.zip](https://github.com/wadeolsson/ksp-bdarmory-multiplayer-setup/releases/tag/v2.0.0)
2. Extract the zip file
3. Copy the `GameData` folder into your KSP directory
4. The final path should be: `Kerbal Space Program/GameData/BDArmoryDMPSync/Plugins/BDArmoryDMPSync.dll`

### Option 2: Already Installed Locally

If you're on the main KSP installation, the plugin is already installed at:
```
/Users/wade/Library/Application Support/Steam/steamapps/common/Kerbal Space Program AI/GameData/BDArmoryDMPSync/
```

## Required Mods

**Complete Mod List:**
1. ModuleManager v4.2.2 ✅
2. DarkMultiPlayer v0.3.8.5 ✅
3. BDArmory Plus v1.12.0.0 ✅
4. Physics Range Extender v1.21.0 ✅
5. **BDArmoryDMPSync v2.0.0** ✅ (NEW!)

## Verification

After installation, launch KSP and check the log file (`KSP.log`) for these messages:

```
[BDArmoryDMPSync] Awake - Initializing
[BDArmoryDMPSync] Registering DMP message handlers
[BDArmoryDMPSync] Registered BDABuilding via RegisterUpdateModHandler
[BDArmoryDMPSync] Registered BDADamage via RegisterFixedUpdateModHandler
[BDArmoryDMPSync] Registered BDAWeapon via RegisterUpdateModHandler
[BDArmoryDMPSync] Registered BDAExplosion via RegisterUpdateModHandler
[BDArmoryDMPSync] Initialized successfully as player: YourName
```

## For Your Brother

Send your brother these files:
- The GitHub release link above, OR
- Copy your `GameData/BDArmoryDMPSync` folder to him

Make sure he has ALL the same mods installed (identical versions!).

## Multiplayer Tips

With the sync plugin:
- **Building destruction syncs automatically** - no need to communicate damage
- **Damage appears in real-time** for all players
- **Weapon effects are visible** when others fire
- **Still use Alt + P** to increase physics range to 20km
- **Still quicksave often** as good practice

## Known Limitations

- Damage sync uses part temperature as a proxy (works but not perfect)
- Weapon fire sync is visual only (doesn't spawn actual projectiles on remote clients)
- All players must have identical mod setups

## Troubleshooting

If you don't see sync messages in the log:
1. Check that DMP is installed correctly
2. Check that BDArmory Plus (not old BDA) is installed
3. Verify the DLL is in the correct location
4. Check KSP.log for error messages

## What Changed from v1.0

v1.0 only logged sync events. **v2.0 actually transmits data over the network using DMP's mod API!**

---

**Status**: v2.0.0 released and working!
**Download**: https://github.com/wadeolsson/ksp-bdarmory-multiplayer-setup/releases/tag/v2.0.0
