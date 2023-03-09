# Minish Cap Randomizer

[![Discord](https://discordapp.com/api/guilds/342341497024151553/embed.png?style=shield)](https://discord.gg/ndFuWbV)
[![Twitter Follow](https://img.shields.io/badge/follow-%40minishmaker-blue.svg?style=flat&logo=twitter)](https://twitter.com/minishmaker)

* [Installation](#installation)
* [What is this?](#what-is-this?)
* [Getting Stuck?](#getting-stuck)
* [Settings](#settings)
  * [What is Shuffled?](#what-is-shuffled?)
  
This version: `0.7.0a`

Please see the [releases](https://github.com/minishmaker/randomizer/releases) page for the latest version.

Version 0.7 has been long overdue, this latest release is all thanks to newly added members of Team Minish Maker.
Over the last 1.5 years we have been testing and privately implementing ideas that haven't been available to anyone outside of our Discord community. This release is the combination of all these little bits of work into one of the biggest feature updates seen in a randomizer.

#### What is new in 0.7?

For just the changes between the individual recent alpha release and more details:
- [0.7.0 alpha-rev3 changelog](https://docs.google.com/document/d/e/2PACX-1vQDn6mkV-_287o-Zjhei87shQsl885P03d283LHlvpOySgNolOfIKdec-paY6k9hUbiZ-IYTcXbo1LB/pub)
- [0.7.0 alpha-rev2 changelog](https://docs.google.com/document/d/e/2PACX-1vQMxbygcsiiiANsenMDNLc_L1I-81M-qLYFo5Wapk6HsXzKHQ55_TMbgwFKu6DeSbXoOHRlDwauljik/pub)
- [0.7.0 alpha-rev1 changelog](https://docs.google.com/document/d/e/2PACX-1vSqAyicqgeJkUuUbMjL0wFJHbs-TURr4dafB1yn-O_oASplko2q1P-AbSPY1VKMw6VL6ROq6cEqtuaX/pub)

For all the changes since version 0.6.2:

UI overhaul:
- More Tabs and Grouped settings, much cleaner and user friendly design.
- File addresses are saved so you don't have to reselect your ROM each time you start the program.
- Tooltips: Mouse over any option to read a description.
- Settings Strings added.
	- Generate a string from the settings you choose, send to other people or save for later.
	- Load a string entered to activate those settings.
- `.bps` patcher added.
	- Create a sharable `.bps` file of your generated seed to share with other people.
	- Load a `.bps` file given to you to play a seed someone else made.

All the following settings have Tooltips that fully explain their function:
- New Key settings: Small Keys, Big Keys, and Maps&Compasses are independent.
	- Removed (Keasy), for Keys only
	- Start With, for Maps&Compasses only
	- Vanilla
	- Own Dungeon
	- Own Region
	- Any Dungeon
	- Any Region
	- Anywhere (Keysanity)
- New Option: NonElement Dungeons options: 
	- Standard.
	- Unrequired: No items found in these dungeons are needed to beat the game.
	- Barren: No Major items can be found in these dungeons at all.
	- Regions Unrequired: Includes Dungeon Regions
	- Regions Barren: Includes Dungeon Regions
- New Option: Requirement Reward: Enable a new item location, the item of which is given to the player once they meet the selected requirements (elements, sword, dungeons and figurines).
	- Disabled
	- DHC Big Key
	- Random Item
- "Required Number of Dungeons" added to requirements for Dark Hyrule Castle.
- New Option: Dojo Swordmasters: (Previously always set to 'Any Item')
	- Vanilla Scrolls
	- Randomized Scrolls
	- Any Item
- New Option: Cucco Minigame: Select the number of playable rounds from 0 to 10 (previously always 1).
- New Option: Goron Merchant: Select the number of randomized sets of 3 items each from 0 to 5 (previously always 0).
- New Option: Heart Pieces and Containers are randomized. (Previously always on, now a togglable setting)
- Obscure Spots is split into three different settings: 
	- Special Pots
	- Dig Spots
	- Underwater Spots
- New Option: Golden Enemies can have randomized items.

- New Option: Open World: 
	- Removes all temporary obstacles, Blocks, Bomb Walls, Doors... you name it and its opened.
- New Option: Dungeon Entrance Shuffle: 
	- All Dungeons that can have elements get their entrances shuffled.
- Fusions overhaul: Gold, Red, Blue and Green Kinstone Fusions are independent.
	- Each type can be: Removed, Vanilla, Combined, or Completed.
	- New Option: Defickle Fusers.
	- New Option: Seeded Shared Fusion Pool.
- Fusion Cutscene Setting Fixes.
	- New Option: Always show the map after fusing.
- New Options: Wind Crests active from the start:
	- Mt Crenel, Veil Falls, Cloud Tops, Hyrule Town, Swamp, Link's House and Minish Woods can all be independently toggled.
- New Options: Dungeon Warps active from the start:
	- The 12 blue and red dungeon warps can all be independently toggled.

- New Option: Location Reachability: Determines which items and locations are guaranteed to be reachable. There are 3 options:
	- All Locations
	- All NonKeys
	- Only the Goal
- New Option: Visible items are collectable from a distance:
	- Not Allowed
	- Allowed
	- Require
	- Require Hard
- New Option: No Logic
- New Option: Weapons considered by logic
	- Bombs, Bows, Gust Jar and Lantern get their own independent dropdown selectors.
- New Section: Trick requirements:
	- ~20 Tricks used while playing the game, These can be toggled to choose if they should be considered by the logic or left as optional out of logic plays.
	- Ranges from simple strats like using dust to blow away dust instead of the gust jar, all the way up to difficult strats like using a sword and a bomb to skip cloning at the Canon rooms in DHC.

- New Option: Item Pool Settings.
	- Plentiful: Extra copies of important items are added to the pool.
	- Balanced: Normal item pool.
	- Reduced: Unnecessary items removed.
- New Option: Items are Progressive
- New Option: Fun Junk items
- New Option: Random Bottle contents.
- New Option: Maximum Health.
- New Option: Damage Multiplier.
- New Option: Instant Text.
- New Option: Item Text Display added.
- New Option: Shy Fairies.
- New Option: Homewarp on Sleep option.
- New Option: Use boots on L, possible even while Minish sized.
- New Option: Select button to use Ocarina.
- New Option: Goron Merchant uses JP/US prices instead of EU.
- New Option: Disable Kinstone Drops.
- New Option: Disable Shell Drops.
- New Option: Firerod dev tool.

- Music Options: Vanilla, Shuffled, Disabled.
- New Cosmetic Option: Low Health Beep.

- Text speed on newly created files preset to `FAST`
- Spoiler Log restructure with better names, a more logical order, and a new `Playthrough` section.
- Version number is displayed on the File Select screen.
- Traps activated are counted and displayed in the credit stats.
	- Traps have smart disguises that depend on settings.
- Refill Hearts have been added to the item pool as junk items.
- Smith Sword (Quest), Broken Picori Blade, and Bakery goods have been added to the Fun Junk Item Pool
- Door mimics added to credit stats.
- Created clone count is now controllable by the player
- Simon now requires rupees in the pool.
- Dojo tutorial cutscenes skipped.
- Café sprite bug fixed.
- Fixed the `Đig Butterfly` display.
- Language Localisations have been expanded, nearly every piece of text in the game has a translation.
- Fixed the Village curtain opening if you have Earth Elements.
- Fixed a bug where rupee count isn't displayed.
- Resolve some attempted fixes in `Disable Glitches`
- In Game Timer is more accurate than before, Real Timer is still the most accurate.

#### Why version 0.7 but not 1.0?

Good question. We have reached the point where All seeds are always beatable a long time ago and have been slowly adding more and more features onto the base randomization. We have decided that 1.0 release will be when Web Randomization is achieved, so despite 0.7 being by far the biggest update to the randomizer you will have to wait a little bit longer.

## Installation

This randomizer requires The Legend of Zelda: The Minish Cap `EU`, The European version of the game, which should be a .gba file.
This program is a Windows Only application, Linux and Mac users can run it through a Windows Virtual Machine.
Download the latest [release](https://github.com/minishmaker/randomizer/releases) directly from github, download the latest `MinishRandomizer.v0.X.X.zip` and not the source code.
Extract all Files to a folder on your PC before running the program. With the program open, provide your European ROM and select the settings you wish to use. Press Randomize and save the newly created ROM, your original ROM is untouched.

`WARNING:` Running the game on an inacurate emulator will cause crashes and a poor gameplay experience. A Blue screen has been added when the ROM boots to indicate to the player that the emulator is inacurate.

These are the recommended emulators:

- [Bizhawk](https://github.com/TASVideos/BizHawk/releases/tag/2.4.2), Ideal for PC users, recommended to use 2.4.2 for the best experience when using Autotracking and other Lua Scripts.
- [mGBA](https://mgba.io/downloads.html), If Bizhawk isn't an option then this is sure to work on PC/Mac/Linux.
- [RetroArch](https://play.google.com/store/apps/details?id=com.retroarch&hl=en&gl=US), Ideal of Android Mobile users, viable for other platforms.

Please read our [Installation Guide](http://bombch.us/DB1q) and [FAQ](http://bombch.us/DQOh) to get set up and for information about all aspects of the randomizer.

## CLI Information

The CLI version of the program allows you to run the randomizer on Linux as well as Windows! Commands supported by the CLI are shows while running, since it is an interactive CLI.

The CLI also has support for a command file that runs automated commands with certain values. To use a command file, run the CLI from a command line and pass in the path to the file as a parameter, like `./MinishCapRandomizerCLI.exe Commands.txt`. All file extensions are supported.

The commands supported by command files are as follows, with (option) used to denote required options, [option] used to denote optional options, and | used to specify that you should put one of the options shown:
* LoadRom (Filename) - Loads the specified file as the target ROM. This should always be the first thing done in the command file.
* ChangeSeed (R | S) [Seed] - If R is selected it will pick a random seed and ignore the seed parameter, if S is selected then seed is required and is used as the seed for randomization.
* LoadLogic [PathToLogicFile] - If a path is specified it will load that logic file and update options. If no path is specified it will load the default logic file.
* LoadPatch [PathToROMBuildfile] - If a path is specified it will load those patches. If no path is specified it will load the default patches.
* UseYAML (1 | 2) [PathToLogicYAMLFile] [PathToCosmeticYAMLFile] - Passing 1 as the first parameter means all option types use the same source (same YAML file or all selected options), whereas with 2 they can be loaded from different files or one type from a file and the other from selected options. In the former case, the third parameter is ignored. If a YAML path is specified it will use that YAML file for seed generation to use as a Preset or Mystery weights. If no path is specified seed generation will use the loaded options instead.
* LoadYAML (PathToYAMLFile) (1 | 2 | 3) - Loads a YAML file to use as a Preset. This overwrites the currently loaded options.
* LoadSettings (Setting String) - Loads the settings provided by the setting string into the randomizer.
* Logging (1 | 2 | 3) [1 | 2 | PathToSaveLogs] - Sets one of the logging properties. Passing 1 as the first parameter sets the verbosity, in which case the second parameter needs to be either 1 (verbose) or 2 (only errors). Passing 2 as the first parameter sets the output file path, in which case the second parameter needs to be the path where you want to save the logs to. Passing 3 as the first parameter will publish the logs to the current output path, and does not take a second parameter.
* Randomize (RetryCount) - Generates a seed. RetryCount is the maximum number of times we should attempt to generate a seed in the case of failure.
* SaveRom (Output Path) - Saves the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the rom to the output path.
* SavePatch (Output Path) - Saves a patch for the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the patch to the output path.
* SaveSpoiler (Output Path) - Saves the spoiler log for the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the spoiler to the output path.
* GetSettingString - Gets the setting string for the currently selected settings and prints it to the console.
* GetFinalSettingString - Gets the setting string for the settings used for the randomized seed from a call to Randomize and prints it to the console. If no seed is available this will fail.
* Rem - when at the beginning of a line this comments out that line which makes it ignored by the command parser. The line is printed to the console.
* BulkGenerateSeeds (Number of seeds) - Generates the specified number of seeds in bulk and says the failure rate when it is done. This should only be used for testing purposes, as the seeds cannot be saved.
* Exit - Exits the CLI. This should always be at the end of the command file. If it is not, it will still exit, but it will print a warning about how no call to Exit was found. Using Exit is good as it prevents unwanted commands from running.

## What is this?

This program takes The Legend of Zelda: The Minish Cap and randomizes the location of items to provide a replayable experience. Logic is used to dictate where items are placed in order to create a completable playthrough, without the need of glitches or fear of being unable to progress.
The randomizer guarantees that a glitchless path through the game is possible, however this path might not be obvious and it is up to the player to figure out where they need to go. Under Default/Beginner settings nothing will be too complicated, but by adding more settings you will have to search more thoroughly for items and use those items in more creative/unintuitive ways.
The game has 7 Dungeons, Dark Hyrule Castle is the final dungeon and will normally be the end of your quest, and 6 other dungeons in the world (yes we count Royal Crypt as a dungeon). Most settings will have 4 Elements shuffled between the 6 Dungeons of the game which must be collected to gain acces to Dark Hyrule Castle, Check your map at any point to see where they reside. You can press `SELECT` when selecting a map quadrant to zoom into the dungeon view and vice versa.
This game is originally completely linear, a lot of work has been put in to open up the world and allow playthroughs in a multitude of different paths. Many 'Quality of Life' changes have been made to aid the player, some of which can be toggled on or off in the settings: 
- Cutscenes and Textbox Triggers (especially those pesky ones where Ezlo tells you the solution to a puzzle) have been removed.
- Text advances at an incredible speed while simply holding any button.
- Recently collected items are displayed at the bottom left corner of the screen.
- The Pegasus Boots can be used by just holding `L` and the Ocarina used by pressing `SELECT`.
- Quickwarp, found on the SAVE menu, it will warp you to the last area entrance you used to save a lot of backtracking.
- Remember that Figurine Gatcha Game? Yeah we removed it, just talk to Carlov for the reward right away.

## Getting Stuck

Started playing and got stuck? Don't worry, your seed isn't unbeatable, you are just lacking the knowledge to progress, we have all been there before. The first recommendation is to play with a Map Tracker! If you are on PC we recommend [Emotracker](https://emotracker.net/service/install/emotracker_setup.exe), just load it up and use one of the regularly updated Minish Cap Packages.

My Tracker is empty or My Tracker is telling me to go somewhere I can't reach? Make sure you configure the settings in your Tracker to reflect the settings your seed was randomized with, these trackers are powerful and can be configured for all combinations of settings you might be using. Later sections explain all the settings available for randomization.

Tracker not an option? If you really want to know where items are then its all available if you save the 'Spoiler Log', The first half lists all the locations (and what items are there) sorted by region, the second half of the log has a playthrough of the game using the Sphere System. The Sphere System is: Sphere 1 is all the locations reachable without ANY items, Sphere 2 is all the locations you can now reach with all the items from Sphere 1, Sphere 3 is then reached by items from Sphere 2 and lower, this repeats to reach all locations in the game.

Can't make heads nor tails of the Spoiler log? Ask in the [Discord](https://discord.gg/ndFuWbV)! there are people regularly playing the randomizer that you can ask for help if it is simple questions like "Where is `LonLon_CaveSecret_Chest`?" (You bomb the right wall). We aren't psychics though, if you are just stuck in general then you need to give us some information so we can help you, things like: The Spoiler Log (if you don't have that then: The Settings String and Seed Number you used) and The items you have collected so far.

## Settings

The Randomizer has 2 options for Win conditions:

- Defeat Vaati. Dark Hyrule Castle has various requirements to open.
- Enter the Sanctuary after all requirements are met.

There are different requirements that can be combined however you want:

- Number of Dungeons Completed
- Number of Elements Collected (Dungeon Rewards)
- Sword level
- Figurine Hunt (Triforce Hunt)

### What is Shuffled?

The following things are always added to the item pool (These can be adjusted by selecting a different Item Pool)

- All Main Inventory Items (Swords and other quipable items, Unequipable items like Flippers)
- Item Upgrades (Butterflies, Sword Scrolls, Remote bombs...)

Settings allow you to the following items to the pool (and where they should be shuffled to):
- Heart Pieces and Heart Containers
- Dungeon Items (Small Keys, Big Keys, Maps and Compasses)
- Kinstones (Gold/Red/Blue/Green)
- Elements
- Special items depending on the game mode (Figurines and Timer Clocks)
- A lot of junk items :)
- Music (if set)
- Dungeon Entrances

### Where can they be found?

Here are all the locations that you can expect to find items, depending on the settings you select:
- All chests in the game
- Given by NPCs
- Purchased from Shops
- Freestanding HP locations
- Keys that drop from the ceiling
- In special pots
- In certain dig spots
- In special underwater spots

### Known Issues

- None? this wont last long.

Please join our [Discord server](https://discord.gg/ndFuWbV) to discuss the project and assist with testing!
We're also working on trackers for assisting with the randomizer, and would love any and all feedback.
