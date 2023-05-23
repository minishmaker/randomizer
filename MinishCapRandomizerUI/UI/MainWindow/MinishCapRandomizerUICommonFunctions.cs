using System.Globalization;
using System.Net;
using System.Reflection;
using MinishCapRandomizerUI.DrawConstants;
using MinishCapRandomizerUI.Elements;
using MinishCapRandomizerUI.UI.Config;
using MinishCapRandomizerUI.UI.MainWindow.Helpers;
using Newtonsoft.Json;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Exceptions;

namespace MinishCapRandomizerUI.UI.MainWindow;

/// <summary>
/// This is just here so you can actually edit this file in the code editor without going through a bunch of designer crap
/// </summary>
public class MinishCapRandomizerUIHelpers
{

}

/// <summary>
/// Useful functions for UI generation, updates, and displaying dialogues.
/// </summary>
partial class MinishCapRandomizerUI
{
    /// <summary>
    /// Displays an alert and runs the specified callback if not null. If callbackExecuteOnResult is null and a callback
    /// is supplied, then the callback will be executed no matter what.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    /// <param name="buttons"></param>
    /// <param name="icon"></param>
    /// <param name="callbackExecuteOnResult"></param>
    /// <param name="callback"></param>
    private static void DisplayAlert(
        string text,
        string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        DialogResult? callbackExecuteOnResult = null,
        Action? callback = null)
    {
        var result = MessageBox.Show(text, caption, buttons, icon);

        if ((callbackExecuteOnResult == null && callback != null) ||
            (callbackExecuteOnResult != null && callback != null && callbackExecuteOnResult == result))
        {
            callback.Invoke();
        }
    }

