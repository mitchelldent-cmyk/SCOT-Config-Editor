# SCOT Config Editor

A WinForms utility for editing NCR ENCOR Self-Checkout (SCO) configuration files stored in `C:\scot\config`.

Built with **.NET Framework 4.8** for compatibility with POS terminal operating systems.

## What It Does

Provides a tabbed editor for the five key SCO config files. Each tab loads the current values from `C:\scot\config` and saves changes back to that location. A `.bak` backup is created automatically before each save.

## Files Edited

| File | Format | Key Settings |
|------|--------|-------------|
| `Scotopts.000` | INI | Receipt header, AllowAltIDEntry, CashBack amounts, AMPM loyalty options |
| `Scotopts.dat` | INI | Store-wide tender flags (Cash/Credit/Debit/EBT), CashBack and EBTCashBack amounts |
| `SCOTTend.dat` | INI | Per-button tender definitions: Text, TenderType, CashBack, NeedPin |
| `SSCOStrings.en-US.custom.dat` | UTF-16 LE | Custom display strings (store name in thank-you messages, etc.) |
| `SSCOUI.exe.config` | XML | Auto-restart settings, display flags, logo/background/welcome image paths |

## Navigation

Use the left-side file list to switch between tabs. Each tab has a status bar showing the file path (green = saved, red = file not found).

## Path

All files are read from and written to `C:\scot\config\`. Run the app directly on the SCO terminal (or with the SCO drive mapped).

## Project Structure

```
SCOTConfigEditor/
├── Helpers/
│   ├── IniFile.cs          # Line-preserving INI reader/writer (preserves comments)
│   ├── StringsFile.cs      # UTF-16 LE reader/writer for SSCOStrings format
│   └── XmlConfigFile.cs    # XmlDocument-based reader/writer for .config XML
└── UI/
    ├── MainForm.cs         # Load/save logic for all 5 files
    └── MainForm.Designer.cs # Left-nav layout, cards, and per-tab controls
```

## Building

```
dotnet build -c Release
```

Requires .NET Framework 4.8.
