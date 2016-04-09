# MoreBlocks
MoreBlocks is a Mod Project for SkyNations Servers with the aim to provide a greater variety of decorative Blocks and generally Blocks and Items the Players have asked for. Comments and ideas go over here: [Sky Nations Forum Thread for this Mod](http://skynations.net/community/viewtopic.php?f=11&t=1009 "Sky Nations Forum Thread for this Mod").  
First release is planned for shortly after the release of patch SkyNations patch 0.99.  
Contributors (in no particular order): Vanto (Scripting, XML), Kirby677 (Textures), Fryor (Textures), EstEst (Textures), Aerion (Scripting, Textures, XML, Maintenance)

Thank you everyone!

## Blocks and Items
---

### Implemented so far:

Blocks:
- Decking: Steel, Copper, Titanium, Iron, White Carpet
- Platform: Titanium, Steel, Iron, Copper
- Railings: Titanium, Iron
- Steps (lower): Titanium, Steel, Iron, Copper, Glass, Hard Glass
- Steps (upper): Titanium, Steel, Iron, Copper, Glass, Hard Glass
- Struts: Titanium, Steel, Iron
- Doors: Steel, Wood, Titanium, Iron
- Wall tiles: Titanium, Steel, Iron, Copper, Wood
- Hatch: Titanium, Steel, Iron, Copper, Wood
- Shutters: Titanium, Iron, Copper
- Wedges: Copper Strut, Iron Strut, Steel Strut, Titanium Strut

Scripts:

- universal Script to open/close doors
- universal Script to open/close hatches
- Teleporter and Teleport Activator Script
- lots of utility and helper functions making life with Chunks easier

### Wishlist:
### Set 1

- Decking: ~~Titanium~~, ~~Steel~~, ~~Iron~~, ~~Copper~~, ~~possibly a white one to be colored (as carpet?)~~
- Platform: ~~Titanium~~, ~~Steel~~, ~~Iron~~, ~~Copper~~
- Steps (lower): ~~Titanium~~, ~~Steel~~, ~~Iron~~, ~~Copper~~, ~~Glass~~, ~~Hard Glass~~, Stone Brick, Red Stone Brick, White Stone Brick, Black Granite Brick, Marble
- Steps (upper): ~~Titanium~~, ~~Steel~~, ~~Iron~~, ~~Copper~~, ~~Glass~~, ~~Hard Glass~~, Stone Brick, Red Stone Brick, White Stone Brick, Black Granite Brick, Marble
- Wedges: Cargo Plate, Titanium Shutter, Steel Shutter, Iron Shutter, Copper Shutter, Wood Shutter, H Red/White Wedge
- Wall tiles: ~~Titanium~~, ~~Steel~~, ~~Iron~~, ~~Copper~~, ~~Wood~~, Glass, Hard Glass

textures needed:
- Railings: ~~Titanium~~, ~~Iron~~, Stone
- Doors: ~~Steel~~, ~~Wood~~, ~~Titanium~~, ~~Iron~~
- Struts: ~~Titanium~~, ~~Steel~~, ~~Iron~~
- Shutters: ~~Titanium~~, ~~Iron~~, ~~Copper~~, Wood
- Hatch: ~~Wood~~, ~~Steel~~, ~~Titanium~~, ~~Iron~~, ~~Copper~~
- **Teleporter and ForceField (!)**
- Railing Gate / half a door
- grey Sand (depleted Sand)
- Fireplace
- Mossy Bricks / Stone
- Railing Gate / half a door
- Console Block with some kind of speed-o-meter (animation)
- Console Block with different blinking lights (several frames for animation) (see old CTRL Block texture for idea of light placement)
- Generic Motor / Machine
- Decorative Lights (possibly just the older versions of the lanterns)
- ~~Bookshelf~~
- Chimney / Stove (possibly with fire animation)
- ~~Brick Block (more like classical Bricks with white grout in between the red Bricks)~~
- More Brick Colors
- Fire
- Grass?

### Set 2
Blocks:
- ~~Teleporter Blocks~~ (land based to get from A to B, eg. in Capital)
- Hangar Doors
- Hatches / Doors openable via Button
- Different Woods
- Different Stones
- more ores (something green?)
- Metal Table
- Red Sand
- Chairs
- Banks
- Stairs with Steps
- Stained Glass (possible with limitations)
- Flower Pot
- Bed
- Colored Ice
- ~~Corner Wedges~~ (vertical wedges content of patch 0.99)
- Corner pieces (to fit between wedges?)
- Railing corners
- Turbo thrusters (generating more than two thrust)
- Dynamite Blocks
- Shield block for Ships (Deflector Shield / Shield generator)
- Soundproof Blocks (likely not working)
- mechanic to transport Items between chests (aka unloading the mining ship chests onto land) (Ben said he has plans for this)
- med station block
- ship repair field

Items:
- Tool to mine Steel, Copper, Iron, etc. more efficiently
- Handheld laser, Laser Gun
- Plantable Explosives
- Grenades

## How to Install the Mod?

Until there is a release you can download the repository, and paste the contents of the `_MOD\` folder into your Skynations-Server root directory. After enabling the Mod in your `Server Settings.xml` (example provided) you can start your server up and use the new Blocks. Until further notice, this Mod is NOT production ready!

## Organizational

At the moment the goal is to provide more decorative Blocks, preferably ones that can be crafted from existing materials within the game. Blocks which require scripting to properly work because they have a mechanic, or Blocks and Items which would likely be found in a new Biome are, for now, out of the scope of this Mod, but will be listed here regardles, for future reference. 

### Set 1
- simple Blocks with a shape that already exists in the game (cube, wedges, railings, ...)
- no mechanics (scripts) needed to use them

= simple craft and place blocks, nice to look at and used for decoration
### Set 2
- all others

This are mostly blocks that either need to have a new shape/mesh made for them and/or which need a script to work properly within the game world and/or need new Biomes.

## Priority
Set 1 takes priority for now as its easy to implement and almost anyone can help with that. Its as easy as filling out a form (Tiles.xml and Blocks.xml) and drawing a picture (creating the texture).

## How do I contribute?
Pick any block you want to see in the game and provide files for it in a pull request. You don't have to provide everything needed to implement a new block, its fine if you only submit a texture, the .xml files or a script, this is an open project and maybe someone else helps providing the other parts needed.
**Do not submit a pull request that changes anything within the `_MOD\` folder**, it will not be accepted. Create a new folder for your changes within `Contributions\` and put your files in there. The folder structure therein should ideally mirror the one over in `_MOD\`. Your contribution will be reviewed, open for discussion and, if needed, subject to change (e.g. weight and cost of blocks have to be balanced in the grand scheme of the game etc.). Later it will be copied over into the `_MOD\` folder by a maintainer and released. 

## About Modding
For further information about modding in Sky Nations, go to [Sky Nations Wiki](http://wiki.skynations.net/doku.php?id=modding "Sky Nations Wiki - Modding").  
Do not forget to check out the Tiles.xml in the GameData folder, there you can find the full list of Blocks already in the game and their properties, so you get an idea about the attributes you can use for your custom Blocks.
Additionally you can download the Sky Nations Default Block Textures here: [Sky Nations Forum](http://skynations.net/community/viewtopic.php?f=11&t=879 "Sky Nations Forum - Sky Nations Default Textures - For making new texture packs")
