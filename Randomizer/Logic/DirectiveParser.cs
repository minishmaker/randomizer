using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MinishRandomizer.Core;

namespace MinishRandomizer.Randomizer.Logic
{
    public class DirectiveParser
    {
        public uint? RomCrc;
        public string LogicName;
        public string LogicVersion;
        public List<LogicOption> Options;
        private List<LogicDefine> Defines;
        public Dictionary<Item, Location.LocationType> LocationTypeOverrides;
        public Dictionary<Item, ChanceItemSet> Replacements;
        public List<EventDefine> EventDefines;
        private int IfCounter;

        public DirectiveParser()
        {
            Options = new List<LogicOption>();
            Defines = new List<LogicDefine>();
            EventDefines = new List<EventDefine>();
            LocationTypeOverrides = new Dictionary<Item, Location.LocationType>();
            Replacements = new Dictionary<Item, ChanceItemSet>();
            IfCounter = 0;
        }

        private string[] SplitDirective(string line)
        {
            // Split define into parameters
            string[] untrimmed = line.Split('-');

            // Trim each parameter down
            string[] trimmed = new string[untrimmed.Length];
            for (int i = 0; i < untrimmed.Length; i++)
            {
                trimmed[i] = untrimmed[i].Trim();
            }

            return trimmed;
        }

        /// <summary>
        /// Check if a directive should be parsed on load or on randomization
        /// </summary>
        /// <param name="line">The line that contains the given directive</param>
        /// <returns>True if the directive should be parsed on load, throws an error if the directive is invalid.</returns>
        public bool ParseOnLoad(string line)
        {
            string directive = SplitDirective(line)[0];
            switch (directive)
            {
                case "!flag":
                case "!color":
                case "!name":
                case "!version":
                case "!crc":
                case "!dropdown":
                    return true;
                case "!define":
                case "!undefine":
                case "!eventdefine":
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                case "!replace":
                case "!settype":
                    return false;
                default:
                    throw new ParserException($"\"{directive}\" is not a valid directive! The logic file may be bad.");
            }
        }

        public void ParseDirective(string line)
        {
            string[] mainDirectiveParts = SplitDirective(line);

            switch (mainDirectiveParts[0])
            {
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                    ParseConditionalDirective(mainDirectiveParts);
                    return;
            }

            if (!ShouldIgnoreLines())
            {
                switch (mainDirectiveParts[0])
                {
                    case "!crc":
                        RomCrc = ParseCrcDirective(mainDirectiveParts);
                        break;
                    case "!flag":
                        Options.Add(ParseFlagDirective(mainDirectiveParts));
                        break;
                    case "!replace":
                        ParseReplaceDirective(mainDirectiveParts);
                        break;
                    case "!settype":
                        ParseSetTypeDirective(mainDirectiveParts);
                        break;
                    case "!color":
                        Options.Add(ParseColorDirective(mainDirectiveParts));
                        break;
                    case "!dropdown":
                        Options.Add(ParseDropdownDirective(mainDirectiveParts));
                        break;
                    case "!define":
                        AddDefine(ParseDefineDirective(mainDirectiveParts));
                        break;
                    case "!undefine":
                        RemoveDefine(ParseDefineDirective(mainDirectiveParts));
                        break;
                    case "!eventdefine":
                        EventDefines.Add(ParseEventDefine(mainDirectiveParts));
                        break;
                    case "!name":
                        LogicName = mainDirectiveParts[1];
                        break;
                    case "!version":
                        LogicVersion = mainDirectiveParts[1];
                        break;
                    default:
                        throw new ParserException($"\"{mainDirectiveParts[0]}\" is not a valid directive!");
                }
            }
        }

        private void ParseConditionalDirective(string[] directiveParts)
        {
            switch (directiveParts[0])
            {
                case "!ifdef":
                    if (ShouldIgnoreLines() || !IsDefined(directiveParts[1]))
                    {
                        IfCounter++;
                    }
                    return;
                case "!ifndef":
                    if (ShouldIgnoreLines() || IsDefined(directiveParts[1]))
                    {
                        IfCounter++;
                    }
                    return;
                case "!else":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1)
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;
                /*Currently removed cause it causes problems
                 * case "!elseifdef":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1 && IsDefined(directiveParts[1]))
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;
                case "!elseifndef":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1 && IsDefined(directiveParts[1]))
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;*/
                case "!endif":
                    if (IfCounter > 0)
                    {
                        IfCounter--;
                    }
                    return;
            }
        }

        private bool IsDefined(string defineText)
        {
            foreach (LogicDefine define in Defines)
            {
                if (define.Name == defineText)
                {
                    return true;
                }
            }

            return false;
        }

        public void ClearDefines()
        {
            Defines.Clear();
            EventDefines.Clear();
        }

