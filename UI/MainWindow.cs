using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MinishRandomizer.Core;
using MinishRandomizer.Randomizer;
using MinishRandomizer.Randomizer.Logic;
using MinishRandomizer.Utilities;

namespace MinishRandomizer
{
    public partial class MainWindow : Form
    {
        private ROM ROM_;
        private Shuffler shuffler;
        private bool Randomized;
        private uint LogicHash;
        private List<LogicOption> Settings;
        private List<LogicOption> Gimmicks;
        private readonly uint BuiltinLogicHash;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize seed to random value
            seedField.Text = new Random().Next().ToString();

            // Load default logic, get hash
            byte[] logicBytes;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("MinishRandomizer.Resources.default.logic"))
            {
                logicBytes = new byte[stream.Length];
                stream.Read(logicBytes, 0, (int)stream.Length);
            }

            BuiltinLogicHash = PatchUtil.Crc32(logicBytes, logicBytes.Length);

            // Create shuffler and populate logic options
            shuffler = new Shuffler();

            UpdateOptions();
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
                // If the seed is valid, load locations from the logic and randomize their contents
                if (int.TryParse(seedField.Text, out int seed))
                {
                    // Make sure the RNG is set to the seed, so the seed can be regenerated
                    shuffler.SetSeed(seed);

                    if (customLogicCheckBox.Checked)
                    {
                        shuffler.LoadLocations(customLogicPath.Text);
                    }
                    else
                    {
                        shuffler.LoadLocations();
                    }


                    shuffler.RandomizeLocations();
                }
                else
                {
                    MessageBox.Show("The seed value is not valid!\nMake sure it's not too large and only contains numeric characters.");
                    return;
                }

                // Mark that a seed has been generated so seed-specific options can be used
                Randomized = true;

                if (!mainTabs.TabPages.Contains(generatedTab))
                {
                    // Change the tab to the seed output tab
                    mainTabs.TabPages.Add(generatedTab);
                }

                mainTabs.SelectedTab = generatedTab;

                // Show ROM information on seed page
                generatedSeedValue.Text = seed.ToString();
                generatedLogicLabel.Text = shuffler.GetLogicIdentifier();

                settingHashValue.Text = StringUtil.AsStringHex8((int)shuffler.GetSettingHash());
                gimmickHashValue.Text = StringUtil.AsStringHex8((int)shuffler.GetGimmickHash());

