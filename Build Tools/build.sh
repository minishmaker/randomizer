#!/usr/bin/env bash
set -euo pipefail
# Simple cross-platform build script for Avalonia UI and CLI
# Usage: ./build.sh <output-dir>

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <output-dir>"
  exit 1
fi
OUTDIR="$1"
SOLUTION_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$SOLUTION_DIR"

# Build UI (Avalonia) self-contained single-file
publish_ui(){
  local rid="$1"
  local dest="$OUTDIR/UI/$rid"
  dotnet publish MinishCapRandomizerUI.Avalonia/MinishCapRandomizerUI.Avalonia.csproj -c Release -r "$rid" \
    -p:PublishSingleFile=true -p:SelfContained=true -p:PublishTrimmed=true -p:TrimMode=partial \
    -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "$dest"
}

# Build CLI
publish_cli(){
  local rid="$1"
  local dest="$OUTDIR/CLI/$rid"
  dotnet publish MinishCapRandomizerCLI/MinishCapRandomizerCLI.csproj -c Release -r "$rid" \
    -p:PublishSingleFile=true -p:SelfContained=true -o "$dest"
}

for rid in linux-x64 linux-arm64 win-x64 win-arm64; do
  publish_ui "$rid"
  publish_cli "$rid"
done

echo "Builds published to $OUTDIR"

