# GTASaveData
[![Build Status](https://github.com/whampson/gta-save-data/workflows/CoreLib/badge.svg)](https://github.com/whampson/gta-save-data/actions)
[![Build Status](https://github.com/whampson/gta-save-data/workflows/GTA3/badge.svg)](https://github.com/whampson/gta-save-data/actions)
[![Build Status](https://github.com/whampson/gta-save-data/workflows/LCS/badge.svg)](https://github.com/whampson/gta-save-data/actions)

A .NET library for reading and writing save files from various
*Grand Theft Auto* games.

## Overview
This project aims to create a versatile API for reading and writing save data
files from various *Grand Theft Auto* games. The API provides access to all
currently-known data fields for each game's save file. Each editable property
fires a `PropertyChanged` event when modified, making this API WPF-friendly.

## Support
Fully supported:
  * *Grand Theft Auto III*
  * *Grand Theft Auto: Liberty City Stories*

In progress:
  * *Grand Theft Auto: Vice City Stories*

Planned:
  * *Grand Theft Auto: Vice City*
  * *Grand Theft Auto: San Andreas*
  * *Grand Theft Auto IV & Episodes*
