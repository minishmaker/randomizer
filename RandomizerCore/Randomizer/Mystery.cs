using System.Drawing;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RandomizerCore.Randomizer;

internal static class Mystery
{
    private static string indent = "  ";
    private static int commentOffset = 50;

    public static string CreateYAML(string? name, string? description, List<LogicOptionBase> logicOptions, bool mysteryFlag)
    {
        var content = "";
        content += "name: \"" + (string.IsNullOrEmpty(name) ? "(No name set)" : name) + "\"" + Environment.NewLine;
        content += "description: \"" + (string.IsNullOrEmpty(description) ? "(No description set)" : description) + "\"" + Environment.NewLine;
        content += "settings:" + Environment.NewLine;
        var pages = logicOptions.GroupBy(option => option.SettingPage);
        foreach (var page in pages)
        {
            content += indent + "## " + page.ToList()[0].SettingPage + ":" + Environment.NewLine;
            var groups = page.GroupBy(option => option.SettingGroup);
            foreach (var group in groups)
            {
                content += indent + "#  " + group.ToList()[0].SettingGroup + ":" + Environment.NewLine;
                foreach (var option in group)
                {
                    switch (option)
                    {
                        case LogicFlag:
                            if (mysteryFlag)
                            {
                                content += PadComment(indent + option.Name + ":", option.NiceName, commentOffset);
                                content += indent + indent + "on: 1" + Environment.NewLine;
                                content += indent + indent + "off: 1" + Environment.NewLine;
                            }
                            else
                                content += PadComment(indent + option.Name + ": " + (option.Active ? "on" : "off"), option.NiceName, commentOffset);
                            break;
                        case LogicDropdown dropdown:
                            if (mysteryFlag)
                            {
                                content += PadComment(indent + option.Name + ":", option.NiceName, commentOffset);
                                foreach(var sel in dropdown.Selections)
                                    content += PadComment(indent + indent + sel.Value + ": 1", indent + sel.Key, commentOffset);
                            }
                            else
                                content += PadComment(indent + option.Name + ": " + dropdown.Selections[dropdown.Selection], option.NiceName, commentOffset);
                            break;
                        case LogicColorPicker colorPicker:
                            if (mysteryFlag)
                            {
                                content += PadComment(indent + option.Name + ":", option.NiceName, commentOffset);
                                Color color = colorPicker.BaseColor;
                                content += indent + indent + "vanilla: 1" + Environment.NewLine;
                                content += indent + indent + "default: 1" + Environment.NewLine;
                                content += indent + indent + "random: 1" + Environment.NewLine;
                                content += indent + indent + color.R + " " + color.G + " " + color.B + ": 1" + Environment.NewLine;
                                content += indent + indent + '"' + ColorTranslator.ToHtml(color) + "\": 1" + Environment.NewLine;
                            }
                            else
                            {
                                if (colorPicker.Active)
                                {
                                    if (colorPicker.UseRandomColor)
                                        content += PadComment(indent + option.Name + ": random", option.NiceName, commentOffset);
                                    else
                                    {
                                        var color = colorPicker.DefinedColor;
                                        content += PadComment(indent + option.Name + ": " + color.R + " " + color.G + " " + color.B, option.NiceName, commentOffset);
                                    }
                                }
                                else
                                    content += PadComment(indent + option.Name + ": vanilla", option.NiceName, commentOffset);
                            }
                            break;
                        case LogicNumberBox box:
                            if (mysteryFlag)
                            {
                                content += PadComment(indent + option.Name + ":", option.NiceName, commentOffset);
                                content += indent + indent + box.MinValue + ": 1" + Environment.NewLine;
                                if (box.DefaultValue != box.MinValue)
                                    content += indent + indent + box.DefaultValue + ": 1" + Environment.NewLine;
                                if (box.DefaultValue != box.MaxValue)
                                    content += indent + indent + box.MaxValue + ": 1" + Environment.NewLine;
                                content += indent + indent + box.MinValue + " " + box.MaxValue + " 1: 1" + Environment.NewLine;
                            }
                            else
                                content += PadComment(indent + option.Name + ": " + box.Value, option.NiceName, commentOffset);
                            break;
                    }
                }
            }
        }
        content += "subweights: {}" + Environment.NewLine;
        return content;
    }

    private static string PadComment(string line, string comment, int padding)
    {
        return line + new string(' ', Math.Max(0, padding-line.Length)) + "# " + comment + Environment.NewLine;
    }

    public static YAMLResult ParseYAML(string file, List<LogicOptionBase> baseOptions, Random random)
    {
        OptionList newOptions = new OptionList(baseOptions.Count);
        var optionMap = new Dictionary<string, LogicOptionBase>(baseOptions.Count);
        foreach (var setting in baseOptions)
        {
            var option = (LogicOptionBase) setting.Clone();
            option.Reset();
            newOptions.Add(option);
            optionMap.Add(option.Name, option);
        }
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var weights = deserializer.Deserialize<MysteryWeights>(file);
        HandleWeights(weights, optionMap, random);
        return new YAMLResult(newOptions, weights.Name ?? "(No name set)", weights.Description ?? "(No description set)");
    }

