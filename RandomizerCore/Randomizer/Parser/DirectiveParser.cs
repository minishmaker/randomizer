using System.Drawing;
using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Randomizer.Logic.Imports;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Parser;

public class DirectiveParser
{
	private readonly List<ItemAmountSet> _amountReplacementReferences;
	public Dictionary<Item, List<ItemAmountSet>> AmountReplacements; //dont want to modify the original
	private readonly Dictionary<Item, List<int>> _amountReplacementsTemplate;
	private readonly List<LogicDefine> _defines;

	public List<EventDefine> EventDefines;
	private int _ifCounter;
	private readonly List<ItemAmountSet> _incrementalReplacementReferences;
	public Dictionary<Item, List<ItemAmountSet>> IncrementalReplacements;
	private readonly Dictionary<Item, List<int>> _incrementalReplacementsTemplate;
	public Dictionary<Item, LocationType> LocationTypeOverrides;
	public string? LogicName;
	public string? LogicVersion;
	public List<LogicOptionBase> Options;
	public Dictionary<Item, ChanceItemSet> Replacements;
	public uint? RomCrc;

	public DirectiveParser()
	{
		Options = new List<LogicOptionBase>();
		_defines = new List<LogicDefine>();
		EventDefines = new List<EventDefine>();
		LocationTypeOverrides = new Dictionary<Item, LocationType>();
		Replacements = new Dictionary<Item, ChanceItemSet>();
		_amountReplacementReferences = new List<ItemAmountSet>();
		_amountReplacementsTemplate = new Dictionary<Item, List<int>>();
		_incrementalReplacementReferences = new List<ItemAmountSet>();
		_incrementalReplacementsTemplate = new Dictionary<Item, List<int>>();
		_ifCounter = 0;
	}

	private string[] SplitDirective(string line)
	{
		// Split define into parameters
		var untrimmed = line.Split('-');

		// Trim each parameter down
		var trimmed = new string[untrimmed.Length];
		for (var i = 0; i < untrimmed.Length; i++) trimmed[i] = untrimmed[i].Trim();

		return trimmed;
	}

