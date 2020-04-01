# GTASaveData
A .NET library for manipulating GTA save files.

## Goal
Provide access to the data inside a *Grand Theft Auto* save file in as simple
and liberal of a way as possible.

## Overview
This project aims to create a versatile API for reading and writing save data
from various *Grand Theft Auto* games. The API provides access to as much data
as what is current known for each game's save file. In addition, the
editable properties are equipped to be bound to PropertyChange listeners in a
UI, making this API perfect for writing save editors.

Data extracted from and written to save files with as much accuracy as possible,
even going so far as to mimic what the actual game code does in some places. The
API even handles differences file format differences between game systems. Game
script differences, however are **not** handled by this API, but these can
easily be implemented using the API.

## Support
The following games are supported:
  * *Grand Theft Auto III* (in progress)
  * *Grand Theft Auto: Vice City* (in progress)
  * *Grand Theft Auto: San Andreas* (in progress)
  * *Grand Theft Auto: Liberty City Stories* (planned)
  * *Grand Theft Auto: Vice City Stories* (planned)
  * *Grand Theft Auto IV & Episodes* (planned)
