# KSP Multiplayer Setup Guide

## Installation Complete!

DarkMultiPlayer v0.3.8.5 has been installed for KSP 1.12.5

## How to Play with Your Brothers (Localhost)

### Step 1: Start the Server (One Person Only)
The server files are located at:
```
DMPServer/
```

To start the server, run:
```bash
cd DMPServer
./start-server.sh
```

Or manually:
```bash
cd DMPServer
mono DMPServer.exe
```

The server will start and display connection information. Keep this terminal window open while playing.

### Step 2: Launch KSP (Everyone)
1. Launch Kerbal Space Program normally
2. You'll see a new "DarkMultiPlayer" button on the main menu

### Step 3: Connect to Server (Everyone)
1. Click the DarkMultiPlayer button
2. Enter connection details:
   - **Address**: `127.0.0.1` or `localhost`
   - **Port**: `6702` (default)
   - **Player Name**: Your name
3. Click Connect

### Step 4: Play Together!
- You can all build, launch, and fly vessels together
- Time warp works in "subspace" mode (each player can warp independently)
- Career mode is supported with shared funds

## Important Notes

### For Localhost Play:
- All players must be on the **same computer** OR on the **same local network (LAN)**
- If on the same network, use the host computer's local IP instead of localhost
  - Find your local IP: Open Terminal and run `ipconfig getifaddr en0` (WiFi) or `ipconfig getifaddr en1` (Ethernet)
  - Other players connect to that IP address (e.g., `192.168.1.10`)

### Server Settings
Server configuration files are in `DMPServer/Config/`
- Edit `Settings.txt` to customize server name, MOTD, max players, etc.

### Mod Compatibility
- All players must have the **exact same mods** installed
- Stock game works best for first-time setup
- Test with stock before adding mods

## Troubleshooting

**Can't connect?**
- Make sure the server is running (check the terminal)
- Verify you're using the correct IP address
- Check firewall settings if connecting over LAN

**Game crashes?**
- Make sure all players have identical mod setups
- Try with a fresh save in sandbox mode first

**Weird physics/desyncs?**
- This is normal in multiplayer KSP
- Stay within physics range (2.5km) when possible
- Quicksave often!

## Files Installed

- **Client**: `GameData/DarkMultiPlayer/` - The multiplayer mod
- **Server**: `DMPServer/` - The server application
- **This Guide**: `MULTIPLAYER-SETUP.md`

## Resources

- Official Site: https://d-mp.org/
- GitHub: https://github.com/godarklight/DarkMultiPlayer
- KSP Forum: https://forum.kerbalspaceprogram.com/topic/71656-darkmultiplayer/

Have fun exploring space with your brothers!
