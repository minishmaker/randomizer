using System.Drawing;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicColorPicker : LogicOptionBase
{
    public LogicColorPicker(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type,
        Color startingColor) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        Active = true;
        BaseColor = startingColor;
        DefinedColor = startingColor;
        InitialColors = [startingColor];
        UseRandomColor = false;
    }

    public LogicColorPicker(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type,
        List<Color> colors) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        Active = true;
        BaseColor = colors[0];
        DefinedColor = colors[0];
        InitialColors = colors;
        UseRandomColor = false;
    }

    public override bool IsReset()
    {
        return Active && !UseRandomColor && DefinedColor == BaseColor;
    }

    public override void Reset()
    {
        Active = true;
        UseRandomColor = false;
        DefinedColor = BaseColor;
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        var colorPicker = (LogicColorPicker)option;
        Active = colorPicker.Active;
        UseRandomColor = colorPicker.UseRandomColor;
        DefinedColor = colorPicker.DefinedColor;
    }

    public Color BaseColor { get; }
    public List<Color> InitialColors { get; }

    public bool Active { get; set; }
    public bool UseRandomColor { get; set; }
    public Color DefinedColor { get; set; }

    public void PickRandomColor()
    {
        var random = new SquaresRandomNumberGenerator();
        DefinedColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(3);

        // Only true if a color has been selected
        if (!Active) return defineList;

        var FinalColor = DefinedColor;

        if (UseRandomColor)
        {
            var random = new System.Random();
            FinalColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        defineList.Add(new LogicDefine(Name));

        defineList.AddRange(InitialColors
            .Select(color => new GbaColor(ColorUtil.AdjustHue(color, BaseColor, FinalColor)))
            .Select((newColor, i) => new LogicDefine(Name + "_" + i, StringUtil.AsStringHex4(newColor.CombinedValue))));

        return defineList;
    }

    public override byte GetSelectionHashByte()
    {
        // Maybe not a great way to represent, leaves some info out and is likely to cause easy collisions
        return Active ? (byte)(DefinedColor.R ^ DefinedColor.G ^ DefinedColor.B) : (byte)00;
    }

    public override string GetOptions()
    {
        return "A color code in ARGB format";
    }

    public override string GetOptionUiType()
    {
        return "Color Picker";
    }

    public override string GetValueAsString()
    {
        return $"{(Active ? "true" : "false")}_{(UseRandomColor ? "true" : "false")}_{ColorTranslator.ToHtml(DefinedColor)}";
    }

    public override void SetValueFromString(string value)
    {
        var properties = value.Split("_");
        if (properties.Length != 3) throw new Exception($"Invalid value \"{value}\"");
        if (!bool.TryParse(properties[0], out var active)) throw new Exception($"Invalid value \"{value}\"");
        Active = active;
        if (!bool.TryParse(properties[1], out var random)) throw new Exception($"Invalid value \"{value}\"");
        UseRandomColor = random;
        DefinedColor = ColorTranslator.FromHtml(properties[2]);
    }
}
