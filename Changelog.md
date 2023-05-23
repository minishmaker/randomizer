# Version 1.0.0 RC 1


#### This is a pre-release version

## What's New!

---

### Randomization Updates

The random number generator has been changed to support 64 bit seeds now, written out as a 16 character hex string. This is to ensure more integrity for our races.

### UI Updates

#### New & Improved Pages

- About page has now been created! It contains some useful links for the randomizer, links include
  - A link to the TMCR Discord
  - A link to the ZSR Discord
  - A link to the Github
  - A link to the releases
  - A link to download EmoTracker
  - A link to download Bizhawk
- Seed output page now shows what the in-game hash for the seed is when you generate a seed! This means no more having to run the seed to see the hash.
  - A button was added to let you copy the hash to the clipboard

#### New UI Features

- Verbose logger makes its return! With the issues plaguing the verbose logger finally fixed, it can now be enabled. Especially useful for people who want to mess with logic!
- Automatic update checker! When the randomizer is started it will automatically check to see if an update is available. If Github is down for any reason it will have a slight delay before letting you know it failed to check.
  - A setting was added to let you choose whether or not to check for updates on start, this can be found under the "Other" tab
  - When the alert is displayed letting you know that an update is available, there will be text you can click that will take you to the page where you can download the latest update
    - We are exploring solutions to make it so you can click a button and it will update the app for you, however this is very much still an idea and likely will not be coming for some time
- A button to check for updates has also been added to the "About" tab that when clicked will run the update check.
- Hendrus Shuffler has graduated from the Experimental category! Now you too can generate plandos with ease!
- Removed the ability to turn off the logger. When errors occur we need the logs as developers so we can fix bugs.
- Removed the changelog page from the UI

### Mystery Settings

Mystery settings have been added to the randomizer! There are some default mystery setting presets that come built in, and people can also make their own!
Mystery settings use YAML. You can use the "Save Mystery YAML Template" button found by clicking "Logic" on the menu bar to export a copy of the template that you can edit.

- Mystery settings can be found at the bottom of the "Advanced" page
- You can use both Mystery Settings as well as Mystery Cosmetics
- Included Mystery Settings Presets are:
  - Expert
  - Friendly
  - Template
  - Unweighted (Everything has an equal chance of being on or off)
- Included Mystery Cosmetics Presets are:
  - Random Follower
  - Template
  - Unweighted
- Buttons have also been added next to the dropdown that you can click to generate settings from the preset and view them in the UI
- To save a mystery preset, save it in the "Presets" folder in either "Mystery Settings" if it is a settings preset or "Mystery Cosmetics" if it is a cosmetics preset
  - Buttons to add and remove presets coming in the next version
- You can click the button "Save Selected Options as YAML Preset" to save options as a preset and share them with others
  - You can load presets shared in this way by selecting "Use Global YAML" on the "Advanced" page

#### Changes for Mystery Settings

- When mystery settings are being used you cannot copy the settings string to the clipboard and it will not be shown on the output page
  - This is also the case for mystery cosmetics
- When any YAML setting is used there is some obfuscation done on the icons for the "settings" done in the hash meaning that if you use the same settings with different seeds you will get different results. This is to ensure mystery setting races don't run the risk of people memorizing what settings map to what icons.

### Settings

#### Decouple Open World

The following settings will no longer be affected when toggling Open World on/off:

- All Key settings, specifically locked doors
- All Fusion settings
- Wind Crests
- Dungeon Warps

There is the potential for further customization of settings in the future, these changes have these ideas in mind without having to commit to completing them in the short term.

#### Features added by Catobat

- Kinstone Packs and Key Chains
  - A multiplier is added that determines how many Pieces/Keys are added to the player inventory when one is collected.
- Speed up Pedestal Cutscene (When collecting an item)
- Dungeon Maps and Compasses have independent shuffle options.
- Vanilla Elements option added to force elements onto their vanilla locations.
- Wind Tribe now has an setting to be automatically set to opened from the start of the game.
  - Tingle brothers also received a similar setting, skipping having to talk to Tingle first.
- New Cosmetic option: Heart Border Colour.
- The game can no longer be started on inaccurate emulators.
- Gentari has text that tells you the goal and requirements for the seed, useful for Mystery settings seeds.

