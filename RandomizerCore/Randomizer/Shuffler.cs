using System.Reflection;
using System.Text;
using ColorzCore;
using RandomizerCore.Controllers;
using RandomizerCore.Core;
using RandomizerCore.Properties;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Helpers;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer;

internal class Shuffler
{
	public readonly string Version;

	private readonly List<Location> _locations;
	private readonly Parser.Parser _logicParser;
	private string? _logicPath;

	//Item lists are sorted in the order they are processed
	private readonly List<Item> _music;
	private readonly List<Item> _unshuffledItems;
	private readonly List<Item> _dungeonEntrances;
	private readonly List<Item> _dungeonConstraints;
	private readonly List<Item> _overworldConstraints;
	private readonly List<Item> _dungeonPrizes;
	private readonly List<Item> _dungeonMajorItems;
    private readonly List<Item> _dungeonMinorItems;
    private readonly List<Item> _majorItems;
	private readonly List<Item> _minorItems;
	private readonly List<Item> _fillerItems;
	private Random? _rng;
	private bool _randomized = false;
	
	public int Seed { get; set; } = -1;

	public Shuffler()
	{
		Version = GetVersionName();

		_locations = new List<Location>();
		
		_music = new List<Item>();
		_unshuffledItems = new List<Item>();

		_dungeonEntrances = new List<Item>();
		_dungeonConstraints = new List<Item>();
		_overworldConstraints = new List<Item>();
		
		_dungeonPrizes = new List<Item>();
		_dungeonMajorItems = new List<Item>();
        _dungeonMinorItems = new List<Item>();
		_majorItems = new List<Item>();
		
		_minorItems = new List<Item>();
		_fillerItems = new List<Item>();
		
		_logicParser = new Parser.Parser();
	}

	/// <summary>
	/// Throws a <code>ShufflerConfigurationException</code> if state is not valid with a message describing the
	/// validation failure
	/// </summary>
	public void ValidateState(bool checkIfRandomized = false)
	{
		Logger.Instance.LogInfo("Beginning Shuffler State Validation");
		
		if (Rom.Instance == null)
			throw new ShufflerConfigurationException("No ROM loaded! You must load a ROM before randomization.");

        if (!RomCrcValid(Rom.Instance))
			throw new ShufflerConfigurationException("ROM does not match the expected CRC for the logic file!");
		
		if (Seed < 0)
			throw new ShufflerConfigurationException($"Supplied Seed is invalid! Seeds must be a number from 0 to {int.MaxValue}");
		
		if (checkIfRandomized && !_randomized)
			throw new ShufflerConfigurationException("You must randomize the ROM before saving the ROM or a patch file!");
		
		Logger.Instance.LogInfo("Shuffler State Validation Succeeded");
		Logger.Instance.SaveLogTransaction();
	}

	public string GetVersionName()
	{
        //#if DEBUG
        //		return $"{AssemblyInfo.GetGitTag()}-DEBUG-{AssemblyInfo.GetGitHash()}";
        //#else
        //		return $"{AssemblyInfo.GetGitTag()}";
        //#endif
        return "0.7.0";
    }

	public string GetLogicIdentifier()
	{
		string fallbackName;
		string fallbackVersion;
		if (_logicPath != null)
		{
			fallbackName = Path.GetFileNameWithoutExtension(_logicPath);
			fallbackVersion = File.GetLastWriteTime(_logicPath).ToShortDateString();
		}
		else
		{
			fallbackName = "Default";
			fallbackVersion = Version;
		}

		var name = _logicParser.SubParser.LogicName ?? fallbackName;
		var version = _logicParser.SubParser.LogicVersion ?? fallbackVersion;

		return name;
	}

	public void SetSeed(int seed)
	{
		Seed = seed;
		_rng = new Random(seed);
		Logger.Instance.LogInfo($"Randomization seed set to {seed}");
	}

	public List<LogicOptionBase> GetSortedSettings()
	{
		return _logicParser.SubParser.GetSortedSettings();
	}

	public List<LogicOptionBase> GetSortedCosmetics()
	{
		return _logicParser.SubParser.GetSortedCosmetics();
	}
	
	public uint GetLogicOptionsCrc32()
	{
		return _logicParser.SubParser.GetLogicOptionsCrc32();
	}

	public uint GetCosmeticOptionsCrc32()
	{
		return _logicParser.SubParser.GetCosmeticOptionsCrc32();
	}

	public List<LogicOptionBase> GetOptions()
	{
		return _logicParser.SubParser.Options;
	}

	public uint GetSettingHash()
	{
		var settingBytes = _logicParser.SubParser.GetSettingBytes();

		return settingBytes.Length > 0 ? settingBytes.Crc32() : 0;
	}

	public uint GetCosmeticsHash()
	{
		var cosmeticBytes = _logicParser.SubParser.GetCosmeticBytes();

		return cosmeticBytes.Length > 0 ? cosmeticBytes.Crc32() : 0;
	}

	public string GetOptionsIdentifier()
	{
		return StringUtil.AsStringHex8((int)GetSettingHash()) + "-" + StringUtil.AsStringHex8((int)GetCosmeticsHash());
	}

	/// <summary>
	///     Load the flags that a logic file uses to customize itself
	/// </summary>
	public void LoadOptions(string? logicFile = null)
	{
		Logger.Instance.LogInfo("Loading Logic Options");
		_logicParser.SubParser.ClearOptions();

		string[] logicStrings;

		if (string.IsNullOrEmpty(logicFile))
		{
			// Load default logic if no alternative is specified
			var assembly = Assembly.GetAssembly(typeof(Shuffler));
			using (var stream = assembly?.GetManifestResourceStream("RandomizerCore.Resources.default.logic"))
			using (var reader = new StreamReader(stream))
			{
				var allLocations = reader.ReadToEnd();
				// Each line is a different location, split regardless of return form
				logicStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			}
		}
		else
		{
			logicStrings = File.ReadAllLines(logicFile);
		}

		_logicParser.PreParse(logicStrings);
	}

	public bool RomCrcValid(Rom rom)
	{
		if (_logicParser.SubParser.RomCrc != null)
			return rom.romData.Crc32() == _logicParser.SubParser.RomCrc;
		return true;
	}

	/// <summary>
	///     Reads the list of locations from a file, or the default logic if none is specified
	/// </summary>
	/// <param name="logicFile">The file to read locations from</param>
	public void LoadLocations(string? logicFile = null)
	{
		// Change the logic file path to match
		_logicPath = logicFile;

		// Reset everything to allow rerandomization
		ClearLogic();

		string[] locationStrings;

		// Get the logic file as an array of strings that can be parsed
		if (string.IsNullOrEmpty(logicFile))
		{
			// Load default logic if no alternative is specified
			var assembly = Assembly.GetAssembly(typeof(Shuffler));
			using (var stream = assembly?.GetManifestResourceStream("RandomizerCore.Resources.default.logic"))
			using (var reader = new StreamReader(stream))
			{
				var allLocations = reader.ReadToEnd();
				// Each line is a different location, split regardless of return form
				locationStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			}
		}
		else
		{
			locationStrings = File.ReadAllLines(logicFile);
		}

		var locationAndItems = _logicParser.ParseLocationsAndItems(locationStrings, _rng);

		_logicParser.SubParser.DuplicateAmountReplacements();
		_logicParser.SubParser.DuplicateIncrementalReplacements();
		locationAndItems.locations.ForEach(AddLocation);
		locationAndItems.items.ForEach(AddItem);
	}