                statusText.Text = $"Successfully randomzied seed {seed}";
            }
            catch (ParserException error)
            {
                MessageBox.Show(error.Message, "Error Parsing Logic", MessageBoxButtons.OK);
                statusText.Text = $"Error parsing logic: {error.Message}";
            }
            catch (ShuffleException error)
            {
                MessageBox.Show(error.Message, "Error Randomizing Seed", MessageBoxButtons.OK);
                statusText.Text = $"Error randomizing seed: {error.Message}";
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
                ROM_ = null;
                return;
            }

            if (!shuffler.RomCrcValid(ROM_))
            {
                Console.WriteLine(StringUtil.AsStringHex8((int)PatchUtil.Crc32(ROM_.romData, ROM_.romData.Length)));
                MessageBox.Show("ROM does not match the expected CRC for the logic file", "Incorrect ROM", MessageBoxButtons.OK);
                statusText.Text = "ROM not valid";
                ROM_ = null;
                return;
            }
        }

        private void CustomLogicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            browseLogicButton.Enabled = customLogicCheckBox.Checked;
            customLogicPath.Enabled = customLogicCheckBox.Checked;

            // Reload logic options to make sure they're correct
            if (customLogicCheckBox.Checked)
            {
                UpdateOptions(customLogicPath.Text);
            }
            else
            {
                UpdateOptions();
            }
        }

        private void CustomLogicPath_TextChanged(object sender, EventArgs e)
        {
            if (shuffler == null)
            {
                return;
            }

            if (customLogicCheckBox.Checked)
            {
                UpdateOptions(customLogicPath.Text);
            }
            else
            {
                UpdateOptions();
            }
        }

        private void CustomPatchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable UI for checkedness
            if (customPatchCheckBox.Checked)
            {
                browsePatchButton.Enabled = true;
                customPatchPath.Enabled = true;

                if (!File.Exists(customPatchPath.Text))
                {
                    patchNotExistLabel.Visible = true;
                }
                else
                {
                    patchNotExistLabel.Visible = false;
                }
            }
            else
            {
                browsePatchButton.Enabled = false;
                customPatchPath.Enabled = false;
                patchNotExistLabel.Visible = false;
            }
        }

        private void CustomPatchPath_TextChanged(object sender, EventArgs e)
        {
            if (!File.Exists(customPatchPath.Text))
            {
                patchNotExistLabel.Visible = true;
            }
            else
            {
                patchNotExistLabel.Visible = false;
            }
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
            if (!Randomized)
            {
                return;
            }

            // Get the default name for the saved ROM
            string fileName = $"MinishRandomizer_{shuffler.Version}_{shuffler.GetLogicIdentifier()}_{shuffler.GetOptionsIdentifier()}_{shuffler.Seed}";

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
            statusText.Text = $"Successfully saved \"{sfd.FileName}\"";
        }

        private void SavePatchButton_Click(object sender, EventArgs e)
        {
            if (!Randomized)
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
            if (!Randomized)
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

        private void UpdateOptions(string path = null)
        {
            if (path != null && !File.Exists(path))
            {
                logicNotExistLabel.Visible = true;
                Console.WriteLine("new'n");
                return;
            }
            else
            {
                Console.WriteLine("old'n");
                logicNotExistLabel.Visible = false;
            }


            try
            {
                shuffler.LoadOptions(path);
            }
            catch (ParserException e)
            {
                MessageBox.Show(e.Message, "Invalid Logic File", MessageBoxButtons.OK);
                return;
            }

            List<LogicOption> options = shuffler.GetOptions();

            Settings = options.Where(opt => opt.Type == LogicOptionType.Setting).ToList();
            Gimmicks = options.Where(opt => opt.Type == LogicOptionType.Gimmick).ToList();

            LoadOptionControls(options);

            UpdateLogicHash(path);

            UpdateSettingsString();
            UpdateGimmicksString();
        }

        private void LoadOptionControls(List<LogicOption> options)
        {
            // If there options, show the options tab
            if (options.Count >= 1)
            {

                // Make sure not to add the tab multiple times
                if (!mainTabs.TabPages.Contains(optionTabPage))
                {
                    // Insert logic tab at the right place
                    mainTabs.TabPages.Insert(1, optionTabPage);
                }
            }
            else
            {
                mainTabs.TabPages.Remove(optionTabPage);
                return;
            }

            optionControlLayout.Controls.Clear();
            gimmickControlLayout.Controls.Clear();
            foreach (LogicOption option in options)
            {
                Control optionControl = option.GetControl();

                if (option.Type == LogicOptionType.Setting)
                {
                    option.ChangeHash = UpdateSettingsString;
                    optionControlLayout.Controls.Add(optionControl);
                }
                else if (option.Type == LogicOptionType.Gimmick)
                {
                    option.ChangeHash = UpdateGimmicksString;
                    gimmickControlLayout.Controls.Add(optionControl);
                }
            }

            if (customLogicCheckBox.Checked)
            {
                statusText.Text = $"Loaded options for {customLogicPath.Text}";
            }
            else
            {
                statusText.Text = "Loaded options for default logic";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            seedField.Text = new Random().Next().ToString();
        }

        private void UpdateLogicHash(string logicLocation)
        {
            byte[] logicBytes;

            if (logicLocation != null)
            {
                logicBytes = File.ReadAllBytes(logicLocation);
                LogicHash = PatchUtil.Crc32(logicBytes, logicBytes.Length);
            }
            else
            {
                LogicHash = BuiltinLogicHash;
            }
        }

        private void UpdateSettingsString()
        {
            List<byte> settingsBytes = new List<byte>();
            foreach (LogicOption setting in Settings)
            {
                settingsBytes.AddRange(setting.GetOptionBytes());
            }

            // Begin with logic hash portion
            string newSettingsString = StringUtil.AsStringHex4((int)LogicHash & 0xFFFF) + "-";

            ulong accumulation = 0;

            // Notably includes gimmicksBytes.Count, the i-1th entry in gimmicksBytes is used
            for (int i = 1; i < settingsBytes.Count; i++)
            {
                // ulong is full, start next one
                if (i % 8 == 0 && i != 0)
                {
                    newSettingsString += Base36Util.Encode(accumulation);
                    accumulation = 0;
                }

                // Shift byte into accumulation as appropriate
                accumulation += (uint)(settingsBytes[i] << (8 * (i % 8)));
            }

            if (settingsBytes.Count % 8 != 0)
            {
                newSettingsString += Base36Util.Encode(accumulation);
            }

            settingsStringBox.Text = newSettingsString;
        }

        private void UpdateGimmicksString()
        {
            List<byte> gimmicksBytes = new List<byte>();
            foreach (LogicOption gimmick in Gimmicks)
            {
                gimmicksBytes.AddRange(gimmick.GetOptionBytes());
            }

            // Begin with logic hash portion
            string newGimmicksString = StringUtil.AsStringHex4((int)LogicHash & 0xFFFF) + "-";

            ulong accumulation = 0;

            for (int i = 0; i < gimmicksBytes.Count; i++)
            {
                // ulong is full, start next one
                if (i % 8 == 0 && i != 0)
                {
                    newGimmicksString += Base36Util.Encode(accumulation);
                    accumulation = 0;
                }

                // Shift byte into accumulation as appropriate
                accumulation += (uint)(gimmicksBytes[i] << (8 * (i % 8)));
            }

            if (gimmicksBytes.Count % 8 != 0)
            {
                newGimmicksString += Base36Util.Encode(accumulation);
            }

            gimmicksStringBox.Text = newGimmicksString;
        }
    }
}
