@echo off
echo 🧹 Cleaning FastForm project...

cd FastForm

:: Remove bin and obj directories
echo Removing bin and obj directories...
if exist bin rmdir /s /q bin
if exist obj rmdir /s /q obj

:: Clear NuGet cache
echo Clearing NuGet cache...
dotnet nuget locals all --clear

:: Restore packages
echo Restoring NuGet packages...
dotnet restore --force --no-cache

:: Clean project
echo Cleaning project...
dotnet clean

:: Build project
echo Building project...
dotnet build --no-incremental

echo ✅ Clean build completed!
pause