	public void AddItem(Item item)
	{
		var newItem = CheckReplacements(item);

		if (newItem.HasValue) item = newItem.Value;
		
		switch (item.ShufflePool)
		{
			case ItemPool.Music:
				_music.Add(item);
				break;
			case ItemPool.Unshuffled:
				break;
			case ItemPool.DungeonEntrance:
				_dungeonEntrances.Add(item);
				break;
			case ItemPool.DungeonConstraint:
				_dungeonConstraints.Add(item);
				break;
			case ItemPool.OverworldConstraint:
				_overworldConstraints.Add(item);
				break;
			case ItemPool.DungeonPrize:
				_dungeonPrizes.Add(item);
				break;
			case ItemPool.DungeonMajor:
				_dungeonMajorItems.Add(item);
				break;
            case ItemPool.DungeonMinor:
                _dungeonMinorItems.Add(item);
                break;
            case ItemPool.Major:
				_majorItems.Add(item);
				break;
			case ItemPool.Minor:
				_minorItems.Add(item);
				break;
			case ItemPool.Filler:
				_fillerItems.Add(item);
				break;
			default:
				_minorItems.Add(item);
				break;
		}
	}

	public void AddLocation(Location location)
	{
		// All locations are in the master location list
		_locations.Add(location);

		if (!location.Contents.HasValue) return;
		
		var newItem = CheckReplacements(location.Contents.Value);
		
		if (newItem.HasValue) location.SetItem(newItem.Value);


		if (_logicParser.SubParser.LocationTypeOverrides.ContainsKey(location.Contents.Value))
			location.Type = _logicParser.SubParser.LocationTypeOverrides[location.Contents.Value];

		if (location.Type != LocationType.Unshuffled) return;
		
		// Unshuffled locations require contents, so add them here
		location.Fill(location.Contents!.Value);
		_unshuffledItems.Add(location.Contents!.Value);
	}

	private Item? CheckReplacements(Item item)
	{
		if (_logicParser.SubParser.IncrementalReplacements.ContainsKey(item))
		{
			var set = _logicParser.SubParser.IncrementalReplacements[item];
			var replacement = set[0];
			if (replacement.amount != 0)
			{
				replacement.amount -= 1;
				var newItem = new Item(replacement.item.Type,
					(byte)((replacement.item.SubValue + replacement.amount) % 256), replacement.item.Dungeon, false, item.ShufflePool);
				return newItem;
			}

			if (replacement.amount == 0)
			{
				set.RemoveAt(0);
				if (_logicParser.SubParser.IncrementalReplacements[item].Count == 0)
				{
					_logicParser.SubParser.IncrementalReplacements.Remove(item);
					Logger.Instance.LogInfo($"Removed incremental item, key {item.Type}");
				}
			}
		}

		if (_logicParser.SubParser.AmountReplacements.ContainsKey(item))
		{
			var set = _logicParser.SubParser.AmountReplacements[item];
			var replacement = set[0];
			if (replacement.amount != 0)
			{
				replacement.amount -= 1;
                var newItem = replacement.item;
				return new Item(newItem.Type, newItem.SubValue, newItem.Dungeon, false, item.ShufflePool);
			}

			if (replacement.amount == 0)
			{
				set.RemoveAt(0);
				if (_logicParser.SubParser.AmountReplacements[item].Count == 0)
				{
					_logicParser.SubParser.AmountReplacements.Remove(item);
					Console.WriteLine("removed key:" + item.Type);
				}
			}
		}

        if (_logicParser.SubParser.Replacements.ContainsKey(item))
		{
			var chanceSet = _logicParser.SubParser.Replacements[item];
			var number = _rng.Next(chanceSet.totalChance);
			var val = 0;

			for (var i = 0; i < chanceSet.randomItems.Count(); i++)
			{
				val += chanceSet.randomItems[i].chance;
				if (number < val)
				{
                    var newItem = chanceSet.randomItems[i].item;
					return new Item(newItem.Type, newItem.SubValue, newItem.Dungeon, false, item.ShufflePool);
				}
			}
		}

		return null;
	}

	public void ApplyPatch(string romLocation, string? patchFile = null)
	{
		if (string.IsNullOrEmpty(patchFile))
		{
			// Get directory of MinishRandomizer 
			var assemblyPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
			patchFile = assemblyPath + "/Patches/ROM Buildfile.event";
		}

		// Write new patch file to patch folder/extDefinitions.event
		File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

		string[] args = { "A", "FE8", "-input:" + patchFile, "-output:" + romLocation };

		Program.CustomOutputStream = null;

		Program.Main(args);
	}

	public void ApplyPatch(Stream patchedRom, string? patchFile = null)
	{
		if (string.IsNullOrEmpty(patchFile))
		{
			// Get directory of MinishRandomizer 
			var assemblyPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
			patchFile = assemblyPath + "/Patches/ROM Buildfile.event";
		}

		// Write new patch file to patch folder/extDefinitions.event
		File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

		string[] args = { "A", "FE8", "-input:" + patchFile, "-output:" + "usingAlternateStream" };

		Program.CustomOutputStream = patchedRom;

		Program.Main(args);
	}



	/// <summary>
	///     Shuffles all locations, ensuring the game is beatable within the logic and all Major/Nice items are reachable.
	/// </summary>
	public void RandomizeLocations(bool useSphereBasedShuffler = false)
	{
		var locationGroups = _locations.GroupBy(location => location.Type).ToList();
		//We now do randomization in phases, following the ordering of items in <code>LocationType</code>
		//Make it so randomized music doesn't affect randomization
		var temp = _rng;
		_rng = new Random(Seed);

		var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Music) ? locationGroups.First(group => group.Key == LocationType.Music).ToList() : new List<Location>();

		nextLocationGroup.Shuffle(_rng);
		FastFillLocations(_music, nextLocationGroup);
		
		_rng = temp;