#### DHC added to Dungeon entrance shuffle

It was a terribly kept secret in my earlier commits, and a few races were done on dev builds so the cat is out the bag. Here are some of the changes to the first attempt to the added option.

- Fixed a bug Myth made in the Green Warp destination coordinates when leaving a dungeon located in Castle Gardens.
- Pedestal needs to check for access requirements (Thanks Deoxis)
- Fixed generation failure when DHC was set to 'Never' and entrances where shuffled, resulting in the chance of a dungeon being shuffled to DHC but never being considered reachable.


#### Misc Features

- Progressive items have been seperated and have independent options.
  - Swords, Bows, Boomerangs, Shields, and Spin Scrolls
  - This also includes a general bugfix to Dojos
- Certain unshuffled locations that held major items in vanilla (HPs, LLR Key, Kinstones) now hold **NO** item, it is completely empty when you don't shuffle it into the pool.
- Various clarifications in the Tooltips and neatening the UI.
- Homewarp will be always be enabled when the setting combination has the possibility of allowing a softlock.
- Certain combinations of Shuffled Dungeons with specific settings resulted in low generation success rates, we adjusted what was possible to make these far more likely to successfully generate seeds.

### Bug Fixes

#### Community sourced bugs

The Discord community has been invaluable, bringing us to the attention of many issues that would be difficult to find otherwise:

- DWS Blue Warp not spawning (Unfortunately affected many people)
- ToD Blue Warp logic (Thanks Deoxis)
- CoF Blue Warp logic (Thanks Deoxis)
- Trilby/Crenel logical access from swamp wind crest (Thanks Nesman)
- Spoiler logs sphere section breaking when generating multiple spoiler logs (Thanks Tabbomat)
- Fix the logic of the North Minish Woods HP to first check for access to Minish Woods first when considering if it is grabbable from a distance. (Thanks CapTem)

#### 'Bulk Generation Testing' sourced bugs

A bulk generator has been implemented for use with the CLI which has allowed us to generate X number of seeds with random settings to find setting combinations that cause failures, here are the following bugs resolved through this method:

- Dojos causing seed failures when red fusions removed.
- Dungeon Warp setting causing certain vanilla key placements not be logically reachable.
- Dungeons required lowered to a valid amount if not all dungeons reachable.
  - Dungeon requirement and shuffled elements now correctly redefine the non-element dungeon settings
- Keys having a lot of issues:
  - Temple of Droplets Key Logic got overhauled to behave when dungeon warps are enabled and to obey regional key rules.
  - Vanilla placements are correctly checked instead of being assumed, and play nice when other dungeon items are placable.
- Recursive dependency for Beat Vaati making some end loaded seeds fail.

#### Misc Bug fixes

- Picking up free standing 10 or 30 bombs or arrows no longer gives a text box.
- Spoiler log playthrough now lists all major items correctly
- One of the graves in the graveyard was closed when it was automatically open in vanilla
- Fixed the sword trick logic in DHC being inverted.
- Melari now respects barren rules if Cave of Flames is Non-Element.
- Minor fix to the 'Failure Rate' stat when batch generating test seeds, is now a 'Success Rate' and is now correctly a %
- Comment out Fuzzyness Setting. Sorry but no one uses it and it was taking space in the UI from Heart Colour Borders, which is unlikely people will complain about having instead. Don't @ Myth
- A 'bushwhacker' has been taken to the Logic file in the overgrown location paramater splurge, found just before the Location defines, its a bit better than before but still needs completely ripping out and moving somewhere else in the file (or into a different file 👀?)

### CLI Improvements

- BulkGenerateSeeds has more stats
  - Outputs the success rate instead of the failure rate
  - Lists the time taken to generate all the seeds and the average time per seed
- Logs from BulkGenerateSeeds are now written to a file called "CLIBulkGenOutput.json" where you ran the CLI from
- BulkGenerateSeeds will now also try and patch the seeds as well to test patches work
- YAML is now supported by the CLI
- 'UseRandomColor' can be used for color options

### Setting Presets

Due to the large amount of settings available in the randomizer it can be hard to decide what to choose, so we made some presets for players to be able to experience all the main features of the randomizer!
Here are some short descriptions of what you can find in each preset.

