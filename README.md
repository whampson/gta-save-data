# GTASaveData
[![Build Status](https://github.com/whampson/gta-save-data/workflows/CoreLib/badge.svg)](https://github.com/whampson/gta-save-data/actions)
[![Build Status](https://github.com/whampson/gta-save-data/workflows/GTA3/badge.svg)](https://github.com/whampson/gta-save-data/actions)

A .NET library for reading and writing save files from various
*Grand Theft Auto* games.

## Overview
This project aims to create a versatile API for reading and writing save data
files from various *Grand Theft Auto* games. The API should provide access to
all currently known data fields for each game's save file. Each editable
property fires a `PropertyChanged` event when modified.

Data is extracted from and written to save files with as much accuracy as
possible. Some parts of the code even mimic what the actual game code does when
reading and writing save data.

## Support
The following games are fully supported:
  * *Grand Theft Auto III*

The following games are in-progress:
  * *Grand Theft Auto: Vice City*

The following games are planned:
  * *Grand Theft Auto: San Andreas*
  * *Grand Theft Auto: Liberty City Stories*
  * *Grand Theft Auto: Vice City Stories*
  * *Grand Theft Auto IV & Episodes*