		//Shuffle dungeon entrances
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonEntrance) ? locationGroups.First(group => group.Key == LocationType.DungeonEntrance).ToList() : new List<Location>();
		nextLocationGroup.Shuffle(_rng);
		FastFillLocations(_dungeonEntrances, nextLocationGroup);
		
		//Grab all items that we need to beat the seed
		var allItems = _majorItems.Concat(_dungeonMajorItems).Concat(_unshuffledItems).ToList();

		//Like entrances, constraints shouldn't check logic when placing
		//Shuffle constraints
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonConstraint) ? locationGroups.First(group => group.Key == LocationType.DungeonConstraint).ToList() : new List<Location>();
		nextLocationGroup.Shuffle(_rng);
		FastFillLocations(_dungeonConstraints, nextLocationGroup);

		
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.OverworldConstraint) ? locationGroups.First(group => group.Key == LocationType.OverworldConstraint).ToList() : new List<Location>();
		nextLocationGroup.Shuffle(_rng);
		FastFillLocations(_overworldConstraints, nextLocationGroup);

		// //Shuffle prizes
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonPrize) ? locationGroups.First(group => group.Key == LocationType.DungeonPrize).ToList() : new List<Location>();
		var unfilledLocations = FillLocationsFrontToBack(_dungeonPrizes, nextLocationGroup, allItems);

		//For dungeon majors, assume we have all majors and unshuffled items
		var allMajorsAndUnshuffled = _majorItems.Concat(_unshuffledItems).ToList();

		//Shuffle dungeon majors
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Dungeon) ? locationGroups.First(group => group.Key == LocationType.Dungeon).ToList() : new List<Location>();
		unfilledLocations.AddRange(FillLocationsFrontToBack(_dungeonMajorItems,
			nextLocationGroup,
			allMajorsAndUnshuffled,
			unfilledLocations));

        //Shuffle dungeon minors
        unfilledLocations.AddRange(FillLocationsFrontToBack(_dungeonMinorItems,
            unfilledLocations,
            allItems));

        unfilledLocations = unfilledLocations.Distinct().ToList();

		if (useSphereBasedShuffler)
			FillWithSphereBasedShuffler(locationGroups, ref unfilledLocations);
		else
			FillWithUniformShuffler(locationGroups, ref unfilledLocations);

		_randomized = true;
	}

	private void FillWithSphereBasedShuffler(List<IGrouping<LocationType, Location>> locationGroups, ref List<Location> unfilledLocations)
	{
		var itemsToPlace = _majorItems.Concat(_minorItems).ToList();
		var locationsToFill = _locations.Where(_ =>
				_.Type is LocationType.Any or LocationType.Major or LocationType.Minor).Concat(unfilledLocations).ToList();

		while (itemsToPlace.Count < locationsToFill.Count)
			itemsToPlace.Add(_fillerItems[_rng.Next(_fillerItems.Count)]);

		unfilledLocations = FillLocationsSphereBased(itemsToPlace,
			_locations.Where(location =>
				location.Filled && location.Contents.HasValue && location.Type is not LocationType.Helper
					and not LocationType.Inaccessible and not LocationType.Untyped).ToList(), locationsToFill);

		
		// Get every item that can be logically obtained, to check if the game can be completed
		var finalMajorItems = GetAvailableItems(new List<Item>());

		if (!new LocationDependency("BeatVaati").DependencyFulfilled(finalMajorItems, _locations))
			throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
		
		var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Inaccessible) ? 
			locationGroups.First(group => group.Key == LocationType.Inaccessible).ToList() : new List<Location>();
		unfilledLocations.AddRange(nextLocationGroup);
		unfilledLocations = unfilledLocations.Distinct().ToList();
		unfilledLocations.Shuffle(_rng);
		FastFillLocations(_fillerItems, unfilledLocations);
	}

	private void FillWithUniformShuffler(List<IGrouping<LocationType, Location>> locationGroups, ref List<Location> unfilledLocations)
	{
		//Now that all dungeon items are placed, we add all the rest of the locations to the pool of unfilled locations
		var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Any) ? locationGroups.First(group => group.Key == LocationType.Any).ToList() : new List<Location>();
		unfilledLocations.AddRange(nextLocationGroup);
		unfilledLocations = unfilledLocations.Distinct().ToList();

		//Shuffle all other majors, do not assume any items are already obtained anymore
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Major) ? locationGroups.First(group => group.Key == LocationType.Major).ToList() : new List<Location>();
		unfilledLocations.AddRange(FillLocationsUniform(_majorItems,
			nextLocationGroup,
			null,
			unfilledLocations));
		unfilledLocations = unfilledLocations.Distinct().ToList();
		
		// Put minor and filler items in remaining locations locations, not checking logic
		nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Minor) ? locationGroups.First(group => group.Key == LocationType.Minor).ToList() : new List<Location>();
		unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();
        unfilledLocations.Shuffle(_rng);
        FastFillLocations(_minorItems.Concat(_fillerItems).ToList(), unfilledLocations);
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Inaccessible) ? locationGroups.First(group => group.Key == LocationType.Inaccessible).ToList() : new List<Location>();
		unfilledLocations.AddRange(nextLocationGroup);
		unfilledLocations = unfilledLocations.Distinct().ToList();
		unfilledLocations.Shuffle(_rng);
		FastFillLocations(_fillerItems.ToList(), unfilledLocations);

        // Get every item that can be logically obtained, to check if the game can be completed
        var finalMajorItems = GetAvailableItems(new List<Item>());

        if (!new LocationDependency("BeatVaati").DependencyFulfilled(finalMajorItems, _locations))
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
    }

    private List<Location> FillLocationsSphereBased(List<Item> allShuffledItems, List<Location> preFilledLocations, List<Location> allPlaceableLocations)
	{
		var spheres = new List<Sphere>();

		var sphereNumber = 0;
		var obtainedItems = new List<Item>();

		var canBeatVaati = false;

		var locationsAvailableThisSphere =
			allPlaceableLocations.Where(location => location.IsAccessible(obtainedItems, _locations)).ToList();
		
		var shuffledLocationsThisSphere = locationsAvailableThisSphere
			.Where(location => location.Type is not LocationType.Unshuffled).ToList();
		
		var maxRetries = allShuffledItems.Count;

		var sphere = new Sphere
		{
			Locations = locationsAvailableThisSphere,
			TotalShuffledLocations = shuffledLocationsThisSphere.Count,
			SphereNumber = sphereNumber,
			MaxRetryCount = maxRetries,
		};

		foreach (var location in locationsAvailableThisSphere)
		{
			allPlaceableLocations.Remove(location);
		}

		var retryCount = 0;
        var totalPermutations = 0;

        while (!canBeatVaati)
		{
            if (totalPermutations++ > 50000)
                throw new ShuffleException("Could not find a viable seed with Hendrus Shuffler after 50,000 permutations! Please let the dev team know what settings and seed you are using!");

            shuffledLocationsThisSphere.Shuffle(_rng);

            var availableMajorItems = allShuffledItems.Where(item => item.ShufflePool is ItemPool.Major or ItemPool.DungeonMajor).ToList();
			var placedItemsThisSphere = new List<Item>();
			
			var forLoopRetryCount = 0;
			const int forLoopMaxRetries = 15;
			shuffledLocationsThisSphere.Shuffle(_rng);
			for (var i = 0; i < shuffledLocationsThisSphere.Count && forLoopRetryCount < forLoopMaxRetries; )
			{
				var location = shuffledLocationsThisSphere[0];

				var itemsWithSameDungeon = allShuffledItems.Where(item => location.Dungeons.Contains(item.Dungeon)).ToList();

				var placeableItems = allShuffledItems.Where(item => string.IsNullOrEmpty(item.Dungeon) || location.Dungeons.Contains(item.Dungeon)).ToList();

				var item = placeableItems[_rng.Next(placeableItems.Count)];

				if (itemsWithSameDungeon.Any())
					item = itemsWithSameDungeon[_rng.Next(itemsWithSameDungeon.Count)];

				if (!location.CanPlace(item, obtainedItems, _locations))
				{
					forLoopRetryCount++;
					shuffledLocationsThisSphere.Shuffle(_rng);
					continue;
				}
				
				location.Fill(item);
				shuffledLocationsThisSphere.Remove(location);
				allShuffledItems.Remove(item);
				placedItemsThisSphere.Add(item);
			}

			var preFilledLocationsPlacedThisSphere = new List<Location>();
			var preFilledItemsPlacedThisSphere = new List<Item>();

			for (var i = 0; i < preFilledLocations.Count;)
			{
				var pf = preFilledLocations[i];
				if (pf.IsAccessible(obtainedItems, _locations))
				{
					preFilledItemsPlacedThisSphere.Add(pf.Contents!.Value);
					preFilledLocationsPlacedThisSphere.Add(pf);
					preFilledLocations.Remove(pf);
					continue;
				}
				++i;
			}

			var locationsAvailableNextSphere = allPlaceableLocations.Where(location => location.IsAccessible(obtainedItems.Concat(placedItemsThisSphere).Concat(preFilledItemsPlacedThisSphere).ToList(), _locations)).ToList();

			if (locationsAvailableNextSphere.Count == 0 && preFilledLocationsPlacedThisSphere.Count == 0)
			{
				retryCount++;
				if (retryCount > maxRetries)
				{
					if (sphereNumber == 0)
						throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");

					sphereNumber--;

					allPlaceableLocations.AddRange(locationsAvailableThisSphere);
					allShuffledItems.AddRange(placedItemsThisSphere);

					var lastSphere = spheres[sphereNumber];
					allShuffledItems.AddRange(lastSphere.Items);
					
					foreach (var item in lastSphere.Items)
						obtainedItems.Remove(item);

					foreach (var item in lastSphere.PreFilledItemsAddedThisSphere)
						obtainedItems.Remove(item);

					preFilledLocations.AddRange(lastSphere.PreFilledLocationsAddedThisSphere);

					while (lastSphere.TotalShuffledLocations == 0)
					{
						spheres.Remove(lastSphere);
						sphereNumber--;
						allPlaceableLocations.AddRange(lastSphere.Locations);

						if (sphereNumber < 0)
							throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");
						lastSphere = spheres[sphereNumber];
						allShuffledItems.AddRange(lastSphere.Items);
						
						foreach (var item in lastSphere.Items)
							obtainedItems.Remove(item);

						foreach (var item in lastSphere.PreFilledItemsAddedThisSphere)
							obtainedItems.Remove(item);

						preFilledLocations.AddRange(lastSphere.PreFilledLocationsAddedThisSphere);
					}

					locationsAvailableThisSphere = lastSphere.Locations;
		
					shuffledLocationsThisSphere = locationsAvailableThisSphere
						.Where(location => location.Type is not LocationType.Unshuffled).ToList();

					sphere = new Sphere
					{
						Locations = locationsAvailableThisSphere,
						TotalShuffledLocations = shuffledLocationsThisSphere.Count,
						SphereNumber = sphereNumber,
					};

					placedItemsThisSphere = new List<Item>();

					maxRetries = lastSphere.MaxRetryCount;
					retryCount = lastSphere.CurrentAttemptCount + 1;
					spheres.Remove(lastSphere);
					continue;
				}
				
				shuffledLocationsThisSphere = locationsAvailableThisSphere
					.Where(location => location.Type is not LocationType.Unshuffled).ToList();

				preFilledLocations.AddRange(preFilledLocationsPlacedThisSphere);

				allShuffledItems.AddRange(placedItemsThisSphere);
				placedItemsThisSphere = new List<Item>();
				
				continue;
			}

			sphereNumber++;

			obtainedItems.AddRange(placedItemsThisSphere);

			if (shuffledLocationsThisSphere.Any())
			{
				allPlaceableLocations.AddRange(shuffledLocationsThisSphere);
				sphere.Locations.RemoveAll(location => shuffledLocationsThisSphere.Contains(location));
				sphere.TotalShuffledLocations -= shuffledLocationsThisSphere.Count;
				locationsAvailableNextSphere.AddRange(shuffledLocationsThisSphere.Where(location => location.IsAccessible(obtainedItems, _locations)));
			}

			sphere.Items = placedItemsThisSphere;
			sphere.CurrentAttemptCount = retryCount;

			obtainedItems.AddRange(preFilledItemsPlacedThisSphere);

			sphere.PreFilledItemsAddedThisSphere = preFilledItemsPlacedThisSphere;
			sphere.PreFilledLocationsAddedThisSphere = preFilledLocationsPlacedThisSphere;

			spheres.Add(sphere);

			canBeatVaati = new LocationDependency("BeatVaati").DependencyFulfilled(obtainedItems, _locations);

			locationsAvailableThisSphere = locationsAvailableNextSphere;
			
			shuffledLocationsThisSphere = locationsAvailableThisSphere
				.Where(location => location.Type is not LocationType.Unshuffled).ToList();

			maxRetries = shuffledLocationsThisSphere.Count == 0 ? 0 : allShuffledItems.Count / shuffledLocationsThisSphere.Count;

			if (maxRetries > 15) maxRetries = 15;

			sphere = new Sphere
			{
				Locations = locationsAvailableThisSphere,
				TotalShuffledLocations = shuffledLocationsThisSphere.Count,
				SphereNumber = sphereNumber,
				MaxRetryCount = maxRetries,
			};

			foreach (var location in locationsAvailableThisSphere)
			{
				allPlaceableLocations.Remove(location);
			}
			retryCount = 0;

		}

		allPlaceableLocations.AddRange(locationsAvailableThisSphere);
		return allPlaceableLocations;
	}

	/// <summary>
	///     Fills in locations from front to back and makes sure they are accessible, meant to be used for dungeon items or for linear progression seeds
	/// </summary>
	/// <param name="items">The items to fill with</param>
	/// <param name="locations">The locations to be filled</param>
	/// <param name="assumedItems">The items that are available by default</param>
	/// <returns>A list of the locations that were filled</returns>
	private List<Location> FillLocationsFrontToBack(List<Item> items, List<Location> locations, List<Item>? assumedItems = null, List<Location>? fallbackLocations = null)
	{
		var filledLocations = new List<Location>();

		if (fallbackLocations == null) fallbackLocations = new List<Location>();

		if (assumedItems == null) assumedItems = new List<Item>();

		var errorIndexes = new List<int>();
		for ( ; items.Count > 0; )
		{
			// Get a random item from the list and save its index
			var usingFallback = false;
			var itemIndex = _rng.Next(items.Count);
			while (errorIndexes.Contains(itemIndex))
				itemIndex = _rng.Next(items.Count);
			
			var item = items[itemIndex];

			// Take item out of pool
			items.RemoveAt(itemIndex);

			var availableItems = GetAvailableItems(assumedItems).ToList();

			// Find locations that are available for placing the item
			var availableLocations = locations.Where(location => location.CanPlace(item, availableItems, _locations))
				.ToList();

			if (availableLocations.Count == 0)
			{
				availableLocations = fallbackLocations
					.Where(location => location.CanPlace(item, availableItems, _locations)).ToList();
				usingFallback = true;
			}

			if (availableLocations.Count <= 0)
			{
				errorIndexes.Add(itemIndex);
				Logger.Instance.LogInfo($"Error Count: {errorIndexes.Count}");
				Logger.Instance.LogInfo($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
				items.Insert(itemIndex, item);
				if (errorIndexes.Count == items.Count)
				{
					// The filler broke
					throw new ShuffleException($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
				}

				continue;
			}

			var locationIndex = _rng.Next(availableLocations.Count);

			availableLocations[locationIndex].Fill(item);
			Logger.Instance.LogInfo(
				$"Placed {item.Type} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining");

			if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
			else locations.Remove(availableLocations[locationIndex]);

			filledLocations.Add(availableLocations[locationIndex]);
			
			errorIndexes.Clear();
		}

		return locations.Concat(fallbackLocations).ToList();
	}

	/// <summary>
	///     Uniformly fills items in locations, checking to make sure the items are logically available.
	/// </summary>
	/// <param name="items">The items to fill with</param>
	/// <param name="locations">The locations to be filled</param>
	/// <param name="assumedItems">The items that are available by default</param>
	/// <returns>A list of the locations that were filled</returns>
	private List<Location> FillLocationsUniform(List<Item> items, List<Location> locations, List<Item>? assumedItems = null, List<Location>? fallbackLocations = null)
	{
		var filledLocations = new List<Location>();

		if (fallbackLocations == null) fallbackLocations = new List<Location>();

		if (assumedItems == null) assumedItems = new List<Item>();

		var errorIndexes = new List<int>();
		for (; items.Count > 0;)
		{
			// Get a random item from the list and save its index
			var usingFallback = false;
			var itemIndex = _rng.Next(items.Count);
			while (errorIndexes.Contains(itemIndex))
				itemIndex = _rng.Next(items.Count);

			var item = items[itemIndex];

			// Take item out of pool
			items.RemoveAt(itemIndex);

			var availableItems = GetAvailableItems(items.Concat(assumedItems).ToList());

			// Find locations that are available for placing the item
			var availableLocations = locations.Where(location => location.CanPlace(item, availableItems, _locations))
				.ToList();

			if (availableLocations.Count == 0)
			{
				availableLocations = fallbackLocations
					.Where(location => location.CanPlace(item, availableItems, _locations)).ToList();
				usingFallback = true;
			}

			if (availableLocations.Count <= 0)
			{
				errorIndexes.Add(itemIndex);
				Logger.Instance.LogInfo($"Error Count: {errorIndexes.Count}");
				Logger.Instance.LogInfo($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
				items.Insert(itemIndex, item);
				if (errorIndexes.Count == items.Count)
				{
					// The filler broke
					throw new ShuffleException($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
				}

				continue;
			}

			var locationIndex = _rng.Next(availableLocations.Count);

			availableLocations[locationIndex].Fill(item);
			Logger.Instance.LogInfo(
				$"Placed {item.Type.ToString()} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining");

			if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
			else locations.Remove(availableLocations[locationIndex]);

			filledLocations.Add(availableLocations[locationIndex]);

			errorIndexes.Clear();
		}

		return locations.Concat(fallbackLocations).ToList();
	}

	/// <summary>
	///     Fill items in locations that are available at the start of the fill.
	///     Slower than FastFillLocations, but will not place in unavailable locations.
	/// </summary>
	/// <param name="items">The items to fill with</param>
	/// <param name="locations">The locations in which to fill the items</param>
	private void CheckedFastFillLocations(List<Item> items, List<Location> locations)
	{
		var finalMajorItems = GetAvailableItems(new List<Item>());
		var availableLocations =
			locations.Where(location => location.IsAccessible(finalMajorItems, _locations)).ToList();

		foreach (var item in items)
		{
			var locationIndex = _rng.Next(0, availableLocations.Count);
			availableLocations[locationIndex].Fill(item);
		}
	}

	/// <summary>
	///     Fill items in locations without checking logic for speed
	/// </summary>
	/// <param name="items">The items to fill with</param>
	/// <param name="locations">The locations in which to fill the items</param>
	private void FastFillLocations(List<Item> items, List<Location> locations)
	{
		var nonFillerItems = items.Where(item => item.ShufflePool is not ItemPool.Filler);
		// Don't need to check logic, cause the items being placed do not affect logic
		foreach (var item in nonFillerItems)
		{
			if (locations.Count == 0) return;
			
			locations[0].Fill(item);
			locations.RemoveAt(0);
		}

		if (locations.Count == 0) return;
		
		var fillItems = items.Where(item => item.ShufflePool == ItemPool.Filler).ToList();
		var rand = new Random(Seed);
		while (locations.Count > 0)
		{
			locations[0].Fill(fillItems[rand.Next(fillItems.Count)]);
			locations.RemoveAt(0);
		}
	}


	/// <summary>
	///     Gets all the available items with a given item set, looping until there are no more items left to get
	/// </summary>
	/// <param name="preAvailableItems">Items that are available from the start</param>
	/// <returns>A list of all the items that are logically accessible</returns>
	private List<Item> GetAvailableItems(List<Item> preAvailableItems)
	{
		var availableItems = preAvailableItems.ToList();

		var filledLocations = _locations.Where(location =>
			location is { Filled: true, Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible }).ToList();

		int previousSize;

		// Get "spheres" until the next sphere contains no new items
		do
		{
			var accessibleLocations =
				filledLocations.Where(location => location.IsAccessible(availableItems, _locations)).ToList();
			previousSize = accessibleLocations.Count;

			filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

			var newItems = Location.GetItems(accessibleLocations);

			availableItems.AddRange(newItems);
		} while (previousSize > 0);

		return availableItems;
	}

	/// <summary>
	///     Get a byte[] of the randomized data
	/// </summary>
	/// <returns>The data of the randomized ROM</returns>
	public byte[] GetRandomizedRom()
	{
		// Create a copy of the ROM data to modify for output
		var outputBytes = new byte[Rom.Instance.romData.Length];
		Array.Copy(Rom.Instance.romData, 0, outputBytes, 0, outputBytes.Length);

		using (var ms = new MemoryStream(outputBytes))
		{
			var writer = new Writer(ms);
			foreach (var location in _locations) location.WriteLocation(writer);

			WriteElementPositions(writer);
			UpdateEntrances(writer);
		}

		return outputBytes;
	}

	/// <summary>
	///     Get the contents of the spoiler log, including playthrough
	/// </summary>
	/// <returns>The contents of the spoiler log</returns>
	public string GetSpoiler()
	{
		var spoilerBuilder = new StringBuilder();
		spoilerBuilder.AppendLine("Spoiler for Minish Cap Randomizer");
		spoilerBuilder.AppendLine($"Seed: {Seed}");
        spoilerBuilder.AppendLine($"Version: {ShufflerController.VersionIdentifier} {ShufflerController.RevisionIdentifier}");
        spoilerBuilder.AppendLine($"Settings String: {MinifiedSettings.GenerateSettingsString(GetSortedSettings(), GetLogicOptionsCrc32())}");

        spoilerBuilder.AppendLine();
		AppendLocationSpoiler(spoilerBuilder);

		spoilerBuilder.AppendLine();
		AppendPlaythroughSpoiler(spoilerBuilder);

		spoilerBuilder.AppendLine();
		AddActualPlaythroughSpoiler(spoilerBuilder);


		return spoilerBuilder.ToString();
	}

	/// <summary>
	///     Create list of filled locations and their contents
	/// </summary>
	/// <param name="spoilerBuilder">The running spoiler log builder to append the locations to</param>
	private void AppendLocationSpoiler(StringBuilder spoilerBuilder)
	{
		spoilerBuilder.AppendLine("Location Contents:");
		// Get the locations that have been filled
		var nonNullLocations = _locations.Where(location => location.Contents is not null);
		
		var filledLocations = nonNullLocations.Where(location => 
			location is { Filled: true, Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible }).ToList();

		var locationsWithRealItems = filledLocations.Where(location => location.Contents!.Value.Type is not ItemType.Untyped && !location.HideFromSpoilerLog);

		var hackToFilterOutLanternGarbage = locationsWithRealItems.Where(location =>
			location.Contents!.Value.Type != ItemType.Lantern || location.Contents!.Value.SubValue == 0);
		
		foreach (var location in hackToFilterOutLanternGarbage)
		{
			spoilerBuilder.AppendLine($"{location.Name}: {location.Contents!.Value.Type}");

			AppendSubvalue(spoilerBuilder, location);

			spoilerBuilder.AppendLine();
		}
	}

	/// <summary>
	///     Create list of items in the order they can logically be collected
	/// </summary>
	/// <param name="spoilerBuilder">The running spoiler log builder to append the playthrough to</param>
	private void AppendPlaythroughSpoiler(StringBuilder spoilerBuilder)
	{
		spoilerBuilder.AppendLine("Spheres:");

		var nonNullLocations = _locations.Where(location => location.Contents is not null);
		
		var filledLocations = nonNullLocations.Where(location =>
			location is { Filled: true, Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible });

		var hackToFilterOutLanternGarbage = filledLocations.Where(location =>
			location.Contents!.Value.Type != ItemType.Lantern || location.Contents!.Value.SubValue == 0).ToList();

		var availableItems = new List<Item>();

		int previousSize;
		var sphereCounter = 1;

		do
		{
			var accessibleLocations =
				hackToFilterOutLanternGarbage.Where(location => location.IsAccessible(availableItems, _locations)).ToList();
			previousSize = accessibleLocations.Count;

			hackToFilterOutLanternGarbage.RemoveAll(location => accessibleLocations.Contains(location));

			var newItems = Location.GetItems(accessibleLocations);
			availableItems.AddRange(newItems);

			foreach (var location in accessibleLocations)
			{
				if (location.HideFromSpoilerLog) continue;
				spoilerBuilder.AppendLine($"Sphere {sphereCounter}: {location.Contents!.Value.Type} in {location.Name}");

				AppendSubvalue(spoilerBuilder, location);
				spoilerBuilder.AppendLine();
			}

			sphereCounter++;
			spoilerBuilder.AppendLine();
		} while (previousSize > 0);
	}

	private void AddActualPlaythroughSpoiler(StringBuilder builder)
	{
		builder.AppendLine("Playthrough:");

		var nonNullLocations = _locations.Where(location => location.Contents is not null);

		var filledLocations = nonNullLocations.Where(location =>
			location is { Filled: true, Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible });

		var hackToFilterOutLanternGarbage = filledLocations.Where(location =>
			location.Contents!.Value.Type != ItemType.Lantern || location.Contents!.Value.SubValue == 0).ToList();

		var availableItems = new List<Item>();

		int previousSize;
		var sphereCounter = 1;

		var majorDungeonItems = _locations.Where(_ => _.Contents.HasValue && _.Contents!.Value.ShufflePool is ItemPool.Major or ItemPool.DungeonMajor or ItemPool.DungeonPrize).Select(_ => _.Contents!.Value).Distinct().ToList();

		do
		{
			if (new LocationDependency("BeatVaati").DependencyFulfilled(availableItems, _locations))
			{
				builder.AppendLine($"Sphere {sphereCounter}: {{").AppendLine("\tBeat Vaati").AppendLine("}").AppendLine();
				return;
			}

			var accessibleLocations =
				hackToFilterOutLanternGarbage.Where(location => location.IsAccessible(availableItems, _locations)).ToList();
			previousSize = accessibleLocations.Count;

			hackToFilterOutLanternGarbage.RemoveAll(location => accessibleLocations.Contains(location));

			var newItems = Location.GetItems(accessibleLocations);
			availableItems.AddRange(newItems);

			var majorLocations = accessibleLocations.Where(location => majorDungeonItems.Any(item => location.Contents!.Value.Equals(item)) || location.Contents!.Value.Type is ItemType.BombBag).ToList();

			builder.AppendLine($"Sphere {sphereCounter}: {{");

			foreach (var location in majorLocations)
			{
				if (location.HideFromSpoilerLog) continue;
			
				builder.AppendLine($"\t{location.Contents!.Value.Type} in {location.Name}");
				AppendSubvalue(builder, location);
				if (majorLocations.Last() != location)
					builder.AppendLine();
			}

			builder.AppendLine("}");

			sphereCounter++;
			builder.AppendLine();
		} while (previousSize > 0);
	}

	private void AppendSubvalue(StringBuilder spoilerBuilder, Location location)
	{
        if (!location.Contents.HasValue) return;

        // Display subvalue if relevant
        if (location.Contents.Value.Type == ItemType.Kinstone)
            spoilerBuilder.AppendLine($"\t\tKinstone Type: {location.Contents.Value.Kinstone}");
        else if (location.Contents.Value.Type is ItemType.ProgressiveItem)
            spoilerBuilder.AppendLine($"\t\tItem: {GetProgressiveItemName(location.Contents.Value.SubValue)}");
        else if (location.Contents.Value.ShufflePool is ItemPool.DungeonEntrance)
            spoilerBuilder.AppendLine($"\t\tDungeon: {GetEntranceNameFromSubvalue(location.Contents.Value.SubValue)}");
        else if (location.Contents.Value.ShufflePool is ItemPool.DungeonMajor ||
                (location.Contents.Value.SubValue != 0 &&
                location.Contents.Value.Type is ItemType.SmallKey or ItemType.BigKey or ItemType.Compass or ItemType.DungeonMap))
            spoilerBuilder.AppendLine($"\t\tDungeon: {GetDungeonNameFromDungeonSubvalue(location.Contents.Value.SubValue)}");
        else if (location.Contents.Value.SubValue != 0)
            spoilerBuilder.AppendLine($"\t\tSubvalue: {location.Contents.Value.SubValue}");

        // Display dungeon contents if relevant
        //if (!string.IsNullOrEmpty(location.Contents.Value.Dungeon))
        //	spoilerBuilder.AppendLine($"\t\tDungeon: {location.Contents.Value.Dungeon}");
    }

    private string GetProgressiveItemName(int subvalue)
	{
		return (ProgressiveItemType)subvalue switch
		{
			ProgressiveItemType.Sword => "Progressive Sword",
			ProgressiveItemType.Bow => "Progressive Bow",
			ProgressiveItemType.Boomerang => "Progressive Boomerang",
			ProgressiveItemType.Shield => "Progressive Shield",
			ProgressiveItemType.SpinAttack => "Progressive Scroll",
			_ => $"{subvalue}"
		};
	}

	private string GetEntranceNameFromSubvalue(int subvalue)
	{
		return (DungeonEntranceType)subvalue switch
		{
			DungeonEntranceType.DWS => "Deepwood Shrine",
			DungeonEntranceType.CoF => "Cave of Flames",
			DungeonEntranceType.FoW => "Fortress of Winds",
			DungeonEntranceType.ToD => "Temple of Droplets",
			DungeonEntranceType.Crypt => "Royal Crypt",
			DungeonEntranceType.PoW => "Palace of Winds",
			_ => $"{subvalue}"
		};
	}

	private string GetDungeonNameFromDungeonSubvalue(int subvalue)
	{
		return (DungeonType)subvalue switch
		{
			DungeonType.Dojo => "Dojo",
			DungeonType.DWS => "Deepwood Shrine",
			DungeonType.CoF => "Cave of Flames",
			DungeonType.FoW => "Fortress of Winds",
			DungeonType.ToD => "Temple of Droplets",
			DungeonType.PoW => "Palace of Winds",
			DungeonType.DHC => "Dark Hyrule Castle",
			DungeonType.Crypt => "Royal Crypt",
			_ => $"{subvalue}"
		};
	}

	public string GetEventWrites()
	{
		var eventBuilder = new StringBuilder();

		foreach (var location in _locations) location.WriteLocationEvent(eventBuilder);

		foreach (var define in _logicParser.GetEventDefines()) define.WriteDefineString(eventBuilder);

		var seedValues = new byte[4];
		seedValues[0] = (byte)((Seed >> 00) & 0xFF);
		seedValues[1] = (byte)((Seed >> 08) & 0xFF);
		seedValues[2] = (byte)((Seed >> 16) & 0xFF);
		seedValues[3] = (byte)((Seed >> 24) & 0xFF);

		eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8((int)CrcUtil.Crc32(seedValues, 4)));
		eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetSettingHash()));

		return eventBuilder.ToString();
	}

	/// <summary>
	///     Move the elements around in a randomized ROM
	/// </summary>
	/// <param name="w">Writer to write with</param>
	private void WriteElementPositions(Writer w)
	{
		// Write coordinates for each element
		var earthLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.EarthElement);
		MoveElement(w, earthLocation);

		var fireLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.FireElement);
		MoveElement(w, fireLocation);

		var waterLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.WaterElement);
		MoveElement(w, waterLocation);

		var windLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.WindElement);
		MoveElement(w, windLocation);
	}

    /// <summary>
    ///     Moves a single element marker to the location that contains it
    /// </summary>
    /// <param name="w">The writer to write to</param>
    /// <param name="location">The location that contains the element</param>
    private void MoveElement(Writer w, Location prizeLocation)
    {
        // Coordinates for the unzoomed map
        var largeCoords = new byte[2];

        // Coordinates for the zoomed in map
        var smallCoords = new ushort[2];
        (ushort largeAddress, uint smallAdress) coords = (0, 0);
        Location correspondingEntrance;
        switch (prizeLocation.Name)
        {
            case "Deepwood_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.DWS);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            case "CoF_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.CoF);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            case "Fortress_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.FoW);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            case "Droplets_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.ToD);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            case "Crypt_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.Crypt);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            case "Palace_Prize":
                correspondingEntrance = _locations.First(location => location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled && location.Contents is not null && (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.PoW);
                coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                break;
            default:
                return;
        }

        largeCoords[0] = (byte)((coords.largeAddress >> 8) & 0xFF);
        largeCoords[1] = (byte)(coords.largeAddress & 0xFF);

        smallCoords[0] = (ushort)((coords.smallAdress >> 16) & 0xFFFF);
        smallCoords[1] = (ushort)(coords.smallAdress & 0xFFFF);

        int largeAddress;
        int smallAddress;

        switch (prizeLocation.Contents!.Value.Type)
        {
            case ItemType.EarthElement:
                largeAddress = 0x128699;
                smallAddress = 0x12869C;
                break;
            case ItemType.FireElement:
                largeAddress = 0x1286A1;
                smallAddress = 0x1286A4;
                break;
            case ItemType.WaterElement:
                largeAddress = 0x1286B1;
                smallAddress = 0x1286B4;
                break;
            case ItemType.WindElement:
                largeAddress = 0x1286A9;
                smallAddress = 0x1286AC;
                break;
            default:
                return;
        }

        // Write zoomed out coordinates
        w.SetPosition(largeAddress);
        w.WriteByte(largeCoords[0]);
        w.WriteByte(largeCoords[1]);

        // Write zoomed in coordinates
        w.SetPosition(smallAddress);
        w.WriteUInt16(smallCoords[0]);
        w.WriteUInt16(smallCoords[1]);
    }

    private (ushort largeAddress, uint smallAdress) GetAddressFromDungeonEntranceName(string locationName)
    {
        return locationName switch
        {
            "Deepwood_Entrance" => (0xB27A, 0x0D7D0AC8),
            "CoF_Entrance" => (0x3B1B, 0x01E80178),
            "Fortress_Entrance" => (0x4B77, 0x03780A78),
            "Droplets_Entrance" => (0xB54B, 0x0DB80638),
            "Crypt_Entrance" => (0x5A15, 0x04DC0148),
            "Palace_Entrance" => (0xB51B, 0x0D8800E8),
        };
    }

    private void UpdateEntrances(Writer w)
	{
		var entranceChangedLocations = _locations.Where(location => location.Type is LocationType.DungeonEntrance).ToList();

		foreach (var entrance in entranceChangedLocations)
		{
			UpdateSpecialEntrances(w, entrance);
		}
	}

	/// <summary>
	/// Updates the following dungeon entrances to point to the correct entrance in dungeon entrance rando and sets the correct entrance type based on the dungeon
	///     Enter ToD from Portal in Lake Hylia
	///     Leave ToD
	///     Enter PoW from the Large Tornado at top of Wind Tribe
	///     Leave PoW
	///     After Crypt Prize
	///     After Fortress Prize
	///     Green warps in: DWS, CoF, FoW, ToD, PoW
	///     Other entrance shuffle
	/// </summary>
	/// <param name="w"></param>
	/// <param name="location"></param>
	private void UpdateSpecialEntrances(Writer w, Location location)
	{
		//Dungeon exits and green warps
		const uint dwsExit = 0x138172;
		const uint dwsGreenWarp = 0xDF06C;

		const uint cofExit = 0x138352;
		const uint cofGreenWarp = 0xE0F34;

		const uint fowExit = 0x13549A;
		const uint fowGreenWarp = 0xE2308;
		const uint fowAfterElement = 0x13A25E; //FoW has two ways of leaving, one on element, one on green warp

		const uint todExit = 0x13A47A;
		const uint todGreenWarp = 0xE40F4; //Normally warps back into the dungeon, we change it to go outside

		const uint cryptExit = 0x138EAA;
		const uint cryptElementWarp = 0x13A2AE; //Crypt doesn't have a green warp, just a warp after getting the item from King

		const uint powExit = 0x1082A1;
		const uint powGreenWarp = 0xE6A14;

		if (!location.Contents.HasValue || location.Contents.Value.ShufflePool is not ItemPool.DungeonEntrance) return;

		var entranceAndExitOffsets = new List<uint>();

		ushort entranceX;
		ushort entranceY;
		byte entranceLayerOrHeight;
		byte entranceAnimation;
		byte targetArea;
		byte targetRoom;
		byte facingDirection;
		var exitAddress = 0u;
		var greenWarpAddress = 0u;
		var elementWarpAddress = 0u;
		var holeWarpAddress = 0u;

		switch ((DungeonEntranceType)location.Contents.Value.SubValue)
		{
			case DungeonEntranceType.DWS:
				entranceX = 0xA8;
				entranceY = 0xD8;
				entranceLayerOrHeight = 0x1;
				targetArea = 0x48;
				targetRoom = 0x0B;
				entranceAnimation = 0x0;
				facingDirection = 0x0;
				exitAddress = dwsExit;
				greenWarpAddress = dwsGreenWarp;
				break;
			case DungeonEntranceType.CoF:
				entranceX = 0x88;
				entranceY = 0xA8;
				entranceLayerOrHeight = 0x1;
				targetArea = 0x50;
				targetRoom = 0x03;
				entranceAnimation = 0x0;
				facingDirection = 0x0;
				exitAddress = cofExit;
				greenWarpAddress = cofGreenWarp;
				break;
			case DungeonEntranceType.FoW:
				entranceX = 0x01D8;
				entranceY = 0xB0;
				entranceLayerOrHeight = 0x1;
				targetArea = 0x18;
				targetRoom = 0x00;
				entranceAnimation = 0x0;
				facingDirection = 0x0;
				exitAddress = fowExit;
				greenWarpAddress = fowGreenWarp;
				elementWarpAddress = fowAfterElement;
				break;
			case DungeonEntranceType.ToD:
				entranceX = 0x0108;
				entranceY = 0xC8;
				entranceLayerOrHeight = 0x2;
				targetArea = 0x60;
				targetRoom = 0x03;
				entranceAnimation = 0x2;
				facingDirection = 0x4;
				exitAddress = todExit;
				greenWarpAddress = todGreenWarp;
				break;
			case DungeonEntranceType.Crypt:
				entranceX = 0x88;
				entranceY = 0x78;
				entranceLayerOrHeight = 0x1;
				targetArea = 0x68;
				targetRoom = 0x08;
				entranceAnimation = 0x0;
				facingDirection = 0x0;
				exitAddress = cryptExit;
				elementWarpAddress = cryptElementWarp;
				break;
			case DungeonEntranceType.PoW:
				entranceX = 0x268;
				entranceY = 0x58;
				entranceLayerOrHeight = 0x1;
				targetArea = 0x70;
				targetRoom = 0x31;
				entranceAnimation = 0x0A;
				facingDirection = 0x6;
				holeWarpAddress = powExit;
				greenWarpAddress = powGreenWarp;
				break;
			default:
				return;
		}

		ITransition transition;

		switch (location.Name)
		{
			case "Deepwood_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.DWS);
				break;
			case "CoF_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.CoF);
				break;
			case "Fortress_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.FoW);
				break;
			case "Droplets_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.ToD);
				break;
			case "Crypt_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.Crypt);
				break;
			case "Palace_Entrance":
				transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.PoW);
				break;
			default:
				return;
		}

		var addressesToWrite = transition.GetTransitionOffsets(targetArea, targetRoom, entranceLayerOrHeight, entranceAnimation, entranceX, entranceY, facingDirection, exitAddress, greenWarpAddress, elementWarpAddress, holeWarpAddress);

		foreach (var address in addressesToWrite)
		{
			if (address.Value is ushort)
			{
				w.SetPosition(address.Key);
				w.WriteUInt16((ushort)address.Value);
			}
			else if (address.Value is byte)
			{
				w.SetPosition(address.Key);
				w.WriteByte((byte)address.Value);
			}
		}
	}

	public void ClearLogic()
	{
		_locations.Clear();
		
		_music.Clear();
		_unshuffledItems.Clear();
		
		_dungeonEntrances.Clear();
		_dungeonConstraints.Clear();
		_overworldConstraints.Clear();
		
		_dungeonPrizes.Clear();
		_dungeonMajorItems.Clear();
        _dungeonMinorItems.Clear();
        _majorItems.Clear();
		
		_minorItems.Clear();
		_fillerItems.Clear();

		_logicParser.SubParser.ClearTypeOverrides();
		_logicParser.SubParser.ClearIncrementalReplacements();
		_logicParser.SubParser.ClearReplacements();
		_logicParser.SubParser.ClearAmountReplacements();
		_logicParser.SubParser.ClearDefines();
		_logicParser.SubParser.AddOptions();
	}
}
