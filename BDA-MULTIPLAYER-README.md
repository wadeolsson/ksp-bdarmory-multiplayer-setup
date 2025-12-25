# BDArmory Multiplayer Setup - COMPLETE! ✅

## Installation Summary

All required mods for BDArmory multiplayer combat are now installed!

### Installed Mods:

1. ✅ **ModuleManager v4.2.2** - Mod configuration manager
2. ✅ **DarkMultiPlayer v0.3.8.5** - Multiplayer framework
3. ✅ **BDArmory v0.11.0.1** - Weapons and combat
4. ✅ **Physics Range Extender v1.21.0** - Extended physics range for combat

### File Locations:

```
GameData/
├── ModuleManager.4.2.2.dll ✅
├── DarkMultiPlayer/ ✅
├── BDArmory/ ✅
└── PhysicsRangeExtender/ ✅
```

---

## Quick Start Guide

### For YOU (Server Host):

**1. Start the server:**
```bash
cd DMPServer
./start-server.sh
```

**2. Find your IP:**
```bash
ipconfig getifaddr en0
```
Result: `192.168.1.181` (your current IP)

**3. Launch KSP and connect:**
- Click "DarkMultiPlayer"
- Add server: `localhost` or `127.0.0.1`
- Port: `6702`
- Connect!

### For YOUR BROTHER:

**1. He needs to install the same mods** - See: [BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md](BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md)

**2. Connect to your server:**
- Address: `192.168.1.181` (your IP)
- Port: `6702`

---

## Important Documents Created:

1. **[BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md](BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md)**
   - Complete installation guide for your brother
   - Download links for all mods
   - Step-by-step instructions
   - Verification checklist

2. **[BDA-MULTIPLAYER-TROUBLESHOOTING.md](BDA-MULTIPLAYER-TROUBLESHOOTING.md)**
   - Solutions for "Connection Refused"
   - Fixes for "Unhandled error while syncing!"
   - Physics desync solutions
   - Performance optimization tips
   - Server configuration guide

3. **[MULTIPLAYER-SETUP.md](MULTIPLAYER-SETUP.md)**
   - General DarkMultiPlayer setup guide
   - Basic troubleshooting

4. **[QUICK-START-MULTIPLAYER.txt](QUICK-START-MULTIPLAYER.txt)**
   - Quick reference card

---

## What Changed?

### The Problem:
You were getting **"Unhandled error while syncing!"** because:
- Physics Range Extender was missing (required for BDArmory multiplayer)
- Without it, vessels beyond 2.5km go "on rails" and can't engage in combat

### The Solution:
- Installed Physics Range Extender v1.21.0
- Copied ModuleManager v4.2.2 (was missing in this installation)
- Created comprehensive guides for your brother

---

## Next Steps:

1. **Test your setup:**
   - Launch KSP
   - Check for mod loading errors
   - Connect to `localhost` first

2. **Share with brother:**
   - Send him `BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md`
   - Or copy your `GameData/` mods to USB drive for him

3. **Start small:**
   - Test connection with stock craft first
   - Then try simple armed planes
   - Build up to epic battles!

---

## Physics Range Extender Controls:

Once in-game, press **Alt + P** to open Physics Range Extender:
- Default range: 2.5km (vanilla KSP)
- Recommended for combat: 10-20km
- Higher range = more CPU usage

Both players should use similar PRE settings!

---

## Tips for Best Experience:

✅ **DO:**
- Use voice chat (Discord)
- Coordinate time warps
- Keep vessels under 100 parts
- Quicksave often
- Start in Sandbox mode

❌ **DON'T:**
- Time warp during combat
- Build massive lag-inducing vessels
- Mix different mod versions

---

## Troubleshooting Quick Reference:

| Problem | Solution |
|---------|----------|
| Connection Refused | Check server is running, verify IP |
| Unhandled sync error | Verify identical mods on both machines |
| Vessels disappear | Increase Physics Range (Alt + P) |
| Missiles don't sync | Stay within physics range, no time warp |
| High lag | Reduce vessel complexity, lower PRE range |

For detailed solutions, see [BDA-MULTIPLAYER-TROUBLESHOOTING.md](BDA-MULTIPLAYER-TROUBLESHOOTING.md)

---

## Mod Versions (For Reference):

Share this list with your brother so he gets the exact versions:

- **ModuleManager**: v4.2.2
- **DarkMultiPlayer**: v0.3.8.5
- **BDArmory**: v0.11.0.1
- **Physics Range Extender**: v1.21.0
- **KSP Version**: 1.12.5

---

## Support Resources:

- **DMP Official Site**: https://d-mp.org/
- **DMP Forum**: https://forum.kerbalspaceprogram.com/topic/71656-darkmultiplayer/
- **BDA Forum**: https://forum.kerbalspaceprogram.com/forum/107-add-on-discussions/
- **PRE GitHub**: https://github.com/jrodrigv/PhysicsRangeExtender

---

## Ready to Play!

Everything is set up and ready for multiplayer BDArmory combat! 🚀💥

Start the server, get your brother connected, and enjoy some epic dogfights in space!

May your missiles fly true and your landings be (mostly) intact! 🎯
