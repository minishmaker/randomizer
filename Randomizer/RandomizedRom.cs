using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinishRandomizer.Core;
using MinishRandomizer.Randomizer.Logic;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer
{
    public class RandomizedRom
    {
        public string LogicIdentifier;

        public string[] Spoiler { get; private set; }

        public int Seed;
        public string Settings;
        public string Gimmicks;
        public string LogicPath;
        public string PatchPath;
        private List<Location> Locations;
        private ROM InputRom;
        private Dictionary<string, ColorzCore.Parser.Definition> Definitions;
        

        public RandomizedRom(int seed, List<Location> outputLocations, string logicIdentifier, string settingsString, string gimmickString, string logicPath, string patchPath, Dictionary<string, ColorzCore.Parser.Definition> StartingDefinitions)
        {
            Seed = seed;

            LogicIdentifier = logicIdentifier;

            Locations = outputLocations;

            Settings = settingsString;
            Gimmicks = gimmickString;

            LogicPath = logicPath;
            PatchPath = patchPath;

            Spoiler = GenerateSpoiler();

            Definitions = StartingDefinitions;
        }

        public void SetInputRom(ROM rom)
        {
            InputRom = rom;
        }

        public byte[] GetRom()
        {
            if (InputRom == null)
            {
                throw new Exception("Could not output rom as no input rom is specified.");
            }

            // Create a copy of the ROM data to modify for output
            byte[] outputBytes = new byte[ROM.Instance.romData.Length];
            Array.Copy(ROM.Instance.romData, 0, outputBytes, 0, outputBytes.Length);

            // Get all defines from locations
            List<EventDefine> defines = new List<EventDefine>();

            foreach (Location location in Locations)
            {
                defines.AddRange(location.GetEventDefines());
            }

            Dictionary<string, ColorzCore.Parser.Definition> definitions = EventUtil.GetDefinitions(defines);

            using (MemoryStream ms = new MemoryStream(outputBytes))
            {
                EventPatch(InputRom.romData, ms, definitions);

                Writer writer = new Writer(ms);
                foreach (Location location in Locations)
                {
                    location.WriteLocation(writer);
                }

                outputBytes = ms.ToArray();
            }

            return outputBytes;
        }

        private void EventPatch(byte[] inputBytes, Stream outStream, Dictionary<string, ColorzCore.Parser.Definition> definitions)
        {
            Stream inStream = File.OpenRead(PatchPath);
            ColorzCore.IO.Log log = new ColorzCore.IO.Log();
            bool success = ColorzCore.Program.EAParse("EA", "/Language Raws", ".txt", inStream, "TMCR Rom", outStream, log, definitions);

            if (!success)
            {
                throw new Exception("Patching failed for some reason!");
            }
        }

        public byte[] GetPatch()
        {
            throw new NotImplementedException();
        }

        private string[] GenerateSpoiler()
        {
            throw new NotImplementedException();
        }

        public uint GetSettingHash()
        {
            byte[] settingsBytes = Encoding.UTF8.GetBytes(Settings);

            if (settingsBytes.Length > 0)
            {
                return PatchUtil.Crc32(settingsBytes, settingsBytes.Length);
            }
            else
            {
                return 0;
            }
        }

        public uint GetGimmickHash()
        {
            byte[] gimmicksBytes = Encoding.UTF8.GetBytes(Gimmicks);

            if (gimmicksBytes.Length > 0)
            {
                return PatchUtil.Crc32(gimmicksBytes, gimmicksBytes.Length);
            }
            else
            {
                return 0;
            }
        }

        public string GetOptionsIdentifier()
        {
            return $"{Settings} - {Gimmicks}";
        }

        /*
        
        /// <summary>
        /// Get a byte[] of the randomized data
        /// </summary>
        /// <returns>The data of the randomized ROM</returns>
        public byte[] GetRandomizedRom()
        {
            // Create a copy of the ROM data to modify for output
            byte[] outputBytes = new byte[ROM.Instance.romData.Length];
            Array.Copy(ROM.Instance.romData, 0, outputBytes, 0, outputBytes.Length);

            using (MemoryStream ms = new MemoryStream(outputBytes))
            {
                Writer writer = new Writer(ms);
                foreach (Location location in Locations)
                {
                    location.WriteLocation(writer);
                }

                WriteElementPositions(writer);
            }

            return outputBytes;
        }

        public void ApplyPatch(string romLocation, string patchFile = null)
        {
            if (string.IsNullOrEmpty(patchFile))
            {
                // Get directory of MinishRandomizer 
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                patchFile = assemblyPath + "/Patches/ROM buildfile.event";
            }

            // Write new patch file to patch folder/extDefinitions.event
            File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

            string[] args = new[] { "A", "FE8", "-input:" + patchFile, "-output:" + romLocation };

            ColorzCore.Program.Main(args);
        }

        public string GetLogicIdentifier()
        {
            string fallbackName;
            string fallbackVersion;
            if (LogicPath != null)
            {
                fallbackName = Path.GetFileNameWithoutExtension(LogicPath);
                fallbackVersion = File.GetLastWriteTime(LogicPath).ToShortDateString();
            }
            else
            {
                fallbackName = "Default";
                fallbackVersion = Version;
            }

            string name = LogicParser.SubParser.LogicName ?? fallbackName;
            string version = LogicParser.SubParser.LogicVersion ?? fallbackVersion;

            return name + "-" + version;
        }

        /// <summary>
        /// Create list of filled locations and their contents
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the locations to<</param>
        private void AppendLocationSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Location Contents:");
            // Get the locations that have been filled
            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();

            foreach (Location location in filledLocations)
            {
                spoilerBuilder.AppendLine($"{location.Name}: {location.Contents.Type}");

                AppendSubvalue(spoilerBuilder, location);

                spoilerBuilder.AppendLine();
            }
        }

        /// <summary>
        /// Create list of items in the order they can logically be collected
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the playthrough to</param>
        private void AppendPlaythroughSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Playthrough:");

            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();
            List<Item> availableItems = new List<Item>();

            int previousSize;
            int sphereCounter = 1;

            do
            {
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations, false)).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                List<Item> newItems = Location.GetItems(accessibleLocations);
                availableItems.AddRange(newItems);

                foreach (Location location in accessibleLocations)
                {
                    spoilerBuilder.AppendLine($"Sphere {sphereCounter}: {location.Contents.Type} in {location.Name}");

                    AppendSubvalue(spoilerBuilder, location);
                    spoilerBuilder.AppendLine();
                }

                sphereCounter++;
                spoilerBuilder.AppendLine();
            }
            while (previousSize > 0);
        }

        private void AppendSubvalue(StringBuilder spoilerBuilder, Location location)
        {
            // Display subvalue if relevant
            if (location.Contents.Type == ItemType.KinstoneX)
            {
                spoilerBuilder.AppendLine($"Kinstone Type: {location.Contents.Kinstone}");
            }
            else if (location.Contents.SubValue != 0)
            {
                spoilerBuilder.AppendLine($"Subvalue: {location.Contents.SubValue}");
            }

            // Display dungeon contents if relevant
            if (!string.IsNullOrEmpty(location.Contents.Dungeon))
            {
                spoilerBuilder.AppendLine($"Dungeon: {location.Contents.Dungeon}");
            }
        }

        public string GetEventWrites()
        {
            StringBuilder eventBuilder = new StringBuilder();

            foreach (Location location in Locations)
            {
                location.WriteLocationEvent(eventBuilder);
            }

            foreach (EventDefine define in LogicParser.GetEventDefines())
            {
                define.WriteDefineString(eventBuilder);
            }

            byte[] seedValues = new byte[4];
            seedValues[0] = (byte)((Seed >> 00) & 0xFF);
            seedValues[1] = (byte)((Seed >> 08) & 0xFF);
            seedValues[2] = (byte)((Seed >> 16) & 0xFF);
            seedValues[3] = (byte)((Seed >> 24) & 0xFF);

            eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8((int)PatchUtil.Crc32(seedValues, 4)));
            eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetSettingHash()));

            return eventBuilder.ToString();
        }

        /// <summary>
        /// Move the elements around in a randomized ROM
        /// </summary>
        /// <param name="w">Writer to write with</param>
        private void WriteElementPositions(Writer w)
        {
            // Write coordinates for each element
            Location earthLocation = Locations.Where(loc => loc.Contents.Type == ItemType.EarthElement).First();
            MoveElement(w, earthLocation);

            Location fireLocation = Locations.Where(loc => loc.Contents.Type == ItemType.FireElement).First();
            MoveElement(w, fireLocation);

            Location waterLocation = Locations.Where(loc => loc.Contents.Type == ItemType.WaterElement).First();
            MoveElement(w, waterLocation);

            Location windLocation = Locations.Where(loc => loc.Contents.Type == ItemType.WindElement).First();
            MoveElement(w, windLocation);
        }

        /// <summary>
        /// Moves a single element marker to the location that contains it
        /// </summary>
        /// <param name="w">The writer to write to</param>
        /// <param name="location">The location that contains the element</param>
        private void MoveElement(Writer w, Location location)
        {
            // Coordinates for the unzoomed map
            byte[] largeCoords = new byte[2];

            // Coordinates for the zoomed in map
            ushort[] smallCoords = new ushort[2];
            switch (location.Name)
            {
                case "DeepwoodPrize":
                    largeCoords[0] = 0xB2;
                    largeCoords[1] = 0x7A;

                    smallCoords[0] = 0x0D7D;
                    smallCoords[1] = 0x0AC8;
                    break;
                case "CoFPrize":
                    largeCoords[0] = 0x3B;
                    largeCoords[1] = 0x1B;

                    smallCoords[0] = 0x01E8;
                    smallCoords[1] = 0x0178;
                    break;
                case "FortressPrize":
                    largeCoords[0] = 0x4B;
                    largeCoords[1] = 0x77;

                    smallCoords[0] = 0x0378;
                    smallCoords[1] = 0x0A78;
                    break;
                case "DropletsPrize":
                    largeCoords[0] = 0xB5;
                    largeCoords[1] = 0x4B;

                    smallCoords[0] = 0x0DB8;
                    smallCoords[1] = 0x0638;
                    break;
                case "KingGift":
                    largeCoords[0] = 0x5A;
                    largeCoords[1] = 0x15;

                    smallCoords[0] = 0x04DC;
                    smallCoords[1] = 0x0148;
                    break;
                case "PalacePrize":
                    largeCoords[0] = 0xB5;
                    largeCoords[1] = 0x1B;

                    smallCoords[0] = 0x0D88;
                    smallCoords[1] = 0x00E8;
                    break;
                default:
                    return;
            }

            int largeAddress;
            int smallAddress;

            switch (location.Contents.Type)
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

        */
    }
}
