# BDArmory Multiplayer - Installation Guide for Brother

## Required Mods (MUST Match Exactly!)

For multiplayer to work, you need the **EXACT SAME MODS** as Wade. Here's the complete list:

### 1. ModuleManager v4.2.2 ✅
**What it does**: Manages mod configurations
**Download**: https://github.com/sarbian/ModuleManager/releases/tag/4.2.2
**File**: `ModuleManager.4.2.2.dll`
**Installation**: Copy to `GameData/` folder (root level, NOT in a subfolder)

### 2. DarkMultiPlayer v0.3.8.5 ✅
**What it does**: Enables multiplayer
**Download**: https://d-mp.org/downloads (get v0.3.8.5 CLIENT)
**Installation**:
1. Download `DMPClient.zip`
2. Extract it
3. Copy the `GameData/DarkMultiPlayer` folder to your `GameData/` folder

### 3. BDArmory Plus v1.12.0.0 ✅
**What it does**: Weapons, combat, and dogfighting
**Download**: https://spacedock.info/mod/2487/BDArmory%20for%20Runway%20Project/download/1.12.0.0
**Installation**: Copy `BDArmory` folder to your `GameData/` folder

### 4. Physics Range Extender v1.21.0 ✅
**What it does**: Extends physics range so you can dogfight beyond 2.5km
**Download**: https://github.com/jrodrigv/PhysicsRangeExtender/releases/tag/1.21.0
**Installation**: Copy `PhysicsRangeExtender` folder to your `GameData/` folder

### 5. BDArmory-DMP-Sync v1.0.0 ✅ **NEW!**
**What it does**: Syncs building destruction, combat damage, and weapon fire in multiplayer
**Download**: https://github.com/wadeolsson/ksp-bdarmory-multiplayer-setup/releases
**Installation**:
1. Download `BDArmoryDMPSync.zip`
2. Extract it
3. Copy `BDArmoryDMPSync` folder to your `GameData/` folder

**Why you need this**: Without this plugin, building destruction and combat damage won't sync between players. With it, you'll see when your brother blows stuff up!

---

## Step-by-Step Installation

### Step 1: Find Your GameData Folder
**Windows**: `C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\GameData\`
**Mac**: `~/Library/Application Support/Steam/steamapps/common/Kerbal Space Program/GameData/`
**Linux**: `~/.steam/steam/steamapps/common/Kerbal Space Program/GameData/`

### Step 2: Download All Mods
Download each mod from the links above. Make sure you get the **exact versions** listed!

### Step 3: Install Each Mod

**ModuleManager**:
- Place `ModuleManager.4.2.2.dll` directly in the `GameData/` folder

**DarkMultiPlayer**:
- Extract DMPClient.zip
- Copy `GameData/DarkMultiPlayer/` folder into your `GameData/` folder

**BDArmory**:
- Copy the `BDArmory/` folder into your `GameData/` folder

**Physics Range Extender**:
- Copy the `PhysicsRangeExtender/` folder into your `GameData/` folder

### Step 4: Verify Your Installation

Your `GameData/` folder should look like this:
```
GameData/
├── ModuleManager.4.2.2.dll
├── Squad/  (stock game files)
├── DarkMultiPlayer/
│   └── Plugins/
│       └── DarkMultiPlayer.dll
├── BDArmory/
│   ├── Parts/
│   ├── Plugins/
│   └── ...
└── PhysicsRangeExtender/
    └── Plugins/
        └── PhysicsRangeExtender.dll
```

### Step 5: Launch KSP and Test

1. Start Kerbal Space Program
2. Check the loading screen - you should see all mods loading
3. On the main menu, you should see a "DarkMultiPlayer" button
4. Check the log file (KSP.log) for any errors

---

## Connecting to Wade's Server

### Connection Details:
- **Server Address**: `192.168.1.181` (Wade's local IP)
- **Port**: `6702`
- **Player Name**: Your name

### How to Connect:
1. Launch KSP
2. Click "DarkMultiPlayer" button
3. Click "Add" to add a new server
4. Fill in:
   - **Name**: "Wade's Server" (or whatever you want to call it)
   - **Address**: `192.168.1.181`
   - **Port**: `6702`
5. Click "Add" to save
6. Select the server from the list
7. Enter your **Player Name**
8. Click "Connect"

### Important Notes:
- You must be on the **same WiFi network** as Wade
- Wade needs to have the server running first (he runs `./start-server.sh` in Terminal)
- If connection fails, check Wade's current IP address (it might change)

---

## Troubleshooting

### "Connection Refused"
- Make sure Wade's server is running
- Verify you're using the correct IP address (`192.168.1.181`)
- Make sure you're on the same WiFi network

### "Unhandled error while syncing!"
- This means your mods don't match Wade's exactly
- Double-check all mod versions
- Make sure you have ALL four mods installed

### "Mod version mismatch"
- You have a different version of a mod than Wade
- Re-download the exact versions listed above

### Game Crashes on Load
- Check KSP.log for errors
- Make sure ModuleManager loads first (it should be directly in GameData/)
- Try removing mods one by one to find the problem

### Can't See DarkMultiPlayer Button
- DarkMultiPlayer didn't load
- Check that `GameData/DarkMultiPlayer/Plugins/DarkMultiPlayer.dll` exists
- Check KSP.log for loading errors

---

## Quick Verification Checklist

Before connecting, verify:
- [ ] ModuleManager.4.2.2.dll is in GameData/
- [ ] DarkMultiPlayer folder is in GameData/
- [ ] BDArmory folder is in GameData/
- [ ] PhysicsRangeExtender folder is in GameData/
- [ ] KSP launches without errors
- [ ] "DarkMultiPlayer" button appears on main menu
- [ ] You're on the same WiFi as Wade
- [ ] Wade's server is running

---

## Alternative: Copy Mods from Wade's Computer

If downloads are difficult, Wade can copy his entire mod setup to a USB drive:

**What Wade needs to copy**:
1. From `GameData/`:
   - `ModuleManager.4.2.2.dll`
   - `DarkMultiPlayer/` folder
   - `BDArmory/` folder
   - `PhysicsRangeExtender/` folder

**What you do**:
1. Copy those files/folders from the USB
2. Paste them into your `GameData/` folder
3. Launch KSP

This ensures 100% identical mod installations!

---

## Need Help?

If you're stuck:
1. Check KSP.log file for errors
2. Ask Wade to verify his server is running
3. Double-check all mod versions match exactly
4. Try with just DarkMultiPlayer first (remove BDA temporarily) to isolate issues

Good luck and have fun dogfighting in space! 🚀
