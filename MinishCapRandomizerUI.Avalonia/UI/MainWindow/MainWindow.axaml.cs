using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Layout;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;
using MinishCapRandomizerUI.Avalonia.Wrappers;
using MinishCapRandomizerUI.Avalonia.UI.Config;
using SkiaSharp;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using Avalonia.Threading;
using System.Diagnostics.CodeAnalysis;
using MinishCapRandomizerUI.Avalonia.Elements;
using Avalonia.Controls.Primitives;
using System.Text.RegularExpressions;
using Avalonia.Input;

namespace MinishCapRandomizerUI.Avalonia;

public partial class MainWindow : Window
{
#pragma warning disable CS8602, CS8604
    [return: NotNull]
    private T FC<T>(string name) where T: Control
    {
        var c = this.FindControl<T>(name);
        if (c != null) return c;
        var desc = this.GetVisualDescendants().OfType<Control>().FirstOrDefault(x=>x.Name==name);
        if (desc is T t) return t;
        throw new Exception($"Control {name} not found");
    }
    private T? TryGet<T>(string name) where T: Control
        => this.GetVisualDescendants().OfType<Control>().FirstOrDefault(x=>x.Name==name) as T;
#pragma warning restore CS8604
#pragma warning restore CS8602

    private UIConfiguration _configuration = new();
    private bool _isApplyPatchMode = true;
    private SettingPresets _settingPresets = new();
    private string? _recentSettingsPreset = null;
    private string? _recentCosmeticsPreset = null;
    private uint _recentSettingsPresetHash;
    private uint _recentCosmeticsPresetHash;
    private string? _outputSettingsString = null;
    private string? _outputCosmeticsString = null;
    private bool _outputUsedYAML = false;
    private string? _outputFilename = null;
    private ControllerBase _previousShuffler = null!;
    private bool _useCompactUI = true;

    private ShufflerController _shufflerController = null!;
    private YamlController _yamlController = null!;

    private string _presetPath = string.Empty;

