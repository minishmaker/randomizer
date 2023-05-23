using System.Diagnostics;

namespace MinishCapRandomizerUI.UI.UrlDialog
{
    public partial class UrlDialog : Form
    {
        private string _url;
        
        public UrlDialog(string url)
        {
            _url = url;
            InitializeComponent();
            MessageIcon.Image = SystemIcons.Information.ToBitmap();
        }

        private void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Link.LinkVisited = true;
            Process.Start(new ProcessStartInfo(_url) {UseShellExecute = true});
        }

        private void Okay_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
