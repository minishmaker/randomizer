using RandomizerCore.Randomizer;

public struct YAMLResult
{
    public OptionList Options;
    public string Name;
    public string Description;

    public YAMLResult(OptionList options, string name, string description)
    {
        Options = options;
        Name = name;
        Description = description;
    }
}
