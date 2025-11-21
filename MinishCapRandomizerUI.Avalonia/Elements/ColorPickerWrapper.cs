using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using RandomizerCore.Randomizer.Logic.Options;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public class ColorPickerWrapper : WrapperBase, ILogicOptionObserver
{
    private const int NameTextWidth = 125;
    private const int CheckboxWidth = 125;
    private const int PreviewTextWidth = 55;
    private const int ButtonWidth = 100;
    private const int PictureBoxWidth = 60;
    private const int Height = 23;
    private const string CheckboxText = "Use Random Color";
    private const string SelectColorText = "Select Color";
    private const string SelectRandomColorText = "Pick Random";
    private const string UseDefaultColorText = "Use Default";
    private const string ColorPreviewText = "Preview:";

    private static readonly int ElementWidthInternal = CheckboxWidth + NameTextWidth + PreviewTextWidth + PictureBoxWidth + 3 * ButtonWidth + 7 * Constants.WidthMargin;

    private readonly LogicColorPicker _picker;

    private TextBlock? _nameLabel;
    private CheckBox? _useRandomCheckbox;
    private Button? _selectColorButton;
    private Button? _selectRandomColorButton;
    private Button? _useDefaultColorButton;
    private TextBlock? _previewLabel;
    private Border? _colorPreview;

    public ColorPickerWrapper(LogicColorPicker picker) : base(ElementWidthInternal, Height, picker.SettingGroup, picker.SettingPage)
    {
        _picker = picker;
        _picker.RegisterObserver(this);
    }

    public override IList<Control> BuildControls(int initialX, int initialY)
    {
        if (_nameLabel != null && _useRandomCheckbox != null && _selectColorButton != null && _selectRandomColorButton != null &&
            _useDefaultColorButton != null && _previewLabel != null && _colorPreview != null)
        {
            return new List<Control>{ _nameLabel, _useRandomCheckbox, _selectColorButton, _selectRandomColorButton, _useDefaultColorButton, _previewLabel, _colorPreview };
        }

        _nameLabel = new TextBlock
        {
            Text = _picker.NiceName + ":",
            VerticalAlignment = VerticalAlignment.Center,
            Width = NameTextWidth
        };
        ToolTip.SetTip(_nameLabel, _picker.DescriptionText);

        _useRandomCheckbox = new CheckBox
        {
            Content = CheckboxText,
            IsChecked = _picker.UseRandomColor,
            Width = CheckboxWidth,
            VerticalAlignment = VerticalAlignment.Center
        };
        ToolTip.SetTip(_useRandomCheckbox, "If enabled, a random color will be selected on seed generation");
        _useRandomCheckbox.IsCheckedChanged += (_, __) =>
        {
            _picker.UseRandomColor = _useRandomCheckbox.IsChecked == true;
            UpdatePreviewColor();
            UpdateButtonEnablement();
            _picker.NotifyChildren();
        };

        _selectColorButton = new Button
        {
            Content = SelectColorText,
            Width = ButtonWidth
        };
        ToolTip.SetTip(_selectColorButton, "Opens the system color picker dialog");
        _selectColorButton.Click += async (_, __) => await OpenSystemColorDialogAsync();

        _selectRandomColorButton = new Button
        {
            Content = SelectRandomColorText,
            Width = ButtonWidth
        };
        ToolTip.SetTip(_selectRandomColorButton, "A random color is selected now and shown in the preview");
        _selectRandomColorButton.Click += (_, __) =>
        {
            _picker.PickRandomColor();
            UpdatePreviewColor();
            _picker.NotifyChildren();
        };

        _useDefaultColorButton = new Button
        {
            Content = UseDefaultColorText,
            Width = ButtonWidth
        };
        ToolTip.SetTip(_useDefaultColorButton, "Resets the selected color back to its default");
        _useDefaultColorButton.Click += (_, __) =>
        {
            _picker.DefinedColor = _picker.BaseColor;
            UpdatePreviewColor();
            _picker.NotifyChildren();
        };

        _previewLabel = new TextBlock
        {
            Text = ColorPreviewText,
            VerticalAlignment = VerticalAlignment.Center,
            Width = PreviewTextWidth
        };

        _colorPreview = new Border
        {
            Width = PictureBoxWidth,
            Height = Height - 4,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1)
        };

        UpdatePreviewColor();
        UpdateButtonEnablement();

        return new List<Control>{ _nameLabel, _useRandomCheckbox, _selectColorButton, _selectRandomColorButton, _useDefaultColorButton, _previewLabel, _colorPreview };
    }

    private async Task OpenSystemColorDialogAsync()
    {
        if (_useRandomCheckbox?.IsChecked == true) return;

        var c = _picker.DefinedColor;
        System.Drawing.Color? selected = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            selected = await Task.Run(() => ShowWindowsColorDialog(c));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            selected = await ShowMacColorDialogAsync(c);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            selected = await ShowLinuxColorDialogAsync(c);
        }

        if (selected.HasValue)
        {
            _picker.DefinedColor = System.Drawing.Color.FromArgb(255, selected.Value.R, selected.Value.G, selected.Value.B);
            _picker.UseRandomColor = false;
            if (_useRandomCheckbox != null) _useRandomCheckbox.IsChecked = false;
            UpdatePreviewColor();
            _picker.NotifyChildren();
        }
    }

    private static System.Drawing.Color? ShowWindowsColorDialog(System.Drawing.Color initial)
    {
        try
        {
            const int CC_RGBINIT = 0x00000001;
            const int CC_FULLOPEN = 0x00000002;

            var customColors = Marshal.AllocHGlobal(sizeof(int) * 16);
            try
            {
                var cc = new CHOOSECOLOR();
                cc.lStructSize = Marshal.SizeOf<CHOOSECOLOR>();
                cc.hwndOwner = IntPtr.Zero;
                cc.rgbResult = (initial.B << 16) | (initial.G << 8) | initial.R;
                cc.lpCustColors = customColors;
                cc.Flags = CC_RGBINIT | CC_FULLOPEN;

                if (ChooseColor(ref cc))
                {
                    int rgb = cc.rgbResult;
                    var r = (byte)(rgb & 0xFF);
                    var g = (byte)((rgb >> 8) & 0xFF);
                    var b = (byte)((rgb >> 16) & 0xFF);
                    return System.Drawing.Color.FromArgb(255, r, g, b);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(customColors);
            }
        }
        catch
        {
        }
        return null;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct CHOOSECOLOR
    {
        public int lStructSize;
        public IntPtr hwndOwner;
        public IntPtr hInstance;
        public int rgbResult;
        public IntPtr lpCustColors;
        public int Flags;
        public IntPtr lCustData;
        public IntPtr lpfnHook;
        public string? lpTemplateName;
    }

    [DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ChooseColor(ref CHOOSECOLOR cc);

    private static async Task<System.Drawing.Color?> ShowMacColorDialogAsync(System.Drawing.Color initial)
    {
        try
        {
            int ri = initial.R * 257, gi = initial.G * 257, bi = initial.B * 257;
            var psi = new ProcessStartInfo
            {
                FileName = "osascript",
                ArgumentList = { "-e", $"choose color default color {{{ri},{gi},{bi}}}" },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var p = Process.Start(psi);
            if (p == null) return null;
            string output = await p.StandardOutput.ReadToEndAsync();
            await p.WaitForExitAsync();
            if (p.ExitCode != 0) return null;
            var parts = output.Trim().Split(',');
            if (parts.Length >= 3 &&
                int.TryParse(parts[0].Trim(), out var r16) &&
                int.TryParse(parts[1].Trim(), out var g16) &&
                int.TryParse(parts[2].Trim(), out var b16))
            {
                byte r = (byte)Math.Clamp(r16 / 257, 0, 255);
                byte g = (byte)Math.Clamp(g16 / 257, 0, 255);
                byte b = (byte)Math.Clamp(b16 / 257, 0, 255);
                return System.Drawing.Color.FromArgb(255, r, g, b);
            }
        }
        catch
        {
        }
        return null;
    }

    private static async Task<System.Drawing.Color?> ShowLinuxColorDialogAsync(System.Drawing.Color initial)
    {
        var initHex = $"#{initial.R:X2}{initial.G:X2}{initial.B:X2}";
        var result = await RunProcessAndCaptureAsync("zenity", new[] { "--color-selection", "--show-palette", "--color", initHex });
        if (result.success)
        {
            var text = result.output.Trim();
            if (text.StartsWith("#"))
            {
                if (TryParseHexColor(text, out var c)) return c;
            }
            else if (text.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                var inner = text.Trim().TrimStart('r','g','b','(').TrimEnd(')');
                var parts = inner.Split(',');
                if (parts.Length >= 3 &&
                    int.TryParse(parts[0], out var r) &&
                    int.TryParse(parts[1], out var g) &&
                    int.TryParse(parts[2], out var b))
                {
                    return System.Drawing.Color.FromArgb(255, ClampByte(r), ClampByte(g), ClampByte(b));
                }
            }
        }
        result = await RunProcessAndCaptureAsync("kdialog", new[] { "--getcolor", initHex });
        if (result.success)
        {
            var text = result.output.Trim();
            if (TryParseHexColor(text, out var c)) return c;
        }
        return null;
    }

    private static byte ClampByte(int v) => (byte)Math.Clamp(v, 0, 255);

    private static bool TryParseHexColor(string hex, out System.Drawing.Color color)
    {
        color = System.Drawing.Color.Empty;
        if (string.IsNullOrWhiteSpace(hex)) return false;
        var t = hex.Trim();
        if (t.StartsWith("#")) t = t[1..];
        if (t.Length == 6 &&
            byte.TryParse(t.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out var r) &&
            byte.TryParse(t.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out var g) &&
            byte.TryParse(t.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out var b))
        {
            color = System.Drawing.Color.FromArgb(255, r, g, b);
            return true;
        }
        return false;
    }

    private static async Task<(bool success, string output)> RunProcessAndCaptureAsync(string fileName, string[] args)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            foreach (var a in args) psi.ArgumentList.Add(a);
            using var p = Process.Start(psi);
            if (p == null) return (false, string.Empty);
            string output = await p.StandardOutput.ReadToEndAsync();
            await p.WaitForExitAsync();
            return (p.ExitCode == 0, output);
        }
        catch
        {
            return (false, string.Empty);
        }
    }

    private static Queue<Color>? _recent;
    private void EnqueueRecent(Color c)
    {
        _recent ??= new Queue<Color>();
        if (_recent.Count >= 10) _recent.Dequeue();
        _recent.Enqueue(c);
    }

    private void UpdatePreviewColor()
    {
        if (_colorPreview == null) return;
        var c = _picker.DefinedColor;
        if (_useRandomCheckbox?.IsChecked == true)
        {
            _colorPreview.Background = new SolidColorBrush(Colors.Transparent);
        }
        else
        {
            _colorPreview.Background = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }
    }

    private void UpdateButtonEnablement()
    {
        var disabled = _useRandomCheckbox?.IsChecked == true;
        if (_selectColorButton != null) _selectColorButton.IsEnabled = !disabled;
        if (_selectRandomColorButton != null) _selectRandomColorButton.IsEnabled = !disabled;
        if (_useDefaultColorButton != null) _useDefaultColorButton.IsEnabled = !disabled;
    }

    public void NotifyObserver()
    {
        if (_useRandomCheckbox != null)
            _useRandomCheckbox.IsChecked = _picker.UseRandomColor;
        UpdateButtonEnablement();
        UpdatePreviewColor();
    }
}
