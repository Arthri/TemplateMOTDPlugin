# TemplateMOTDPlugin
TemplateMOTDPlugin replaces TShock's MOTD engine with [Scriban](github.com/scriban/scriban)

# Installation
1. Download the [latest release](github.com/Arthri/TemplateMOTDPlugin/releases/latest)
2. Drag and drop the `.dll` file into the server's `ServerPlugins` folder
3. **Optional**ly, make a copy of `tshock/motd.txt` then clear the original file's contents
  3.1. If you skip the above step, then the plugin will automatically do it for you. A backup of the MOTD will be located at `config/templatemotd/motd.old.tshock.{timestamp}.txt`. The `{timestamp}` is the UTC Unix timestamp of MOTD file's creation date
4. Done!

# Usage

## Viewing the MOTD
The MOTD can be viewed when joining and when running the command `/motd`

## Reloading the MOTD
Run the command `/reload` to reload the MOTD.

For performance reasons, the MOTD is cached in memory. Whereas TShock doesn't and reads it from the file when needed.

# License
MIT-0
