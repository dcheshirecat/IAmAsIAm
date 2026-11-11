#!/usr/bin/env bash
# Compiles and auto-plays the Ink story (see Program.cs). Writes Assets/_Project/Story/main.json.
set -euo pipefail
here="$(cd "$(dirname "$0")" && pwd)"
rm -rf "$here/.ink-src" && mkdir -p "$here/.ink-src"
tar -xzf "$here/../../Packages/com.inkle.ink-unity-integration-2.0.0.tgz" -C "$here/.ink-src"
dotnet run --project "$here/InkCheck.csproj" -- "$here/../.."
