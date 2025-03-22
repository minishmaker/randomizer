# Minish Cap Randomizer

[![Discord](https://discordapp.com/api/guilds/342341497024151553/embed.png?style=shield)](https://discord.gg/ndFuWbV)
[![Twitter Follow](https://img.shields.io/badge/follow-%40minishmaker-blue.svg?style=flat&logo=twitter)](https://twitter.com/minishmaker)

* [Installation](#installation)
* [What is this?](#what-is-this?)
* [Getting Stuck?](#getting-stuck)
* [Settings](#settings)
  * [What is Shuffled?](#what-is-shuffled?)
  * [Where can they be found?](#where-can-they-be-found?)
  * [Known Issues](#known-issues)
* [CLI Information](#cli-information)
* [Version History](#version-history)

This version: `1.0.0-rc2.1`
This is a pre-release version before `1.0` proper

Please see the [releases](https://github.com/minishmaker/randomizer/releases) page for the latest version.

## Installation

This randomizer requires The Legend of Zelda: The Minish Cap `EU`, The European version of the game, which should be a .gba file.
This program is a Windows Only application, Linux and Mac users can run it through a Windows Virtual Machine.
Download the latest [release](https://github.com/minishmaker/randomizer/releases) directly from github, download the latest `MinishRandomizer.v0.X.X.zip` and not the source code.
Extract all Files to a folder on your PC before running the program. With the program open, provide your European ROM and select the settings you wish to use. Press Randomize and save the newly created ROM, your original ROM is untouched.

`WARNING:` Running the game on an inaccurate emulator will cause crashes and a poor gameplay experience. A Blue screen has been added when the ROM boots to indicate to the player that the emulator is inaccurate.

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
* UseYAML [PathToYAMLFile] - If a path is specified it will load that YAML file to use as a Preset or Mystery weights. If no path is specified seed generation will use the loaded options instead.
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
- Elements (normally Dungeon Prizes, but can be shuffled anywhere)
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
- Delivering Elements to the Sanctuary Pedestal
- Completing the DHC requirements, normally gives the player the DHC Big Key but can give a random item as well.

### Known Issues

- The UI is only supported on Windows machines and with display scaling set to 100%.
- With Removed Gold Fusions, Dungeon Entrance Shuffle on, and Elements either placed Anywhere or forced in their Vanilla Dungeons or a high number of Dungeons required, some settings combinations have a high chance of failing to beat Vaati. This has no easy code fix until Logic v2 is done. To work around this, click the "New Seed" button and try again, or set your "Max Randomization Attempts" to 5 or more and try and randomize again.
- Logic for Non-Progressive Swords can sometimes require unintuitive tricks with clones.
- Regional Dungeon Items combined with Dungeon Entrance Shuffle is unintuitive, eg: If Fortress is shuffled to Crenel, Small Keys for Fortress can be found in Wind Ruins but not in Crenel.
- Followers cause a fair amount of visual bugs ranging from miss coloured palettes, to certain sprites being completely missing.

Please join our [Discord server](https://discord.gg/ndFuWbV) to discuss the project and assist with testing!
We're also working on trackers for assisting with the randomizer, and would love any and all feedback.

## CLI Information

The CLI version of the program allows you to run the randomizer on Linux as well as Windows! Commands supported by the CLI are shows while running, since it is an interactive CLI.

The CLI also has support for a command file that runs automated commands with certain values. To use a command file, run the CLI from a command line and pass in the path to the file as a parameter, like `./MinishCapRandomizerCLI.exe Commands.txt`. All file extensions are supported.

The commands supported by command files are as follows, with (option) used to denote required options, [option] used to denote optional options, and | used to specify that you should put one of the options shown:
* LoadRom (Filename) - Loads the specified file as the target ROM. This should always be the first thing done in the command file.
* ChangeSeed (R | S) [Seed] - If R is selected it will pick a random seed and ignore the seed parameter, if S is selected then seed is required and is used as the seed for randomization.
* LoadLogic [PathToLogicFile] - If a path is specified it will load that logic file and update options. If no path is specified it will load the default logic file.
* LoadPatch [PathToROMBuildfile] - If a path is specified it will load those patches. If no path is specified it will load the default patches.
* LoadSettings (Setting String) - Loads the settings provided by the setting string into the randomizer.
* UseYAML (1 | 2) [PathToLogicYAMLFile] [PathToCosmeticYAMLFile] - Passing 1 as the first parameter means all option types use the same source (same YAML file or all selected options), whereas with 2 they can be loaded from different files or one type from a file and the other from selected options. In the former case, the third parameter is ignored. If a YAML path is specified it will use that YAML file for seed generation to use as a Preset or Mystery weights. If no path is specified seed generation will use the loaded options instead.
* LoadYAML (PathToYAMLFile) (1 | 2 | 3) - Loads a YAML file to use as a Preset. This overwrites the currently loaded options.
* Logging (1 | 2 | 3) [1 | 2 | PathToSaveLogs] - Sets one of the logging properties. Passing 1 as the first parameter sets the verbosity, in which case the second parameter needs to be either 1 (verbose) or 2 (only errors). Passing 2 as the first parameter sets the output file path, in which case the second parameter needs to be the path where you want to save the logs to. Passing 3 as the first parameter will publish the logs to the current output path, and does not take a second parameter.
* Randomize (RetryCount) - Generates a seed. RetryCount is the maximum number of times we should attempt to generate a seed in the case of failure.
* SaveRom (Output Path) - Saves the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the rom to the output path.
* SavePatch (Output Path) - Saves a patch for the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the patch to the output path.
* SaveSpoiler (Output Path) - Saves the spoiler log for the randomized seed from a call to Randomize. If no seed is available this will fail. Writes the spoiler to the output path.
* GetSettingString - Gets the setting string for the currently selected settings and prints it to the console.
* Rem - when at the beginning of a line this comments out that line which makes it ignored by the command parser. The line is printed to the console.
* BulkGenerateSeeds (Number of seeds) (true | false) - Generates the specified number of seeds in bulk and says the failure rate when it is done. If the second parameter is passed true, then it will pick random settings after every successful generation (additionally, after 10 consecutive failures on the same settings, new settings will be chosen). If false, then it will use whatever settings are loaded when bulk generate is called. This should only be used for testing purposes, as the seeds cannot be saved.
* Exit - Exits the CLI. This should always be at the end of the command file. If it is not, it will still exit, but it will print a warning about how no call to Exit was found. Using Exit is good as it prevents unwanted commands from running.

## Version History

An up to date version history can be found on the [releases](https://github.com/minishmaker/randomizer/releases) page.

For just the changes between the individual recent alpha release and more details:
- [v0.7.0 alpha-rev3 changelog](https://docs.google.com/document/d/e/2PACX-1vQDn6mkV-_287o-Zjhei87shQsl885P03d283LHlvpOySgNolOfIKdec-paY6k9hUbiZ-IYTcXbo1LB/pub)
- [v0.7.0 alpha-rev2 changelog](https://docs.google.com/document/d/e/2PACX-1vQMxbygcsiiiANsenMDNLc_L1I-81M-qLYFo5Wapk6HsXzKHQ55_TMbgwFKu6DeSbXoOHRlDwauljik/pub)
- [v0.7.0 alpha-rev1 changelog](https://docs.google.com/document/d/e/2PACX-1vSqAyicqgeJkUuUbMjL0wFJHbs-TURr4dafB1yn-O_oASplko2q1P-AbSPY1VKMw6VL6ROq6cEqtuaX/pub)

### Legacy History

#### 2021

- v0.6.2_Tourney
- v0.6.2a

#### 2020

- v0.6.1a
- v0.6.0a
- v0.5.7a

#### 2019

- v0.5.6a
- v0.5.5a
- v0.5.4a
- v0.5.3a
- v0.5.2a
- v0.5.1a
- v0.5.0a
- v0.4.3a
- v0.4.2a
- v0.4.1a
- v0.4.0a
- v0.3.1a
- v0.3.0a
- v0.2.1a
- v0.2.0a
- v0.1.0a