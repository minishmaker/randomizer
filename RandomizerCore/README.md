# Minish Cap Randomizer

[![Discord](https://discordapp.com/api/guilds/342341497024151553/embed.png?style=shield)](https://discord.gg/ndFuWbV)
[![Twitter Follow](https://img.shields.io/badge/follow-%40minishmaker-blue.svg?style=flat&logo=twitter)](https://twitter.com/minishmaker)

* [Installation](#installation)
* [What is this?](#what-is-this?)
* [Getting Stuck?](#getting-stuck)
* [Settings](#settings)
  * [What is Shuffled?](#what-is-shuffled?)
  
This version: `0.7.0a`


Version 0.7 has been long overdue, this latest release is all thanks to newly added members of Team Minish Maker.

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

## What is this?

This program takes The Legend of Zelda: The Minish Cap and randomizes the location of items to provide a replayable experience. Logic is used to dictate where items are placed in order to create a completable playthrough, without the need of glitches or fear of being unable to progress.
The randomizer guarantees that a glitchless path through the game is possible, however this path might not be obvious and it is up to the player to figure out where they need to go. Under Default/Beginner settings nothing will be too complicated, but by adding more settings you will have to search more thoroughly for items and use those items in more creative/unintuitive ways.
The game has 7 Dungeons, Dark Hyrule Castle is the final dungeon and will normally be the end of your quest, and 6 other dungeons in the world (yes we count Royal Crypt as a dungeon). Most settings will have 4 Elements shuffled between the 6 Dungeons of the game which must be collected to gain acces to Dark Hyrule Castle, Check your map at any point to see where they reside. You can press `SELECT` when selecting a map quadrant to zoom into the dungeon view and vice versa.
This game is originally completely linear, a lot of work has been put in to open up the world and allow playthroughs in a multitude of different paths. Many 'Quality of Life' changes have been made to aid the player: 
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

### Where can they be found?

Here are all the locations that you can expect to find items:
- All chests in the game
- Freestanding HP locations (if enabled)
- 

### Resources

The most complete resource for the randomizer, including maps of all locations, can be
found [here](https://docs.google.com/document/d/e/2PACX-1vQrBNKQQnZ1xtjDzbWabN5tjd7801j2jAvz9-QNC1acpMxGioExWfzbfOOMGKk9f5MkL5MHCofVeaEc/pub)
. It was developed by the fabulous Myth197.
More comprehensive resources pertaining to randomizer gotchas and speed strats are pending.

### Usage

Download the latest zip from the [releases](https://github.com/minishmaker/randomizer/releases) page, you'll need all
the files it comes with. Choose a seed or leave it default, choose settings and gimmicks, then hit randomize. You'll
need to provide an EU copy of Minish Cap, don't ask us where to get one. Hitting 'Save ROM' will prompt you to save the
randomized game. You can also save a spoiler log for if you get stuck/want some help.

### Known Issues

- None? this wont last long.

Please join our [Discord server](https://discord.gg/ndFuWbV) to discuss the project and assist with testing!
We're also working on trackers for assisting with the randomizer, and would love any and all feedback.