    private WriteableBitmap? _lastHashBitmap;
    private string _displayedInputSeed = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
        InitializeYamlUi();
        InitializeBaseUi();
        ResolvePresetPath();
        Title = $"{_shufflerController!.AppName} {_shufflerController.VersionName} {_shufflerController.RevName}";
        Closing += (_, e) => SaveConfig();
        LoadConfig();
        this.Opened += OnOpened;
    }

    private bool _uiLoaded;
    private void OnOpened(object? sender, EventArgs e)
    {
        if (_uiLoaded) return;
        _uiLoaded = true;
        Dispatcher.UIThread.Post(() =>
        {
            EnsurePresetDirectories();
            InitializeUi();
            UpdateUIWithLogicOptions();
            WireEvents();
            LoadPresetsWithRetry();
        }, DispatcherPriority.Loaded);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task ShowAlert(string text, string caption)
    {
        await MessageBoxManager.GetMessageBoxStandard(caption, text).ShowWindowDialogAsync(this);
    }

    private async Task<bool> ShowConfirm(string text, string caption)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(caption, text);
        await box.ShowWindowDialogAsync(this);
        return true;
    }

    private void InitializeBaseUi()
    {
        _shufflerController = new ShufflerController();
        _previousShuffler = _shufflerController;
    }

    private void InitializeYamlUi()
    {
        _yamlController = new YamlController();
    }

    private void LoadConfig()
    {
        try
        {
            var path = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory)!, "config.json");
            if (File.Exists(path))
            {
                _configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<UIConfiguration>(File.ReadAllText(path))!;
            }
        }
        catch { _configuration = new UIConfiguration(); }
    }

    private void SaveConfig()
    {
        try
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_configuration, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory)!, "config.json"), json);
        }
        catch { }
    }

    private async void InitializeUi()
    {
        var tab = TryGet<TabControl>("TabPane");
        var seedOutput = TryGet<TabItem>("SeedOutput");
        if (seedOutput != null) seedOutput.IsVisible = false;
        var seed = new SquaresRandomNumberGenerator().Next();
        var seedBox = TryGet<TextBox>("Seed"); if (seedBox != null) seedBox.Text = $"{seed:X}";
        var logicPath = _configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : "";
        var result = _shufflerController.LoadLogicFile(logicPath);
        _yamlController.LoadLogicFile(logicPath);
        if (!result.WasSuccessful && _configuration.UseCustomLogic)
        {
            await ShowAlert("Could not load custom logic file from path in the config file. The file may have been moved or deleted.", "Could Not Load Logic File");
            _shufflerController.LoadLogicFile();
            _yamlController.LoadLogicFile();
        }
        else if (result.WasSuccessful && _configuration.UseCustomLogic)
        {
            var cLogic = TryGet<CheckBox>("UseCustomLogic"); if (cLogic != null) cLogic.IsChecked = _configuration.UseCustomLogic;
            var logicPathBox = TryGet<TextBox>("LogicFilePath"); if (logicPathBox != null) logicPathBox.Text = _configuration.CustomLogicFilepath;
        }
        var browseLogicBtn = TryGet<Button>("BrowseCustomLogicFile"); if (browseLogicBtn != null) browseLogicBtn.IsEnabled = (TryGet<CheckBox>("UseCustomLogic")?.IsChecked == true);
        if (!string.IsNullOrEmpty(_configuration.RomPath))
        {
            result = _shufflerController.LoadRom(_configuration.RomPath);
            if (!result.WasSuccessful)
                await ShowAlert("Could not load ROM from path in the config file. The file may have been moved or deleted.", "Could Not Load ROM");
            else
            { var romPathBox = TryGet<TextBox>("RomPath"); if (romPathBox != null) romPathBox.Text = _configuration.RomPath; }
        }
        var cPatch = TryGet<CheckBox>("UseCustomPatch"); if (cPatch != null) cPatch.IsChecked = _configuration.UseCustomPatch;
        var romBuildBox = TryGet<TextBox>("RomBuildfilePath"); if (romBuildBox != null) romBuildBox.Text = _configuration.CustomPatchFilepath;
        var browsePatchBtn = TryGet<Button>("BrowseCustomPatch"); if (browsePatchBtn != null) browsePatchBtn.IsEnabled = _configuration.UseCustomPatch;
        var cYaml = TryGet<CheckBox>("UseCustomYAML"); if (cYaml != null) cYaml.IsChecked = _configuration.UseCustomYAML;
        var yamlPathBox = TryGet<TextBox>("YAMLPath"); if (yamlPathBox != null) yamlPathBox.Text = _configuration.CustomYAMLFilepath;
        var browseYamlBtn = TryGet<Button>("BrowseCustomYAML"); if (browseYamlBtn != null) browseYamlBtn.IsEnabled = _configuration.UseCustomYAML;
        var compactChk = TryGet<CheckBox>("UseCompactUI"); if (compactChk != null) compactChk.IsChecked = _configuration.UseCompactUIOnStart;
        var compactLabel = TryGet<TextBlock>("CompactUINotSupportedLabel"); if (compactLabel != null) compactLabel.IsVisible = !_shufflerController.IsCompactModeSupported();
        _useCompactUI = (compactChk?.IsChecked == true) && _shufflerController.IsCompactModeSupported();
        _shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);
        if (!string.IsNullOrEmpty(_configuration.DefaultLoggerPath)) _shufflerController.SetLogOutputPath(_configuration.DefaultLoggerPath);
        var attemptsBox = TryGet<TextBox>("RandomizationAttempts"); if (attemptsBox != null) attemptsBox.Text = $"{_configuration.MaximumRandomizationRetryCount}";
        SetMenuCheckVisual("CompactUiDefaultMenu", _configuration.UseCompactUIOnStart);
        SetMenuCheckVisual("LogAllTransactionsMenu", _configuration.UseVerboseLogger);
        SetMenuCheckVisual("CheckForUpdatesOnStartMenu", _configuration.CheckForUpdatesOnStart);
        if (_configuration.CheckForUpdatesOnStart) CheckForUpdatesMenu_Click(this, null!);
    }

    private void SetMenuCheckVisual(string name, bool enabled)
    {
        var item = TryGet<MenuItem>(name);
        if (item == null) return;
        var header = item.Header?.ToString() ?? name;
        var baseHeader = header.Contains('[') ? header.Split('[')[0].TrimEnd() : header;
        item.Header = $"{baseHeader} {(enabled ? "[✓]" : "[ ]")}";
    }

    private void WireEvents()
    {
        void HookBtn(string name, EventHandler<RoutedEventArgs> handler){ var b = TryGet<Button>(name); if (b != null) { b.Click -= handler; b.Click += handler; } }
        void HookChk(string name, EventHandler<RoutedEventArgs> handler){ var c = TryGet<CheckBox>(name); if (c != null) { c.Click -= handler; c.Click += handler; } }
        void HookRadio(string name, Action<RadioButton> onChecked){ var r = TryGet<RadioButton>(name); if (r != null) { r.IsCheckedChanged -= (_, __) => { }; r.IsCheckedChanged += (_, __) => { if (r.IsChecked == true) onChecked(r); }; } }

        HookBtn("BrowseRom", BrowseRom_Click);
        HookBtn("RandomSeed", RandomSeed_Click);
        HookRadio("ApplyPatchButton", _ => ApplyPatchButton_CheckedChanged(_, EventArgs.Empty));
        HookRadio("GeneratePatchButton", _ => GeneratePatchButton_CheckedChanged(_, EventArgs.Empty));
        HookBtn("BrowsePatchOrRom", BrowsePatchOrRom_Click);
        HookBtn("LoadSettings", LoadSettings_Click);
        HookBtn("GenerateSettings", GenerateSettings_Click);
        HookBtn("ResetDefaultSettings", ResetDefaultSettings_Click);
        HookBtn("LoadCosmetics", LoadCosmetics_Click);
        HookBtn("GenerateCosmetics", GenerateCosmetics_Click);
        HookBtn("ResetDefaultCosmetics", ResetDefaultCosmetics_Click);
        HookBtn("LoadSettingPreset", LoadSettingPreset_Click);
        HookBtn("SaveSettingPreset", SaveSettingPreset_Click);
        HookBtn("DeleteSettingPreset", DeleteSettingPreset_Click);
        HookBtn("LoadCosmeticPreset", LoadCosmeticPreset_Click);
        HookBtn("SaveCosmeticPreset", SaveCosmeticPreset_Click);
        HookBtn("DeleteCosmeticPreset", DeleteCosmeticPreset_Click);
        HookBtn("PatchRomAndGenPatch", PatchRomAndGenPatch_Click);
        HookBtn("SaveSpoiler", SaveSpoiler_Click);
        HookBtn("SavePatch", SavePatch_Click);
        HookBtn("SaveRom", SaveRom_Click);
        HookBtn("CopySettingsHashToClipboard", CopySettingsHashToClipboard_Click);
        HookBtn("CopyCosmeticsHashToClipboard", CopyCosmeticsHashToClipboard_Click);
        HookBtn("CopyHashToClipboard", CopyHashToClipboard_Click);
        HookBtn("Randomize", Randomize_Click);

        HookChk("UseSphereBasedShuffler", UseSphereBasedShuffler_CheckedChanged);
        HookChk("UseCustomLogic", UseCustomLogic_CheckedChanged);
        HookChk("UseCustomPatch", UseCustomPatch_CheckedChanged);
        HookChk("UseCustomYAML", UseCustomYAML_CheckedChanged);
        HookChk("UseMysterySettings", UseMysterySettings_CheckedChanged);
        HookChk("UseMysteryCosmetics", UseMysteryCosmetics_CheckedChanged);
        HookChk("UseCompactUI", UseCompactUI_Click);
        HookBtn("BrowseCustomLogicFile", BrowseCustomLogicFile_Click);
        HookBtn("BrowseCustomPatch", BrowseCustomPatch_Click);
        HookBtn("BrowseCustomYAML", BrowseCustomYAML_Click);
        HookBtn("LoadSettingSample", LoadSettingSample_Click);
        HookBtn("LoadCosmeticSample", LoadCosmeticSample_Click);

        var tabs = TryGet<TabControl>("TabPane");
        if (tabs != null)
            tabs.SelectionChanged += (_, __) => {
                Dispatcher.UIThread.Post(() => {
                    WireAdvancedTabEvents();
                    PopulateMysteryWeightCombos();
                    WireSeedOutputTabEvents();
                    var selected = tabs.SelectedItem as TabItem;
                    if ((selected?.Header?.ToString() ?? string.Empty) == "Seed Output")
                        UpdateSeedOutputVisuals();
                }, DispatcherPriority.Loaded);
            };

        WireAdvancedTabEvents();
        PopulateMysteryWeightCombos();
        WireSeedOutputTabEvents();
    }

    private void WireAdvancedTabEvents()
    {
        void HookBtn(string name, EventHandler<RoutedEventArgs> handler){ var b = TryGet<Button>(name); if (b != null) { b.Click -= handler; b.Click += handler; } }
        void HookChk(string name, EventHandler<RoutedEventArgs> handler){ var c = TryGet<CheckBox>(name); if (c != null) { c.Click -= handler; c.Click += handler; } }

        HookBtn("BrowseCustomLogicFile", BrowseCustomLogicFile_Click);
        HookBtn("BrowseCustomPatch", BrowseCustomPatch_Click);
        HookBtn("BrowseCustomYAML", BrowseCustomYAML_Click);
        HookBtn("LoadSettingSample", LoadSettingSample_Click);
        HookBtn("LoadCosmeticSample", LoadCosmeticSample_Click);
        HookChk("UseMysterySettings", UseMysterySettings_CheckedChanged);
        HookChk("UseMysteryCosmetics", UseMysteryCosmetics_CheckedChanged);
        HookChk("UseCustomLogic", UseCustomLogic_CheckedChanged);
        HookChk("UseCustomPatch", UseCustomPatch_CheckedChanged);
        HookChk("UseCustomYAML", UseCustomYAML_CheckedChanged);

        var logicChk = TryGet<CheckBox>("UseCustomLogic");
        var patchChk = TryGet<CheckBox>("UseCustomPatch");
        var yamlChk  = TryGet<CheckBox>("UseCustomYAML");
        var mystSet  = TryGet<CheckBox>("UseMysterySettings");
        var mystCos  = TryGet<CheckBox>("UseMysteryCosmetics");
        if (logicChk != null) { var en = logicChk.IsChecked == true; var btn = TryGet<Button>("BrowseCustomLogicFile"); var tb = TryGet<TextBox>("LogicFilePath"); if (btn!=null) btn.IsEnabled = en; if (tb!=null) tb.IsEnabled = en; }
        if (patchChk != null) { var en = patchChk.IsChecked == true; var btn = TryGet<Button>("BrowseCustomPatch"); var tb = TryGet<TextBox>("RomBuildfilePath"); if (btn!=null) btn.IsEnabled = en; if (tb!=null) tb.IsEnabled = en; }
        if (yamlChk  != null) { var en = yamlChk.IsChecked  == true; var btn = TryGet<Button>("BrowseCustomYAML"); var tb = TryGet<TextBox>("YAMLPath"); if (btn!=null) btn.IsEnabled = en; if (tb!=null) tb.IsEnabled = en; }
        if (mystSet  != null) { var en = mystSet.IsChecked  == true; var cb = TryGet<ComboBox>("SettingsWeights"); var btn = TryGet<Button>("LoadSettingSample"); if (cb!=null) cb.IsEnabled = en; if (btn!=null) btn.IsEnabled = en; }
        if (mystCos  != null) { var en = mystCos.IsChecked  == true; var cb = TryGet<ComboBox>("CosmeticsWeights"); var btn = TryGet<Button>("LoadCosmeticSample"); if (cb!=null) cb.IsEnabled = en; if (btn!=null) btn.IsEnabled = en; }
    }

    private void WireSeedOutputTabEvents()
    {
        void HookBtn(string name, EventHandler<RoutedEventArgs> handler)
        {
            var b = TryGet<Button>(name);
            if (b != null)
            {
                b.Click -= handler;
                b.Click += handler;
            }
        }
        HookBtn("SaveSpoiler", SaveSpoiler_Click);
        HookBtn("SavePatch", SavePatch_Click);
        HookBtn("SaveRom", SaveRom_Click);
        HookBtn("CopySettingsHashToClipboard", CopySettingsHashToClipboard_Click);
        HookBtn("CopyCosmeticsHashToClipboard", CopyCosmeticsHashToClipboard_Click);
        HookBtn("CopyHashToClipboard", CopyHashToClipboard_Click);
    }

    private async Task DisplayOpenDialog(string title, string[] filters, Func<string, Task> onOk)
    {
        var top = TopLevel.GetTopLevel(this);
        var options = new FilePickerOpenOptions { Title = title, AllowMultiple = false };
        if (filters.Length > 0)
        {
            options.FileTypeFilter = filters.Select(f => {
                var parts = f.Split('|');
                var name = parts[0];
                var extsPart = parts.Length > 1 ? parts[1] : string.Empty;
                var exts = extsPart
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(e => e.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(e => e.Trim().Trim('*').Trim('.'))
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Select(e => e == "*" ? "*.*" : ($"*.{e}"))
                    .Distinct()
                    .ToList();
                if (exts.Count == 0) exts.Add("*.*");
                return new FilePickerFileType(name) { Patterns = exts };
            }).ToList();
        }
        var picked = await top!.StorageProvider.OpenFilePickerAsync(options);
        if (picked?.Count > 0)
        {
            var path = picked[0].TryGetLocalPath();
            if (!string.IsNullOrEmpty(path)) await onOk(path!);
        }
    }

    private async Task DisplaySaveDialog(string title, string suggestedFileName, string[] filters, Func<string, Task> onOk)
    {
        var top = TopLevel.GetTopLevel(this);
        var options = new FilePickerSaveOptions { Title = title, SuggestedFileName = suggestedFileName };
        if (filters.Length > 0)
        {
            options.FileTypeChoices = filters.Select(f => {
                var parts = f.Split('|');
                var name = parts[0];
                var extsPart = parts.Length > 1 ? parts[1] : string.Empty;
                var exts = extsPart
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(e => e.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(e => e.Trim().Trim('*').Trim('.'))
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Select(e => e == "*" ? "*.*" : ($"*.{e}"))
                    .Distinct()
                    .ToList();
                if (exts.Count == 0) exts.Add("*.*");
                return new FilePickerFileType(name) { Patterns = exts };
            }).ToList();
        }
        var saved = await top!.StorageProvider.SaveFilePickerAsync(options);
        if (saved != null)
        {
            var path = saved.TryGetLocalPath();
            if (!string.IsNullOrEmpty(path)) await onOk(path!);
        }
    }

    private void UpdateSeedOutputVisuals()
    {
        void safeSet(string name, string text){ var tb = TryGet<TextBlock>(name); if (tb != null) tb.Text = text ?? string.Empty; }
        safeSet("InputSeedLabel", _displayedInputSeed ?? string.Empty);
        safeSet("OutputSeedLabel", $"{_previousShuffler.FinalSeed:X}");
        var settingsString = _previousShuffler.GetFinalSettingsString();
        var cosmeticsString = _previousShuffler.GetFinalCosmeticsString();
        bool IsChecked(string name) => TryGet<CheckBox>(name)?.IsChecked == true;
        _outputUsedYAML = IsChecked("UseMysteryCosmetics") || IsChecked("UseMysterySettings") || IsChecked("UseCustomYAML");
        if (_outputUsedYAML)
        {
            safeSet("SettingNameLabel", _yamlController.IsUsingLogicYaml() ? _yamlController.GetLogicYamlName() : (_recentSettingsPreset != null && _yamlController.GetSelectedOptions().OnlyLogic().GetHash() == _recentSettingsPresetHash ? _recentSettingsPreset : "Custom"));
            safeSet("CosmeticNameLabel", _yamlController.IsUsingCosmeticsYaml() ? _yamlController.GetCosmeticsYamlName() : (_recentCosmeticsPreset != null && _yamlController.GetSelectedOptions().OnlyCosmetic().GetHash() == _recentCosmeticsPresetHash ? _recentCosmeticsPreset : "Custom"));
            _outputSettingsString = settingsString;
            _outputCosmeticsString = cosmeticsString;
            _outputFilename = GetFilenameYamlShuffler();
            safeSet("SettingHashLabel", _yamlController.IsUsingLogicYaml() ? "Settings string is not shown when using mystery settings" : settingsString);
            safeSet("CosmeticStringLabel", _yamlController.IsUsingCosmeticsYaml() ? "Cosmetics string is not shown when using mystery cosmetics" : cosmeticsString);
            var copySet = TryGet<Button>("CopySettingsHashToClipboard"); if (copySet!=null) copySet.IsEnabled = !_yamlController.IsUsingLogicYaml();
            var copyCos = TryGet<Button>("CopyCosmeticsHashToClipboard"); if (copyCos!=null) copyCos.IsEnabled = !_yamlController.IsUsingCosmeticsYaml();
        }
        else
        {
            safeSet("SettingNameLabel", _recentSettingsPreset ?? "Custom");
            safeSet("CosmeticNameLabel", _recentCosmeticsPreset ?? "Custom");
            _outputSettingsString = settingsString;
            _outputCosmeticsString = cosmeticsString;
            _outputFilename = _previousShuffler.SeedFilename;
            safeSet("SettingHashLabel", settingsString);
            safeSet("CosmeticStringLabel", cosmeticsString);
            var copySet = TryGet<Button>("CopySettingsHashToClipboard"); if (copySet!=null) copySet.IsEnabled = true;
            var copyCos = TryGet<Button>("CopyCosmeticsHashToClipboard"); if (copyCos!=null) copyCos.IsEnabled = true;
        }
        BuildSeedHashImage();
    }

    private void BuildSeedHashImage()
    {
        const byte hashMask = 0b111111;
        var panel = TryGet<StackPanel>("RomHashPanel");
        var yamlLabel = TryGet<TextBlock>("YamlHashNotShownLabel");
        if (panel == null) return;

        panel.HorizontalAlignment = HorizontalAlignment.Center;
        panel.VerticalAlignment = VerticalAlignment.Top;

        var useYaml = (TryGet<CheckBox>("UseCustomYAML")?.IsChecked == true) || (TryGet<CheckBox>("UseMysterySettings")?.IsChecked == true) || (TryGet<CheckBox>("UseMysteryCosmetics")?.IsChecked == true);
        if (yamlLabel != null) yamlLabel.IsVisible = useYaml;
        panel.Children.Clear();
        if (useYaml) { _lastHashBitmap = null; return; }
        var eventLines = _previousShuffler.GetEventWrites().Split('\n');
        static bool TryParseDefine(string[] lines, string key, out uint value)
        {
            value = 0;
            var line = lines.FirstOrDefault(l => l.Contains(key));
            if (line == null) return false;
            var parts = line.Split(new[]{'\t',' '}, StringSplitOptions.RemoveEmptyEntries);
            var token = parts.LastOrDefault(p => p.StartsWith("0x") || p.StartsWith("\"0x"));
            if (token == null) return false;
            token = token.Trim('"');
            if (token.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) token = token[2..];
            return uint.TryParse(token, System.Globalization.NumberStyles.HexNumber, null, out value);
        }
        if (!TryParseDefine(eventLines, "seedHashed", out var seed)) return;
        if (!TryParseDefine(eventLines, "settingHash", out var settings)) return;
        uint customRng;
        if (!TryParseDefine(eventLines, "customRNG", out customRng))
        {
            unchecked
            {
                uint a = seed, b = settings;
                uint x = a ^ (b + 0x9E3779B9u + (a << 6) + (a >> 2));
                x ^= 0x85EBCA6Bu;
                x ^= x >> 13;
                x *= 0xC2B2AE35u;
                x ^= x >> 16;
                customRng = x;
            }
        }
        using var resStream = typeof(RandomizerCore.Controllers.ShufflerController).Assembly.GetManifestResourceStream("RandomizerCore.Resources.hashicons.png");
        if (resStream == null) return;
        using var ms = new MemoryStream();
        resStream.CopyTo(ms); ms.Position = 0;
        var src = SkiaSharp.SKBitmap.Decode(ms);
        var targetW = 384; var targetH = 48;
        var wb = new global::Avalonia.Media.Imaging.WriteableBitmap(new global::Avalonia.PixelSize(targetW, targetH), new global::Avalonia.Vector(96,96), global::Avalonia.Platform.PixelFormat.Bgra8888, global::Avalonia.Platform.AlphaFormat.Premul);
        using (var fb = wb.Lock())
        unsafe {
            var ptr = (byte*)fb.Address;
            var bad1 = new SkiaSharp.SKColor(0x30,0xA0,0xAC);
            var bad2 = new SkiaSharp.SKColor(0x30,0xA0,0x78);
            var rep = new SkiaSharp.SKColor(0x08,0x19,0xAD);
            for (int block=0; block<8; ++block){
                uint idx = block switch { 0 => (seed>>24)&hashMask, 1 => (seed>>16)&hashMask, 2 => (seed>>8)&hashMask, 3 => seed & hashMask, 4 => (customRng>>8)&hashMask, 5 => 64U, 6 => (settings>>8)&hashMask, 7 => (settings>>16)&hashMask, _=>0};
                var k = 16*(int)idx; var l = 16*block*3; // 3x horizontal scale per source pixel
                for (int sy=0; sy<16; ++sy){
                    for (int sx=0; sx<16; ++sx){
                        var c = src.GetPixel(sx, sy+k);
                        if ((c.Red==bad1.Red && c.Green==bad1.Green && c.Blue==bad1.Blue) || (c.Red==bad2.Red && c.Green==bad2.Green && c.Blue==bad2.Blue)) c = rep;
                        for (int dy=0; dy<3; ++dy){
                            for (int dx=0; dx<3; ++dx){
                                int x = sx*3 + dx + l; int y = sy*3 + dy; if (x>=targetW|| y>=targetH) continue; var off=(y*targetW + x)*4; ptr[off]=c.Blue; ptr[off+1]=c.Green; ptr[off+2]=c.Red; ptr[off+3]=c.Alpha;
                            }
                        }
                    }
                }
            }
        }
        panel.Children.Add(new Image{ Source = wb, Width = targetW, Height = targetH, Stretch = Stretch.None });
        _lastHashBitmap = wb;
    }

    private void ResolvePresetPath()
    {
        var baseDir = Path.GetDirectoryName(AppContext.BaseDirectory)!;
        var candidates = new List<string>();
        candidates.Add(Path.Combine(baseDir, "Resources", "Presets"));
        candidates.Add(Path.Combine(baseDir, "Presets"));

        var dir = new DirectoryInfo(baseDir);
        for (int i = 0; i < 8 && dir != null; i++)
        {
            candidates.Add(Path.Combine(dir.FullName, "Resources", "Presets"));
            candidates.Add(Path.Combine(dir.FullName, "MinishCapRandomizerUI.Avalonia", "Resources", "Presets"));
            dir = dir.Parent;
        }

        string? chosen = candidates
            .Distinct()
            .FirstOrDefault(c => Directory.Exists(c) && Directory.GetFiles(c, "*.yaml", SearchOption.AllDirectories).Any());

        if (string.IsNullOrEmpty(chosen))
            chosen = Path.Combine(baseDir, "Resources", "Presets");

        _presetPath = chosen.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
    }

    private void EnsurePresetDirectories()
    {
        var usingResources = _presetPath.Contains($"{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}Presets{Path.DirectorySeparatorChar}");
        string[] dirs = { "Settings", "Cosmetics", "Mystery Settings", "Mystery Cosmetics" };
        foreach (var d in dirs)
        {
            var path = Path.Combine(_presetPath, d);
            if (!Directory.Exists(path) && !usingResources) Directory.CreateDirectory(path);
        }
    }

    private async void BrowseRom_Click(object? sender, RoutedEventArgs e)
    {
        await DisplayOpenDialog("Select TMC ROM", new[] { "GBA ROMs|*.gba", "All Files|*.*" }, async filename =>
        {
            var result = _shufflerController.LoadRom(filename);
            if (result.WasSuccessful)
            {
                FC<TextBox>("RomPath").Text = filename;
                _configuration.RomPath = filename;
            }
            else
            {
                await ShowAlert("Failed to load ROM!", "Failed to Load ROM");
            }
        });
    }

    private void RandomSeed_Click(object? sender, RoutedEventArgs e)
    {
        var seed = new SquaresRandomNumberGenerator().Next();
        FC<TextBox>("Seed").Text = $"{seed:X}";
        _shufflerController.SetRandomizationSeed(seed);
        _yamlController.SetRandomizationSeed(seed);
    }

    private void ApplyPatchButton_CheckedChanged(object? sender, EventArgs e)
    {
        _isApplyPatchMode = true;
        var label = FC<TextBlock>("BpsPatchAndPatchedRomLabel");
        var button = FC<Button>("PatchRomAndGenPatch");
        FC<TextBox>("BpsPatchAndPatchedRomPath").Text = string.Empty;
        label.Text = "BPS Patch File Path:";
        button.Content = "Patch Rom";
    }

    private void GeneratePatchButton_CheckedChanged(object? sender, EventArgs e)
    {
        _isApplyPatchMode = false;
        var label = FC<TextBlock>("BpsPatchAndPatchedRomLabel");
        var button = FC<Button>("PatchRomAndGenPatch");
        FC<TextBox>("BpsPatchAndPatchedRomPath").Text = string.Empty;
        label.Text = "Patched ROM Path:";
        button.Content = "Generate BPS Patch";
    }

    private async void BrowsePatchOrRom_Click(object? sender, RoutedEventArgs e)
    {
        if (_isApplyPatchMode)
            await DisplayOpenDialog("BPS Patch", new[] { "BPS Patch|*.bps", "All Files|*.*" }, async f => { FC<TextBox>("BpsPatchAndPatchedRomPath").Text = f; await Task.CompletedTask; });
        else
            await DisplayOpenDialog("Patched ROM", new[] { "GBA ROMs|*.gba", "All Files|*.*" }, async f => { FC<TextBox>("BpsPatchAndPatchedRomPath").Text = f; await Task.CompletedTask; });
    }

    private async void BrowseCustomLogicFile_Click(object? sender, RoutedEventArgs e)
    {
        await DisplayOpenDialog("Select Logic File", new[]{"Logic Files|*.logic;*.txt","All Files|*.*"}, async path =>
        {
            var result = _shufflerController.LoadLogicFile(path);
            _yamlController.LoadLogicFile(path);
            if (result.WasSuccessful)
            {
                FC<TextBox>("LogicFilePath").Text = path;
                _configuration.CustomLogicFilepath = path;
                FC<TextBlock>("CompactUINotSupportedLabel").IsVisible = !_shufflerController.IsCompactModeSupported();
                _useCompactUI = FC<CheckBox>("UseCompactUI").IsChecked == true && _shufflerController.IsCompactModeSupported();
                UpdateUIWithLogicOptions();
            }
            else
            {
                await ShowAlert("Failed to load logic file!","Load Logic Failed");
            }
        });
    }

    private async void BrowseCustomPatch_Click(object? sender, RoutedEventArgs e)
    {
        await DisplayOpenDialog("Select ROM Buildfile", new[]{"Build Files|*.txt;*.patch;*.yaml;*.yml","All Files|*.*"}, async path =>
        {
            FC<TextBox>("RomBuildfilePath").Text = path;
            _configuration.CustomPatchFilepath = path;
            await Task.CompletedTask;
        });
    }

    private async void BrowseCustomYAML_Click(object? sender, RoutedEventArgs e)
    {
        await DisplayOpenDialog("Select Global YAML File", new[]{"YAML Files|*.yaml;*.yml","All Files|*.*"}, async path =>
        {
            FC<TextBox>("YAMLPath").Text = path;
            _configuration.CustomYAMLFilepath = path;
            await Task.CompletedTask;
        });
    }

    private async void LoadSettingSample_Click(object? sender, RoutedEventArgs e)
    {
        var weightsBox = TryGet<ComboBox>("SettingsWeights");
        if (weightsBox?.SelectedItem is string name && _settingPresets.SettingsWeights.Any(p=>p.PresetName==name))
        {
            var info = _settingPresets.SettingsWeights.First(p=>p.PresetName==name);
            var yamlPath = Path.Combine(_presetPath, "Mystery Settings", info.Filename + ".yaml");
            var load = _shufflerController.LoadLogicSettingsFromYaml(yamlPath);
            if (load.WasSuccessful)
            {
                UpdateUIWithLogicOptions();
                await ShowAlert("Sample settings loaded from weights.","Sample Settings Loaded");
            }
            else
            {
                await ShowAlert(load.ErrorMessage ?? "Failed to load mystery settings weights.","Sample Load Failed");
            }
        }
        else
        {
            await ShowAlert("No mystery settings weights selected.","Sample Load Failed");
        }
    }

    private async void LoadCosmeticSample_Click(object? sender, RoutedEventArgs e)
    {
        var weightsBox = TryGet<ComboBox>("CosmeticsWeights");
        if (weightsBox?.SelectedItem is string name && _settingPresets.CosmeticsWeights.Any(p=>p.PresetName==name))
        {
            var info = _settingPresets.CosmeticsWeights.First(p=>p.PresetName==name);
            var yamlPath = Path.Combine(_presetPath, "Mystery Cosmetics", info.Filename + ".yaml");
            var load = _shufflerController.LoadCosmeticsFromYaml(yamlPath);
            if (load.WasSuccessful)
            {
                UpdateUIWithLogicOptions();
                await ShowAlert("Sample cosmetics loaded from weights.","Sample Cosmetics Loaded");
            }
            else
            {
                await ShowAlert(load.ErrorMessage ?? "Failed to load mystery cosmetics weights.","Sample Load Failed");
            }
        }
        else
        {
            await ShowAlert("No mystery cosmetics weights selected.","Sample Load Failed");
        }
    }

    private void PopulateMysteryWeightCombos()
    {
        void FillBox(ComboBox? box, IEnumerable<string> names)
        {
            if (box == null) return;
            if (box.ItemsSource == null)
                box.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<string>(names);
            else
            {
                var coll = box.ItemsSource as System.Collections.ObjectModel.ObservableCollection<string>;
                if (coll != null)
                {
                    foreach (var n in names)
                        if (!coll.Contains(n)) coll.Add(n);
                }
            }
            if (box.SelectedIndex < 0 && box.ItemCount > 0) box.SelectedIndex = 0;
        }
        FillBox(TryGet<ComboBox>("SettingsWeights"), _settingPresets?.SettingsWeights?.Select(p=>p.PresetName) ?? Array.Empty<string>());
        FillBox(TryGet<ComboBox>("CosmeticsWeights"), _settingPresets?.CosmeticsWeights?.Select(p=>p.PresetName) ?? Array.Empty<string>());
    }

    private bool _presetsLoaded;
    private int _presetLoadAttempts;
    private void LoadPresetsWithRetry()
    {
        try
        {
            if (!_presetsLoaded)
            {
                LoadPresets();
                _presetsLoaded = true;
            }
            ApplyPresetComboBoxes();
        }
        finally
        {
            if (_presetLoadAttempts < 20)
            {
                _presetLoadAttempts++;
                Dispatcher.UIThread.Post(LoadPresetsWithRetry, DispatcherPriority.Background);
            }
        }
    }

    private void LoadPresets()
    {
        _settingPresets = new SettingPresets
        {
            SettingsPresets = ScanPresetList("Settings"),
            CosmeticsPresets = ScanPresetList("Cosmetics"),
            SettingsWeights = ScanPresetList("Mystery Settings"),
            CosmeticsWeights = ScanPresetList("Mystery Cosmetics"),
        };
    }

    private void ApplyPresetComboBoxes()
    {
        void FillBox(ComboBox? box, IEnumerable<string> names)
        {
            if (box == null) return;
            if (box.ItemsSource == null)
                box.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<string>(names);
            else
            {
                var coll = box.ItemsSource as System.Collections.ObjectModel.ObservableCollection<string>;
                if (coll != null)
                {
                    foreach (var n in names)
                        if (!coll.Contains(n)) coll.Add(n);
                }
            }
            if (box.SelectedIndex < 0 && box.ItemCount > 0) box.SelectedIndex = 0;
        }
        FillBox(TryGet<ComboBox>("SettingPresets"), _settingPresets.SettingsPresets.Select(p=>p.PresetName));
        FillBox(TryGet<ComboBox>("CosmeticsPresets"), _settingPresets.CosmeticsPresets.Select(p=>p.PresetName));
        FillBox(TryGet<ComboBox>("SettingsWeights"), _settingPresets.SettingsWeights.Select(p=>p.PresetName));
        FillBox(TryGet<ComboBox>("CosmeticsWeights"), _settingPresets.CosmeticsWeights.Select(p=>p.PresetName));
    }

    private List<PresetFileInfo> ScanPresetList(string directory)
    {
        var dir = Path.Combine(_presetPath, directory);
        if (!Directory.Exists(dir)) return new List<PresetFileInfo>();
        var files = Directory.GetFiles(dir).Where(file => file.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase));
        var presetFiles = files.Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
        var presets = new List<PresetFileInfo>(presetFiles.Count);
        foreach (var filename in presetFiles)
        {
            PresetFileInfo? info = null;
            if (filename.Contains('_'))
            {
                var lastIndex = filename.LastIndexOf('_');
                if (int.TryParse(filename[(lastIndex + 1)..], out var priority))
                {
                    info = new PresetFileInfo { Filename = filename, PresetName = filename[..lastIndex], SortIndex = priority };
                }
            }
            info ??= new PresetFileInfo { Filename = filename, PresetName = filename, SortIndex = int.MaxValue };
            if (!presets.Any(preset => preset.PresetName == info.PresetName)) presets.Add(info);
        }
        presets.Sort((a, b) => a.SortIndex - b.SortIndex);
        return presets;
    }

    private static void AddItemToPresetsBox(ComboBox box, string presetName)
    {
        if (box == null) return;
        if (box.ItemsSource == null)
            box.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<string>();
        var coll = box.ItemsSource as System.Collections.ObjectModel.ObservableCollection<string>; if (coll == null) return;
        if (!coll.Contains(presetName)) coll.Add(presetName);
        if (box.SelectedIndex < 0) box.SelectedIndex = 0;
    }

    private static void RemoveItemFromPresetsBox(ComboBox box, string presetName)
    {
        var coll = box.ItemsSource as System.Collections.ObjectModel.ObservableCollection<string>; if (coll == null) return;
        var wasSelected = (box.SelectedItem as string) == presetName;
        coll.Remove(presetName);
        if (wasSelected && coll.Count > 0) box.SelectedIndex = 0;
    }

    private async Task<string> PromptForPresetName(string title)
    {
        var dialog = new MinishCapRandomizerUI.Avalonia.UI.InputDialog.InputDialog();
        dialog.Setup(title, "Enter a preset name:");
        var result = await dialog.ShowDialogAsync(this);
        if (string.IsNullOrWhiteSpace(result))
            return $"Preset_{DateTime.Now:yyyyMMdd_HHmmss}";
        return result.Trim();
    }

    private void Randomize_Click(object? sender, RoutedEventArgs e)
    {
        bool isChecked(string n) => (TryGet<CheckBox>(n)?.IsChecked ?? false) == true;
        if (isChecked("UseCustomYAML") || isChecked("UseMysterySettings") || isChecked("UseMysteryCosmetics"))
            RandomizeWithYamlShuffler();
        else
            RandomizeWithBaseShuffler();
    }

    private void UseSphereBasedShuffler_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        _configuration.UseHendrusShuffler = FC<CheckBox>("UseSphereBasedShuffler").IsChecked == true;
    }

    private void UseCustomLogic_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        var isChecked = FC<CheckBox>("UseCustomLogic").IsChecked == true;
        FC<Button>("BrowseCustomLogicFile").IsEnabled = isChecked;
        FC<TextBox>("LogicFilePath").IsEnabled = isChecked;
        _configuration.UseCustomLogic = isChecked;
        if (!isChecked)
        {
            _shufflerController.LoadLogicFile();
            _yamlController.LoadLogicFile();
        }
        else if (!string.IsNullOrEmpty(FC<TextBox>("LogicFilePath").Text))
        {
            var path = FC<TextBox>("LogicFilePath").Text;
            _shufflerController.LoadLogicFile(path);
            _yamlController.LoadLogicFile(path);
        }
        FC<TextBlock>("CompactUINotSupportedLabel").IsVisible = !_shufflerController.IsCompactModeSupported();
        _useCompactUI = FC<CheckBox>("UseCompactUI").IsChecked == true && _shufflerController.IsCompactModeSupported();
        UpdateUIWithLogicOptions();
    }

    private void UseCustomPatch_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        var isChecked = FC<CheckBox>("UseCustomPatch").IsChecked == true;
        FC<Button>("BrowseCustomPatch").IsEnabled = isChecked;
        FC<TextBox>("RomBuildfilePath").IsEnabled = isChecked;
        _configuration.UseCustomPatch = isChecked;
    }

    private void UseCustomYAML_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        var isChecked = FC<CheckBox>("UseCustomYAML").IsChecked == true;
        FC<Button>("BrowseCustomYAML").IsEnabled = isChecked;
        FC<TextBox>("YAMLPath").IsEnabled = isChecked;
        _configuration.UseCustomYAML = isChecked;
    }

    private void UseMysterySettings_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        var enabled = FC<CheckBox>("UseMysterySettings").IsChecked == true;
        FC<ComboBox>("SettingsWeights").IsEnabled = enabled;
        FC<Button>("LoadSettingSample").IsEnabled = enabled;
        _configuration.UseCustomYAML = _configuration.UseCustomYAML || enabled;
        PopulateMysteryWeightCombos();
    }

    private void UseMysteryCosmetics_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        var enabled = FC<CheckBox>("UseMysteryCosmetics").IsChecked == true;
        FC<ComboBox>("CosmeticsWeights").IsEnabled = enabled;
        FC<Button>("LoadCosmeticSample").IsEnabled = enabled;
        _configuration.UseCustomYAML = _configuration.UseCustomYAML || enabled;
        PopulateMysteryWeightCombos();
    }

    private void UseCompactUI_Click(object? sender, RoutedEventArgs e)
    {
        var wantCompact = FC<CheckBox>("UseCompactUI").IsChecked == true;
        if (wantCompact && !_shufflerController.IsCompactModeSupported())
        {
            FC<TextBlock>("CompactUINotSupportedLabel").IsVisible = true;
            FC<CheckBox>("UseCompactUI").IsChecked = false;
            _useCompactUI = false;
            return;
        }
        _useCompactUI = wantCompact;
        UpdateUIWithLogicOptions();
    }

    private async void RandomizeWithBaseShuffler()
    {
        _previousShuffler = _shufflerController;
        _displayedInputSeed = TryGet<TextBox>("Seed")?.Text ?? string.Empty;

        // Parse seed from UI like WinForms
        if (!ulong.TryParse((_displayedInputSeed ?? string.Empty).Trim(), System.Globalization.NumberStyles.HexNumber, null, out var parsedSeed))
        {
            await ShowAlert("Invalid Seed Provided!", "Invalid Seed");
            return;
        }
        _shufflerController.SetRandomizationSeed(parsedSeed);
        _yamlController.SetRandomizationSeed(parsedSeed);

        var retries = Math.Max(1, _configuration.MaximumRandomizationRetryCount);
        var useSphere = _configuration.UseHendrusShuffler;

        // Load locations BEFORE randomizing to populate event defines for patching
        var logicPath = _configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : string.Empty;
        var load = _shufflerController.LoadLocations(logicPath);
        if (!load.WasSuccessful)
        {
            await ShowAlert(load.ErrorMessage ?? "Failed to parse logic!", "Failed to Parse Logic");
            return;
        }

        var result = _shufflerController.Randomize(retries, useSphere);
        if (!result.WasSuccessful)
        {
            await ShowAlert(result.ErrorMessage ?? "Randomization failed.", "Randomization Failed");
            return;
        }
        DisplayAndUpdateSeedInfoPage();
    }

    private string GetFilenameYamlShuffler()
    {
        var seed = _shufflerController.FinalSeed;
        var logicName = _yamlController.IsUsingLogicYaml() ? _yamlController.GetLogicYamlName() : (_recentSettingsPreset ?? "Custom");
        var cosName = _yamlController.IsUsingCosmeticsYaml() ? _yamlController.GetCosmeticsYamlName() : (_recentCosmeticsPreset ?? "Custom");
        return $"Minish Randomizer-{seed:X}-{RandomizerCore.Controllers.Models.ControllerBase.VersionIdentifier}-{logicName}-{cosName}";
    }

    private async void RandomizeWithYamlShuffler()
    {
        _previousShuffler = _yamlController;
        _displayedInputSeed = TryGet<TextBox>("Seed")?.Text ?? string.Empty;

        // Parse seed and set on both controllers
        if (!ulong.TryParse((_displayedInputSeed ?? string.Empty).Trim(), System.Globalization.NumberStyles.HexNumber, null, out var parsedSeed))
        {
            await ShowAlert("Invalid Seed Provided!", "Invalid Seed");
            return;
        }
        _shufflerController.SetRandomizationSeed(parsedSeed);
        _yamlController.SetRandomizationSeed(parsedSeed);

        var logicPath = _configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : string.Empty;
        var useGlobal = (_configuration.UseCustomYAML);
        string? logicYaml = null, cosmeticsYaml = null;
        if ((TryGet<CheckBox>("UseMysterySettings")?.IsChecked ?? false) == true)
        {
            if (TryGet<ComboBox>("SettingsWeights")?.SelectedItem is string name)
            {
                var info = _settingPresets.SettingsWeights.FirstOrDefault(p=>p.PresetName==name);
                if (info != null) logicYaml = Path.Combine(_presetPath, "Mystery Settings", info.Filename + ".yaml");
            }
        }
        if ((TryGet<CheckBox>("UseMysteryCosmetics")?.IsChecked ?? false) == true)
        {
            if (TryGet<ComboBox>("CosmeticsWeights")?.SelectedItem is string name)
            {
                var info = _settingPresets.CosmeticsWeights.FirstOrDefault(p=>p.PresetName==name);
                if (info != null) cosmeticsYaml = Path.Combine(_presetPath, "Mystery Cosmetics", info.Filename + ".yaml");
            }
        }
        var load = _yamlController.LoadLocations(logicPath, logicYaml, cosmeticsYaml, useGlobal);
        if (!load.WasSuccessful)
        {
            await ShowAlert(load.ErrorMessage ?? "Failed to load YAML.", "YAML Error");
            return;
        }
        var retries = Math.Max(1, _configuration.MaximumRandomizationRetryCount);
        var useSphere = _configuration.UseHendrusShuffler;
        var result = _yamlController.Randomize(retries, useSphere);
        if (!result.WasSuccessful)
        {
            await ShowAlert(result.ErrorMessage ?? "Randomization failed.", "Randomization Failed");
            return;
        }
        DisplayAndUpdateSeedInfoPage();
    }

    private void SwitchToNormalMode()
    {
        _useCompactUI = false;
        var chk = TryGet<CheckBox>("UseCompactUI"); if (chk != null) chk.IsChecked = false;
        UpdateUIWithLogicOptions();
    }

    private void UpdateUIWithLogicOptions()
    {
        var tab = this.FindControl<TabControl>("TabPane"); if (tab == null) return;
        var map = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase){
            ["Main Settings"]="Main Settings",
            ["World Settings"]="World Settings",
            ["Logic"]="Logic Settings",
            ["Logic Settings"]="Logic Settings",
            ["Item Pool"]="Item Pool",
            ["Start Inventory"]="Start Inventory",
            ["Gameplay"]="Gameplay",
            ["Cosmetics"]="Cosmetics",
            ["Advanced"]="Advanced"};

        // Tabs whose content is static XAML and must not be cleared/overwritten here
        var staticTabs = new HashSet<string>(StringComparer.OrdinalIgnoreCase){"General","Advanced","Seed Output"};

        var tabItemsByHeader = tab.Items!.OfType<TabItem>()
            .ToDictionary(t => t.Header?.ToString() ?? string.Empty, t => t, StringComparer.OrdinalIgnoreCase);

        // Hide and clear only dynamic tabs (non-static) before repopulating
        foreach (var header in map.Values.Distinct())
        {
            if (!tabItemsByHeader.TryGetValue(header, out var tItem)) continue;
            if (staticTabs.Contains(header)) continue; // don’t touch static tabs
            tItem.IsVisible = false;
            tItem.Content = null;
        }

        var options = _useCompactUI ? _shufflerController.GetCompactOptions() : _shufflerController.GetSelectedOptions();
        var wrapped = MinishCapRandomizerUI.Avalonia.Elements.WrappedLogicOptionFactory.BuildGenericWrappedLogicOptions(options);

        // Build and show only the dynamic tabs that exist for the current logic; never overwrite static tabs
        foreach (var pageGroup in wrapped.GroupBy(w=>w.Page??string.Empty))
        {
            if (!map.TryGetValue(pageGroup.Key, out var header)) continue;
            if (staticTabs.Contains(header)) continue; // skip Advanced/General/Seed Output
            if (!tabItemsByHeader.TryGetValue(header, out var tabItem)) continue;

            var stack = new StackPanel{Spacing=6,Margin=new Thickness(6)};
            foreach(var group in pageGroup.GroupBy(w=>w.SettingGrouping).Where(g=>!string.IsNullOrWhiteSpace(g.Key)))
                stack.Children.Add(MinishCapRandomizerUI.Avalonia.Elements.WrappedLogicOptionFactory.BuildGroupContainer(group.Key!, group));

            tabItem.Content = new ScrollViewer{ Content = stack };
            tabItem.IsVisible = true;
        }

        // Ensure static tabs remain visible
        foreach (var header in staticTabs)
            if (tabItemsByHeader.TryGetValue(header, out var t)) t.IsVisible = true;

        // If the currently selected tab became hidden, move selection to the first visible tab
        var selected = tab.SelectedItem as TabItem;
        if (selected != null && selected.IsVisible == false)
        {
            var firstVisible = tab.Items!.OfType<TabItem>().FirstOrDefault(t => t.IsVisible);
            if (firstVisible != null)
                tab.SelectedItem = firstVisible;
        }
    }

    private void DisplayAndUpdateSeedInfoPage()
    {
        var tab = FC<TabControl>("TabPane");
        var seedOutput = FC<TabItem>("SeedOutput");
        seedOutput.IsVisible = true;
        tab.SelectedItem = seedOutput;
        Dispatcher.UIThread.Post(() => {
            WireSeedOutputTabEvents();
            UpdateSeedOutputVisuals();
        }, DispatcherPriority.Loaded);
    }

    private async void LoadSettingPreset_Click(object? sender, RoutedEventArgs e)
    {
        var presets = _settingPresets.SettingsPresets;
        var sel = TryGet<ComboBox>("SettingPresets")?.SelectedItem as string;
        if (string.IsNullOrEmpty(sel) || presets.All(p => p.PresetName != sel))
        {
            await ShowAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset");
            return;
        }
        var filename = presets.First(p => p.PresetName == sel).Filename;
        var result = _shufflerController.LoadLogicSettingsFromYaml(Path.Combine(_presetPath, "Settings", filename + ".yaml"));
        var successMessage = "Settings loaded successfully!";
        if (result.WasSuccessful)
        {
            _recentSettingsPreset = sel;
            _recentSettingsPresetHash = _shufflerController.GetSelectedOptions().OnlyLogic().GetHash();
            if (_useCompactUI)
            {
                var isCompatible = _shufflerController.UpdateCompactOptionValues(false);
                if (!isCompatible)
                {
                    SwitchToNormalMode();
                    successMessage += "\nSettings UI was switched to normal mode to support all the options from the settings preset.";
                }
                else _shufflerController.UpdateCompactOptionValues(true);
            }
            await ShowAlert(successMessage, "Settings Loaded");
        }
        else
        {
            await ShowAlert(result.ErrorMessage ?? "Failed to load Settings preset!", "Failed to Load Settings");
        }
    }

    private bool IsValidPresetName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim() != name) return false;
        var match = Regex.Match(name, "[\\x00-\\x1F<>:\"/\\\\|?*]");
        return !match.Success;
    }

    private async void SaveSettingPreset_Click(object? sender, RoutedEventArgs e)
    {
        var name = await PromptForPresetName("Enter Setting Preset Name");
        name = name.Trim();
        var presets = _settingPresets.SettingsPresets;
        if (!IsValidPresetName(name)) { await ShowAlert("Preset name not valid!", "Failed to Save Preset"); return; }
        if (presets.Any(p => string.Equals(p.PresetName, name, StringComparison.CurrentCultureIgnoreCase)))
        { await ShowAlert("A setting preset with the specified name already exists!", "Failed to Save Preset"); return; }
        var max = presets.Count == 0 ? 0 : presets.Select(p => p.SortIndex).Max();
        var updatedName = name + (max == int.MaxValue ? "" : $"_{max + 1}");
        var result = _shufflerController.SaveSettingsAsYaml(Path.Combine(_presetPath, "Settings", updatedName + ".yaml"), name, _shufflerController.GetSelectedOptions().OnlyLogic());
        if (result.WasSuccessful)
        {
            presets.Add(new PresetFileInfo { Filename = updatedName, PresetName = name, SortIndex = max == int.MaxValue ? max : (max + 1) });
            AddItemToPresetsBox(FC<ComboBox>("SettingPresets"), name);
            _recentSettingsPreset = name;
            _recentSettingsPresetHash = _shufflerController.GetSelectedOptions().OnlyLogic().GetHash();
            await ShowAlert("Preset saved successfully!", "Preset Saved");
        }
        else
        {
            await ShowAlert("An error occurred while trying to save the preset!", "Failed to Save Preset");
        }
    }

    private async void DeleteSettingPreset_Click(object? sender, RoutedEventArgs e)
    {
        var box = FC<ComboBox>("SettingPresets");
        var sel = box.SelectedItem as string;
        if (string.IsNullOrEmpty(sel)) { await ShowAlert("Select a preset to delete.", "Delete Preset"); return; }
        var ok = await ShowConfirm($"Are you sure you wish to delete preset \"{sel}\"? This action cannot be undone!", "Delete Preset");
        if (!ok) return;
        var presets = _settingPresets.SettingsPresets;
        if (presets.All(p => p.PresetName != sel)) { await ShowAlert("No preset matching the specified name could be found!", "Failed to Delete Preset"); return; }
        try
        {
            var info = presets.First(p => p.PresetName == sel);
            presets.Remove(info);
            File.Delete(Path.Combine(_presetPath, "Settings", info.Filename + ".yaml"));
            RemoveItemFromPresetsBox(box, sel);
            await ShowAlert("Preset deleted successfully!", "Preset deleted");
        }
        catch
        {
            await ShowAlert("An error occurred while trying to delete the preset!", "Failed to delete Preset");
        }
    }

    private async void LoadCosmeticPreset_Click(object? sender, RoutedEventArgs e)
    {
        var presets = _settingPresets.CosmeticsPresets;
        var sel = TryGet<ComboBox>("CosmeticsPresets")?.SelectedItem as string;
        if (string.IsNullOrEmpty(sel) || presets.All(p => p.PresetName != sel))
        {
            await ShowAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset");
            return;
        }
        var filename = presets.First(p => p.PresetName == sel).Filename;
        var result = _shufflerController.LoadCosmeticsFromYaml(Path.Combine(_presetPath, "Cosmetics", filename + ".yaml"));
        var successMessage = "Cosmetics loaded successfully!";
        if (result.WasSuccessful)
        {
            _recentCosmeticsPreset = sel;
            _recentCosmeticsPresetHash = _shufflerController.GetSelectedOptions().OnlyCosmetic().GetHash();
            if (_useCompactUI)
            {
                var isCompatible = _shufflerController.UpdateCompactOptionValues(false);
                if (!isCompatible)
                {
                    SwitchToNormalMode();
                    successMessage += "\nSettings UI was switched to normal mode mode to support all the options from the cosmetics preset.";
                }
                else _shufflerController.UpdateCompactOptionValues(true);
            }
            await ShowAlert(successMessage, "Cosmetics Loaded");
        }
        else
        {
            await ShowAlert(result.ErrorMessage ?? "Failed to load cosmetics preset!", "Failed to Load Cosmetics");
        }
    }

    private async void SaveCosmeticPreset_Click(object? sender, RoutedEventArgs e)
    {
        var name = await PromptForPresetName("Enter Cosmetic Preset Name");
        name = name.Trim();
        var presets = _settingPresets.CosmeticsPresets;
        if (!IsValidPresetName(name)) { await ShowAlert("Preset name not valid!", "Failed to Save Preset"); return; }
        if (presets.Any(p => string.Equals(p.PresetName, name, StringComparison.CurrentCultureIgnoreCase)))
        { await ShowAlert("A cosmetic preset with the specified name already exists!", "Failed to Save Preset"); return; }
        var max = presets.Count == 0 ? 0 : presets.Select(p => p.SortIndex).Max();
        var updatedName = name + (max == int.MaxValue ? "" : $"_{max + 1}");
        var result = _shufflerController.SaveSettingsAsYaml(Path.Combine(_presetPath, "Cosmetics", updatedName + ".yaml"), name, _shufflerController.GetSelectedOptions().OnlyCosmetic());
        if (result.WasSuccessful)
        {
            presets.Add(new PresetFileInfo { Filename = updatedName, PresetName = name, SortIndex = max == int.MaxValue ? max : (max + 1) });
            AddItemToPresetsBox(FC<ComboBox>("CosmeticsPresets"), name);
            _recentCosmeticsPreset = name;
            _recentCosmeticsPresetHash = _shufflerController.GetSelectedOptions().OnlyCosmetic().GetHash();
            await ShowAlert("Preset saved successfully!", "Preset Saved");
        }
        else
        {
            await ShowAlert("An error occurred while trying to save the preset!", "Failed to Save Preset");
        }
    }

    private async void DeleteCosmeticPreset_Click(object? sender, RoutedEventArgs e)
    {
        var box = FC<ComboBox>("CosmeticsPresets");
        var sel = box.SelectedItem as string;
        if (string.IsNullOrEmpty(sel)) { await ShowAlert("Select a preset to delete.", "Delete Preset"); return; }
        var ok = await ShowConfirm($"Are you sure you wish to delete preset \"{sel}\"? This action cannot be undone!", "Delete Preset");
        if (!ok) return;
        var presets = _settingPresets.CosmeticsPresets;
        if (presets.All(p => p.PresetName != sel)) { await ShowAlert("No preset matching the specified name could be found!", "Failed to Delete Preset"); return; }
        try
        {
            var info = presets.First(p => p.PresetName == sel);
            presets.Remove(info);
            File.Delete(Path.Combine(_presetPath, "Cosmetics", info.Filename + ".yaml"));
            RemoveItemFromPresetsBox(box, sel);
            await ShowAlert("Preset deleted successfully!", "Preset deleted");
        }
        catch
        {
            await ShowAlert("An error occurred while trying to delete the preset!", "Failed to delete Preset");
        }
    }

    private async void LoadSettings_Click(object? sender, RoutedEventArgs e)
    {
        var successMessage = "Settings loaded successfully!";
        var result = _shufflerController.LoadSettingsFromSettingString(FC<TextBox>("SettingString").Text ?? string.Empty);
        if (result.WasSuccessful && _useCompactUI)
        {
            var isCompatible = _shufflerController.UpdateCompactOptionValues(false);
            if (!isCompatible)
            {
                SwitchToNormalMode();
                successMessage += "\nSettings UI was switched to normal mode to support all the options from the settings string.";
            }
            else _shufflerController.UpdateCompactOptionValues(true);
        }
        await ShowAlert(result.WasSuccessful ? successMessage : (result.ErrorMessage ?? "Failed to load Settings string!"), result.WasSuccessful ? "Settings Loaded" : "Failed to Load Settings");
    }

    private void GenerateSettings_Click(object? sender, RoutedEventArgs e)
    {
        var box = FC<TextBox>("SettingString");
        box.Text = _shufflerController.GetSelectedSettingsString();
        box.Focus();
        box.SelectionStart = 0; box.SelectionEnd = box.Text?.Length ?? 0;
    }

    private async void ResetDefaultSettings_Click(object? sender, RoutedEventArgs e)
    {
        var ok = await ShowConfirm("Are you sure you wish to load default settings?", "Load Default Settings");
        if (!ok) return;
        var settings = (_useCompactUI ? _shufflerController.GetCompactOptions() : _shufflerController.GetSelectedOptions()).OnlyLogic();
        foreach (var setting in settings)
        {
            setting.Reset();
            setting.NotifyObservers();
        }
    }

    private async void LoadCosmetics_Click(object? sender, RoutedEventArgs e)
    {
        var successMessage = "Cosmetics loaded successfully!";
        var result = _shufflerController.LoadCosmeticsFromCosmeticsString(FC<TextBox>("CosmeticsString").Text ?? string.Empty);
        if (result.WasSuccessful && _useCompactUI)
        {
            var isCompatible = _shufflerController.UpdateCompactOptionValues(false);
            if (!isCompatible)
            {
                SwitchToNormalMode();
                successMessage += "\nSettings UI was switched to normal mode to support all the options from the cosmetics string.";
            }
            else _shufflerController.UpdateCompactOptionValues(true);
        }
        await ShowAlert(result.WasSuccessful ? successMessage : (result.ErrorMessage ?? "Failed to load cosmetics string!"), result.WasSuccessful ? "Cosmetics Loaded" : "Failed to Load Cosmetics");
    }

    private void GenerateCosmetics_Click(object? sender, RoutedEventArgs e)
    {
        var box = FC<TextBox>("CosmeticsString");
        box.Text = _shufflerController.GetSelectedCosmeticsString();
        box.Focus();
        box.SelectionStart = 0; box.SelectionEnd = box.Text?.Length ?? 0;
    }

    private async void ResetDefaultCosmetics_Click(object? sender, RoutedEventArgs e)
    {
        var ok = await ShowConfirm("Are you sure you wish to load default cosmetics?", "Load Default Cosmetics");
        if (!ok) return;
        var settings = (_useCompactUI ? _shufflerController.GetCompactOptions() : _shufflerController.GetSelectedOptions()).OnlyCosmetic();
        foreach (var setting in settings)
        {
            setting.Reset();
            setting.NotifyObservers();
        }
    }

    private async void PatchRomAndGenPatch_Click(object? sender, RoutedEventArgs e)
    {
        const string applyModeAlertText = "You must select a patch file before patching the ROM!";
        const string generateModeAlertText = "You must select a patched ROM before generating a patch!";
        const string applyModeAlertTitle = "Missing BPS Patch";
        const string generateModeAlertTitle = "Missing Patched ROM";
        const string applyModeSuccessText = "ROM Patched Successfully!";
        const string generateModeSuccessText = "Patch Generated Successfully!";
        const string applyModeSuccessTitle = "ROM Generated";
        const string generateModeSuccessTitle = "Patch Generated";
        const string generateModeFilter = "BPS Patch|*.bps|All Files|*.*";
        const string applyModeFilter = "GBA ROMs|*.gba|All Files|*.*";
        const string generateModeTitle = "BPS Patch";
        const string applyModeTitle = "Patched ROM";
        const string generateModeFilename = "Patch.bps";
        const string applyModeFilename = "Patched ROM.gba";

        var pathBox = FC<TextBox>("BpsPatchAndPatchedRomPath");
        if (string.IsNullOrEmpty(pathBox.Text))
        {
            await ShowAlert(_isApplyPatchMode ? applyModeAlertText : generateModeAlertText, _isApplyPatchMode ? applyModeAlertTitle : generateModeAlertTitle);
            return;
        }
        await DisplaySaveDialog(_isApplyPatchMode ? applyModeTitle : generateModeTitle, _isApplyPatchMode ? applyModeFilename : generateModeFilename, new[] { _isApplyPatchMode ? applyModeFilter : generateModeFilter }, async filename =>
        {
            var result = _isApplyPatchMode
                ? _shufflerController.PatchRom(filename, pathBox.Text)
                : _shufflerController.SaveRomPatch(filename, pathBox.Text);
            await ShowAlert(result.WasSuccessful ? (_isApplyPatchMode ? applyModeSuccessText : generateModeSuccessText) : (result.ErrorMessage ?? (_isApplyPatchMode ? "Failed to patch ROM!" : "Failed to generate patch!")), result.WasSuccessful ? (_isApplyPatchMode ? applyModeSuccessTitle : generateModeSuccessTitle) : (_isApplyPatchMode ? "ROM Patch Failed" : "Patch Generation Failed"));
        });
    }

    private async void SaveSpoiler_Click(object? sender, RoutedEventArgs e)
    {
        var fname = ($"{_outputFilename ?? _previousShuffler.SeedFilename}-Spoiler.txt").Replace('/', '_');
        await DisplaySaveDialog("Save Spoiler Log", fname, new[] { "Text File|*.txt", "All Files|*.*" }, async filename =>
        {
            var result = _previousShuffler.SaveSpoiler(filename);
            await ShowAlert(result.WasSuccessful ? "Spoiler Saved Successfully!" : (result.ErrorMessage ?? "Failed to save spoiler!"), result.WasSuccessful ? "Spoiler Saved" : "Spoiler Save Failed");
        });
    }

    private async void SavePatch_Click(object? sender, RoutedEventArgs e)
    {
        var fname = ($"{_outputFilename ?? _previousShuffler.SeedFilename}-Patch.bps").Replace('/', '_');
        await DisplaySaveDialog("Save Patch", fname, new[] { "BPS Patch|*.bps", "All Files|*.*" }, async filename =>
        {
            string? patchPath = null;
            if (_configuration.UseCustomPatch)
            {
                var pp = _configuration.CustomPatchFilepath;
                if (string.IsNullOrWhiteSpace(pp) || !File.Exists(pp))
                {
                    await ShowAlert("Custom patch file not found. Please select a valid patch buildfile.", "Patch File Missing");
                    return;
                }
                patchPath = pp;
            }
            var result = _previousShuffler.CreatePatch(filename, patchPath);
            await ShowAlert(result.WasSuccessful ? "Patch Saved Successfully!" : (result.ErrorMessage ?? "Failed to save patch!"), result.WasSuccessful ? "Patch Saved" : "Patch Save Failed");
        });
    }

    private async void SaveRom_Click(object? sender, RoutedEventArgs e)
    {
        var fname = ($"{_outputFilename ?? _previousShuffler.SeedFilename}-ROM.gba").Replace('/', '_');
        await DisplaySaveDialog("Save ROM", fname, new[] { "GBA ROM|*.gba", "All Files|*.*" }, async filename =>
        {
            string? patchPath = null;
            if (_configuration.UseCustomPatch)
            {
                var pp = _configuration.CustomPatchFilepath;
                if (string.IsNullOrWhiteSpace(pp) || !File.Exists(pp))
                {
                    await ShowAlert("Custom patch file not found. Please select a valid patch buildfile.", "Patch File Missing");
                    return;
                }
                patchPath = pp;
            }
            var result = _previousShuffler.SaveAndPatchRom(filename, patchPath);
            await ShowAlert(result.WasSuccessful ? "ROM Saved Successfully!" : (result.ErrorMessage ?? "Failed to save ROM!"), result.WasSuccessful ? "ROM Saved" : "ROM Save Failed");
        });
    }

    private async void CopySettingsHashToClipboard_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_outputSettingsString)) return;
        var top = TopLevel.GetTopLevel(this);
        if (top?.Clipboard != null)
            await top.Clipboard.SetTextAsync(_outputSettingsString);
    }

    private async void CopyCosmeticsHashToClipboard_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_outputCosmeticsString)) return;
        var top = TopLevel.GetTopLevel(this);
        if (top?.Clipboard != null)
            await top.Clipboard.SetTextAsync(_outputCosmeticsString);
    }

    private async void CopyHashToClipboard_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var lines = _previousShuffler.GetEventWrites().Split('\n');
            static bool TryParseDefine(string[] lines, string key, out uint value)
            {
                value = 0;
                var line = lines.FirstOrDefault(l => l.Contains(key));
                if (line == null) return false;
                var parts = line.Split(new[]{'\t',' '}, StringSplitOptions.RemoveEmptyEntries);
                var token = parts.LastOrDefault(p => p.StartsWith("0x") || p.StartsWith("\"0x"));
                if (token == null) return false;
                token = token.Trim('"');
                if (token.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) token = token[2..];
                return uint.TryParse(token, System.Globalization.NumberStyles.HexNumber, null, out value);
            }
            if (!TryParseDefine(lines, "seedHashed", out var seed) || !TryParseDefine(lines, "settingHash", out var settings))
                return;
            uint customRng;
            if (!TryParseDefine(lines, "customRNG", out customRng))
            {
                unchecked
                {
                    uint a = seed, b = settings;
                    uint x = a ^ (b + 0x9E3779B9u + (a << 6) + (a >> 2));
                    x ^= 0x85EBCA6Bu;
                    x ^= x >> 13;
                    x *= 0xC2B2AE35u;
                    x ^= x >> 16;
                    customRng = x;
                }
            }
            byte mask = 0b111111;
            var indices = new List<uint>{
                (seed>>24)&mask,
                (seed>>16)&mask,
                (seed>>8)&mask,
                seed & mask,
                (customRng>>8)&mask,
                64,
                (settings>>8)&mask,
                (settings>>16)&mask
            };
            var text = string.Join("-", indices.Select(i => i.ToString("D2")));
            var top = TopLevel.GetTopLevel(this);
            if (top?.Clipboard != null)
                await top.Clipboard.SetTextAsync(text);
        }
        catch { }
    }

    private async void CheckForUpdatesMenu_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("MinishCapRandomizerUI.Avalonia");
            var resp = await http.GetAsync("https://api.github.com/repositories/177660043/releases");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            var arr = JArray.Parse(json);
            if (arr.Count == 0)
            {
                await ShowAlert("No releases found.", "Check for Updates");
                return;
            }
            var latest = arr[0];
            var htmlUrl = latest.Value<string>("html_url") ?? "";
            if (string.IsNullOrEmpty(htmlUrl)) { await ShowAlert("Could not parse release URL.", "Check for Updates"); return; }
            var dlg = new MinishCapRandomizerUI.Avalonia.UI.UrlDialog.UrlDialog();
            dlg.Setup(htmlUrl, "A new release may be available. Click link to open in browser.");
            await dlg.ShowDialog(this);
        }
        catch (Exception ex)
        {
            await ShowAlert($"Update check failed: {ex.Message}", "Update Check Failed");
        }
    }

    private async void ExportDefaultLogicMenu_Click(object? sender, RoutedEventArgs e)
    {
        await ShowAlert("Export Default Logic is not implemented in this build.", "Not Implemented");
    }

    private async void SavePresetYamlMenu_Click(object? sender, RoutedEventArgs e)
    {
        await ShowAlert("Saving selected options as a YAML preset from the menu is not implemented yet. Use the Presets section on the General tab.", "Not Implemented");
    }

    private async void SaveMysteryYamlMenu_Click(object? sender, RoutedEventArgs e)
    {
        await ShowAlert("Saving a Mystery YAML template is not implemented in this build.", "Not Implemented");
    }

    private void CompactUiDefaultMenu_Click(object? sender, RoutedEventArgs e)
    {
        _configuration.UseCompactUIOnStart = !_configuration.UseCompactUIOnStart;
        SetMenuCheckVisual("CompactUiDefaultMenu", _configuration.UseCompactUIOnStart);
    }

    private async void SetLoggerOutputPathMenu_Click(object? sender, RoutedEventArgs e)
    {
        await DisplaySaveDialog("Select Logger Output File", "randomizer.log", new[]{"Log file|*.log;*.txt","All Files|*.*"}, async path => {
            _configuration.DefaultLoggerPath = path;
            _shufflerController.SetLogOutputPath(path);
            await ShowAlert($"Logger output path set to:\n{path}", "Logger Path");
        });
    }

    private void LogAllTransactionsMenu_Click(object? sender, RoutedEventArgs e)
    {
        _configuration.UseVerboseLogger = !_configuration.UseVerboseLogger;
        _shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);
        SetMenuCheckVisual("LogAllTransactionsMenu", _configuration.UseVerboseLogger);
    }

    private async void WriteAndFlushLoggerMenu_Click(object? sender, RoutedEventArgs e)
    {
        var msg = _shufflerController.PublishLogs();
        await ShowAlert(msg, "Logger");
    }

    private async void AboutMenu_Click(object? sender, RoutedEventArgs e)
    {
        var about = new MinishCapRandomizerUI.Avalonia.UI.About.AboutWindow();
        await about.ShowDialog(this);
    }

    private void CheckForUpdatesOnStartMenu_Click(object? sender, RoutedEventArgs e)
    {
        _configuration.CheckForUpdatesOnStart = !_configuration.CheckForUpdatesOnStart;
        SetMenuCheckVisual("CheckForUpdatesOnStartMenu", _configuration.CheckForUpdatesOnStart);
    }

    private async void EnglishMenu_Click(object? sender, RoutedEventArgs e)
    { await ShowAlert("Localization switching is not implemented.", "Localization"); }
    private async void FrenchMenu_Click(object? sender, RoutedEventArgs e)
    { await ShowAlert("Localization switching is not implemented.", "Localization"); }
    private async void GermanMenu_Click(object? sender, RoutedEventArgs e)
    { await ShowAlert("Localization switching is not implemented.", "Localization"); }
    private async void SpanishMenu_Click(object? sender, RoutedEventArgs e)
    { await ShowAlert("Localization switching is not implemented.", "Localization"); }
    private async void ItalianMenu_Click(object? sender, RoutedEventArgs e)
    { await ShowAlert("Localization switching is not implemented.", "Localization"); }
}
