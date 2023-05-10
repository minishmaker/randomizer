using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;

namespace RandomizerCore.Randomizer.Shuffler;

internal class SphereBasedShuffler : Shuffler
{
    public void RandomizeLocations(bool useSphereShuffler)
    {
        if (!useSphereShuffler)
        {
            base.RandomizeLocations();
            return;
        }
        
        var time = DateTime.Now;

        var groupsAndLocations = PlaceImportantItems();

        var locationGroups = groupsAndLocations.locationGroups;
        var unfilledLocations = groupsAndLocations.unfilledLocations;
        
        var itemsToPlace = _majorItems.Concat(_minorItems).ToList();
        var locationsToFill = _locations.Where(_ =>
            _.Type is LocationType.Any or LocationType.Major or LocationType.Minor).Concat(unfilledLocations).ToList();

        while (itemsToPlace.Count < locationsToFill.Count)
            itemsToPlace.Add(_fillerItems[_rng.Next(_fillerItems.Count)]);

        unfilledLocations = FillLocations(itemsToPlace,
            _locations.Where(location =>
                location.Filled && location.Contents.HasValue && location.Type is not LocationType.Helper
                    and not LocationType.Inaccessible and not LocationType.Untyped).ToList(), locationsToFill);


        // Get every item that can be logically obtained, to check if the game can be completed
        var finalMajorItems = GetAvailableItems(new List<Item>());

        if (!DependencyBase.BeatVaatiDependency!.DependencyFulfilled())
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
        
        finalMajorItems.ForEach(item => item.NotifyParentDependencies(false));

        var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Inaccessible)
            ? locationGroups.First(group => group.Key == LocationType.Inaccessible).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();
        unfilledLocations.Shuffle(_rng);
        FastFillLocations(_fillerItems, unfilledLocations);

        var diff = DateTime.Now - time;
        Logger.Instance.BeginLogTransaction();
        Logger.Instance.LogInfo($"Timing Benchmark - Shuffling with seed {Seed:X} and settings {MinifiedSettings.GenerateSettingsString(GetSortedSettings(), GetLogicOptionsCrc32())} took {diff.Seconds}.{diff.Milliseconds} seconds!");
        Logger.Instance.SaveLogTransaction(true);
        