        public void AddDefine(LogicDefine define)
        {
            foreach (LogicDefine predefined in Defines)
            {
                if (define.Name == predefined.Name)
                {
                    predefined.Replacement = define.Replacement;
                    return;
                }
            }

            Defines.Add(define);
        }

        public void RemoveDefine(LogicDefine define)
        {
            foreach (LogicDefine predefined in Defines)
            {
                if (define.Name == predefined.Name)
                {
                    Defines.Remove(predefined);
                    return;
                }
            }
        }

        public byte[] GetSettingBytes()
        {
            List<LogicOption> settings = Options.Where(option => option.Type == LogicOptionType.Setting).ToList();
            byte[] bytes = new byte[settings.Count];

            for (int i = 0; i < settings.Count; i++)
            {
                bytes[i] = settings[i].GetHashByte();
            }

            return bytes;
        }

        public byte[] GetGimmickBytes()
        {
            List<LogicOption> gimmicks = Options.Where(option => option.Type == LogicOptionType.Gimmick).ToList();
            byte[] bytes = new byte[gimmicks.Count];

            for (int i = 0; i < gimmicks.Count; i++)
            {
                bytes[i] = gimmicks[i].GetHashByte();
            }

            return bytes;
        }

        public void ClearOptions()
        {
            Options.Clear();
        }

        public void ClearReplacements()
        {
            Replacements.Clear();
        }

        public void ClearTypeOverrides()
        {
            LocationTypeOverrides.Clear();
        }

        public void AddOptions()
        {
            foreach (LogicOption option in Options)
            {
                Defines.AddRange(option.GetLogicDefines());
            }
        }

        public string ReplaceDefines(string locationString)
        {
            string outString = locationString;
            foreach (LogicDefine define in Defines)
            {
                if (define.CanReplace(outString))
                {
                    outString = define.Replace(outString);
                }

                if (outString.IndexOf("`") == -1)
                {
                    return outString;
                }
            }

            throw new ParserException($"{locationString} has an invalid/undefined define!");
        }

        public bool ShouldIgnoreLines()
        {
            Console.WriteLine(IfCounter);
            return IfCounter != 0;
        }

        private LogicFlag ParseFlagDirective(string[] directiveParts)
        {
            if (directiveParts.Length < 4)
            {
                throw new ParserException("A flag somewhere has an incorrect number of parameters!");
            }

            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A flag somewhere has an invalid type! ({directiveParts[1]})");
            }

            bool defaultActive;
            if (directiveParts.Length >= 5)
            {
                if (!bool.TryParse(directiveParts[4], out defaultActive))
                {
                    throw new ParserException($"{directiveParts[4]} is not a valid boolean value");
                }
            }
            else
            {
                defaultActive = false;
            }

