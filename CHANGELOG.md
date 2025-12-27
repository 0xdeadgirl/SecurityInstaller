# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
## [ToDo]
- Install uBO in Firefox
- Uninstall old version(s) of MB before attempting to reinstall/update
- Get/display the top MAC address returned by ipconfig
- [optional] Create de-branded fork

## [Unreleased]
## [3.b.1] - 12-26-2025
### Changed
- Version number (2.4.0 -> 3.0) - New anticipated release version number

## [3.b.0] - 12-26-2025
### Added
- Macrium Reflect 7 (Programs tab)
- New "Ninite" tab - Populated by executables in a local `ninite` folder
- "Export" button - Writes computer/asset info into a local file, named `asset.txt`
- Memory speed now collected/reported
- "Disable unnecessary services" checkbox - Downloads/runs a batch script, which disabled CCleaner, Glary Utilities, DiagTrack, and SysMain services (see github.com/0xdeadgirl/batch-scripts/blob/main/bloatkiller.bat)
### Changed
- AdwCleaner executable is copied to `C:\AdwCleaner\`, and the NOC folder shortcut points to this
- Replaced Cortana and spaceman with Nerd
- Calling Card shortcut created in NOC folder
- Data sizes are calculated, instead of being explicitly defined (MB, GB, etc.)
- Lots of UI tweaks
### Removed
- Removed CCleaner, and replaced many references to it with Macrium Reflect
### Fixed
- Partitions not being reported when CD-ROMs and removable drives present
### Security
- Updated vulnerable dependencies

## [Released]

[2.3.4]\: https://github.com/AGiggleSniffer/SecurityInstaller/compare/v2.3.2...v2.3.4