# BDArmory + DarkMultiPlayer Troubleshooting Guide

## Common Connection Issues

### ❌ "Connection Refused"

**Cause**: Server isn't running or wrong IP address

**Solutions**:
1. **Verify server is running**:
   ```bash
   cd "/Users/wade/Library/Application Support/Steam/steamapps/common/Kerbal Space Program AI/DMPServer"
   ./start-server.sh
   ```
   You should see: "Starting DarkMultiPlayer Server..."

2. **Check server IP** (changes sometimes):
   ```bash
   ipconfig getifaddr en0
   ```
   Make sure your brother is using the correct IP

3. **Test connection from your own machine first**:
   - Try connecting to `localhost` or `127.0.0.1`
   - If YOU can't connect, the server has a problem
   - If you CAN connect, it's a network issue

4. **Firewall check**:
   - Mac Firewall might be blocking port 6702
   - Go to: System Preferences → Security & Privacy → Firewall → Firewall Options
   - Make sure DMPServer or mono is allowed

---

### ❌ "Unhandled error while syncing!"

**Cause**: Mod mismatch between players

**Solutions**:
1. **Verify EXACT mod versions**:
   - Check `GameData/` folder on both machines
   - ALL mods must be identical versions
   - Even minor version differences cause sync errors

2. **Most common culprits**:
   - Missing Physics Range Extender on one machine
   - Different BDArmory versions
   - One player has extra mods the other doesn't

3. **The nuclear option** (guaranteed fix):
   - Copy Wade's entire `GameData/` folder to USB drive
   - Replace brother's `GameData/` with Wade's copy
   - This ensures 100% identical setup

4. **Check KSP.log**:
   - Look for mod loading errors
   - Compare loaded mods list between both players

---

### ❌ "Mod Version Mismatch" Error

**Cause**: DarkMultiPlayer detected different mod versions

**Solution**:
- The error message will tell you which mod is mismatched
- Download the EXACT version specified
- Both players must match versions perfectly

---

## Physics & Combat Issues

### 🎯 Vessels Disappear When Far Away

**Cause**: Physics range limit (vanilla KSP is 2.5km)

**Solution**:
- Install Physics Range Extender (v1.21.0) on BOTH machines
- Verify it's loading: Check KSP.log for "PhysicsRangeExtender" entry

**Adjust range in-game**:
1. Launch KSP
2. Press `Alt + P` (default keybind)
3. Physics Range Extender window opens
4. Increase range (default is 2.5km, try 10km-20km for combat)
5. Note: Higher range = more CPU usage

---

### 🎯 Missiles/Bullets Not Syncing

**Cause**: Physics desync between players

**Solutions**:
1. **Stay within physics range**:
   - Both vessels must be within the PRE range
   - Increase PRE range if needed

2. **Reduce time warp**:
   - Don't time warp during combat
   - Can cause massive desync issues

3. **Server subspace settings**:
   - DMP uses "subspace" time system
   - Both players should be in the same subspace
   - Check the time warp status in DMP UI

---

### 🎯 Vessels "Jittering" or Rubber-banding

**Cause**: Network lag or physics updates not syncing

**Solutions**:
1. **Check network connection**:
   - WiFi can be unstable
   - Try wired connection if possible
   - Ping the server: `ping 192.168.1.181`

2. **Reduce physics load**:
   - Smaller vessels = better sync
   - Fewer parts = less network data
   - Turn down part count on vessels

3. **Increase DMP update rate**:
   - Edit `DMPServer/Config/Settings.txt`
   - Look for update interval settings
   - Lower = more frequent updates = smoother (but more bandwidth)

---

### 🎯 Explosions Don't Sync Properly

**Cause**: BDA explosions are clientside by default

**Solution**:
- This is a known BDA limitation in multiplayer
- Each player sees slightly different explosions
- Damage IS synced, just the visual effects aren't

---

## Performance Issues

### 🐌 Game Runs Slow in Multiplayer

**Solutions**:
1. **Reduce Physics Range**:
   - Lower PRE range (Alt + P)
   - Try 5km instead of 20km

2. **Limit vessel part count**:
   - Smaller, simpler vessels
   - BDA weapons are lightweight - use those

3. **Close background apps**:
   - KSP + mods + multiplayer = heavy CPU usage