    private static void HandleWeights(MysteryWeights weights, Dictionary<string, LogicOptionBase> optionMap, Random random)
    {
        if (weights.Settings != null)
        {
            foreach (var setting in weights.Settings)
            {
                if (setting.Key is string && optionMap.ContainsKey((string) setting.Key))
                {
                    if (setting.Value is string)
                    {
                        SetOptionValue(optionMap[(string) setting.Key], (string) setting.Value, random);
                    }
                    else
                        if (setting.Value is Dictionary<object, object>)
                        {
                            var choices = ((Dictionary<object, object>) setting.Value).Keys.ToList();
                            var chances = ((Dictionary<object, object>) setting.Value).Values.Select(c => int.Parse("" + c)).ToList();
                            var totalChance = chances.Sum();
                            var number = random.Next(totalChance);
                            var val = 0;
                            for (var i = 0; i < choices.Count; i++)
                            {
                                val += chances[i];
                                if (number < val)
                                {
                                    SetOptionValue(optionMap[(string) setting.Key], (string) choices[i], random);
                                    break;
                                }
                            }
                        }
                }
            }
        }
        if (weights.Subweights != null)
        {
            foreach (var subweightGroup in weights.Subweights)
            {
                var choices = new List<MysteryWeights>(subweightGroup.Value.Count);
                var chances = new List<int>(subweightGroup.Value.Count);
                var totalChance = 0;
                foreach (var weightSet in subweightGroup.Value)
                {
                    if (weightSet.Value.Chance is int chance)
                    {
                        choices.Add(weightSet.Value);
                        chances.Add(chance);
                        totalChance += chance;
                    }
                }
                var number = random.Next(totalChance);
                var val = 0;
                for (var i = 0; i < choices.Count; i++)
                {
                    val += chances[i];
                    if (number < val)
                    {
                        HandleWeights(choices[i], optionMap, random);
                        break;
                    }
                }
            }
        }
    }

    private static void SetOptionValue(LogicOptionBase option, string value, Random random)
    {
        switch (option)
        {
            case LogicFlag:
                if (string.Equals(value, "on", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || value == "1")
                    option.Active = true;
                else
                    if (string.Equals(value, "off", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) || value == "0")
                        option.Active = false;
                    else
                        throw new ParserException($"Invalid value \"{value}\" for Flag option \"{option.Name}\"");
                break;
            case LogicDropdown dropdown:
                if (dropdown.Selections.ContainsValue(value))
                    dropdown.Selection = dropdown.Selections.Keys.ToList()[dropdown.Selections.Values.ToList().IndexOf(value)];
                else
                    throw new ParserException($"Invalid value \"{value}\" for Dropdown option \"{option.Name}\"");
                break;
            case LogicColorPicker colorPicker:
                if (string.Equals(value, "vanilla", StringComparison.OrdinalIgnoreCase))
                {
                    colorPicker.Active = false;
                    colorPicker.UseRandomColor = false;
                    colorPicker.DefinedColor = colorPicker.BaseColor;
                }
                else
                    if (string.Equals(value, "default", StringComparison.OrdinalIgnoreCase))
                    {
                        colorPicker.Active = true;
                        colorPicker.UseRandomColor = false;
                        colorPicker.DefinedColor = colorPicker.BaseColor;
                    }
                    else
                    {
                        if (string.Equals(value, "random", StringComparison.OrdinalIgnoreCase))
                        {
                            colorPicker.Active = true;
                            colorPicker.UseRandomColor = true;
                            colorPicker.DefinedColor = colorPicker.BaseColor;
                        }
                        else
                        {
                            if (value.StartsWith("#"))
                            {
                                colorPicker.Active = true;
                                colorPicker.UseRandomColor = false;
                                colorPicker.DefinedColor = ColorTranslator.FromHtml(value);
                            }
                            else
                            {
                                var colorValues = value.Split(" ");
                                if (colorValues.Length == 3 && int.TryParse(colorValues[0], out int r) && int.TryParse(colorValues[1], out int g) && int.TryParse(colorValues[2], out int b))
                                {
                                    colorPicker.Active = true;
                                    colorPicker.UseRandomColor = false;
                                    colorPicker.DefinedColor = Color.FromArgb(r, g, b);
                                }
                                else
                                    throw new ParserException($"Invalid value \"{value}\" for Color option \"{option.Name}\"");
                            }
                        }
                    }
                break;
            case LogicNumberBox box:
                if (int.TryParse(value, out var i) && i >= box.MinValue && i <= box.MaxValue)
                    box.Value = i.ToString();
                else
                {
                    var split = value.Split(" ");
                    if (split.Length >= 2 && int.TryParse(split[0], out var min) && int.TryParse(split[1], out var max) && min >= box.MinValue && max <= box.MaxValue)
                    {
                        var step = 1;
                        if (split.Length >= 3)
                            int.TryParse(split[2], out step);
                        if (step <= 0)
                            throw new ParserException($"Invalid value \"{value}\" for Number option \"{option.Name}\"");
                        var val = min + random.Next((max - min) / step + 1) * step;
                        box.Value = val.ToString();
                    }
                    else
                        throw new ParserException($"Invalid value \"{value}\" for Number option \"{option.Name}\"");
                }
                break;
        }
    }

    private struct MysteryWeights
    {
        public string? Name;
        public string? Description;
        public int? Chance;
        public Dictionary<object, object>? Settings;
        public Dictionary<object, Dictionary<object, MysteryWeights>>? Subweights;
    }
}
