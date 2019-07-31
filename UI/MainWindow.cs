using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinishRandomizer.Core;
using MinishRandomizer.Randomizer;

namespace MinishRandomizer
{
    public partial class MainWindow : Form
    {
        private ROM ROM_;
        private Shuffler shuffler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRom();
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            if (ROM_ == null)
            {
                return;
            }


            if (shuffler == null)
            {
                return;
            }

            try
            {
                shuffler.RandomizeLocations();
                MessageBox.Show("Your rom has been randomized.", "Randomization finished", MessageBoxButtons.OK);
            }
            catch (ShuffleException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void LoadRom()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "GBA ROMs|*.gba|All Files|*.*",
                Title = "Select TMC ROM"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                ROM_ = new ROM(ofd.FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (ROM.Instance.version.Equals(RegionVersion.None))
            {
                MessageBox.Show("Invalid TMC ROM. Please Open a valid ROM.", "Incorrect ROM", MessageBoxButtons.OK);
                statusText.Text = "Unable to determine ROM.";
                return;
            }

            shuffler = new Shuffler(Path.GetDirectoryName(ROM.Instance.path));

            try
            {
                if (customLogicCheckBox.Checked)
                {
                    shuffler.LoadLocations(customLogicPath.Text);
                }
                else
                {
                    shuffler.LoadLocations(null);
                }

                if (customPatchCheckBox.Checked)
                {
                    shuffler.PatchRom(customPatchPath.Text);
                }
                else
                {
                    shuffler.PatchRom(null);
                }
            }
            catch (ShuffleException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void CustomLogicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            browseLogicButton.Enabled = customLogicCheckBox.Checked;
            customLogicPath.Enabled = customLogicCheckBox.Checked;
        }

        private void CustomPatchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            browsePatchButton.Enabled = customPatchCheckBox.Checked;
            customPatchPath.Enabled = customPatchCheckBox.Checked;
        }

        private void BrowseLogicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Logic Data|*.logic|All Files|*.*",
                Title = "Select Custom Logic"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            customLogicPath.Text = ofd.FileName;
        }

        private void BrowsePatchButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "UPS Patch|*.ups|All Files|*.*",
                Title = "Select Custom Patch"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            customPatchPath.Text = ofd.FileName;
        }
    }
}