        _randomized = true;
    }
    
    private List<Location> FillLocations(List<Item> allShuffledItems, List<Location> preFilledLocations, List<Location> allPlaceableLocations)
    {
        var spheres = new List<Sphere>();

        var sphereNumber = 0;
        var obtainedItems = new List<Item>();

        var canBeatVaati = false;

        var locationsAvailableThisSphere =
            allPlaceableLocations.Where(location => location.IsAccessible()).ToList();

        var shuffledLocationsThisSphere = locationsAvailableThisSphere
            .Where(location => location.Type is not LocationType.Unshuffled).ToList();

        var maxRetries = 10000; //Constant amount to make sure we really try to gen a sphere before giving up

        var sphere = new Sphere
        {
            Locations = locationsAvailableThisSphere,
            TotalShuffledLocations = shuffledLocationsThisSphere.Count,
            SphereNumber = sphereNumber,
            MaxRetryCount = maxRetries
        };

        foreach (var location in locationsAvailableThisSphere) allPlaceableLocations.Remove(location);

        var retryCount = 0;
        var totalPermutations = 0;

        while (!canBeatVaati)
        {
            if (totalPermutations++ > 200000)
                throw new ShuffleException(
                    "Could not find a viable seed with Hendrus Shuffler after 50,000 permutations! Please let the dev team know what settings and seed you are using!");

            shuffledLocationsThisSphere.Shuffle(_rng);

            var placedItemsThisSphere = new List<Item>();

            var forLoopRetryCount = 0;
            const int forLoopMaxRetries = 15;
            shuffledLocationsThisSphere.Shuffle(_rng);
            for (var i = 0; i < shuffledLocationsThisSphere.Count && forLoopRetryCount < forLoopMaxRetries;)
            {
                var location = shuffledLocationsThisSphere[0];

                var itemsWithSameDungeon =
                    allShuffledItems.Where(item => location.Dungeons.Contains(item.Dungeon)).ToList();

                var placeableItems = allShuffledItems.Where(item =>
                    string.IsNullOrEmpty(item.Dungeon) || location.Dungeons.Contains(item.Dungeon)).ToList();

                var item = placeableItems[_rng.Next(placeableItems.Count)];

                if (itemsWithSameDungeon.Any())
                    item = itemsWithSameDungeon[_rng.Next(itemsWithSameDungeon.Count)];

                if (!location.CanPlace(item, _locations))
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
                if (pf.IsAccessible())
                {
                    preFilledItemsPlacedThisSphere.Add(pf.Contents!.Value);
                    preFilledLocationsPlacedThisSphere.Add(pf);
                    preFilledLocations.Remove(pf);
                    continue;
                }

                ++i;
            }
            
            placedItemsThisSphere.ForEach(item => item.NotifyParentDependencies(true));
            preFilledItemsPlacedThisSphere.ForEach(item => item.NotifyParentDependencies(true));

            var locationsAvailableNextSphere =
                allPlaceableLocations.Where(location => location.IsAccessible()).ToList();

            if (locationsAvailableNextSphere.Count == 0 && preFilledLocationsPlacedThisSphere.Count == 0)
            {
                retryCount++;

                preFilledLocations.AddRange(preFilledLocationsPlacedThisSphere);
                allShuffledItems.AddRange(placedItemsThisSphere);
                
                placedItemsThisSphere.ForEach(item => item.NotifyParentDependencies(false));
                preFilledItemsPlacedThisSphere.ForEach(item => item.NotifyParentDependencies(false));
                
                if (retryCount > maxRetries)
                {
                    if (sphereNumber == 0)
                        throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");

                    sphereNumber--;

                    allPlaceableLocations.AddRange(locationsAvailableThisSphere);

                    var lastSphere = spheres[sphereNumber];
                    allShuffledItems.AddRange(lastSphere.Items);

                    foreach (var item in lastSphere.Items)
                    {
                        obtainedItems.Remove(item);
                        item.NotifyParentDependencies(false);
                    }

                    foreach (var item in lastSphere.PreFilledItemsAddedThisSphere)
                    {
                        obtainedItems.Remove(item);
                        item.NotifyParentDependencies(false);
                    }

                    preFilledLocations.AddRange(lastSphere.PreFilledLocationsAddedThisSphere);

                    while (lastSphere.TotalShuffledLocations == 0 || lastSphere.CurrentAttemptCount > lastSphere.MaxRetryCount)
                    {
                        spheres.Remove(lastSphere);
                        sphereNumber--;
                        allPlaceableLocations.AddRange(lastSphere.Locations);

                        if (sphereNumber < 0)
                            throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");
                        lastSphere = spheres[sphereNumber];
                        allShuffledItems.AddRange(lastSphere.Items);

                        foreach (var item in lastSphere.Items)
                        {
                            obtainedItems.Remove(item);
                            item.NotifyParentDependencies(false);
                        }

                        foreach (var item in lastSphere.PreFilledItemsAddedThisSphere)
                        {
                            obtainedItems.Remove(item);
                            item.NotifyParentDependencies(false);
                        }

                        preFilledLocations.AddRange(lastSphere.PreFilledLocationsAddedThisSphere);
                    }

                    locationsAvailableThisSphere = lastSphere.Locations;

                    shuffledLocationsThisSphere = locationsAvailableThisSphere
                        .Where(location => location.Type is not LocationType.Unshuffled).ToList();

                    sphere = new Sphere
                    {
                        Locations = locationsAvailableThisSphere,
                        TotalShuffledLocations = shuffledLocationsThisSphere.Count,
                        SphereNumber = sphereNumber
                    };

                    maxRetries = lastSphere.MaxRetryCount;
                    retryCount = lastSphere.CurrentAttemptCount + 1;
                    spheres.Remove(lastSphere);
                    continue;
                }

                shuffledLocationsThisSphere = locationsAvailableThisSphere
                    .Where(location => location.Type is not LocationType.Unshuffled).ToList();
                
                continue;
            }

            sphereNumber++;

            obtainedItems.AddRange(placedItemsThisSphere);

            if (shuffledLocationsThisSphere.Any())
            {
                allPlaceableLocations.AddRange(shuffledLocationsThisSphere);
                sphere.Locations.RemoveAll(location => shuffledLocationsThisSphere.Contains(location));
                sphere.TotalShuffledLocations -= shuffledLocationsThisSphere.Count;
                locationsAvailableNextSphere.AddRange(
                    shuffledLocationsThisSphere.Where(location => location.IsAccessible()));
            }

            sphere.Items = placedItemsThisSphere;
            sphere.CurrentAttemptCount = retryCount;

            obtainedItems.AddRange(preFilledItemsPlacedThisSphere);

            sphere.PreFilledItemsAddedThisSphere = preFilledItemsPlacedThisSphere;
            sphere.PreFilledLocationsAddedThisSphere = preFilledLocationsPlacedThisSphere;

            spheres.Add(sphere);

            canBeatVaati = DependencyBase.BeatVaatiDependency!.DependencyFulfilled();

            locationsAvailableThisSphere = locationsAvailableNextSphere;

            shuffledLocationsThisSphere = locationsAvailableThisSphere
                .Where(location => location.Type is not LocationType.Unshuffled).ToList();

            maxRetries = shuffledLocationsThisSphere.Count == 0
                ? 0
                : allShuffledItems.Count / shuffledLocationsThisSphere.Count;

            if (maxRetries > 15) maxRetries = 15;

            sphere = new Sphere
            {
                Locations = locationsAvailableThisSphere,
                TotalShuffledLocations = shuffledLocationsThisSphere.Count,
                SphereNumber = sphereNumber,
                MaxRetryCount = maxRetries
            };

            foreach (var location in locationsAvailableThisSphere) allPlaceableLocations.Remove(location);
            retryCount = 0;
        }

        allPlaceableLocations.AddRange(locationsAvailableThisSphere);
        obtainedItems.ForEach(item => item.NotifyParentDependencies(false));
        return allPlaceableLocations;
    }
}
