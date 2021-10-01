# TemplateMOTDPlugin
TemplateMOTDPlugin replaces TShock's MOTD engine with [Scriban](https://github.com/scriban/scriban)

# Installation
1. Download the [latest release](https://github.com/Arthri/TemplateMOTDPlugin/releases/latest)
2. Drag and drop the `.zip` file into the server root directory
3. Unzip the `.zip`
4. **Optional**ly, make a copy of `tshock/motd.txt` then clear the original file's contents
  - If you skip the above step, then the plugin will automatically do it for you. A backup of the MOTD will be located at `config/templatemotd/motd.old.tshock.{timestamp}.txt`. The `{timestamp}` is the UTC Unix timestamp of MOTD file's creation date
5. Done!

# Usage

## Viewing the MOTD
The MOTD can be viewed when joining and when running the command `/motd`

## Reloading the MOTD
Run the command `/reload` to reload the MOTD.

For performance reasons, the MOTD is cached in memory. Whereas TShock doesn't and reads it from the file when needed.

## Writing Templates
The templating engine is [Scriban](https://github.com/scriban/scriban). For simplicity's sake, no Scriban usage will be detailed or supported here. Instead, refer to Scriban's documentation https://github.com/scriban/scriban#documentation.

### Differences with TShock
Scriban changes names from `OnlinePlayer` to `online_player` to match Liquid templates.

https://github.com/scriban/scriban/blob/master/doc/runtime.md#member-renamer

### Global Variables
Currently, there is only [one model](https://github.com/Arthri/TemplateMOTDPlugin/blob/master/src/TemplateMOTDPlugin/Templating/DefaultModel.cs). It provides all variables available through TShock, but also a few more(feel free to create an issue to include more).

At the moment, these are the additionally exposed variables:
- `command_silent_specifier` the silent command specifier, by default `.`.
- `players_array` an array of TShock players.

# Building
1. Restore tools: `dotnet tool restore`
2. Restore dependencies: `dotnet paket restore`
3. Build: `dotnet build`

# License
[MIT-0](https://github.com/Arthri/TemplateMOTDPlugin/blob/master/LICENSE)
