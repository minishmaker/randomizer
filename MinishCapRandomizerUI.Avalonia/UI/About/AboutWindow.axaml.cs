using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RandomizerCore.Controllers;
using System.Diagnostics;

namespace MinishCapRandomizerUI.Avalonia.UI.About;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        this.FindControl<Button>("OkButton")!.Click += (_, __) => Close();

        var sc = new ShufflerController();
        this.FindControl<TextBlock>("TitleBlock")!.Text = sc.AppName;
        this.FindControl<TextBlock>("VersionBlock")!.Text = $"Version {sc.VersionName} {sc.RevName}";

        void Open(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch
            {
                // ignore
            }
        }

        this.FindControl<Button>("DiscordRando")!.Click += (_, __) => Open("https://discord.gg/ndFuWbV");
        this.FindControl<Button>("DiscordZsr")!.Click += (_, __) => Open("https://discord.com/invite/zsr");
        this.FindControl<Button>("GithubRepo")!.Click += (_, __) => Open("https://github.com/minishmaker/randomizer");
        this.FindControl<Button>("GithubReleases")!.Click += (_, __) => Open("https://github.com/minishmaker/randomizer/releases");
        this.FindControl<Button>("EmoTracker")!.Click += (_, __) => Open("https://emotracker.net/download/");
        this.FindControl<Button>("BizHawk")!.Click += (_, __) => Open("https://tasvideos.org/Bizhawk/ReleaseHistory");
        this.FindControl<Button>("ColorzCore")!.Click += (_, __) => Open("https://github.com/FireEmblemUniverse/ColorzCore");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
