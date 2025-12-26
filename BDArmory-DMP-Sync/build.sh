#!/bin/bash
# Build script for BDArmory-DMP-Sync

set -e

echo "Building BDArmory-DMP-Sync..."

# Check if mono is installed
if ! command -v msbuild &> /dev/null; then
    echo "ERROR: msbuild not found. Please install Mono:"
    echo "  brew install mono"
    exit 1
fi

# Get the directory of this script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR"

# Clean previous build
echo "Cleaning previous build..."
rm -rf bin obj

# Build the project
echo "Compiling..."
msbuild BDArmoryDMPSync.csproj /p:Configuration=Release /v:minimal

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build successful!"
    echo ""
    echo "Output: bin/Release/BDArmoryDMPSync.dll"
    echo ""
    echo "To install:"
    echo "  1. Create folder: GameData/BDArmoryDMPSync/Plugins/"
    echo "  2. Copy: bin/Release/BDArmoryDMPSync.dll"
    echo "  3. Copy: BDArmoryDMPSync.version"
    echo ""
else
    echo "❌ Build failed!"
    exit 1
fi