#### Basic Presets

It is recommended that players first play **Beginner**, then **Intermediate**, and then **Advanced**, to become familiar with most features of the randomizer.

- **Beginner**: Recommended for anyone who has not played 'The Minish Cap Randomizer' before, especially if you have not played the base game in many years. These settings are very short, removing the longer dungeons found later in the game, so that you can experience a quick introduction to the randomizer in one sitting. You win the game once you visit the sanctuary after finding all sword upgrades to acquire the Four Sword, and collecting all 4 Elements from beating the bosses of the 4 available dungeons.
- **Intermediate**: Recommended once players have re-familiarized themselves with the game world, controls, and has learned the majority of the item locations. Adds more locations, and randomizes more items, to build on what the player has learned so far. You win the game after defeating Vaati at the end of Dark Hyrule Castle, once you have the Four Sword and all 4 Elements which are now shuffled to any of the 6 available dungeons (checked on the map screen).
- **Advanced**: Recommended once players have become comfortable with the options present in **Intermediate**. Adds much more complex settings to show off a greater depth of randomization, such as dungeon shuffle and forcing dungeons without an element to be barren (not containing any important items). It will also test your ability to do more advanced strategies to reach locations not considered required to beat seeds on easier difficulties, such as certain trickier cape jumps and unintuitive routes.

#### Race Presets

These are the current and historic race settings for the randomizer. The current race settings are recommended for players wishing to practice for racing and upcoming tournaments.

- **Version 1.0 Tentative Race settings**: These are the current race settings for community races once v1.0 is released. Similar to Advanced settings, however all logic tricks are *not required*, it is up to the player to decide if they wish to do 'out of logic' checks.
- **Tournament 4 Race settings**: These were the settings used in the previous 2022 Tournament, played on the prior v0.6.2 'Tourney Build'.

#### Extra Presets

The following presets are intended to be played once one has become comfortable with the **Advanced** preset listed above. Some presets are useful to become more comfortable in specific parts of the randomizer, such as kinstones or figurines, while others are meant to be challenging to all players, such as the **Expert** preset. 

- **Expert**: This is the MOST challenging combination of settings currently present. It requires an intimate knowledge of all aspects of the base game as well as all randomization options. Completing these settings in under 3.5 hours is proof you are a 'Minish Cap Expert'.
- **Max Ranom**: This randomizes all possible things in the game that can be randomized, including all possible items and all possible locations. The difficulty is toned down so that it is enjoyable for most skill levels.
- **No Logic Open World**: Items are randomized with no consideration for logic, making seeds have the possibility to be unbeatable, however most world obstacles are removed to give the player a fighting chance. Players will have to think outside of the box, and may have to utilize speedrun glitches, in order to beat these seeds.
- **Firerod**: These settings all focus on the Firerod item, a leftover developer tool that has been re-introduced to give the player a completely different way of playing the game. Given to the player from the start, it allows the player to copy and paste tiles which effectively lets them walk through any wall/obstacle. Many advanced strategies and techniques can be utilised such as warping between regions and cloning items.
- **Funky Fusions**: These settings focus on kinstone fusions. Somewhere between Intermediate and Advanced in terms of difficulty, this is aimed as an introduction to learning vanilla fusion locations and NPC's. Kinstones are in packs, so collecting one you will add the exact amount to your bag to complete all the fusions of that size and shape.
- **v0.1.0 Settings**: These settings emulate the very first version of the randomizer, released in 2019. None of the logic errors are present. The setting hash is two Green rupees since it was the first version of the randomizer.
- **Figurine Hunt**: Similar to 'Triforce Hunt' found in many other Zelda randomizers, These settings are roughly in-between Intermediate and Advanced in terms of difficulty. 30 Figurines are added to the pool and at least 20 must be collected before you can complete the game.
- **Regional Dungeon Items**: Similar to 1.0 Race Settings, but without Dungeon Shuffle. Dungeon locations are expanded to include the locations found in the region surrounding the dungeon. This allows keys and other dungeon items to be shuffled to these locations, and these regions are considered barren if the dungeon does not have an element as the prize.

### Known Issues

