using System.Drawing;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class MirroredColorPicker : LogicColorPicker
{
    public LogicColorPicker OriginalSetting { get; }

    public MirroredColorPicker(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type,
        List<Color> colors,
        LogicColorPicker originalSetting) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type, colors)
    {
        OriginalSetting = originalSetting;
    }

    public override void NotifyChildren()
    {
        UpdateChildren([GetValueAsString()]);
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        return OriginalSetting.GetLogicDefines();
    }

    public override IList<LogicOptionBase> GetChildren()
    {
        return [OriginalSetting];
    }
}
