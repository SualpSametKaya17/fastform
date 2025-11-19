@echo off
echo ============================================
echo   FastForm - Visual Studio Cache Cleaner
echo ============================================
echo.

:: Navigate to solution directory
cd /d "%~dp0"

echo [1/6] Closing Visual Studio instances...
taskkill /F /IM devenv.exe 2>nul
timeout /t 2 /nobreak >nul

echo [2/6] Removing .vs directory...
if exist ".vs" (
    rmdir /s /q ".vs"
    echo     ✓ .vs removed
) else (
    echo     - .vs not found
)

echo [3/6] Removing bin directories...
if exist "FastForm\bin" (
    rmdir /s /q "FastForm\bin"
    echo     ✓ FastForm\bin removed
) else (
    echo     - FastForm\bin not found
)

echo [4/6] Removing obj directories...
if exist "FastForm\obj" (
    rmdir /s /q "FastForm\obj"
    echo     ✓ FastForm\obj removed
) else (
    echo     - FastForm\obj not found
)

echo [5/6] Clearing NuGet cache...
dotnet nuget locals all --clear
echo     ✓ NuGet cache cleared

echo [6/6] Restoring NuGet packages...
cd FastForm
dotnet restore --force --no-cache
cd ..
echo     ✓ Packages restored

echo.
echo ============================================
echo   ✅ Cleanup completed successfully!
echo ============================================
echo.
echo Next steps:
echo 1. Open FastForm.sln in Visual Studio 2022
echo 2. Build ^> Rebuild Solution (Ctrl+Shift+B)
echo 3. Press F5 to run
echo.
echo Press any key to open Visual Studio...
pause >nul

start FastForm.sln