- With closed golds, entrance shuffle on, element shuffle on, some settings combinations have a high chance of failing to beat Vaati. This has no easy code fix until Logic v2 is done.
  - To work around this, click the "New Seed" button and try again, or set your "Max Randomization Attempts" to 5 or more and try and randomize again
- When the Graveyard Key is knocked out of your hands, it looks like the progressive sword.
- Electric Chu’s have buggy animations.
- Regional Keys combined with Dungeon Entrance Shuffle is unintuitive, eg: If Fortress is shuffled to Crenel, Small Keys for Fortress can be found in Wind Ruins but not in Crenel.
- Getting a second bomb bag with remotes equipped sometimes changes it back to regular bombs.
- Followers cause a fair amount of visual bugs ranging from miss coloured palettes, to certain sprites being completely missing.
- The Clouds in Palace of Winds sometimes are invisible but function normally, leaving and re-entering the room they are in can make them visible again.
- Credits stat for Door Mimics is very wrong, how big of a number can you get? Post your record in the discord!

### Useful Resources

- EmoTracker download - https://emotracker.net/download/
  - Get the Deoxis' tracker with this version on EmoTracker, it has support for the newest settings. No other trackers are currently updated with the newest features.
- BizHawk download - https://tasvideos.org/Bizhawk/ReleaseHistory#Bizhawk242
  - Required prerequisites - https://github.com/TASEmulators/BizHawk-Prereqs/releases
- mGBA download - https://mgba.io/downloads.html

# History

# 0.7.0 Rev 3.1

#### This is a pre-release version

## What's New!

---

### Settings

The last remaining issues with the parser and shuffler have been resolved and any settings that went missing in the previous build now return! This also includes the return of location reachability (revision 2 only supported the option where any unrequired items could be locked).

It also means that we can focus more on adding entirely new options. There is no shortage of new and exciting options, please take a look and give them a try!

#### New Settings

- Requirement Reward: Can enable a new item location, the item of which is given to the player once they meet the selected requirements (elements, sword, dungeons and figurines).
	- Disabled: This location doesn’t exist.
	- DHC Big Key: The DHC Big Key is instantly awarded to the player once they meet the selected requirements.
	- Random Item: An item from the random item pool is instantly awarded to the player once they meet the selected requirements.
- Cucco Minigame: Determine how many out of the 10 rounds have a random item. The remaining rounds are skipped (by default, only one round is playable, so the first 9 are skipped). You can even disable it entirely (you can still play round 10 for the rupees, but not for an item).
- Goron Merchant: Choose how many sets of Goron Merchant items are included in the random item pool. Each set consists of 3 items, and all 3 must be bought to advance to the next set. There are 5 sets in total. The remaining sets will be filled with junk items. Note that the Goron Merchant is only available if Blue Fusions aren’t removed.
- Seeded Shared Fusions: This setting affects Red, Blue and Green Fusions if set to Vanilla or Combined. There is a Pool of 18 Shared Fusions in the Game: 16 Green, 1 Blue and 1 Red. These Fusions are randomly selected by an NPC who offers Shared Fusions when you enter the room they are in. If this setting is enabled then the seed generates a listed order these NPCs offer the Fusions in. This makes different people playing the same seed have the same Fusions available which is useful for races.
- Hyrule Town Wind Crest: Previously this used to be always activated from the start. This is no longer the case (unless Homewarp is disabled), now it can be toggled.
- Dungeon Warps: The 12 blue and red dungeon warps can now individually be opened from the start.
- Location Reachability: This setting determines which items and locations are guaranteed to be reachable. There are now 3 options:
	- All Locations: All enabled locations (depending on other settings) are reachable.
    - All NonKeys: Like All Locations, except keys can lock themselves.
    - Only the Goal: Only the goal is guaranteed to be reachable. Some locations and items may be inaccessible. If the DHC setting is set to “Never Open”, then the goal is the pedestal requirements, otherwise the goal is Defeat Vaati.
- Grab items from a distance: If enabled, items can be collected from a distance with either Gust Jar, Boomerang, or even a Sword Slash. There are 4 options:
	- Not Allowed: Items cannot be grabbed from a distance (except rupees and certain junk).
    - Allowed: Items can be grabbed from a distance, but this will never be required by logic.
    - Require: Items can be grabbed from a distance and logic can require grabbing them when they are obviously grabbable.
    - Require Hard: Items can be grabbed from a distance, and logic can require some very difficult Boomerang throws to be able to collect these items. Many of these involve throwing the Boomerang in an arc behind or to the side of the items before making Link run in the opposite direction to get the Boomerang to grab the item on the return journey to Link.
