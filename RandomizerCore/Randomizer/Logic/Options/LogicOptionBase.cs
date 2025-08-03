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
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type)
    {
        Name = name;
        NiceName = niceName;
        Type = type;
        SettingGroup = settingGroup;
        SettingPage = settingPage;
        var tempText = descriptionText.Trim();
        var builder = new StringBuilder();
        foreach (var s in tempText.Split("\\n")) builder.AppendLine(s);
        DescriptionText = builder.ToString();
        Observers = [];
    }

    public string Name { get; }
    public string NiceName { get; }
    public LogicOptionType Type { get; }
    public string SettingGroup { get; }
    public string SettingPage { get; }
    public string DescriptionText { get; }

    public void NotifyObservers()
    {
        NotifyChildren();
        foreach (var observer in Observers) observer.NotifyObserver();
    }

    public void RegisterObserver(ILogicOptionObserver observer)
    {
        Observers.Add(observer);
    }

    public void ClearObservers()
    {
        Observers.Clear();
    }

    public virtual void NotifyChildren()
    {
    }

    public abstract bool IsReset();

    public abstract void Reset();

    public abstract void CopyValueFrom(LogicOptionBase option);

    public abstract List<LogicDefine> GetLogicDefines();

    public abstract string GetOptions();

    public virtual IEnumerable<byte> GetAdditionalHashBytes()
    {
        return [];
    }

    public abstract byte GetSelectionHashByte();

    public abstract string GetOptionUiType();

    public virtual IList<LogicOptionBase> GetChildren()
    {
        return [];
    }

    public abstract string GetValueAsString();

    public abstract void SetValueFromString(string value);

    protected void UpdateChildren(string[] options)
    {
        var originalSettings = GetChildren();
        for (var i = 0; i < originalSettings.Count; i++)
        {
            originalSettings[i].SetValueFromString(options[i]);
            originalSettings[i].NotifyObservers();
        }
    }

    public object Clone() => MemberwiseClone();
}