            return new LogicFlag(directiveParts[2], directiveParts[3], defaultActive, optionType);
        }

        private LogicDefine ParseDefineDirective(string[] directiveParts)
        {
            if (directiveParts.Length == 2)
            {
                if (directiveParts[1] == "RAND_INT")
                {
                    throw new ParserException("Cannot redefine/undefine RAND_INT!");
                }
                return new LogicDefine(directiveParts[1]);
            }
            else if (directiveParts.Length == 3)
            {
                if (directiveParts[1] == "RAND_INT")
                {
                    throw new ParserException("Cannot redefine/undefine RAND_INT!");
                }
                return new LogicDefine(directiveParts[1], directiveParts[2]);
            }
            else
            {
                throw new ParserException($"A define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private LogicColorPicker ParseColorDirective(string[] directiveParts)
        {
            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A color somewhere has an invalid type! ({directiveParts[1]})");
            }

            if (directiveParts.Length == 4)
            {
                return new LogicColorPicker(directiveParts[2], directiveParts[3], optionType, Color.White);
            }
            else if (directiveParts.Length % 3 == 1)
            {

                List<Color> colors = new List<Color>();
                for (int i = 4; i < directiveParts.Length; i += 3)
                {
                    // This parses the color components out, in groups of three.
                    if (int.TryParse(directiveParts[i], NumberStyles.HexNumber, null, out int rComponent))
                    {
                        if (int.TryParse(directiveParts[i + 1], NumberStyles.HexNumber, null, out int gComponent))
                        {
                            if (int.TryParse(directiveParts[i + 2], NumberStyles.HexNumber, null, out int bComponent))
                            {
                                colors.Add(Color.FromArgb(rComponent, gComponent, bComponent));
                            }
                        }
                    }
                }

                return new LogicColorPicker(directiveParts[2], directiveParts[3], optionType, colors);
            }
            else
            {
                throw new ParserException($"A color somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private EventDefine ParseEventDefine(string[] directiveParts)
        {
            if (directiveParts.Length == 2)
            {
                return new EventDefine(directiveParts[1]);
            }
            else if (directiveParts.Length == 3)
            {
                return new EventDefine(directiveParts[1], directiveParts[2]);
            }
            else
            {
                throw new ParserException($"An event define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private LogicOptionType GetOptionType(string typeString)
        {
            if (Enum.TryParse(typeString, out LogicOptionType result))
            {
                return result;
            }
            else
            {
                return LogicOptionType.Untyped;
            }
        }

        private uint ParseCrcDirective(string[] directiveParts)
        {
            if (uint.TryParse(directiveParts[1], NumberStyles.HexNumber, null, out uint outCrc))
            {
                return outCrc;
            }
            else
            {
                throw new ParserException($"\"{directiveParts[1]}\" is not a valid crc!");
            }
        }

        private void ParseReplaceDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 3)
            {
                throw new ParserException("!replace has an invalid amount of arguments");
            }

            var itemData = directiveParts[1].Split(':');
            var itemStrings = itemData[0].Split('.');
            var chanceItems = directiveParts[2].Split(',');

            Item replacedItem;
            var chanceItemList = new List<ChanceItem>();

            ItemType type;
            byte itemsub = 0;

            if (!Enum.TryParse(itemStrings[1], out type))
            {
                throw new ParserException("!replace has an invalid replaced itemType");
            }


            if (itemStrings.Length >= 3)
            {
                if (!byte.TryParse(itemStrings[2], NumberStyles.HexNumber, null, out itemsub))
                {
                    throw new ParserException("!replace has an invalid replaced itemSub");
                }
            }

            var dungeonString = "";
            if (itemData.Length > 1)
            {
                dungeonString = itemData[1];
            }

            replacedItem = new Item(type, itemsub, dungeonString);

            foreach (var chanceItem in chanceItems)
            {

                var chanceItemData = chanceItem.TrimEnd(';').Split(':');
                var chanceItemStrings = chanceItemData[0].Split('.');

                itemsub = 0;
                if (!Enum.TryParse(chanceItemStrings[1], out type))
                {
                    throw new ParserException("!replace has an invalid new item itemType");
                }

                if (chanceItemStrings.Length >= 3)
                {
                    if (!byte.TryParse(chanceItemStrings[2], NumberStyles.HexNumber, null, out itemsub))
                    {
                        throw new ParserException("!replace has an invalid new item itemSub");
                    }
                }

                int chance = 0;
                if (!int.TryParse(chanceItemData[2], out chance))
                {
                    throw new ParserException("!replace has an invalid new item chance value");
                }

                Item item = new Item(type, itemsub, chanceItemData[1]);
                chanceItemList.Add(new ChanceItem(item, chance));
            }
            Replacements.Add(replacedItem, new ChanceItemSet(chanceItemList));
        }

        public void ParseSetTypeDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 3)
            {
                throw new ParserException("!settype has an invalid amount of arguments");
            }

            var itemData = directiveParts[1].Split(':');
            var itemStrings = itemData[0].Split('.');

            Item replacedItem;
            Location.LocationType newType;

            ItemType type;
            byte itemsub = 0;

            if (!Enum.TryParse(itemStrings[1], out type))
            {
                throw new ParserException("!settype has an invalid replaced itemType");
            }


            if (itemStrings.Length >= 3)
            {
                if (!byte.TryParse(itemStrings[2], NumberStyles.HexNumber, null, out itemsub))
                {
                    throw new ParserException("!settype has an invalid replaced itemSub");
                }
            }

            var dungeonString = "";
            if (itemData.Length > 1)
            {
                dungeonString = itemData[1];
            }
            replacedItem = new Item(type, itemsub, dungeonString);

            if (!Enum.TryParse(directiveParts[2], out newType))
            {
                throw new ParserException("!settype has an invalid replaced location type");
            }

            LocationTypeOverrides.Add(replacedItem, newType);

        }


        public struct ChanceItemSet
        {
            public List<ChanceItem> randomItems;
            public int totalChance;

            public ChanceItemSet(List<ChanceItem> randomItems)
            {
                this.randomItems = randomItems;
                totalChance = 0;

                foreach (var randomItem in randomItems)
                {
                    totalChance += randomItem.chance;
                }
            }
        }

        public struct ChanceItem
        {
            public Item item;
            public int chance;

            public ChanceItem(Item item, int chance)
            {
                this.item = item;
                this.chance = chance;
            }
        }

        private LogicOption ParseDropdownDirective(string[] directiveParts)
        {
            if (directiveParts.Length % 2 != 1 || directiveParts.Length < 5)
            {
                throw new ParserException("A dropdown somewhere has an incorrect number of parameters!");
            }

            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A dropdown somewhere has an invalid type! ({directiveParts[1]})");
            }

            Dictionary<string, string> selectionDict = new Dictionary<string, string>((directiveParts.Length / 2) - 1);
            for (int i = 3; i < directiveParts.Length; i += 2)
            {
                selectionDict.Add(directiveParts[i], directiveParts[i + 1]);
            }

            return new LogicDropdown(directiveParts[2], optionType, selectionDict);
        }
    }
}
