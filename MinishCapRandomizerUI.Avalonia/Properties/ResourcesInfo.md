# Properties Folder (Parity Placeholder)

WinForms used `Properties/Resources.resx` and Designer for strongly-typed resources.
Avalonia embeds resources directly via `AvaloniaResource` in the csproj. This placeholder documents parity.

Potential future work:
- Add localization `.axaml` or `.resx` conversions
- Centralize resource URIs or theme assets.
namespace MinishCapRandomizerUI.Avalonia.Github;

// Ported from WinForms version for parity (used for GitHub release deserialization if needed)
public class Release
{
    public string? Html_Url { get; set; }
    public string? Tag_Name { get; set; }
    public string? Name { get; set; }
    public string? Body { get; set; }
}

