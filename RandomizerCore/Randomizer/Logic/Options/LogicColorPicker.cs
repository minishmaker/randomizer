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
        base(name, niceName, true, settingGroup, settingPage, descriptionText, type)
    {
        BaseColor = startingColor;
        DefinedColor = startingColor;
        InitialColors = new List<Color>(1)
        {
            startingColor
        };
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
        base(name, niceName, true, settingGroup, settingPage, descriptionText, type)
    {
        BaseColor = colors[0];
        DefinedColor = colors[0];
        InitialColors = colors;
        UseRandomColor = false;
    }

    public override void Reset()
    {
        Active = true;
        UseRandomColor = false;
        DefinedColor = BaseColor;
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        base.CopyValueFrom(option);
        var colorPicker = (LogicColorPicker)option;
        UseRandomColor = colorPicker.UseRandomColor;
        DefinedColor = colorPicker.DefinedColor;
    }

    public Color BaseColor { get; set; }
    public Color DefinedColor { get; set; }
    public List<Color> InitialColors { get; set; }

    public bool UseRandomColor { get; set; }

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

    public override byte GetHashByte()
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
}
