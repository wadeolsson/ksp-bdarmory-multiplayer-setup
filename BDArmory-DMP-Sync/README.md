# BDArmory-DMP-Sync

**Multiplayer Combat Synchronization for KSP**

This plugin bridges DarkMultiPlayer and BDArmory to enable proper combat synchronization in multiplayer.

## Features

✅ **Building Destruction Sync** - See when other players blow up KSC buildings
✅ **Combat Damage Sync** - Vessel damage syncs across all players
✅ **Weapon Fire Visual Sync** - See when remote players fire weapons
✅ **Explosion Sync** - Explosions visible to all players
✅ **Smart Batching** - Efficient network usage, won't flood the server

## What Gets Synced

### Synced ✅
- Building damage and destruction
- Vessel part damage
- Part destruction/death
- Weapon firing states (visual only)
- Explosion events

### Not Synced ❌
- Individual bullets/projectiles (by design - too much network traffic)
- AI behavior (each client runs AI locally)
- Physics calculations (deterministic, calculated locally)

## How It Works

### Building Sync
- Monitors all `DestructibleBuilding` objects every second
- Detects damage changes
- Broadcasts state to all players
- Remote players apply received damage

### Damage Sync
- Hooks into BDArmory's `DamageService`
- Batches damage events (100ms intervals)
- Only syncs significant damage (>0.1 threshold)
- Uses high-priority messages for real-time feel

### Weapon Sync
- Monitors weapon modules for firing state
- Syncs visual effects only (not projectiles)
- Updates remote weapon animations
- Low-priority messages (visual-only)

## Requirements

- Kerbal Space Program 1.12.5
- DarkMultiPlayer v0.3.8.5
- BDArmory Plus v1.12.0.0
- ModuleManager v4.2.2
- Physics Range Extender v1.21.0

## Installation

1. **Build the DLL** (see Building section below)
2. **Copy to GameData**:
   ```
   GameData/
   └── BDArmoryDMPSync/
       ├── Plugins/
       │   └── BDArmoryDMPSync.dll
       └── BDArmoryDMPSync.version
   ```
3. **Launch KSP** - Plugin auto-detects DMP and BDA
4. **All players must have this mod** for sync to work

## Building

### On Mac:

```bash
# Install Mono if not already installed
brew install mono

# Build the project
cd "/Users/wade/Library/Application Support/Steam/steamapps/common/Kerbal Space Program AI/BDArmory-DMP-Sync"
msbuild BDArmoryDMPSync.csproj /p:Configuration=Release
```

### On Windows:

```cmd
# Use Visual Studio or MSBuild
msbuild BDArmoryDMPSync.csproj /p:Configuration=Release
```

The compiled DLL will be in `bin/Release/BDArmoryDMPSync.dll`

## Configuration

No configuration needed! The plugin auto-configures based on:
- DMP player name
- Active vessels
- Combat events

### Performance Tuning

Edit these values in the source if needed:

**BuildingSyncModule.cs:**
- `syncInterval = 1.0f` - How often to check buildings (seconds)

**DamageSyncHandler.cs:**
- `batchInterval = 0.1f` - Damage message batching (seconds)
- `damageThreshold = 0.1f` - Minimum damage to sync

**WeaponFireSyncHandler.cs:**
- `syncInterval = 0.2f` - Weapon state check interval (seconds)

## Troubleshooting

### Plugin Not Loading
- Check `KSP.log` for errors
- Verify all dependencies are installed
- Make sure DMP and BDA are loaded first

### Buildings Not Syncing
- Both players must be in same subspace
- Check DMP connection is stable
- Verify building IDs match (same KSC layout)

### Damage Not Syncing
- Ensure both players are within physics range
- Check vessel is not packed
- Verify part IDs are consistent

### High Network Usage
- Reduce combat intensity (fewer weapons firing)
- Increase batch intervals in code
- Use simpler vessels (fewer parts)

## Technical Details

### Message Types

1. **BuildingDamage** (MessageType.BuildingDamage)
   - BuildingId (string)
   - DamageFraction (float)
   - PlayerName (string)

2. **VesselDamage** (MessageType.VesselDamage)
   - VesselId (Guid)
   - PartFlightId (uint)
   - Damage (float)
   - IsExplosive (bool)
   - PlayerName (string)

3. **WeaponFire** (MessageType.WeaponFire)
   - VesselId (Guid)
   - WeaponName (string)
   - IsFiring (bool)
   - PlayerName (string)

4. **Explosion** (MessageType.Explosion)
   - ExplosionPower (float)
   - Position (Vector3d)
   - PlayerName (string)

### DMP Integration

Uses DMP's `DMPModInterface` API:
- `RegisterUpdateModHandler()` - Receive messages
- `SendDMPModMessage()` - Send messages
- Messages relay through server to all clients
- High-priority for damage, normal for visuals

### BDArmory Hooks

- Reflects into `DamageService` for damage application
- Monitors `DestructibleBuilding` for building damage
- Hooks `GameEvents.onPartDie` for destruction
- Checks weapon module fields for firing state

## Performance

### Network Bandwidth

Typical usage per player:
- Building sync: ~100 bytes/second (1 building damaged)
- Damage sync: ~500 bytes/second (active combat)
- Weapon sync: ~200 bytes/second (visual effects)
- **Total**: ~800 bytes/second during intense combat

### CPU Impact

Minimal overhead:
- Building check: 1 Hz (once per second)
- Damage batch: 10 Hz (10 times per second)
- Weapon check: 5 Hz (5 times per second)

## Known Limitations

1. **Projectiles not synced** - Bullets/rockets calculated locally
2. **Building layout must match** - Custom KSC configs may not sync
3. **Requires same subspace** - Players in different time can't fight
4. **Physics range limits** - Must be within PRE range (use Alt+P)
5. **No retroactive sync** - Damage before connection is not synced

## Credits

- **DarkMultiPlayer** by godarklight
- **BDArmory Plus** by BrettRyland, SuicidalInsanity, and josue
- **BDAMultiplayer** by jrodrigv (inspiration for sync approach)

## License

MIT License - Free to use, modify, and distribute

## Support

For issues and questions:
- Check KSP.log for errors
- Report bugs on GitHub
- See troubleshooting guide in main repo

---

**Version**: 2.0.0
**Compatible with**: KSP 1.12.5, DMP 0.3.8.5, BDA+ 1.12.0.0
**Last Updated**: December 26, 2025
