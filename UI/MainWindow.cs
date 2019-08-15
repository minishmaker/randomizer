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
        private bool randomized;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize seed to random value
            seedField.Text = new Random().Next().ToString();

            // Fill heart color options
            this.heartColorSelect.DataSource = Enum.GetValues(typeof(HeartColorType));
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRom();
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            if (ROM_ == null)
            {
                LoadRom();

                if (ROM_ == null)
                {
                    return;
                }
            }


            if (shuffler == null)
            {
                return;
            }

            try
            {
                if (int.TryParse(seedField.Text, out int seed))
                {
                    if (customLogicCheckBox.Checked)
                    {
                        shuffler.LoadLocations(customLogicPath.Text);
                    }
                    else
                    {
                        shuffler.LoadLocations();
                    }

                    
                    shuffler.RandomizeLocations(seed);
                }
                else
                {
                    MessageBox.Show("The seed value is not valid!\nMake sure it's not too large and only contains numeric characters.");
                }

                // First randomization needs to change the tab
                if (!randomized)
                {
                    // Change the tab to the seed output tab
                    mainTabs.Controls.Add(generatedTab);
                    mainTabs.SelectedTab = generatedTab;
                }

                randomized = true;

                // Show ROM information on seed page
                generatedSeedValue.Text = seed.ToString();
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
                Filter = "Event Assembler Buildfiles|*.event|All Files|*.*",
                Title = "Select Custom Patch"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            customPatchPath.Text = ofd.FileName;
        }

        private void SaveRomButton_Click(object sender, EventArgs e)
        {
            if (!randomized)
            {
                return;
            }

            // Get the default name for the saved ROM
            string fileName = $"MinishRandomizer_{shuffler.Seed}";

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "GBA ROMs|*.gba|All Files|*.*",
                Title = "Save ROM",
                FileName = fileName
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Write output to ROM, then add patches
            byte[] outputRom = shuffler.GetRandomizedRom();
            File.WriteAllBytes(sfd.FileName, outputRom);

            shuffler.SetHeartColor((HeartColorType)heartColorSelect.SelectedItem);

            if (customPatchCheckBox.Checked)
            {
                
                shuffler.ApplyPatch(sfd.FileName, customPatchPath.Text);
            }
            else
            {
                // Use the default patch
                shuffler.ApplyPatch(sfd.FileName);
            }

            MessageBox.Show("ROM successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SavePatchButton_Click(object sender, EventArgs e)
        {
            if (!randomized)
            {
                return;
            }

            // Get the default name for the saved patch
            string fileName = $"MinishRandomizer_{shuffler.Seed}_patch";

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "UPS Patches|*.ups|All Files|*.*",
                Title = "Save Patch",
                FileName = fileName
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }

        private void SaveSpoilerButton_Click(object sender, EventArgs e)
        {
            if (!randomized)
            {
                return;
            }

            // Get the default name for the saved patch
            string fileName = $"MinishRandomizer_{shuffler.Seed}_spoiler";

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text files|*.txt|All Files|*.*",
                Title = "Save Spoiler",
                FileName = fileName
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Write output to ROM, then add patches
            string spoilerLog = shuffler.GetSpoiler();
            File.WriteAllText(sfd.FileName, spoilerLog);
        }
    }
}
