# KSP BDArmory Multiplayer Setup Guide

Complete setup guide for playing Kerbal Space Program with BDArmory combat in multiplayer using DarkMultiPlayer.

## 🚀 What This Is

This repository contains all the documentation, guides, and setup instructions needed to get KSP multiplayer working with BDArmory (weapons/combat mod) for local/LAN play.

## 📋 Quick Links

- **[Main Setup Guide](BDA-MULTIPLAYER-README.md)** - Start here for overview
- **[Installation Guide for Players](BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md)** - Step-by-step mod installation
- **[Troubleshooting Guide](BDA-MULTIPLAYER-TROUBLESHOOTING.md)** - Solutions for common issues
- **[Quick Start](QUICK-START-MULTIPLAYER.txt)** - Fast reference card

## 🎮 What You Need

### Required Mods (Exact Versions):

1. **ModuleManager** v4.2.2
   - Download: https://github.com/sarbian/ModuleManager/releases/tag/4.2.2

2. **DarkMultiPlayer** v0.3.8.5
   - Download: https://d-mp.org/downloads

3. **BDArmory Plus** v1.12.0.0
   - Download: https://spacedock.info/mod/2487/BDArmory%20for%20Runway%20Project/download/1.12.0.0

4. **Physics Range Extender** v1.21.0
   - Download: https://github.com/jrodrigv/PhysicsRangeExtender/releases/tag/1.21.0

5. **BDArmory-DMP-Sync** v1.0.0 🆕
   - Download: https://github.com/wadeolsson/ksp-bdarmory-multiplayer-setup/releases
   - **Essential!** Syncs building destruction, combat damage, and weapon fire

### Game Version:
- **Kerbal Space Program 1.12.5**

## 📦 Installation

### For Server Host:

1. Install all mods to `GameData/` folder
2. Run the DMP server (included in DMP download)
3. Share your local IP with players

### For Clients:

1. Install **identical mods** (same versions!)
2. Connect to host's IP address
3. Port: `6702` (default)

**Detailed instructions**: See [BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md](BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md)

## 🔧 Quick Start

### Server (Host):
```bash
cd DMPServer
./start-server.sh
```

Find your IP:
```bash
ipconfig getifaddr en0
```

### Clients:
1. Launch KSP
2. Click "DarkMultiPlayer"
3. Add server with host's IP
4. Port: 6702
5. Connect!

## ❓ Common Issues

### "Connection Refused"
- Server not running or wrong IP address
- Check firewall settings

### "Unhandled error while syncing!"
- Mod mismatch between players
- Verify all players have identical mod versions

### "Vessels disappear when far away"
- Physics Range Extender not installed
- Press `Alt + P` in game to increase range

**Full troubleshooting**: [BDA-MULTIPLAYER-TROUBLESHOOTING.md](BDA-MULTIPLAYER-TROUBLESHOOTING.md)

## 🎯 Gameplay Tips

- **No time warp during combat** - causes physics desync
- **Stay within physics range** - Default 2.5km, increase with PRE (Alt + P)
- **Keep vessels simple** - Under 100 parts for best performance
- **Use voice chat** - Discord, etc. for coordination
- **Quicksave often** - Multiplayer can be unpredictable

## 📖 Documentation Files

- `BDA-MULTIPLAYER-README.md` - Complete overview and setup summary
- `BDA-MULTIPLAYER-SETUP-FOR-BROTHER.md` - Detailed installation guide
- `BDA-MULTIPLAYER-TROUBLESHOOTING.md` - Problem solving
- `MULTIPLAYER-SETUP.md` - General DMP setup
- `QUICK-START-MULTIPLAYER.txt` - Quick reference

## 🌐 Resources

- **DMP Official**: https://d-mp.org/
- **DMP Forums**: https://forum.kerbalspaceprogram.com/topic/71656-darkmultiplayer/
- **BDA Forums**: https://forum.kerbalspaceprogram.com/forum/107-add-on-discussions/
- **PRE GitHub**: https://github.com/jrodrigv/PhysicsRangeExtender

## 🤝 Contributing

This is a personal setup guide, but feel free to:
- Report issues with the documentation
- Suggest improvements
- Share your own multiplayer tips

## 📝 License

Documentation only - CC0 (Public Domain)

Mods are licensed by their respective authors:
- DarkMultiPlayer: MIT License
- BDArmory: GNU GPL
- Physics Range Extender: MIT License
- ModuleManager: CC-BY-SA

## ✨ Credits

- **DarkMultiPlayer** by godarklight and contributors
- **BDArmory** by BahamutoD and community
- **Physics Range Extender** by jrodrigv
- **ModuleManager** by sarbian

## 🚀 Have Fun!

Ready for epic space dogfights! May your missiles fly true and your landings be (mostly) intact!

---

*For support, see the troubleshooting guide or visit the official mod forums.*