- Bows: New option Yes + Bosses: Only if this is enabled, bows can be solely required to defeat Gleerok and Mazaal.
- Goron Merchant JP/US prices: If enabled will use the prices found in the JP and US versions for the Goron Merchant items. The tooltip shows all the prices for all regions.
- Low Health Beep: The number of frames between the low health beeps. One second is 60, vanilla value is 90, max 255.

#### Returning Settings

- Figurine Hunt: Figurines are added to the item pool and are required to activate the Sanctuary. The amount of Figurines in the pool and the amount needed for the Sanctuary can be chosen.
- Random Bottle Contents: Bottles will start out filled with various different contents, see if you can find the butter.

#### Now Optional instead of Always On

- Instant Text
- Boots on L (in a dropdown with “Minish Dash”)
- Ocarina on Select (now also works if “Prevent Speedrun Glitches” is off)
- Disable Random Kinstone Drops
- Disable Random Shell Drops

### Improvements to the Previous Build

#### UI Changes

- Window scaling above 100% is improved, but there’s still room for improvement in the future. Let us know if you encounter any issues!
- Custom logic files can be used again! Go to the “Advanced” tab, enable “Use Custom Logic File”, and select the file.
- Custom patch files can be used again! Go to the “Advanced” tab, enable “Use Custom Patch Files”, and select the file.
- The UI is slightly smaller now to support smaller screens.
- Numberbox settings have up and down buttons in the UI, and their value can also be adjusted using arrow keys.

#### CLI Changes

- First of all, the CLI is working again!
- File output was broken if no output path was specified, this is now fixed.
- There are no more “Press enter to continue…” or confirmation messages.
- The “Exit” command works in more situations.
- When listing the settings, the currently selected option is now shown at the end of each line.
- To choose an option to change, you can also enter the name of the option instead of the listed number.
- Flag options can also be disabled by entering “0”.
- There’s a new “Strict” mode that makes it so the CLI exits when an error occurs.
- Command files can now be provided.

#### Shuffler Changes

- Dropdown settings can now have any of their options set as the default option. For example in the default logic, dungeon items now default to “Own Dungeon” like in previous builds.
- Numberbox settings can have a min and a max value set.
- The DungeonMinor item type returns to greatly improve seed generation rates. This type places these items in Dungeon locations just like DungeonMajor, but only after all DungeonMajor items are placed.
- The shuffler will now always attempt to make all “Minor” items (usually including Bottles, Pieces of Heart and Butterflies) reachable (assuming Reachability isn’t set to “Always the Goal” in the default logic file). This can fail if there are not enough locations available, most notably if Gold Fusions are Removed or if a lot of Figurines are included.
- Most items with subtypes now have names instead of numbers in the spoiler (Affects Keys, Maps, Compasses, Bottles and Traps. Figurines are still a WIP).
- Seeds now generate much faster (around 10% as long) due to better caching.

#### Patch Changes

- Shopkeepers mention Kinstone shapes and colors.
- Ocarina Menu change: If you select a Wind Crest and then "No", instead of exiting the menu, you can select another one (like in non-EU versions).
- With Shuffled Dungeon Entrances, elements are displayed on the map screen that actually contains that dungeon instead of the screen that contains that dungeon in vanilla.
- The North Hyrule Field bridge softlock prevention is now removed when Homewarp is enabled.
- When Hyrule Town is entered from Trilby Highlands, the soldier stops blocking that path even if you don’t have the items that normally move him. (This can matter with Open Wind Crests.)
- When the Roc’s Cape is obtained in Hyrule Town, you no longer have to screen transition before you can get the item from the bell.

#### Logic Changes

- Shop money logic is more flexible.
- Small Keys can now lock themselves in DHC_2F_BlueWarp_BigChest (if “Reachability” is set to “All NonKeys”).

#### Bug Fixes since rev2.1