    private static void DisplayOpenDialog(string filter, string title, DialogResult expectedResult, Action<string> callback)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = filter,
            Title = title,
        };

        var result = openDialog.ShowDialog();
        if (result != expectedResult) return;

        callback.Invoke(openDialog.FileName);
    }

    private static void DisplaySaveDialog(string filter, string title, string filename, DialogResult expectedResult,
        Action<string> callback)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = filter,
            Title = title,
            FileName = filename,
        };

        var result = saveFileDialog.ShowDialog();

        if (result != expectedResult) return;

        callback.Invoke(saveFileDialog.FileName);
    }

    private static void ShowInputDialog(string title, string message, Action<string> callback)
    {
        var inputDialog = new InputDialog.InputDialog();
        inputDialog.Initialize(title, message, callback);
        inputDialog.ShowDialog();
    }

    private static void DisplayConditionalAlertFromShufflerResult(ShufflerControllerResult result, string successMessage, string successTitle, string startOfErrorMessage, string errorTitle)
    {
        if (result.WasSuccessful)
        {
            DisplayAlert(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (result.Error != null)
        {
            switch (result.Error)
            {
                case ParserException:
                    DisplayAlert($"{startOfErrorMessage}\nFailed to parse!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ShuffleException:
                    DisplayAlert($"{startOfErrorMessage}\nFailed to shuffle ROM!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ShufflerConfigurationException:
                    DisplayAlert($"{startOfErrorMessage}\nShuffler is in an invalid state!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    DisplayAlert($"{startOfErrorMessage}\nCheck the logs for more information.", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            return;
        }

        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
            DisplayAlert($"{startOfErrorMessage}\nError: {result.ErrorMessage}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }


    private void LoadTooltips()
    {
        var tip = new ToolTip();
        tip.UseFading = true;
        tip.AutoPopDelay = Constants.TooltipDisplayLengthMs;
        tip.InitialDelay = Constants.TooltipInitialShowDelayMs;
        tip.ReshowDelay = Constants.TooltipRepeatDelayMs;
        tip.SetToolTip(UseCustomCosmetics, "Patches in cosmetics based on what you have selected in the cosmetics tab.\nOnly works if both logic and patches are set to default.");
        tip.SetToolTip(UseSphereBasedShuffler, @"Generates the seed using the sphere based shuffler. This shuffler is guaranteed to generate a seed that can beat vaati on the first attempt unless it is impossible to get out of the first sphere or takes more than 200,000 attempts to generate the seed.

This shuffler gets the locations in sphere 1, fills them, checks for progression, advances to the next sphere, etc. until it can beat vaati. 
If no progression is available, it goes to the previous sphere and re-places items there to try and change what is processed the next time.

This shuffler is known for creating very linear and plando-esque seeds, use at your own risk. We have cases where it has generated 30+ sphere seeds when the normal shuffler generated 12-15 sphere seeds.

Generating seeds with this shuffler may freeze the randomizer application for many minutes while it tries different permutations, however if it exceeds 200,000 attempts it will break out and error. If this happens, let us know so we can make improvements to the shuffler.");
    }

    private void LoadPresets()
    {
        try
        {
            _settingPresets = new SettingPresets();

            var files = Directory.GetFiles($"{_presetPath}Settings").Where(file => file.EndsWith(".yaml", true, null));
            var basePresetArray = files.Select(file => file[(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)..]).Select(file => file[..file.LastIndexOf(".", StringComparison.Ordinal)]).ToList();
            
            _settingPresets.SettingsPresets = basePresetArray.ToDictionary(preset => int.Parse(preset[(preset.LastIndexOf('_') + 1)..]), EqualityComparer<int>.Default);

            for (var i = 1; i <= _settingPresets.SettingsPresets.Keys.Max(); ++i)
            {
                if (!_settingPresets.SettingsPresets.TryGetValue(i, out var preset)) continue;

                SettingPresets.Items.Add(preset[..preset.LastIndexOf('_')]);
            }
            
            if (SettingPresets.Items.Count != 0) SettingPresets.SelectedIndex = 0;

            files = Directory.GetFiles($"{_presetPath}Cosmetics").Where(file => file.EndsWith(".yaml", true, null));
            _settingPresets.CosmeticsPresets = files.Select(file => file[(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)..]).Select(file => file[..file.LastIndexOf(".", StringComparison.Ordinal)]).ToList();

            CosmeticsPresets.Items.AddRange(_settingPresets.CosmeticsPresets.ToArray());
            if (CosmeticsPresets.Items.Count != 0) CosmeticsPresets.SelectedIndex = 0;

            files = Directory.GetFiles($"{_presetPath}Mystery Settings").Where(file => file.EndsWith(".yaml", true, null));
            _settingPresets.SettingsWeights = files.Select(file => file[(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)..]).Select(file => file[..file.LastIndexOf(".", StringComparison.Ordinal)]).ToList();

            SettingsWeights.Items.AddRange(_settingPresets.SettingsWeights.ToArray());
            if (SettingsWeights.Items.Count != 0) SettingsWeights.SelectedIndex = 0;

            files = Directory.GetFiles($"{_presetPath}Mystery Cosmetics").Where(file => file.EndsWith(".yaml", true, null));
            _settingPresets.CosmeticsWeights = files.Select(file => file[(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)..]).Select(file => file[..file.LastIndexOf(".", StringComparison.Ordinal)]).ToList();

            CosmeticsWeights.Items.AddRange(_settingPresets.CosmeticsWeights.ToArray());
            if (CosmeticsWeights.Items.Count != 0) CosmeticsWeights.SelectedIndex = 0;
        }
        catch
        {
            DisplayAlert("Could not load settings presets. The preset directories may have been moved or deleted.", "Could Not Load Settings Presets", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _settingPresets = new SettingPresets();
        }
    }

    private void LoadConfig()
    {
        try
        {
            var configAsJson = File.ReadAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/config.json");
            _configuration = JsonConvert.DeserializeObject<UIConfiguration>(configAsJson)!;
            if (_configuration == null)
                _configuration = new UIConfiguration
                {
                    RomPath = "",
                    UseHendrusShuffler = false,
                    MaximumRandomizationRetryCount = 1,
                    DefaultLoggerPath = "",
                    UseVerboseLogger = false,
                    UseCustomLogic = false,
                    CustomLogicFilepath = "",
                    UseCustomPatch = false,
                    CustomPatchFilepath = "",
                };
        }
        catch
        {
            _configuration = new UIConfiguration
            {
                RomPath = "",
                UseHendrusShuffler = false,
                MaximumRandomizationRetryCount = 1,
                DefaultLoggerPath = "",
                UseVerboseLogger = false,
                UseCustomLogic = false,
                CustomLogicFilepath = "",
                UseCustomPatch = false,
                CustomPatchFilepath = "",
            };
        }
    }

    private void SaveConfig(object? sender, FormClosingEventArgs e)
    {
        try
        {
            var configAsJson = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
            File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/config.json", configAsJson);
        }
        catch { }
    }

    private static bool CheckForUpdates()
    {
        try
        {
            var githubData = DownloadUrlAsString();
            var releases = JsonConvert.DeserializeObject<List<Github.Release>>(githubData);
            var tag = Assembly.GetExecutingAssembly().GetCustomAttributesData().First(x => x.AttributeType.Name == "AssemblyInformationalVersionAttribute").ConstructorArguments.First().ToString();
            tag = tag[(tag.IndexOf('+') + 1)..^1];
            if (releases!.First().Tag_Name == tag) return false;

            var url = new UrlDialog.UrlDialog(releases!.First().Html_Url);
            url.ShowDialog();
            return true;
        } 
        catch
        {
            DisplayAlert("Update check failed! Server did not respond!", "Failed to Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }
        
    private static string DownloadUrlAsString()
    {
        var request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repositories/177660043/releases");
        request.UserAgent = "MinishCapRandomizerUI";
        request.KeepAlive = false;
        using var response = (HttpWebResponse)request.GetResponse();
        using var responseStream = new StreamReader(response.GetResponseStream());
        return responseStream.ReadToEnd();
    }

    private void InitializeUi()
    {
        TabPane.TabPages.Remove(SeedOutput);
        var seed = new SquaresRandomNumberGenerator().Next();
        Seed.Text = $@"{seed:X}";

        var result = _shufflerController.LoadLogicFile(_configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : "");
        _yamlController.LoadLogicFile(_configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : "");
        if (!result.WasSuccessful)
        {
            DisplayAlert("Could not load custom logic file from path in the config file. The file may have been moved or deleted.", "Could Not Load Logic File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _shufflerController.LoadLogicFile();
            _yamlController.LoadLogicFile();
        }
        else
        {
            UseCustomLogic.Checked = _configuration.UseCustomLogic;
            LogicFilePath.Text = _configuration.CustomLogicFilepath;
        }
        BrowseCustomLogicFile.Enabled = UseCustomLogic.Checked;

        if (!string.IsNullOrEmpty(_configuration.RomPath))
        {
            result = _shufflerController.LoadRom(_configuration.RomPath);
            if (!result.WasSuccessful)
                DisplayAlert("Could not load ROM from path in the config file. The file may have been moved or deleted.", "Could Not Load ROM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                RomPath.Text = _configuration.RomPath;
        }

        UseCustomPatch.Checked = _configuration.UseCustomPatch;
        RomBuildfilePath.Text = _configuration.CustomPatchFilepath;
        BrowseCustomPatch.Enabled = UseCustomPatch.Checked;

        UseCustomYAML.Checked = _configuration.UseCustomYAML;
        YAMLPath.Text = _configuration.CustomYAMLFilepath;
        BrowseCustomYAML.Enabled = UseCustomYAML.Checked;

        logAllTransactionsToolStripMenuItem.Checked = _configuration.UseVerboseLogger;
        _shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);

        if (!string.IsNullOrEmpty(_configuration.DefaultLoggerPath))
            _shufflerController.SetLogOutputPath(_configuration.DefaultLoggerPath);

        RandomizationAttempts.Text = $"{_configuration.MaximumRandomizationRetryCount}";
        UseSphereBasedShuffler.Checked = _configuration.UseHendrusShuffler;

        _defaultSettings = _shufflerController.GetSelectedSettingsString();
        _defaultCosmetics = _shufflerController.GetSelectedCosmeticsString();

        _shufflerController.SetRandomizationSeed(seed);
        _yamlController.SetRandomizationSeed(seed);

        checkForUpdatesOnStartToolStripMenuItem.Checked = _configuration.CheckForUpdatesOnStart;
        if (_configuration.CheckForUpdatesOnStart)
            CheckForUpdates();
    }

    private void UpdateUIWithLogicOptions()
    {
        for (var i = TabPane.TabPages.Count - 1; i >= 2; --i)
            TabPane.TabPages.RemoveAt(i);

        if (_randomizedRomCreated)
            TabPane.TabPages.Add(SeedOutput);

        var options = _shufflerController.GetSelectedOptions();
        var wrappedOptions = WrappedLogicOptionFactory.BuildGenericWrappedLogicOptions(options);
        var pages = wrappedOptions.GroupBy(option => option.Page);

        foreach (var page in pages)
        {
            TabPane.TabPages.Add(UIGenerator.BuildSettingsPage(page.ToList(), page.Key));
        }
    }

    private void DisplayAndUpdateSeedInfoPage()
    {
        if (!TabPane.TabPages.Contains(SeedOutput))
            TabPane.TabPages.Insert(2, SeedOutput);

        InputSeedLabel.Text = Seed.Text;
        OutputSeedLabel.Text = @$"{_shufflerController.FinalSeed:X}";

        var settingsString = _previousShuffler.GetFinalSettingsString();
        var cosmeticsString = _previousShuffler.GetFinalCosmeticsString();

        if (UseMysteryCosmetics.Checked || UseMysterySettings.Checked || UseCustomYAML.Checked)
            UpdateSeedInfoPageYaml(settingsString, cosmeticsString);
        else
            UpdateSeedInfoPageBase(settingsString, cosmeticsString);
            
        DisplaySeedHash();

        TabPane.SelectedTab = SeedOutput;
    }

    private void DisplaySeedHash()
    {
        const byte hashMask = 0b111111;

        RomHashPanel.Visible = true;
        YamlHashNotShownLabel.Visible = false;

        RomHashPanel.Controls.Clear();

        var badBgColor = Color.FromArgb(0x30, 0xa0, 0xac).ToArgb();
        var otherBadBgColor = Color.FromArgb(0x30, 0xa0, 0x78).ToArgb();
        var newBgColor = Color.FromArgb(0x08, 0x19, 0xad);
        var eventDefines = (UseCustomYAML.Checked || UseMysterySettings.Checked || UseMysteryCosmetics.Checked) ? _yamlController.GetEventWrites().Split('\n') : _shufflerController.GetEventWrites().Split('\n');
        var customRng = uint.Parse(eventDefines.First(line => line.Contains("customRNG")).Split(' ')[2][2..], 
            NumberStyles.HexNumber);
        var seed = uint.Parse(eventDefines.First(line => line.Contains("seedHashed")).Split(' ')[2][2..],
            NumberStyles.HexNumber);
        var settings = uint.Parse(eventDefines.First(line => line.Contains("settingHash")).Split(' ')[2][2..],
            NumberStyles.HexNumber);

        using var stream = Assembly.GetAssembly(typeof(ShufflerController))?.GetManifestResourceStream("RandomizerCore.Resources.hashicons.png");
        var hashImageList = new Bitmap(stream);

        var image = new PictureBox
        {
            Width = 384,
            Height = 48,
            Margin = Padding.Empty
        };
        var hashImage = new Bitmap(384, 48);
            
        for (var imageNum = 0; imageNum < 8; ++imageNum)
        {

            var imageIndex = imageNum switch
            {
                0 => (seed >> 24) & hashMask,
                1 => (seed >> 16) & hashMask,
                2 => (seed >> 8) & hashMask,
                3 => seed & hashMask,
                4 => (customRng >> 8) & hashMask,
                5 => 64U,
                6 => (settings >> 8) & hashMask,
                7 => (settings >> 16) & hashMask
            };

            var k = 16 * (int)imageIndex;
            var l = 16 * imageNum;
            for (var i = 0; i < 16; ++i)
            {
                var i3 = i * 3;
                for (var j = 0; j < 16; ++j)
                {
                    var color = hashImageList.GetPixel(j, i + k);
                    var argb = color.ToArgb();
                    if (argb == badBgColor || argb == otherBadBgColor)
                        color = newBgColor;
                    AddColorToImage3X(hashImage, color, j + l, i3);
                    AddColorToImage3X(hashImage, color, j + l, i3 + 1);
                    AddColorToImage3X(hashImage, color, j + l, i3 + 2);
                }
            }
        }

        image.Image = hashImage;
        RomHashPanel.Controls.Add(image);
    }

    private static void AddColorToImage3X(Bitmap baseImage, Color targetColor, int x, int y)
    {
        x *= 3;
        baseImage.SetPixel(x, y, targetColor);
        baseImage.SetPixel(x + 1, y, targetColor);
        baseImage.SetPixel(x + 2, y, targetColor);
    }

    private string GetSeedFilename()
    {
        if (UseMysteryCosmetics.Checked || UseMysterySettings.Checked || UseCustomYAML.Checked)
            return GetFilenameYamlShuffler();
        return GetFilenameBaseShuffler();
    }

    private static void AddItemToPresetsBox(ComboBox box, string presetName)
    {
        box.Items.Add(presetName);
        if (box.Items.Count == 1) 
            box.SelectedIndex = 0;
    }

    private static void RemoveItemFromPresetsBox(ComboBox box, string presetName)
    {
        var isRemovedPresetSelected = (string)box.SelectedItem == presetName;

        box.Items.Remove(presetName);
        box.Text = "";

        if (isRemovedPresetSelected)
        {
            var newIndex = box.SelectedIndex - 1 >= 0 ? box.SelectedIndex - 1 : 0;

            if (box.Items.Count > 0)
            {
                box.SelectedIndex = newIndex;
            }
        }
    }
}
