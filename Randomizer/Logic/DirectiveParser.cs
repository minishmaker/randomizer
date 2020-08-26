using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MinishRandomizer.Core;
using MinishRandomizer.Utilities;

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

        private List<ItemAmountSet> AmountReplacementReferences;
        private Dictionary<Item, List<int>> AmountReplacementsTemplate;
        public Dictionary<Item, List<ItemAmountSet>> AmountReplacements; //dont want to modify the original
        private List<ItemAmountSet> IncrementalReplacementReferences;
        private Dictionary<Item, List<int>> IncrementalReplacementsTemplate;
        public Dictionary<Item, List<ItemAmountSet>> IncrementalReplacements;

        public List<EventDefine> EventDefines;
        private int IfCounter;

        public DirectiveParser()
        {
            Options = new List<LogicOption>();
            Defines = new List<LogicDefine>();
            EventDefines = new List<EventDefine>();
            LocationTypeOverrides = new Dictionary<Item, Location.LocationType>();
            Replacements = new Dictionary<Item, ChanceItemSet>();
            AmountReplacementReferences = new List<ItemAmountSet>();
            AmountReplacementsTemplate = new Dictionary<Item, List<int>>();
            IncrementalReplacementReferences = new List<ItemAmountSet>();
            IncrementalReplacementsTemplate = new Dictionary<Item, List<int>>();
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
                case "!numberbox":
                    return true;
                case "!define":
                case "!addition":
                case "!undefine":
                case "!eventdefine":
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                case "!replace":
                case "!replaceamount":
                case "!replaceincrement":
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
                        ParseReplaceChanceDirective(mainDirectiveParts);
                        break;
                    case "!replaceamount":
                        ParseReplaceAmountDirective(mainDirectiveParts);
                        break;
                    case "!replaceincrement":
                        ParseReplaceIncrementDirective(mainDirectiveParts);
                        break;
                    case "!addition":
                        AddDefine(ParseAdditionDirective(mainDirectiveParts));
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
                    case "!numberbox":
                        Options.Add(ParseNumberboxDirective(mainDirectiveParts));
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

        public void ClearIncrementalReplacements()
        {
            IncrementalReplacementsTemplate.Clear();
        }
        public void ClearAmountReplacements()
        {
            AmountReplacementsTemplate.Clear();
        }

        public void DuplicateAmountReplacements()
        {
            AmountReplacements = new Dictionary<Item, List<ItemAmountSet>>();
            var referenceClones = new List<ItemAmountSet>();
            foreach (var reference in AmountReplacementReferences)
            {
                referenceClones.Add(reference.Clone());//making sure we dont change the original reference as those need to be reused
            }

            foreach (var key in AmountReplacementsTemplate.Keys)
            {
                var set = AmountReplacementsTemplate[key];
                var newList = new List<ItemAmountSet>();
                foreach (var id in set)
                {
                    newList.Add(referenceClones[id]);
                }
                AmountReplacements.Add(key, newList);
            }
        }

        public void DuplicateIncrementalReplacements()
        {
            IncrementalReplacements = new Dictionary<Item, List<ItemAmountSet>>();
            var referenceClones = new List<ItemAmountSet>();
            foreach (var reference in IncrementalReplacementReferences)
            {
                referenceClones.Add(reference.Clone());//making sure we dont change the original reference as those need to be reused
            }

            foreach (var key in IncrementalReplacementsTemplate.Keys)
            {
                var set = IncrementalReplacementsTemplate[key];
                var newList = new List<ItemAmountSet>();
                foreach (var id in set)
                {
                    newList.Add(referenceClones[id]);
                }
                IncrementalReplacements.Add(key, newList);
            }
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

        private void ParseReplaceChanceDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 3)
            {
                throw new ParserException("!replace has an invalid amount of arguments");
            }

            Item replacedItem;
            var chanceItemList = new List<ChanceItem>();

            replacedItem = new Item(directiveParts[1], "!replace");

            var chanceItems = directiveParts[2].Split(',');
            foreach (var chanceItem in chanceItems)
            {
                var trimmed = chanceItem.TrimEnd(';');
                var chanceItemData = trimmed.Split(':');
                var chanceItemStrings = chanceItemData[0].Split('.');

                int chance = 0;
                if (!int.TryParse(chanceItemData[2], out chance))
                {
                    throw new ParserException("!replace has an invalid new item chance value");
                }

                Item item = new Item(trimmed, "!replace");
                chanceItemList.Add(new ChanceItem(item, chance));
            }
            Replacements.Add(replacedItem, new ChanceItemSet(chanceItemList));
        }

        private void ParseReplaceAmountDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 4)
            {
                throw new ParserException("!replaceamount has an invalid amount of arguments");
            }

            var replacementItem = new Item(directiveParts[1], "!replaceamount");
            
            byte replacementAmount;
            if (!byte.TryParse(directiveParts[2], out replacementAmount))
            {
                throw new ParserException("!replaceamount has an invalid amount");
            }

            AmountReplacementReferences.Add(new ItemAmountSet(replacementItem, replacementAmount));
            var referenceId = AmountReplacementReferences.Count - 1;

            var replacedItems = directiveParts[3].Split(',');
            foreach (var itemString in replacedItems)
            {
                if (itemString == "")
                    continue;
                var item = new Item(itemString, "!replaceamount");
                if (AmountReplacementsTemplate.ContainsKey(item))
                {
                    var set = AmountReplacementsTemplate[item];
                    set.Add(referenceId);
                    AmountReplacementsTemplate[item] = set;
                }
                else 
                {
                    var list = new List<int>();
                    list.Add(referenceId);
                    AmountReplacementsTemplate.Add(item, list);
                }
            }
        }

        private void ParseReplaceIncrementDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 4)
            {
                throw new ParserException("!replaceincrement has an invalid amount of arguments");
            }

            var replacementItem = new Item(directiveParts[1], "!replaceincrement");

            byte replacementAmount;
            if (!byte.TryParse(directiveParts[2], out replacementAmount))
            {
                throw new ParserException("!replaceincrement has an invalid amount");
            }

            IncrementalReplacementReferences.Add(new ItemAmountSet(replacementItem, replacementAmount));
            var referenceId = IncrementalReplacementReferences.Count - 1;

            var replacedItems = directiveParts[3].Split(',');
            foreach (var itemString in replacedItems)
            {
                if (itemString == "")
                    continue;
                var item = new Item(itemString, "!replaceincrement");
                if (IncrementalReplacementsTemplate.ContainsKey(item))
                {
                    var set = IncrementalReplacementsTemplate[item];
                    set.Add(referenceId);
                    IncrementalReplacementsTemplate[item] = set;
                }
                else
                {
                    var list = new List<int>();
                    list.Add(referenceId);
                    IncrementalReplacementsTemplate.Add(item, list);
                }
            }
        }

        public LogicDefine ParseAdditionDirective(string[] directiveParts)
        {
            var name = directiveParts[1];
            int totalValue = 0;
            var values = directiveParts[2].Split(',');
            foreach(string value in values)
            {
                byte number;
                if (!byte.TryParse(value, out number)) 
                {
                    throw new ParserException("!addition has an invalid value");
                }
                totalValue += number;

                if(totalValue>255)
                {
                    throw new ParserException("!addition resulted in a value higher than 255 (0xFF)");
                }
            }
            return new LogicDefine( directiveParts[1] ,totalValue.ToString("X2"));
        }

        public void ParseSetTypeDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 3)
            {
                throw new ParserException("!settype has an invalid amount of arguments");
            }

            Location.LocationType newType;
            
            var replacedItem = new Item(directiveParts[1], "!settype");

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

        public class ItemAmountSet
        { 
            public Item item;
            public int amount;

            public ItemAmountSet(Item item, int amount)
            {
                this.item = item;
                this.amount = amount;
            }

            public ItemAmountSet Clone()
            {
                return new ItemAmountSet(this.item, this.amount);
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

        private LogicOption ParseNumberboxDirective(string[] directiveParts)
        {
            if (directiveParts.Length != 4)
            {
                throw new ParserException("A numberbox somewhere has an incorrect number of parameters!");
            }

            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A numberbox somewhere has an invalid type! ({directiveParts[1]})");
            }

            return new LogicNumberBox(directiveParts[2], optionType, directiveParts[3]);
        }
    }
}