- In Non-Progressive items: Red/Blue/Four Swords can now activate the 2nd Town Dojo items, and the last 3 items can be received in any order.
- Dungeon Entrance Shuffle no longer causes Link to have weird facing angles when going through transitions.
- Fixed digging spots sometimes having major items when digging spot shuffle is turned off.
- Fixed Golden Enemy items sometimes not dropping (for example if it holds 5 bombs, but the player has no Bomb Bag), making it impossible to permanently kill the enemy.
- Dampé can now always be encountered in this house, allowing to fuse with him even when he’s waiting for you to have the Graveyard Key stolen.
- Shared Fusions can now properly be Removed.
- If Green Fusions are Removed and Red Fusions Vanilla or Combined, Tingle’s remaining Fusion still requires the Magic Boomerang.
- A few issues with Pedestal requirements have been fixed, which could occur if no sword or element requirements were set. It’s also possible to have no requirements now.
- Golden Enemies and some Rupee spots used to be shuffled when not supposed to.
- Goron Merchant is now tied to the correct Kinstone color (blue) and spawns under the right circumstances.
- Fixed a logic bug with Upper Crenel.
- In Combined Gold Fusions, the Veil Falls Gold Fusion now logically requires all 9 Golden Kinstones if the Veil Falls or Cloud Tops Wind Crest is activated.
- Fixed some issues with Fortress of Winds with Open World or Barren.
- Barren and Unrequired Dungeons work correctly now.
- Fixes Shared Fusions logic not taking other Kinstones into account.
- The Reduced Item Pool used to contain 3 Bows by accident.
- The Shuffler didn’t verify the CRC of the selected ROM.
- The stone block at the Castor Wilds statues is now also visually destroyed with Open Gold Fusions.
- Item Name Display was buggy when Figurine Hunt was enabled.
- The “Skip Fusion Cutscene” options are now correctly applied.
- Fixed an issue with Ankle (Tingle’s brother on Lon Lon Ranch) that caused him to glitch out. This was also responsible for the flashing clouds on this screen. (This only took 3.5 years to fix!)
- Dungeon requirements are now actually enforced in the game.
- Homewarping no longer messes up the credits statistics and clone count.
- The roll count in the end credits is now accurate.
- Imported functions are now cleared between generating seeds.
- Fixed inaccuracies with vanilla Red Kinstone Piece locations.
- Fixed various logic issues in Open World.
- Fixed an issue when generating seeds with Vanilla Big Keys.

### Miscellaneous

#### Updates Planned for Future Builds

- Further improve UI scaling.
- Have localization options in the UI.
- Display seed hashes in UI and CLI.
- Starting equipment options.
- Completely decouple Open World setting into smaller settings.
- YAML file support for saving and sharing custom presets and Mystery weights.
- Custom item placements and item pool adjustments.
- Allow logic files to be split into multiple files.
- Logic for glitches and Firerod.
- Fix the known issues.

#### Known Issues

- When the Graveyard Key is knocked out of your hands, it looks like the progressive sword.
- Electric Chu’s have buggy animations.
- Regional Keys combined with Dungeon Entrance Shuffle is unintuitive, eg: If Fortress is shuffled to Crenel, Small Keys for Fortress can be found in Wind Ruins but not in Crenel.
- Major items sometimes do not show up properly in the Playthrough section of the spoiler log if they have a Minor version of the same item, or if they are unshuffled. (This has a pending fix in a later version)
- About and Changelog pages are not implemented yet.
- Verbose logger causes massive explosions in log size and has been disabled while we work on a fix.
- Getting a second bomb bag with remotes equipped sometimes changes it back to regular bombs.
- Followers cause a fair amount of visual bugs ranging from miss coloured palettes, to certain sprites being completely missing.
- The Clouds in Palace of Winds sometimes are invisible but function normally, leaving and re-entering the room they are in can make them visible again.

### Useful Resources

- EmoTracker download - https://emotracker.net/download/
  - Get Deoxis' tracker on EmoTracker, it has support for the newest settings. No other trackers are updated with the newest features.
- BizHawk download - https://tasvideos.org/Bizhawk/ReleaseHistory#Bizhawk242
  - Required prerequisites - https://github.com/TASEmulators/BizHawk-Prereqs/releases
- mGBA download - https://mgba.io/downloads.html


