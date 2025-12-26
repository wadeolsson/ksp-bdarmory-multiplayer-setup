# Installation Instructions - Precompiled Version Not Yet Available

## Current Status

The BDArmory-DMP-Sync plugin source code is available on GitHub, but compilation requires fixing some API compatibility issues with the DarkMultiPlayer interface.

## Temporary Solution

While we work on fixing the compilation issues, you have two options:

### Option 1: Use DMP Without the Sync Plugin (Works Now)

You can still play multiplayer with BDArmory, you just won't have:
- Building destruction sync (each player sees their own building states)
- Real-time damage sync (damage might appear delayed)

What DOES work:
- Multiplayer connection
- Vessel synchronization
- Time warp
- Combat (though damage sync isn't perfect)

### Option 2: Wait for Compiled Release

I'm working on fixing the compilation errors and will create a GitHub release with a precompiled DLL soon.

## What Needs Fixing

The plugin code has some issues:
1. DMP callback delegate types need adjustment
2. Part.Damage() method doesn't exist in base KSP
3. Unity Animation module reference needed
4. DMP interface methods might not be static

## For Now: Updated Mod List

**Required Mods (What Actually Works):**
1. ModuleManager v4.2.2 ✅
2. DarkMultiPlayer v0.3.8.5 ✅
3. BDArmory Plus v1.12.0.0 ✅
4. Physics Range Extender v1.21.0 ✅
5. ~~BDArmory-DMP-Sync~~ (Coming Soon)

## Workaround Tips

Without the sync plugin:
- **Communicate via voice** (Discord) about building destruction
- **Stay in same subspace** for better sync
- **Use Alt + P** to increase physics range to 20km
- **Quicksave often** in case of desyncs

---

**Status**: Plugin code uploaded, compilation in progress
**ETA**: Will update when compiled version is ready
