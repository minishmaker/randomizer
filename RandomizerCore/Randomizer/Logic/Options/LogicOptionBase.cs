using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public abstract class LogicOptionBase : ICloneable
{
    private readonly List<ILogicOptionObserver> Observers;

    protected LogicOptionBase(
        string name,
        string niceName,
        bool active,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type = LogicOptionType.Untyped)
    {
        Name = name;
        NiceName = niceName;
        Active = active;
        Type = type;
        SettingGroup = settingGroup;
        SettingPage = settingPage;
        var tempText = descriptionText.Trim();
        var builder = new StringBuilder();
        foreach (var s in tempText.Split("\\n")) builder.AppendLine(s);
        DescriptionText = builder.ToString();
        Observers = new List<ILogicOptionObserver>();
    }

    public string Name { get; set; }
    public string NiceName { get; set; }
    public bool Active { get; set; }
    public LogicOptionType Type { get; set; }
    public string SettingGroup { get; set; }
    public string SettingPage { get; set; }
    public string DescriptionText { get; set; }

    public void NotifyObservers()
    {
        foreach (var observer in Observers) observer.NotifyObserver();
    }

    public void RegisterObserver(ILogicOptionObserver observer)
    {
        Observers.Add(observer);
    }

    public abstract void Reset();

    public virtual void CopyValueFrom(LogicOptionBase option)
    {
        Active = option.Active;
    }

    public abstract List<LogicDefine> GetLogicDefines();

    public abstract string GetOptions();

    public abstract byte GetHashByte();

    public abstract string GetOptionUiType();

    public object Clone() => MemberwiseClone();
}