4. **Check RAM usage**:
   - KSP can use 4-8GB+ with mods
   - Close Chrome/other memory hogs

---

### 🐌 High Network Lag

**Solutions**:
1. **Use wired connection** instead of WiFi
2. **Reduce vessel complexity** (fewer parts to sync)
3. **Stay in same subspace** (don't time warp independently)
4. **Check other network activity** (downloads, streaming, etc.)

---

## Server Issues

### 🖥️ Server Crashes or Won't Start

**Solutions**:
1. **Check Mono is installed**:
   ```bash
   which mono
   ```
   Should return: `/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono`

2. **Check server logs**:
   - Look in `DMPServer/Logs/` folder
   - Recent errors will be at the end

3. **Delete cache and restart**:
   ```bash
   cd DMPServer
   rm -rf Cache/
   ./start-server.sh
   ```

4. **Port already in use**:
   - Another process might be using port 6702
   - Find and kill it:
     ```bash
     lsof -i :6702
     kill -9 <PID>
     ```

---

### 🖥️ "Server Full" Error

**Cause**: Max player limit reached

**Solution**:
- Edit `DMPServer/Config/Settings.txt`
- Change `maxPlayers = X` to higher number
- Restart server

---

## Best Practices for Smooth Multiplayer

### ✅ Combat Guidelines

1. **Start close together**:
   - Launch from same runway/launchpad
   - Easier to stay in sync

2. **No time warp during combat**:
   - Causes massive desync
   - Only warp when traveling to combat zone

3. **Use Alt + P to increase physics range**:
   - Set to 10-20km for dogfighting
   - Both players should match settings

4. **Keep vessels simple**:
   - Under 100 parts if possible
   - More parts = more lag = more desync

5. **Quicksave often**:
   - Things can glitch
   - Both players should save at same time

---

### ✅ General Multiplayer Tips

1. **Use voice chat** (Discord, etc.):
   - Coordinate time warps
   - Call out maneuvers
   - Way more fun!

2. **Start with Sandbox mode**:
   - Easier than Career
   - Focus on fun, not grinding

3. **Test with simple crafts first**:
   - Basic planes before complex bombers
   - Verify sync works before big battles

4. **Both players same FPS**:
   - If one player has 20fps and other has 60fps
   - Can cause physics desync
   - Match graphics settings

---

## Advanced: Server Configuration

**Location**: `DMPServer/Config/Settings.txt`

### Useful Settings:

```ini
# Maximum players
maxPlayers = 8

# Warp mode: SUBSPACE or MASTER
warpMode = SUBSPACE

# Auto-save interval (seconds)
autoSaveInterval = 300

# Screenshot interval
screenshotInterval = 600

# Message of the day
serverMotd = Welcome to Wade's BDA Combat Server!

# Whitelist mode (false = anyone can join)
whitelisted = false
```

After changing settings, restart the server.

---

## Still Having Issues?

### Diagnostic Checklist:

1. [ ] Both players have identical mod versions?
2. [ ] Server is running and showing both players connected?
3. [ ] Both on same WiFi network?
4. [ ] Physics Range Extender installed on both machines?
5. [ ] No errors in KSP.log files?
6. [ ] Can connect with `localhost` first (host player)?
7. [ ] Firewall allowing connections?
8. [ ] Server IP address correct (hasn't changed)?

### Get Help:

- **KSP.log location**:
  - Mac: `/Users/wade/Library/Application Support/Steam/steamapps/common/Kerbal Space Program/KSP.log`
  - Windows: `C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\KSP.log`

- **Server log location**: `DMPServer/Logs/`

- **DMP Forums**: https://forum.kerbalspaceprogram.com/topic/71656-darkmultiplayer/

- **BDA Forums**: https://forum.kerbalspaceprogram.com/forum/107-add-on-discussions/

---

## Quick Reference: Keyboard Shortcuts

**DarkMultiPlayer**:
- `F2` - Toggle DMP UI
- `F8` - Open chat

**BDArmory**:
- `Alt + B` - Open BDA menu
- `Alt + T` - Toggle turret mode
- `G` - Fire guns (when armed)

**Physics Range Extender**:
- `Alt + P` - Open PRE menu

---

Good luck and may your missiles fly true! 🚀💥