	/// <summary>
	///     Check if a directive should be parsed on load or on randomization
	/// </summary>
	/// <param name="line">The line that contains the given directive</param>
	/// <returns>True if the directive should be parsed on load, throws an error if the directive is invalid.</returns>
	public bool ParseOnLoad(string line)
	{
		var directive = SplitDirective(line)[0];
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
			case "!import":
				return false;
			default:
				throw new ParserException($"\"{directive}\" is not a valid directive! The logic file may be bad.");
		}
	}

	public void ParseDirective(string line)
	{
		var mainDirectiveParts = SplitDirective(line);

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
				case "!import":
					ParseImportDirective(mainDirectiveParts);
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

	private void ParseConditionalDirective(string[] directiveParts)
	{
		switch (directiveParts[0])
		{
			case "!ifdef":
				if (ShouldIgnoreLines() || !IsDefined(directiveParts[1])) _ifCounter++;
				return;
			case "!ifndef":
				if (ShouldIgnoreLines() || IsDefined(directiveParts[1])) _ifCounter++;
				return;
			case "!else":
				if (ShouldIgnoreLines())
				{
					if (_ifCounter <= 1) _ifCounter--;
				}
				else
				{
					_ifCounter++;
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
				if (_ifCounter > 0) _ifCounter--;
				return;
		}
	}

	private bool IsDefined(string defineText)
	{
		foreach (var define in _defines)
			if (define.Name == defineText)
				return true;

		return false;
	}

	public void ClearDefines()
	{
		_defines.Clear();
		EventDefines.Clear();
	}

	public void AddDefine(LogicDefine define)
	{
		foreach (var predefined in _defines)
			if (define.Name == predefined.Name)
			{
				predefined.Replacement = define.Replacement;
				return;
			}

		_defines.Add(define);
	}

	public void RemoveDefine(LogicDefine define)
	{
		foreach (var predefined in _defines)
			if (define.Name == predefined.Name)
			{
				_defines.Remove(predefined);
				return;
			}
	}

	public List<LogicOptionBase> GetSortedSettings()
	{
		var settings = Options.Where(option => option.Type == LogicOptionType.Setting).ToList();
		settings.Sort((option1, option2) => string.CompareOrdinal(option1.Name, option2.Name));
		return settings;
	}

	public List<LogicOptionBase> GetSortedGimmicks()
	{
		var settings = Options.Where(option => option.Type == LogicOptionType.Cosmetic).ToList();
		settings.Sort((option1, option2) => string.CompareOrdinal(option1.Name, option2.Name));
		return settings;
	}

	public byte[] GetSettingBytes()
	{
		var settings = Options.Where(option => option.Type == LogicOptionType.Setting).ToList();
		var bytes = new byte[settings.Count];

		for (var i = 0; i < settings.Count; i++) bytes[i] = settings[i].GetHashByte();

		return bytes;
	}

	public byte[] GetCosmeticBytes()
	{
		var cosmetics = Options.Where(option => option.Type == LogicOptionType.Cosmetic).ToList();
		var bytes = new byte[cosmetics.Count];

		for (var i = 0; i < cosmetics.Count; i++) bytes[i] = cosmetics[i].GetHashByte();

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
		_incrementalReplacementsTemplate.Clear();
	}

	public void ClearAmountReplacements()
	{
		_amountReplacementsTemplate.Clear();
	}

	public void DuplicateAmountReplacements()
	{
		AmountReplacements = new Dictionary<Item, List<ItemAmountSet>>();
		var referenceClones = new List<ItemAmountSet>();
		foreach (var reference in _amountReplacementReferences)
			referenceClones.Add(reference
				.Clone()); //making sure we dont change the original reference as those need to be reused

		foreach (var key in _amountReplacementsTemplate.Keys)
		{
			var set = _amountReplacementsTemplate[key];
			var newList = new List<ItemAmountSet>();
			foreach (var id in set) newList.Add(referenceClones[id]);
			AmountReplacements.Add(key, newList);
		}
	}

	public void DuplicateIncrementalReplacements()
	{
		IncrementalReplacements = new Dictionary<Item, List<ItemAmountSet>>();
		var referenceClones = new List<ItemAmountSet>();
		foreach (var reference in _incrementalReplacementReferences)
			referenceClones.Add(reference
				.Clone()); //making sure we dont change the original reference as those need to be reused

		foreach (var key in _incrementalReplacementsTemplate.Keys)
		{
			var set = _incrementalReplacementsTemplate[key];
			var newList = new List<ItemAmountSet>();
			foreach (var id in set) newList.Add(referenceClones[id]);
			IncrementalReplacements.Add(key, newList);
		}
	}

	public void ClearTypeOverrides()
	{
		LocationTypeOverrides.Clear();
	}

	public void AddOptions()
	{
		foreach (var option in Options) _defines.AddRange(option.GetLogicDefines());
	}

	public string ReplaceDefines(string locationString)
	{
		var outString = locationString;
		foreach (var define in _defines)
		{
			if (define.CanReplace(outString)) outString = define.Replace(outString);

			if (outString.IndexOf("`", StringComparison.Ordinal) == -1) return outString;
		}

		throw new ParserException($"{locationString} has an invalid/undefined define!");
	}

	public bool ShouldIgnoreLines()
	{
		Logger.Instance.LogInfo($"If Counter: {_ifCounter}");
		return _ifCounter != 0;
	}

	public void ParseImportDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 2)
			throw new ParserException("Import directive does not have the required number of parameters!");

		var functionName = directiveParts[1];
		
		if (!LogicImports.FunctionValues.TryGetValue(functionName, out var function))
			throw new ParserException("Import directive requires a function that does not exist!");
		
		Location.ShufflerConstraints.Add(function);
	}

	private LogicDefine ParseDefineDirective(string[] directiveParts)
	{
		if (directiveParts.Length == 2)
		{
			if (directiveParts[1] == "RAND_INT") throw new ParserException("Cannot redefine/undefine RAND_INT!");
			return new LogicDefine(directiveParts[1]);
		}

		if (directiveParts.Length == 3)
		{
			if (directiveParts[1] == "RAND_INT") throw new ParserException("Cannot redefine/undefine RAND_INT!");
			return new LogicDefine(directiveParts[1], directiveParts[2]);
		}

		throw new ParserException(
			$"A define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
	}

	private EventDefine ParseEventDefine(string[] directiveParts)
	{
		if (directiveParts.Length == 2)
			return new EventDefine(directiveParts[1]);
		if (directiveParts.Length == 3)
			return new EventDefine(directiveParts[1], directiveParts[2]);
		throw new ParserException(
			$"An event define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
	}

	private LogicOptionType GetOptionType(string typeString)
	{
		if (Enum.TryParse(typeString, out LogicOptionType result))
			return result;
		return LogicOptionType.Untyped;
	}

	private uint ParseCrcDirective(string[] directiveParts)
	{
		if (StringUtil.ParseString(directiveParts[1], out int outCrc))
			return (uint)outCrc;
		throw new ParserException($"\"{directiveParts[1]}\" is not a valid crc!");
	}

	private void ParseReplaceChanceDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 3) throw new ParserException("!replace has an invalid amount of arguments");

		Item replacedItem;
		var chanceItemList = new List<ChanceItem>();

		replacedItem = new Item(directiveParts[1], "!replace");

		var chanceItems = directiveParts[2].Split(',');
		foreach (var chanceItem in chanceItems)
		{
			var trimmed = chanceItem.TrimEnd(';');
			var chanceItemData = trimmed.Split(':');
			var chanceItemStrings = chanceItemData[0].Split('.');

			var chance = 0;
			if (!StringUtil.ParseString(chanceItemData[2], out chance))
				throw new ParserException("!replace has an invalid new item chance value");

			var item = new Item(trimmed, "!replace");
			chanceItemList.Add(new ChanceItem(item, chance));
		}

		Replacements.Add(replacedItem, new ChanceItemSet(chanceItemList));
	}

	private void ParseReplaceAmountDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 4) throw new ParserException("!replaceamount has an invalid amount of arguments");

		var replacementItem = new Item(directiveParts[1], "!replaceamount");

		byte replacementAmount;
		if (!StringUtil.ParseString(directiveParts[2], out replacementAmount))
			throw new ParserException("!replaceamount has an invalid amount");

		_amountReplacementReferences.Add(new ItemAmountSet(replacementItem, replacementAmount));
		var referenceId = _amountReplacementReferences.Count - 1;

		var replacedItems = directiveParts[3].Split(',');
		foreach (var itemString in replacedItems)
		{
			if (itemString == "")
				continue;
			var item = new Item(itemString, "!replaceamount");
			if (_amountReplacementsTemplate.ContainsKey(item))
			{
				var set = _amountReplacementsTemplate[item];
				set.Add(referenceId);
				_amountReplacementsTemplate[item] = set;
			}
			else
			{
				var list = new List<int>();
				list.Add(referenceId);
				_amountReplacementsTemplate.Add(item, list);
			}
		}
	}

	private void ParseReplaceIncrementDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 4)
			throw new ParserException("!replaceincrement has an invalid amount of arguments");

		var replacementItem = new Item(directiveParts[1], "!replaceincrement");

		byte replacementAmount;
		if (!StringUtil.ParseString(directiveParts[2], out replacementAmount))
			throw new ParserException("!replaceincrement has an invalid amount");

		_incrementalReplacementReferences.Add(new ItemAmountSet(replacementItem, replacementAmount));
		var referenceId = _incrementalReplacementReferences.Count - 1;

		var replacedItems = directiveParts[3].Split(',');
		foreach (var itemString in replacedItems)
		{
			if (itemString == "")
				continue;
			var item = new Item(itemString, "!replaceincrement");
			if (_incrementalReplacementsTemplate.ContainsKey(item))
			{
				var set = _incrementalReplacementsTemplate[item];
				set.Add(referenceId);
				_incrementalReplacementsTemplate[item] = set;
			}
			else
			{
				var list = new List<int>();
				list.Add(referenceId);
				_incrementalReplacementsTemplate.Add(item, list);
			}
		}
	}

	public LogicDefine ParseAdditionDirective(string[] directiveParts)
	{
		var totalValue = 0;
		var values = directiveParts[2].Split(',');
		foreach (var value in values)
		{
			byte number;
			if (!StringUtil.ParseString(value, out number)) throw new ParserException("!addition has an invalid value");
			totalValue += number;

			if (totalValue > 255) throw new ParserException("!addition resulted in a value higher than 255 (0xFF)");
		}

		return new LogicDefine(directiveParts[1], totalValue.ToString());
	}

	public void ParseSetTypeDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 3) throw new ParserException("!settype has an invalid amount of arguments");

		LocationType newType;

		var replacedItem = new Item(directiveParts[1], "!settype");

		if (!Enum.TryParse(directiveParts[2], out newType))
			throw new ParserException("!settype has an invalid replaced location type");

		LocationTypeOverrides.Add(replacedItem, newType);
	}

	private LogicFlag ParseFlagDirective(string[] directiveParts)
	{
		if (directiveParts.Length < 7)
			throw new ParserException("A flag somewhere has an incorrect number of parameters!");

		var optionType = GetOptionType(directiveParts[2]);

		if (optionType == LogicOptionType.Untyped)
			throw new ParserException($"A flag somewhere has an invalid type! ({directiveParts[2]})");

		bool defaultActive;
		if (directiveParts.Length > 7)
		{
			if (!bool.TryParse(directiveParts[7], out defaultActive))
				throw new ParserException($"{directiveParts[7]} is not a valid boolean value");
		}
		else
		{
			defaultActive = false;
		}

		return new LogicFlag(directiveParts[4], directiveParts[5], defaultActive, 
			directiveParts[3], directiveParts[1], directiveParts[6], optionType);
	}

	private LogicOptionBase ParseDropdownDirective(string[] directiveParts)
	{
		if (directiveParts.Length % 3 != 1 || directiveParts.Length < 10)
			throw new ParserException("A dropdown somewhere has an incorrect number of parameters!");

		var optionType = GetOptionType(directiveParts[2]);

		if (optionType == LogicOptionType.Untyped)
			throw new ParserException($"A dropdown somewhere has an invalid type! ({directiveParts[2]})");

		var selectionDict = new Dictionary<string, string>();
		var descriptionText = new StringBuilder();
		descriptionText.AppendLine(directiveParts[6]);
		
		for (var i = 7; i < directiveParts.Length; )
		{
			selectionDict.Add(directiveParts[i++], directiveParts[i++]);
			descriptionText.AppendLine($"\n{directiveParts[i++]}");
		}

		return new LogicDropdown(directiveParts[4], directiveParts[5], directiveParts[3], 
			directiveParts[1], descriptionText.ToString(), optionType, selectionDict);
	}

	private LogicColorPicker ParseColorDirective(string[] directiveParts)
	{
		var optionType = GetOptionType(directiveParts[2]);

		if (optionType == LogicOptionType.Untyped)
			throw new ParserException($"Colorpicker has invalid type {directiveParts[2]}!");

		if (directiveParts.Length == 7)
			return new LogicColorPicker(directiveParts[4], directiveParts[5], directiveParts[3],
				directiveParts[1], directiveParts[6], optionType, Color.White);

		if (directiveParts.Length % 3 == 1)
		{
			var colors = new List<Color>();
			for (var i = 7; i < directiveParts.Length; )
				// This parses the color components out, in groups of three.
				if (StringUtil.ParseString(directiveParts[i++], out int rComponent) &&
				    StringUtil.ParseString(directiveParts[i++], out int gComponent) &&
				    StringUtil.ParseString(directiveParts[i++], out int bComponent)
				   )
					colors.Add(Color.FromArgb(rComponent, gComponent, bComponent));
				else
					throw new ParserException(
						"Colorpicker has an incorrect color value number!");

			return new LogicColorPicker(directiveParts[4], directiveParts[5], directiveParts[3], 
				directiveParts[1], directiveParts[6], optionType, colors);
		}

		throw new ParserException(
			"Colorpicker does not have the right number of parameters!");
	}

	private LogicOptionBase ParseNumberboxDirective(string[] directiveParts)
	{
		if (directiveParts.Length != 8)
			throw new ParserException("Numberbox does not have the right number of parameters!");

		var optionType = GetOptionType(directiveParts[2]);

		if (optionType == LogicOptionType.Untyped)
			throw new ParserException($"Numberbox has invalid type {directiveParts[2]})!");
		
		if (!byte.TryParse(directiveParts[7], out var defaultValue))
			throw new ParserException($"Numberbox has invalid default value {directiveParts[7]}!");

		return new LogicNumberBox(directiveParts[4], directiveParts[5], directiveParts[3], 
			directiveParts[1], defaultValue, directiveParts[6], optionType);
	}
}
