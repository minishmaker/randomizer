#### This is a pre-release version of the upcoming update to the Minish Cap Randomizer! This version is an improvement of the alpha-rev2 release posted in the discord to fix a few issues found since that build.

#### Huge thank you to everyone who has helped test previous testing builds! This wouldn't have been possible without all of you! Also big thank you to Dreamie Catobat, Myth197, Henny022, Leonarth, Deoxis, and everyone else who has contributed to development on this build! Your work has been critical in getting this far!

## What's New!

---

### UI Overhaul

#### UI Organization and Improvements:
- Added support for multiple tabs on the UI to make organizing settings much easier.
- Added setting grouping to organize settings on individual tabs. The name of the group is put on the top corner of the box to show what the settings are for. 
- Added tooltips to settings - you can see the tooltips by hovering over the name of the setting.
- Added an `Advanced` page for experimental settings or settings meant for use by developers. Generally most users shouldn't mess with these settings.

#### New Features!
- The UI now stores some data when you close it to make generating seeds easier. The settings saved are:
  - ROM Path (You don't have to browse for the ROM every time now!)
  - Total randomization generation attempts in the event of a failure
  - Using the Sphere Based (Hendrus) Shuffler
  - Using a custom logic file
  - Using custom patches
  - Where to output logs to when you click `Write and Flush Logger`
- Settings & Cosmetics Strings! (& presets too!)
	- Generate a sharable settings string based on the settings you are using! The same also works for cosmetics!
    - When a seed is done being randomized, the settings string and the cosmetics string are on the page where you download the seed. You can click the "Copy to Clipboard" button to copy the settings for easier sharing!
    - You can also now save presets! Have some settings you always play seeds on? Save it as a preset!
    - The following settings presets are included with the randomizer:
      - Beginner settings - newbie friendly settings meant for fast seeds or your first Minish Cap Randomizer. Skips Dark Hyrule Castle.
      - Intermediate settings - settings meant for slightly longer but also newbie friendly seeds. You are required to do Dark Hyrule Castle.
      - Advanced settings - there are two versions of these settings, one with dungeon entrance randomizer and one without it. These are the most common settings people run the randomizer with, with all checks opened and some tricks required in logic.
      - Max Random settings - Every single setting on! Generally makes very long and very hard seeds.
      - 2022 Weekly settings - The weekly race settings from 2022. Does not have dungeon entrance randomizer on.
      - All random cosmetics - Random music, random heart color, tunic color, and split bar color.
	- To load a settings or cosmetics string, paste it in the respective box and click the Load button directly underneath.
    - To load a preset, select the preset from the dropdown and click the load button.
    - To save a preset, select the settings you want and click the save button.
- Added BPS Patcher and BPS Patch Generator!
  - Create a sharable patch in the randomizer itself! There are two ways to create the patch:
    - After randomizing the seed, click the `Save BPS Patch` button, and it will prompt you to save the patch.
    - On the General page in the UI, at the bottom of the page, click the `Generate Patch Mode` button. Load a randomized ROM by clicking `Browse`, and then click `Generate BPS Patch` to save the patch.
  - Apply a BPS Patch in the randomizer as well!
    - To apply the patch go to the General page in the UI, then at the bottom of the page click the `Apply Patch Mode` button, click `Browse` to find the patch file, and then click `Patch ROM` to save the patched ROM.
- Added a logger to the randomizer to make it easier for reporting errors.

#### Sphere based shuffler!

Added a sphere based shuffler to the randomizer! This shuffler fills items in a logical way to guarantee the seed is beatable. This shuffler is currently experimental and may have some issues.

This shuffler is know to produce very linear seeds with very high sphere counts. It can be fun to play but it can also feel like a plandomizer. Generating seeds with this shuffler can take a very long time. If it takes more than 50,000 attempts to generate a seed it will stop trying and print out an error message.

### New Settings

#### Independent Dungeon Items!

Small Keys, Big Keys, and Maps/Compasses are all independent from each other. Now you can choose to have Small keys randomized in their own dungeons, but have Big keys randomized anywhere!

#### Regional Dungeon Items!

You can now shuffle dungeon items to not only be in their own dungeon, but in the surrounding regions! For example, this means that you can find a Royal Crypt key in one of the chests in graveyard. The regions for each dungeon are:
- Deepwood Shrine:
  - Both Belari Checks
  - The fusion chest by Belari
  - The flippers caves by Belari
  - Freestanding Heart Piece and Barrel Item in Minish Village
- Cavern of Flames:
  - Digging Spots in Melari's Mines
  - Fusion Chest on path to Melari's Mines
  - Chest in the block puzzle in cave before Melari's Mines
- Fortress of Winds:
  - All of Wind Ruins
- Temple of Droplets:
  - None
- Royal Crypt:
  - All checks in Graveyard
    - Dampe, Lost Woods Chest, and Royal Valley Great Fairy are not part of this region
- Palace of Winds:
  - All checks on 3rd and 4th floor of Wind Tribe
  - Gregal 2
  - If Red Fusions are removed, then all the checks on the 1st and 2nd floors of wind tribe, as well as Gregal 1, are part of this region.
- Dark Hyrule Castle:
  - Everything in Castle Garden except the Moat Chests

#### Additional Dungeon Item Settings!

We now have more settings for small keys, big keys, and dungeon maps/compasses! Along with what you are used to, we now added:
- Removed (Keasy)/Start With - Unlocks all doors for the given key type in dungeons. For maps and compasses, you start with them with the option set to `Start With`
- Vanilla - Any dungeon items set to this setting will be shuffled to their vanilla location.
- Own Region - The selected dungeon items can be in their own dungeon or in their surrounding region. For the regions, see Regional Dungeon Items above.
- Any Dungeon - The selected dungeon items can be in any of the 6 dungeons and Dark Hyrule Castle (once accessible).
- Any Region - The selected dungeon items can be in any of the 6 dungeons, Dark Hyrule Castle, or their associated regions. For the regions, see Regional Dungeon Items above.

#### Non-Element Dungeons Barren/Unrequired!

You can now make non-element dungeons be either barren or unrequired!
- For barren dungeons, no major items can be placed in the dungeon ever, even if vaati is beatable.
- For unrequired dungeons, major items can be placed in the dungeon but only once vaati is beatable.
- These settings also have regional versions! To see what checks are in each region, see "Regional Dungeon Items" above!

#### Required Dungeons for Dark Hyrule Castle!

You can now choose how many dungeons are required to enter Dark Hyrule Castle! This can be useful if you want to force all dungeons to be completed or require a certain number of dungeons be completed with elements shuffled into the item pool.

#### Better Miscellaneous Check Settings!

You can now choose to randomize each of the following groups of items! This also decouples the old "Obscure" items.
- Heart Pieces and Containers - with this setting off, all heart pieces and containers in the game will be in their vanilla locations.
- Rupees - randomizes all freestanding rupees in the game to be random items.
- Special Pots - randomizes the 5 pots in the game with preset items. The pots are:
  - The pot in Lon Lon Ranch
  - The pot in the Town Inn on the second floor in the bottom right corner of the room
  - Two pots in the room before the boss key chest in Fortress of Winds
  - One of the pots on the Ice Maze in Temple of Droplets on the right side of the dungeon
- Dig Spots - randomizes all digging spots with preset items. Items in digging caves are still randomized even if this setting is off.
- Underwater Spots - randomizes all spots with preset items in dungeons and the overworld where you need to dive with the flippers to get the item.
- Golden Enemies - randomizes the rewards from golden enemies found throughout the overworld. 8 of the golden enemies are spawned from green fusions, 1 of them is spawned from a red fusion.
- Pedestal Items - randomizes the items on the pedestal from going there with 2, 3, and 4 elements. This only has an effect if Dark Hyrule Castle is set to `Always Open`.

#### Dojo Shuffle!

You can now choose whether you want scrolls to only be on Dojos or shuffled into the general item pool!

The following options are available:
- Vanilla - all scrolls are in their vanilla locations.
- Randomized Scroll - scrolls are randomized between dojos and are not shuffled into the greater item pool.
- Any Item - scrolls are shuffled into the general item pool and dojos can have any item.

#### Dungeon Entrance Shuffle!

We added dungeon entrance shuffle to the randomizer! Currently Dark Hyrule Castle is not shuffled. 

Some things to know about dungeon entrance shuffle:
- Elements on the map are displayed over the location where that element is, not over the dungeon that has that element.
  - This is a change from the previous testing build. Previously, if Palace was on Temple of Droplets entrance and Palace had an element, the element would display over Cloud Tops on the map. Now it will instead display over Lake, showing you that the Temple of Droplets entrance has an element.
- Regional items do not currently adjust their region with dungeon entrance shuffle on.
- This setting works with `non-element dungeons barren`, but like with regional items, does not properly affect regions.
  - For example, if Palace of Winds doesn't have an element, and `non-element dungeons and regions are barren` is on, then upper Wind Tribe will be barren, even if Palace is somewhere else in the overworld.

#### Open Wind Crests!

You can now specify what wind crests you want to have available at the start of the seed! This allows some locations to be put in logic once you get the ocarina when they would otherwise be locked behind other items. Castle Town and Lake Hylia crests are always unlocked.

The wind crest locations available to toggle are:
- Mt. Crenel
- Veil Falls
- Cloud Tops
- Castor Wilds
- South Hyrule Field
- Minish Woods

#### Open World!

Opens all of the overworld as well as dungeons at the start of the seed!

The following things are changed with this setting:
- All dungeon blue & red warps are available when you enter the dungeon for the first time
- All puzzles are already solved
- All cutable trees, bombable blocks, bombable walls, and pushable boulders are moved
- All wind crests are open
- All fusions are already completed
- Graveyard is already opened
- All fights for items except dungeon bosses are completed
  - This does not apply to Dark Hyrule Castle

#### Decoupled Fusion Settings!

You can now adjust settings for each color of kinstone fusion rather than changing them all at once! We also added options that were previously only for Red, Green, and Blue fusions to Gold Fusions as well!

The settings available are:
- Removed (Closed) - Removes these fusions completely. This means that the locations locked by these fusions are now accessible. 
  - If Gold Kinstones are set to removed then:
    - All of upper Veil Falls and Cloud Tops are inaccessible without one of Cloud Tops or Veil Falls wind crests being open
    -  All of Palace of Winds and upper Wind Tribe are inaccessible without Cloud Tops wind crest being open, If red fusions are removed then lower Wind Tribe is also inaccessible.
    - All of Wind Ruins and Fortress of Winds are inaccessible
- Vanilla - Makes all of the given color of fusions vanilla. This means you have to fuse with NPCs to open locations on the map.
  - Castor Wilds Golden Kinstones all use the same kinstone even with this setting on
  - Cloud Tops Golden Kinstones all use the same kinstone even with this setting on
- Combined - Makes all the fusions of a given color use one kinstone type for the given color. This means you only need one type of each color kinstone for all the fusions of that color. You still need to fuse with NPCs to open the locations.
- Completed (Open) - Makes all of the fusions of the given color open, making the locations accessible as soon as you can get to them. This setting now works for gold fusions too!

Additionally, we now have an option for Defickle Fusers. In Minish Cap, there are certain NPCs who only offer fusions sometimes. This changes their behavior to always offer their fusion.

#### No Logic!

Added an option to generate a seed without any logic for placing items. Does not work with the Hendrus Shuffler. Most seeds with this setting will be impossible to beat without Open World or starting with the Firerod.

#### Detailed Logic!

You can now toggle on specific logic requirements instead of them being assumed in logic! There are also some new logic settings available.

The logic settings available are:
- Bombs are treated as weapons - If toggled on, then bombs may be required for fights. If `yes + bosses` is selected, then some boss fights may require using bombs. This can be very challenging if you don't know how to do the bosses with bombs.
- Bows are treated as weapons - If toggled on, then the bow may be required for fights.
- Gust Jar is treated as a weapon - If toggled on, then it may be required to kill Ghini or Helmasaurs with the gust jar.
- Lantern is treated as a weapon - If toggled on, then it may be required to complete the wizzrobe fight in Fortress on the ground floor with the Lantern.
- Mole Mitts farm Infinite Rupees - If toggled on, then you may be expected to farm rupees with the mole mitts in front of Link's House in South Hyrule Field for shop items. There is a spot in front of the house where you can dig up a red rupee that respawns.
- Bombs blow away dust - If toggled on, then you may be required to use bombs to reveal switches underneath dust as well as reveal the portal in Mt. Crenel to get the green bean.
- Crenel Mushroom Grab with Gust Jar - If toggled on, this allows you to get to upper Mt. Crenel by sucking the mushroom with the gust jar.
- Light Arrows break Objects - If toggled on, then you may be expected to use light arrows to break objects like pots or cutable trees to progress through the seed.
- Use bobombs to destroy walls - If toggled on, then you may be expected to use the bombombs to blow up the bombable walls in Cavern of Flames instead of bombs.
- LikeLike cave without sword - If toggled on, then the LikeLike digging cave in Minish Woods is in logic with mole mitts even if you don't have a sword.
- Skip town guard using boots - If toggled on, then you may be expected to use a well placed boot dash to skip the guard on the left side of town preventing you from getting into Trilby Highlands.
- Use beam to hit switch on Mt. Crenel - If toggled on, then you may be expected to use Sword Beam or Peril Beam to hit the switch on Mt. Crenel to get to Melari's Mines.
- Use down thrust to flip Spikey Beetles - If toggled on, then you may be expected to use down thrust to flip over the spikey beetles instead of bombs, a shiled, or the cane of pacci.
- Dark rooms without Lantern - If toggled on, then dark rooms will not require the lantern to get through. The one exception is the room before the blue warp in Temple of Droplets, which is in logic without lantern even if this setting is off.
- Tricky cape jumps across water - If toggled on, then you may be required to do cape extension to cross large bodies of water without flippers, or do unintuitive diagonal jumps. This allows you to get to temple of droplets entrance with only cape.
- Minish in Lake without Boots - If toggled on, then you may be required to jump down from the platform in front of Librari's hosue in Lake Hylia to get to the Mayor's Cabin, the water locked Minish Check in Minish Woods, and the water locked Minish Check in the top corner of Lake Hylia.
- Swim to Cabin Without Lilypad - If toggled on, then you may be required to swim into Mayor's cabin as minish without the lilypad. This also lets you get the fusion chest on the board in the water path.
- Kill Cloud Sharks without weapons - If toggled on, then you may be required to kill the cloud sharks in Cloud Tops by baiting them to jump off the edge of the platform.
- Palace of Winds 2F clearable without cane - If toggled on, then you may be required to do a precise cape jump to a moving platform on the 2nd floor of Palace of Winds to skip using the cane.
- Palace Pot Puzzle without Four Sword - If toggled on, then you may be required to hit the switch to get the item normally locked by Pot Puzzle in Palace of Winds by getting to that room in the 2nd half of Palace and hitting the switch then backtracking to the room from the entrance.
- Dark Hyrule Castle Cannon Rooms without Four Sword - If toggled on, then you may be required to complete the two cannon rooms in Dark Hyrule Castle with precise bomb throws. This only applies if you can get into Dark Hyrule Castle without four sword.
- Dark Hyrule Castle Pressure Pads without Four Sword - If toggled on, then you may be required to complete the pressure pad puzzle in Dark Hyrule Castle to get a chest to spawn without the four sword. Only applies with Cannon Rooms without Four Sword on.
- Dark Hyrule Castle Switches without Four Sword - If toggled on, then you may be required to complete the two switch rooms in Dark Hyrule Castle that require cloning with precise spin attacks instead. Only applies with Cannon Rooms without Four Sword on.
- Fortress of Winds pot item early with Gust Jar - If toggled on, then you may be required to grab one of the shuffled pots in Fortress of Winds with the gust jar through the wall. Only applies if Pot Items are shuffled.

#### Item Pool Settings!

You can now switch between three different types of item pools!
- Balanced - The number of required items in the seed matches the vanilla game.
- Plentiful - There are extras of all required items in the seed. Extra keys are only added in Keysanity.
- Reduced Item Pool (RIP) - Only major items required to complete the seed are shuffled. If used with traps on, then extra traps are added to the item pool.

#### Progressive Items!

You can now choose whether items are progressive or not! With progressive items on, there is a new icon to show that something is a progressive item. With progressive items off, you can get items like swords in any order.

#### Firerod!

You can now choose whether to shuffle the firerod into the pool or start with it. This is an item used for debugging by the develoeprs that was left in the game, and we don't recommend it for use by most people.

#### Fun Junk!

Some fun items have been added to the junk item pool! These items have no effect and do nothing, they are just for fun. The items added are:
- Broken Picori Blade from the intro
- Sheathed Smith Sword from the intro
- Brioche, Cake, Pie, and Croissant from the bakery
- Otherwise useful Items that are rendered useless due to the settings selected

#### Item Name Display!

When you pick up items like kinstones or keys in keysanity, it will say what specific type of kinstone or key it is in the bottom left corner of the screen for a few seconds.

#### Shy Fairies!

If you have a fairy in a bottle and die, you will not be revived and will game over. Fairies have to be used at low health instead.

#### Homewarp!

By selecting the sleep option on the pause screen, you can warp back to Link's bedroom in his house. This is useful for getting out of softlocks and quickly getting back to a central part of the map.

#### Minish Dash!

You can now use boots as minish with the setting enabled allowing you to move much faster! This is a little bit buggy.

#### Changing Clones!

In the pause menu while the cursor is over the sword press the `Select` button to swap how many clones you will create! It only lets you cycle between 2 and the maximum number you can create. 

#### Maximum Health!

You can change the maximum number of hearts that you can get! Your health pool can increase above the number, but you will never heal past your maximum number of hearts. This removes sword beam from being required in logic.

#### Damage Multiplier!

You can change the damage multiplier to be 1x, 2x, 3x, or 4x. On 4x, many end game enemies like black darknuts or vaati can 1-hit you.

Note: Do not use this setting with OHKO timer mode, it will fail to patch the ROM due to a conflict.

## Other Changes

### Misc Changes

- Added tournament winners and new developers to the credits
- Added some enemy types that were forgotten about to the credits
- Many setting names have been updated to improve clarity
- Text speed is now default set to fast
- Version number is displayed in game on the top corner of the file select screen
- Dojo tutorial cutscenes are now skipped! No more cutscenes to get scrolls
- Melari has been added to Cavern of Flames dungeon so if Cavern of Flames is not an element and non-element dungeons are barren, Melari will never have an item
- Traps triggered have been added to the credits
- A few more items have been added to the pool such as:
    - Refill Hearts
    - 10 & 30 Bomb Refills
    - 10 & 30 Arrow Refills

### Core

- Upgraded to .net 6 from framework 4.5. This comes with speed improvements and better cross platform support.

### Settings

- Items are able to lock themselves once Vaati is beatable.
  - This means that not all locations are accessible under most circumstances. We plan to add this back in the future.
- Major changes to the logic file structure, added options, and improved randomization.
- Fixes to the shuffler resulting in a way lower likelihood of seeds failing to generate.
- Removed caching from the shuffler due to some randomization issues it caused. This means seeds take much longer to generate than before. We plan on implementing caching in a better way in the future.

### Spoiler Log

- Added the version of the randomizer to the top of the spoiler log
- Added the settings string to the top of the spoiler log so when you share the spoiler it has the settings info in it
- Added a playthrough at the bottom of the spoiler! This shows the progression of major items in the seed to get to vaati.

### Logic Files

- Added a dungeon tag, `NoSpoiler`, which can be added to locations to hide them from the spoiler log.
- Locations can now support multiple dungeon IDs.
- Item pool is now its own section of the logic, decoupled from locations.
- Added the following location types:
  - DungeonEntrance - used to specify this location is a dungeon entrance
  - Music - used to specify this is a helper location for music shuffle
  - DungeonConstraint - used to specify that this location is used as a constraint for shuffling items in dungeons.
  - OverworldConstraint - used to specify that this location is used as a constraint for shuffling items in the overworld.
  - DungeonPrize - used to specify that this location holds a dungeon prize.
  - Major - used to specify that this location can only hold major items.
  - Dungeon - used to specify that this location can only hold dungeon items.
  - Any - used to specify that this location can hold major, minor, dungeon, or filler items.
  - Minor - used to specify that this location is shuffled but can only have junk items.
  - Inaccessible - used to specify that this location is inaccessible and therefore should be filled with junk. Inaccessible locations are hidden from the spoiler log.
- Items now have types that determine what locations they can get shuffled on
- Items can now have a dungeon ID which limits what locations they can be shuffled to
- Added the following item types:
  - Music - used to specify that this item represents a music track, can only get shuffled on music locations
  - DungeonEntrance - used to specify that this item represents a dungeon entrance, can only get shuffled on dungeon entrance locations
  - DungeonConstraint - used to specify that this item is used for a constraint, can only get shuffled on dungeon constraint locations
  - OverworldConstraint - used to specify that this item is used for a constraint, can only get shuffled on overworld constraint locations
  - DungeonPrize - used to specify that this item is a dungeon prize, lets you make any item a prize. Can only get shuffled on dungeon prize locations.
  - DungeonMajor - used to specify that this item is a dungeon major item (small key, big key, map, compass). Can only get shuffled on dungeon or any locations with a matching dungeon id.
  - Major - used to specify that this item is a major item. Can only get shuffled on major or any locations.
  - Minor - used to specify that this item it a minor item. Can get placed anywhere once all other items are placed and doesn't check logic.
  - Filler - used to specify that this item should be used to fill any unfilled locations when randomization ends. Can be placed anywhere and doesn't check logic. 
- Added tooltips to every element type in the logic file
- Added window tab text to every element type in the logic file, used to put settings on specific pages
- Added setting group text to every element type in the logic file, used to group related elements together in boxes on the UI
- Changed "Gimmick" setting type to "Cosmetic" for clarity

## Bug Fixes

- The spoiler log now displays the name of the dungeon a dungeon item belongs to instead of the subvalue, making it easier to read
- Miscellaneous fixes to how keys are placed in dungeons to lower seed generation failure rate
- Fixed a few logic issues in 0.6.2
- Music shuffle no longer changes randomization
  - As such, music has been moved to the cosmetics tab, and you can now choose to use shuffled music if you want without it changing the seed
- Increased accuracy of in game times in the credits

## Useful Resources

- EmoTracker download - https://emotracker.net/download/
  - Get Deoxis' tracker on EmoTracker, it has support for the newest settings. No other trackers are updated with the newest features.
- BizHawk download - https://tasvideos.org/Bizhawk/ReleaseHistory#Bizhawk242
  - Required prerequisites - https://github.com/TASEmulators/BizHawk-Prereqs/releases
- mGBA download - https://mgba.io/downloads.html

## Known Issues

- The application does not look good if Windows Scaling is set above 100%. There is a fix that has been developed but it is still under testing. In the meantime, if you want to generate a seed, set your windows scaling to 100%
- Options in the About tab currently do nothing
- Figurine hunt is currently broken and has been disabled until we finish a fix
- When the Graveyard key is knocked out of link's hand, it looks like the progressive sword but behaves normally
- Electric Chus have buggy animations
- Spoiler log Playthrough isn't perfectly accurate, it may miss some major items.
- Beanstalks, Wind Crests, and many other things when Followers are selected can have buggy sprites but function normally.
- The Clouds in Palace of Winds sometimes are invisible but function normally, leaving and re-entering the room they are in can make them visible again.


