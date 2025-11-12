using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using RandomizerCore.Randomizer.Logic.Options;
using MinishCapRandomizerUI.Avalonia.DrawConstants;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public class ColorPickerWrapper : WrapperBase, ILogicOptionObserver
{
    // Parity constants with WinForms implementation
    private const int NameTextWidth = 125;
    private const int CheckboxWidth = 125;
    private const int PreviewTextWidth = 55;
    private const int ButtonWidth = 100;
    private const int PictureBoxWidth = 60;
    private const int Height = 23; // unified height for buttons / controls
    private const string CheckboxText = "Use Random Color";
    private const string SelectColorText = "Select Color";
    private const string SelectRandomColorText = "Pick Random";
    private const string UseDefaultColorText = "Use Default";
    private const string ColorPreviewText = "Preview:";

    // Calculated width similar to WinForms (spacing approximated with WidthMargin usage)
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

        // Name label
        _nameLabel = new TextBlock
        {
            Text = _picker.NiceName + ":",
            VerticalAlignment = VerticalAlignment.Center,
            Width = NameTextWidth
        };
        ToolTip.SetTip(_nameLabel, _picker.DescriptionText);

        // Random Color checkbox
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

        // Select Color button (opens slider dialog)
        _selectColorButton = new Button
        {
            Content = SelectColorText,
            Width = ButtonWidth
        };
        ToolTip.SetTip(_selectColorButton, "Opens the custom color picker");
        _selectColorButton.Click += async (_, __) => await OpenColorDialog();

        // Pick Random button (immediately randomizes color)
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

        // Use Default button
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

        // Preview label
        _previewLabel = new TextBlock
        {
            Text = ColorPreviewText,
            VerticalAlignment = VerticalAlignment.Center,
            Width = PreviewTextWidth
        };

        // Color preview border
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

    private async Task OpenColorDialog()
    {
        if (_useRandomCheckbox?.IsChecked == true) return; // disabled state

        var dialog = new Window{ Width = 460, Height = 420, Title = "Pick Color" };
        var root = new StackPanel{ Margin = new Thickness(12), Spacing = 10 };

        var current = _picker.DefinedColor;
        var r = new Slider{ Minimum = 0, Maximum = 255, Value = current.R, Width = 260 };
        var g = new Slider{ Minimum = 0, Maximum = 255, Value = current.G, Width = 260 };
        var b = new Slider{ Minimum = 0, Maximum = 255, Value = current.B, Width = 260 };
        var rx = new NumericUpDown{ Minimum = 0, Maximum = 255, Value = current.R, Width = 70 };
        var gx = new NumericUpDown{ Minimum = 0, Maximum = 255, Value = current.G, Width = 70 };
        var bx = new NumericUpDown{ Minimum = 0, Maximum = 255, Value = current.B, Width = 70 };
        var hexBox = new TextBox{ Width = 120, Watermark = "#RRGGBB" };

        var preview = new Border{ Width = 80, Height = 80, Background = new SolidColorBrush(Color.FromRgb(current.R, current.G, current.B)), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(0,8,0,8) };

        void SyncFromSliders()
        {
            rx.Value = (int)r.Value; gx.Value = (int)g.Value; bx.Value = (int)b.Value;
            var c = Color.FromRgb((byte)r.Value, (byte)g.Value, (byte)b.Value);
            preview.Background = new SolidColorBrush(c);
            hexBox.Text = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }
        void SyncFromNumeric()
        {
            r.Value = (double)(rx.Value ?? 0);
            g.Value = (double)(gx.Value ?? 0);
            b.Value = (double)(bx.Value ?? 0);
            var c = Color.FromRgb((byte)r.Value, (byte)g.Value, (byte)b.Value);
            preview.Background = new SolidColorBrush(c);
            hexBox.Text = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }
        void SyncFromHex()
        {
            var t = hexBox.Text?.Trim() ?? string.Empty;
            if (t.StartsWith("#")) t = t[1..];
            if (t.Length == 6 && byte.TryParse(t.Substring(0,2), System.Globalization.NumberStyles.HexNumber, null, out var rr)
                              && byte.TryParse(t.Substring(2,2), System.Globalization.NumberStyles.HexNumber, null, out var gg)
                              && byte.TryParse(t.Substring(4,2), System.Globalization.NumberStyles.HexNumber, null, out var bb))
            {
                r.Value = rr; g.Value = gg; b.Value = bb;
                rx.Value = rr; gx.Value = gg; bx.Value = bb;
                preview.Background = new SolidColorBrush(Color.FromRgb(rr, gg, bb));
            }
        }

        r.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(Slider.Value)) SyncFromSliders(); };
        g.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(Slider.Value)) SyncFromSliders(); };
        b.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(Slider.Value)) SyncFromSliders(); };
        rx.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(NumericUpDown.Value)) SyncFromNumeric(); };
        gx.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(NumericUpDown.Value)) SyncFromNumeric(); };
        bx.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(NumericUpDown.Value)) SyncFromNumeric(); };
        hexBox.PropertyChanged += (_, a) => { if (a.Property.Name == nameof(TextBox.Text)) SyncFromHex(); };

        var sliders = new Grid();
        sliders.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        sliders.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        sliders.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        sliders.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        sliders.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        sliders.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        var lblR = new TextBlock{ Text = "Red", VerticalAlignment = VerticalAlignment.Center };
        var lblG = new TextBlock{ Text = "Green", VerticalAlignment = VerticalAlignment.Center };
        var lblB = new TextBlock{ Text = "Blue", VerticalAlignment = VerticalAlignment.Center };
        Grid.SetRow(lblR,0); Grid.SetColumn(lblR,0);
        Grid.SetRow(r,0); Grid.SetColumn(r,1);
        Grid.SetRow(rx,0); Grid.SetColumn(rx,2);
        Grid.SetRow(lblG,1); Grid.SetColumn(lblG,0);
        Grid.SetRow(g,1); Grid.SetColumn(g,1);
        Grid.SetRow(gx,1); Grid.SetColumn(gx,2);
        Grid.SetRow(lblB,2); Grid.SetColumn(lblB,0);
        Grid.SetRow(b,2); Grid.SetColumn(b,1);
        Grid.SetRow(bx,2); Grid.SetColumn(bx,2);
        sliders.Children.Add(lblR); sliders.Children.Add(r); sliders.Children.Add(rx);
        sliders.Children.Add(lblG); sliders.Children.Add(g); sliders.Children.Add(gx);
        sliders.Children.Add(lblB); sliders.Children.Add(b); sliders.Children.Add(bx);

        var topRow = new StackPanel{ Orientation = Orientation.Horizontal, Spacing = 12 };
        topRow.Children.Add(preview);
        topRow.Children.Add(new StackPanel{ Spacing = 8, Children = { new TextBlock{ Text = "HEX" }, hexBox } });

        // Preset swatches
        var presetColors = new []{ "#E53E3E","#DD6B20","#D69E2E","#38A169","#3182CE","#805AD5","#D53F8C","#718096","#000000","#FFFFFF" };
        var presetsPanel = new WrapPanel();
        foreach (var hex in presetColors)
        {
            var c = Color.Parse(hex);
            var btn = new Button{ Width = 24, Height = 24, Background = new SolidColorBrush(c), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Tag = hex, Margin = new Thickness(3) };
            btn.Click += (_, __) => { hexBox.Text = hex; SyncFromHex(); };
            presetsPanel.Children.Add(btn);
        }

        // Recent colors
        _recent ??= new Queue<Color>();
        var recentsPanel = new WrapPanel();
        foreach (var rc in _recent)
        {
            var btn = new Button{ Width = 24, Height = 24, Background = new SolidColorBrush(Color.FromRgb(rc.R, rc.G, rc.B)), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(3) };
            var cap = rc; btn.Click += (_, __) => { r.Value = cap.R; g.Value = cap.G; b.Value = cap.B; SyncFromSliders(); };
            recentsPanel.Children.Add(btn);
        }

        var buttonsPanel = new StackPanel{ Orientation = Orientation.Horizontal, Spacing = 8 };
        var ok = new Button{ Content = "OK", Width = 90 };
        var cancel = new Button{ Content = "Cancel", Width = 90 };
        ok.Click += (_, __) => {
            var c = (preview.Background as SolidColorBrush)?.Color ?? Color.FromRgb((byte)r.Value,(byte)g.Value,(byte)b.Value);
            _picker.DefinedColor = System.Drawing.Color.FromArgb(255, c.R, c.G, c.B);
            EnqueueRecent(c);
            _picker.NotifyChildren();
            UpdatePreviewColor();
            dialog.Close();
        };
        cancel.Click += (_, __) => dialog.Close();
        buttonsPanel.Children.Add(ok);
        buttonsPanel.Children.Add(cancel);

        root.Children.Add(topRow);
        root.Children.Add(new TextBlock{ Text = "RGB" });
        root.Children.Add(sliders);
        root.Children.Add(new TextBlock{ Text = "Presets" });
        root.Children.Add(presetsPanel);
        if (_recent.Count > 0)
        {
            root.Children.Add(new TextBlock{ Text = "Recent" });
            root.Children.Add(recentsPanel);
        }
        root.Children.Add(buttonsPanel);
        dialog.Content = root;
        var top = TopLevel.GetTopLevel(_selectColorButton);
        if (top is Window owner)
            await dialog.ShowDialog(owner);
        else
            dialog.Show();
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
        // reflect random state & color changes
        if (_useRandomCheckbox != null)
            _useRandomCheckbox.IsChecked = _picker.UseRandomColor;
        UpdateButtonEnablement();
        UpdatePreviewColor();
    }
}
