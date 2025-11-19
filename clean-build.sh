#!/bin/bash

echo "🧹 Cleaning FastForm project..."

# Navigate to project directory
cd "$(dirname "$0")/FastForm"

# Remove bin and obj directories
echo "Removing bin and obj directories..."
rm -rf bin/
rm -rf obj/

# Clear NuGet cache
echo "Clearing NuGet cache..."
dotnet nuget locals all --clear

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore --force --no-cache

# Clean project
echo "Cleaning project..."
dotnet clean

# Build project
echo "Building project..."
dotnet build --no-incremental

echo "✅ Clean build completed!"
