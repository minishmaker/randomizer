using System.Diagnostics;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI.About
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            AboutPageVersionId.Text = @$"TMCR Version {ShufflerController.VersionIdentifier} {ShufflerController.RevisionIdentifier}";
        }

        private void RandoDiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RandoDiscordLink.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://discord.gg/ndFuWbV") {UseShellExecute = true});
        }
        
        private void ZsrDiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ZsrDiscordLink.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://discord.com/invite/zsr") {UseShellExecute = true});
        }

        private void RandomizerGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RandomizerGithub.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://github.com/minishmaker/randomizer") {UseShellExecute = true});
        }

        private void RandomizerReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RandomizerReleases.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://github.com/minishmaker/randomizer/releases") {UseShellExecute = true});
        }

        private void EmotrackerDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EmotrackerDownload.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://emotracker.net/download/") {UseShellExecute = true});
        }

        private void BizhawkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BizhawkDownload.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://tasvideos.org/Bizhawk/ReleaseHistory#Bizhawk242") {UseShellExecute = true});
        }

        private void ColorzCoreLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ColorzCoreLink.LinkVisited = true;
            Process.Start(new ProcessStartInfo("https://github.com/FireEmblemUniverse/ColorzCore") {UseShellExecute = true});
        }
    }
}
