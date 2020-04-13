# GTASaveData
A .NET library for reading and writing save files from various
*Grand Theft Auto* games.

## Overview
This project aims to create a versatile API for reading and writing save data
files from various *Grand Theft Auto* games. The API should provide access to
all currently known data fields for each game's save file. In addition, each
save data object's editable properties can be bound to PropertyChanged
listeners, making this library WPF-ready.

Data is extracted from and written to save files with as much accuracy as
possible. Some parts of the code even mimic what the actual game code does. The
API can handle file format differences between game systems (e.g. PC vs PS2
saves). Save incompatibilities due to differing script versions, however, are
**not** handled by this API, as they are not differences in the save files
themselves. That's for you to implement *using* the API! ;)

## Support
The following games are supported:
  * *Grand Theft Auto III* (in-progress)

The following games are planned:
  * *Grand Theft Auto: Vice City*
  * *Grand Theft Auto: San Andreas*
  * *Grand Theft Auto: Liberty City Stories*
  * *Grand Theft Auto: Vice City Stories*
  * *Grand Theft Auto IV & Episodes*

