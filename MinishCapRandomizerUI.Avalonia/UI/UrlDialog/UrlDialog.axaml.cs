using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace MinishCapRandomizerUI.Avalonia.UI.UrlDialog
{
    public partial class UrlDialog : Window
    {
        private string _url = "";

        public UrlDialog()
        {
            InitializeComponent();
            var okBtn = this.FindControl<Button>("OkButton");
            if (okBtn != null) okBtn.Click += (_, __) => Close();
            var link = this.FindControl<TextBlock>("LinkText");
            if (link != null)
            {
                link.PointerPressed += (_, __) =>
                {
                    if (string.IsNullOrWhiteSpace(_url)) return;
                    try { Process.Start(new ProcessStartInfo { FileName = _url, UseShellExecute = true }); } catch { }
                };
            }
        }

        public void Setup(string url, string message)
        {
            _url = url;
            var link = this.FindControl<TextBlock>("LinkText"); if (link != null) link.Text = url;
            var msg = this.FindControl<TextBlock>("Message"); if (msg != null) msg.Text = message;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
