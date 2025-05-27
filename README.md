# SWOS Magic Trick

A Windows Forms application written in C# that allows you to modify SWOS (Sensible World of Soccer) PC career files (.car) with various gameplay tricks and enhancements.

![image](https://github.com/user-attachments/assets/52dc026d-c86d-42c4-9f6e-f4d36cff925a)


## Description

This tool operates on SWOS PC career files, allowing players to apply specific modifications to enhance their gaming experience. The application provides a simple and intuitive interface to apply popular game modifications safely with automatic backup functionality.

## Features

### Main Functionality
- **Career File Modification**: Directly modify SWOS career files (.car) with binary-level precision
- **Multiple Trick Options**: Choose from three different gameplay modifications
- **Automatic Backup**: Creates safety backups of original files before any modification
- **Smart File Selection**: Default file browser opens in the SWOS 2020 installation directory (C:/SWOS 2020)

### Available Tricks
1. **Get 100M $** (Arrivano gli Sceicchi): Provides substantial financial resources for team management
2. **Unlimited Transfer** (Calciomercato pazzo): Removes transfer limitations for unlimited player trading
3. **Unlimited Career Years** (Voglio giocare per sempre): Extends career mode indefinitely

### Technical Features
- **Binary File Editing**: Precise offset-based modifications for reliable results
- **Comprehensive Logging**: Detailed operation logs with daily reset functionality
- **Error Handling**: Robust error management with user feedback
- **File Validation**: Ensures only .car files are processed
- **Backup Management**: Automatic creation of safety copies before any modification

### Logging System
- **Daily Log Reset**: Log files are automatically reset each day when the application starts
- **Operation Tracking**: All file modifications and operations are logged with detailed information
- **File Details Logging**: Records modified file names and applied trick types
- **Error Recording**: Comprehensive error logging for troubleshooting
- **Success Confirmation**: Confirmation of successful operations with full operation details

## System Requirements

- Windows OS (Windows 7 or later)
- .NET Framework or .NET Core Runtime (automatically installed with ClickOnce if needed)
- SWOS PC game installation
- Read/Write permissions for SWOS installation directory
- Administrator privileges for installation

## Usage

1. Install the application using the ClickOnce installer (setup.exe)
2. Launch "SWOS Career Editor" from the Start Menu or installation directory
3. Select the type of trick you want to apply from the dropdown menu
4. Browse and select your SWOS career file (.car)
   - Default location: C:/SWOS 2020
5. Click "Update data" to apply the modification
6. The application will:
   - Create a backup of your original file
   - Apply the selected modification
   - Confirm the operation status

## Technical Details

The application modifies specific binary offsets in SWOS career files:
- **Get 100M $**: Modifies offset D5DC with bytes 00 E1 F5 05
- **Unlimited Transfer**: Modifies offsets D87B and D87D with FF bytes
- **Unlimited Career Years**: Modifies offset D5C3 with byte 00

## Safety Features

- **Automatic Backup**: Every modification creates a backup copy with .car extension
- **Operation Logging**: All operations are logged for audit purposes
- **Error Prevention**: Input validation and error handling prevent data corruption
- **User Confirmation**: Clear feedback on operation success or failure

## Installation

The application is distributed as a ClickOnce deployment package for easy installation and updates.

### Installation Steps:
1. Download the SwosTrick.rar file
2. Extract the contents to a temporary folder
3. Run `setup.exe` to install the application
4. The application will be installed and accessible from the Start Menu

### Updating to New Versions:
⚠️ **Important**: When a new version is released, you must first uninstall the current version before installing the new one.
1. Uninstall the current version from Windows "Add or Remove Programs"
2. Download the new version
3. Follow the installation steps above

## File Structure

```
SwosTrick.rar (Distribution Package)/
├── Application Files/
│   ├── Launcher.exe
│   ├── setup.exe                    # ClickOnce installer
│   ├── SWOS2020TrickApplier.application
│   ├── SWOS2020TrickApplier.deps.json
│   ├── SWOS2020TrickApplier.dll
│   ├── SWOS2020TrickApplier.exe    # Main application
│   ├── SWOS2020TrickApplier.pdb
│   └── SWOS2020TrickApplier.runtimeconfig.json

After Installation:
├── [Installation Directory]/
│   ├── SWOS2020TrickApplier.exe    # Main application
│   └── swos_editor_YYYYMMDD.log    # Daily log files (created at runtime)
```

## Important Notes

- Always backup your save files before using any modification tools
- The application automatically creates backups, but manual backups are recommended
- Log files are reset daily to prevent excessive disk usage
- Only use with legitimate copies of SWOS PC
- Usually the path of log is similar that: C:\Users\%User%\AppData\Local\Apps\2.0\EG433R1A.ABZ\0E4PYD59.RMM\swos..tion_0000000000000034_2345.0000_52cc77a5364b6a2b\swos_trick_log.txt
- **For version updates**: Always uninstall the previous version before installing a new one
- The ClickOnce installer may require administrator privileges
- Some antivirus software may flag ClickOnce applications - add to exceptions if needed

## Disclaimer

This tool is for educational and entertainment purposes. Use at your own risk. Always maintain backups of your game saves.

## Version

Current Version: 1.0.0.1
Development Status: Active

---

**Note**: This application is an independent tool and is not affiliated with or endorsed by the original SWOS developers.
