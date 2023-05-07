using MinishCapRandomizerUI.DrawConstants;

namespace MinishCapRandomizerUI.UI
{
    sealed partial class MinishCapRandomizerUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinishCapRandomizerUI));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.logicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportDefaultLogicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setLoggerOutputPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logAllTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeAndFlushLoggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.localizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesOnStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Randomize = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.General = new System.Windows.Forms.TabPage();
			this.label14 = new System.Windows.Forms.Label();
			this.panel9 = new System.Windows.Forms.Panel();
			this.UseCustomCosmetics = new System.Windows.Forms.CheckBox();
			this.PatchRomAndGenPatch = new System.Windows.Forms.Button();
			this.GeneratePatchButton = new System.Windows.Forms.RadioButton();
			this.label15 = new System.Windows.Forms.Label();
			this.ApplyPatchButton = new System.Windows.Forms.RadioButton();
			this.BrowsePatchOrRom = new System.Windows.Forms.Button();
			this.BpsPatchAndPatchedRomPath = new System.Windows.Forms.TextBox();
			this.BpsPatchAndPatchedRomLabel = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.panel8 = new System.Windows.Forms.Panel();
			this.DeleteCosmeticPreset = new System.Windows.Forms.Button();
			this.ResetDefaultCosmetics = new System.Windows.Forms.Button();
			this.LoadCosmeticPreset = new System.Windows.Forms.Button();
			this.CosmeticsPresets = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.SaveCosmeticPreset = new System.Windows.Forms.Button();
			this.GenerateCosmetics = new System.Windows.Forms.Button();
			this.LoadCosmetics = new System.Windows.Forms.Button();
			this.CosmeticsString = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.DeleteSettingPreset = new System.Windows.Forms.Button();
			this.ResetDefaultSettings = new System.Windows.Forms.Button();
			this.LoadSettingPreset = new System.Windows.Forms.Button();
			this.SettingPresets = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.SaveSettingPreset = new System.Windows.Forms.Button();
			this.GenerateSettings = new System.Windows.Forms.Button();
			this.LoadSettings = new System.Windows.Forms.Button();
			this.SettingString = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.RandomSeed = new System.Windows.Forms.Button();
			this.Seed = new System.Windows.Forms.TextBox();
			this.RomPath = new System.Windows.Forms.TextBox();
			this.BrowseRom = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.TabPane = new System.Windows.Forms.TabControl();
			this.Advanced = new System.Windows.Forms.TabPage();
			this.label25 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.UseSphereBasedShuffler = new System.Windows.Forms.CheckBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.BrowseCustomPatch = new System.Windows.Forms.Button();
			this.RomBuildfilePath = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.UseCustomPatch = new System.Windows.Forms.CheckBox();
			this.BrowseCustomLogicFile = new System.Windows.Forms.Button();
			this.LogicFilePath = new System.Windows.Forms.TextBox();
			this.UseCustomLogic = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.SeedOutput = new System.Windows.Forms.TabPage();
			this.SaveRom = new System.Windows.Forms.Button();
			this.SavePatch = new System.Windows.Forms.Button();
			this.SaveSpoiler = new System.Windows.Forms.Button();
			this.label17 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.CopyCosmeticsHashToClipboard = new System.Windows.Forms.Button();
			this.CosmeticStringLabel = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.CosmeticNameLabel = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.CopySettingsHashToClipboard = new System.Windows.Forms.Button();
			this.SettingHashLabel = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.SettingNameLabel = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.CopyHashToClipboard = new System.Windows.Forms.Button();
			this.ImagePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label20 = new System.Windows.Forms.Label();
			this.OutputSeedLabel = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.InputSeedLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.RandomizationAttempts = new System.Windows.Forms.TextBox();
			this.menuStrip1.SuspendLayout();
			this.General.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel7.SuspendLayout();
			this.TabPane.SuspendLayout();
			this.Advanced.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel6.SuspendLayout();
			this.SeedOutput.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logicToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.localizationToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(816, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// logicToolStripMenuItem
			// 
			this.logicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDefaultLogicToolStripMenuItem});
			this.logicToolStripMenuItem.Name = "logicToolStripMenuItem";
			this.logicToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.logicToolStripMenuItem.Text = "Logic";
			// 
			// exportDefaultLogicToolStripMenuItem
			// 
			this.exportDefaultLogicToolStripMenuItem.Name = "exportDefaultLogicToolStripMenuItem";
			this.exportDefaultLogicToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.exportDefaultLogicToolStripMenuItem.Text = "Export Default Logic";
			this.exportDefaultLogicToolStripMenuItem.Click += new System.EventHandler(this.exportDefaultLogicToolStripMenuItem_Click);
			// 
			// loggingToolStripMenuItem
			// 
			this.loggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setLoggerOutputPathToolStripMenuItem,
            this.logAllTransactionsToolStripMenuItem,
            this.writeAndFlushLoggerToolStripMenuItem});
			this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
			this.loggingToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
			this.loggingToolStripMenuItem.Text = "Logging";
			// 
			// setLoggerOutputPathToolStripMenuItem
			// 
			this.setLoggerOutputPathToolStripMenuItem.Name = "setLoggerOutputPathToolStripMenuItem";
			this.setLoggerOutputPathToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.setLoggerOutputPathToolStripMenuItem.Text = "Set Logger Output Path";
			this.setLoggerOutputPathToolStripMenuItem.Click += new System.EventHandler(this.setLoggerOutputPathToolStripMenuItem_Click);
			// 
			// logAllTransactionsToolStripMenuItem
			// 
			this.logAllTransactionsToolStripMenuItem.Name = "logAllTransactionsToolStripMenuItem";
			this.logAllTransactionsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.logAllTransactionsToolStripMenuItem.Text = "Log All Transactions";
			this.logAllTransactionsToolStripMenuItem.Click += new System.EventHandler(this.logAllTransactionsToolStripMenuItem_Click);
			// 
			// writeAndFlushLoggerToolStripMenuItem
			// 
			this.writeAndFlushLoggerToolStripMenuItem.Name = "writeAndFlushLoggerToolStripMenuItem";
			this.writeAndFlushLoggerToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.writeAndFlushLoggerToolStripMenuItem.Text = "Write and Flush Logger";
			this.writeAndFlushLoggerToolStripMenuItem.Click += new System.EventHandler(this.writeAndFlushLoggerToolStripMenuItem_Click);
			// 
			// localizationToolStripMenuItem
			// 
			this.localizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem});
			this.localizationToolStripMenuItem.Name = "localizationToolStripMenuItem";
			this.localizationToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
			this.localizationToolStripMenuItem.Text = "Localization";
			// 
			// englishToolStripMenuItem
			// 
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			this.englishToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.englishToolStripMenuItem.Text = "English";
			// 
			// otherToolStripMenuItem
			// 
			this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesOnStartToolStripMenuItem});
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			this.otherToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.otherToolStripMenuItem.Text = "Other";
			// 
			// checkForUpdatesOnStartToolStripMenuItem
			// 
			this.checkForUpdatesOnStartToolStripMenuItem.Name = "checkForUpdatesOnStartToolStripMenuItem";
			this.checkForUpdatesOnStartToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
			this.checkForUpdatesOnStartToolStripMenuItem.Text = "Check for Updates On Start";
			this.checkForUpdatesOnStartToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesOnStartToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changelogToolStripMenuItem,
            this.aboutToolStripMenuItem1,
            this.checkForUpdatesToolStripMenuItem});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// changelogToolStripMenuItem
			// 
			this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
			this.changelogToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.changelogToolStripMenuItem.Text = "Changelog";
			// 
			// aboutToolStripMenuItem1
			// 
			this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
			this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
			this.aboutToolStripMenuItem1.Text = "About";
			this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
			this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
			// 
			// Randomize
			// 
			this.Randomize.Location = new System.Drawing.Point(533, 596);
			this.Randomize.Name = "Randomize";
			this.Randomize.Size = new System.Drawing.Size(255, 23);
			this.Randomize.TabIndex = 3;
			this.Randomize.Text = "Randomize";
			this.Randomize.UseVisualStyleBackColor = true;
			this.Randomize.Click += new System.EventHandler(this.Randomize_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// General
			// 
			this.General.Controls.Add(this.label14);
			this.General.Controls.Add(this.panel9);
			this.General.Controls.Add(this.label11);
			this.General.Controls.Add(this.panel8);
			this.General.Controls.Add(this.label8);
			this.General.Controls.Add(this.panel7);
			this.General.Controls.Add(this.RandomSeed);
			this.General.Controls.Add(this.Seed);
			this.General.Controls.Add(this.RomPath);
			this.General.Controls.Add(this.BrowseRom);
			this.General.Controls.Add(this.label3);
			this.General.Controls.Add(this.label4);
			this.General.Location = new System.Drawing.Point(4, 24);
			this.General.Name = "General";
			this.General.Size = new System.Drawing.Size(787, 535);
			this.General.TabIndex = 1;
			this.General.Text = "General";
			this.General.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label14.Location = new System.Drawing.Point(17, 393);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(181, 15);
			this.label14.TabIndex = 21;
			this.label14.Text = "BPS Patcher and Patch Generator";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel9
			// 
			this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel9.Controls.Add(this.UseCustomCosmetics);
			this.panel9.Controls.Add(this.PatchRomAndGenPatch);
			this.panel9.Controls.Add(this.GeneratePatchButton);
			this.panel9.Controls.Add(this.label15);
			this.panel9.Controls.Add(this.ApplyPatchButton);
			this.panel9.Controls.Add(this.BrowsePatchOrRom);
			this.panel9.Controls.Add(this.BpsPatchAndPatchedRomPath);
			this.panel9.Controls.Add(this.BpsPatchAndPatchedRomLabel);
			this.panel9.Location = new System.Drawing.Point(6, 400);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(760, 110);
			this.panel9.TabIndex = 20;
			// 
			// UseCustomCosmetics
			// 
			this.UseCustomCosmetics.AutoSize = true;
			this.UseCustomCosmetics.Location = new System.Drawing.Point(510, 14);
			this.UseCustomCosmetics.Name = "UseCustomCosmetics";
			this.UseCustomCosmetics.Size = new System.Drawing.Size(231, 19);
			this.UseCustomCosmetics.TabIndex = 26;
			this.UseCustomCosmetics.Text = "Use Custom Cosmetics (Disabled, WIP)";
			this.UseCustomCosmetics.UseVisualStyleBackColor = true;
			// 
			// PatchRomAndGenPatch
			// 
			this.PatchRomAndGenPatch.Location = new System.Drawing.Point(510, 75);
			this.PatchRomAndGenPatch.Name = "PatchRomAndGenPatch";
			this.PatchRomAndGenPatch.Size = new System.Drawing.Size(240, 23);
			this.PatchRomAndGenPatch.TabIndex = 25;
			this.PatchRomAndGenPatch.Text = "Patch Rom";
			this.PatchRomAndGenPatch.UseVisualStyleBackColor = true;
			this.PatchRomAndGenPatch.Click += new System.EventHandler(this.PatchRomAndGenPatch_Click);
			// 
			// GeneratePatchButton
			// 
			this.GeneratePatchButton.Location = new System.Drawing.Point(331, 13);
			this.GeneratePatchButton.Name = "GeneratePatchButton";
			this.GeneratePatchButton.Size = new System.Drawing.Size(168, 19);
			this.GeneratePatchButton.TabIndex = 24;
			this.GeneratePatchButton.Text = "Generate Patch Mode";
			this.GeneratePatchButton.UseVisualStyleBackColor = true;
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label15.Location = new System.Drawing.Point(10, 15);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(133, 15);
			this.label15.TabIndex = 23;
			this.label15.Text = "Patcher Mode:";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ApplyPatchButton
			// 
			this.ApplyPatchButton.Checked = true;
			this.ApplyPatchButton.Location = new System.Drawing.Point(153, 13);
			this.ApplyPatchButton.Name = "ApplyPatchButton";
			this.ApplyPatchButton.Size = new System.Drawing.Size(168, 19);
			this.ApplyPatchButton.TabIndex = 22;
			this.ApplyPatchButton.TabStop = true;
			this.ApplyPatchButton.Text = "Apply Patch Mode";
			this.ApplyPatchButton.UseVisualStyleBackColor = true;
			this.ApplyPatchButton.CheckedChanged += new System.EventHandler(this.ApplyPatchButton_CheckedChanged);
			// 
			// BrowsePatchOrRom
			// 
			this.BrowsePatchOrRom.Location = new System.Drawing.Point(660, 41);
			this.BrowsePatchOrRom.Name = "BrowsePatchOrRom";
			this.BrowsePatchOrRom.Size = new System.Drawing.Size(90, 23);
			this.BrowsePatchOrRom.TabIndex = 21;
			this.BrowsePatchOrRom.Text = "Browse";
			this.BrowsePatchOrRom.UseVisualStyleBackColor = true;
			this.BrowsePatchOrRom.Click += new System.EventHandler(this.BrowsePatchOrRom_Click);
			// 
			// BpsPatchAndPatchedRomPath
			// 
			this.BpsPatchAndPatchedRomPath.Location = new System.Drawing.Point(153, 42);
			this.BpsPatchAndPatchedRomPath.Name = "BpsPatchAndPatchedRomPath";
			this.BpsPatchAndPatchedRomPath.Size = new System.Drawing.Size(501, 23);
			this.BpsPatchAndPatchedRomPath.TabIndex = 16;
			// 
			// BpsPatchAndPatchedRomLabel
			// 
			this.BpsPatchAndPatchedRomLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.BpsPatchAndPatchedRomLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.BpsPatchAndPatchedRomLabel.Location = new System.Drawing.Point(10, 45);
			this.BpsPatchAndPatchedRomLabel.Name = "BpsPatchAndPatchedRomLabel";
			this.BpsPatchAndPatchedRomLabel.Size = new System.Drawing.Size(133, 15);
			this.BpsPatchAndPatchedRomLabel.TabIndex = 16;
			this.BpsPatchAndPatchedRomLabel.Text = "BPS Patch File Path:";
			this.BpsPatchAndPatchedRomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label11.Location = new System.Drawing.Point(17, 232);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(62, 15);
			this.label11.TabIndex = 19;
			this.label11.Text = "Cosmetics";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel8
			// 
			this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel8.Controls.Add(this.DeleteCosmeticPreset);
			this.panel8.Controls.Add(this.ResetDefaultCosmetics);
			this.panel8.Controls.Add(this.LoadCosmeticPreset);
			this.panel8.Controls.Add(this.CosmeticsPresets);
			this.panel8.Controls.Add(this.label12);
			this.panel8.Controls.Add(this.SaveCosmeticPreset);
			this.panel8.Controls.Add(this.GenerateCosmetics);
			this.panel8.Controls.Add(this.LoadCosmetics);
			this.panel8.Controls.Add(this.CosmeticsString);
			this.panel8.Controls.Add(this.label13);
			this.panel8.Location = new System.Drawing.Point(6, 240);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(760, 140);
			this.panel8.TabIndex = 18;
			// 
			// DeleteCosmeticPreset
			// 
			this.DeleteCosmeticPreset.Location = new System.Drawing.Point(510, 100);
			this.DeleteCosmeticPreset.Name = "DeleteCosmeticPreset";
			this.DeleteCosmeticPreset.Size = new System.Drawing.Size(240, 23);
			this.DeleteCosmeticPreset.TabIndex = 30;
			this.DeleteCosmeticPreset.Text = "Delete Preset";
			this.DeleteCosmeticPreset.UseVisualStyleBackColor = true;
			this.DeleteCosmeticPreset.Click += new System.EventHandler(this.DeleteCosmeticPreset_Click);
			// 
			// ResetDefaultCosmetics
			// 
			this.ResetDefaultCosmetics.Location = new System.Drawing.Point(510, 40);
			this.ResetDefaultCosmetics.Name = "ResetDefaultCosmetics";
			this.ResetDefaultCosmetics.Size = new System.Drawing.Size(240, 23);
			this.ResetDefaultCosmetics.TabIndex = 29;
			this.ResetDefaultCosmetics.Text = "Reset Defaults";
			this.ResetDefaultCosmetics.UseVisualStyleBackColor = true;
			this.ResetDefaultCosmetics.Click += new System.EventHandler(this.ResetDefaultCosmetics_Click);
			// 
			// LoadCosmeticPreset
			// 
			this.LoadCosmeticPreset.Location = new System.Drawing.Point(10, 100);
			this.LoadCosmeticPreset.Name = "LoadCosmeticPreset";
			this.LoadCosmeticPreset.Size = new System.Drawing.Size(240, 23);
			this.LoadCosmeticPreset.TabIndex = 28;
			this.LoadCosmeticPreset.Text = "Load Cosmetics Preset";
			this.LoadCosmeticPreset.UseVisualStyleBackColor = true;
			this.LoadCosmeticPreset.Click += new System.EventHandler(this.LoadCosmeticPreset_Click);
			// 
			// CosmeticsPresets
			// 
			this.CosmeticsPresets.FormattingEnabled = true;
			this.CosmeticsPresets.Location = new System.Drawing.Point(153, 72);
			this.CosmeticsPresets.Name = "CosmeticsPresets";
			this.CosmeticsPresets.Size = new System.Drawing.Size(597, 23);
			this.CosmeticsPresets.TabIndex = 27;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label12.Location = new System.Drawing.Point(10, 75);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(133, 15);
			this.label12.TabIndex = 26;
			this.label12.Text = "Cosmetics Presets:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SaveCosmeticPreset
			// 
			this.SaveCosmeticPreset.Location = new System.Drawing.Point(260, 100);
			this.SaveCosmeticPreset.Name = "SaveCosmeticPreset";
			this.SaveCosmeticPreset.Size = new System.Drawing.Size(240, 23);
			this.SaveCosmeticPreset.TabIndex = 25;
			this.SaveCosmeticPreset.Text = "Save Current As Preset";
			this.SaveCosmeticPreset.UseVisualStyleBackColor = true;
			this.SaveCosmeticPreset.Click += new System.EventHandler(this.SaveCosmeticPreset_Click);
			// 
			// GenerateCosmetics
			// 
			this.GenerateCosmetics.Location = new System.Drawing.Point(260, 40);
			this.GenerateCosmetics.Name = "GenerateCosmetics";
			this.GenerateCosmetics.Size = new System.Drawing.Size(240, 23);
			this.GenerateCosmetics.TabIndex = 24;
			this.GenerateCosmetics.Text = "Generate String";
			this.GenerateCosmetics.UseVisualStyleBackColor = true;
			this.GenerateCosmetics.Click += new System.EventHandler(this.GenerateCosmetics_Click);
			// 
			// LoadCosmetics
			// 
			this.LoadCosmetics.Location = new System.Drawing.Point(10, 40);
			this.LoadCosmetics.Name = "LoadCosmetics";
			this.LoadCosmetics.Size = new System.Drawing.Size(240, 23);
			this.LoadCosmetics.TabIndex = 23;
			this.LoadCosmetics.Text = "Load Cosmetics";
			this.LoadCosmetics.UseVisualStyleBackColor = true;
			this.LoadCosmetics.Click += new System.EventHandler(this.LoadCosmetics_Click);
			// 
			// CosmeticsString
			// 
			this.CosmeticsString.Location = new System.Drawing.Point(153, 12);
			this.CosmeticsString.Name = "CosmeticsString";
			this.CosmeticsString.PlaceholderText = "Paste cosmetics string here and click Load Cosmetics to load";
			this.CosmeticsString.Size = new System.Drawing.Size(597, 23);
			this.CosmeticsString.TabIndex = 16;
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label13.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label13.Location = new System.Drawing.Point(10, 15);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(133, 15);
			this.label13.TabIndex = 16;
			this.label13.Text = "Cosmetics String:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label8.Location = new System.Drawing.Point(17, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(49, 15);
			this.label8.TabIndex = 17;
			this.label8.Text = "Settings";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel7
			// 
			this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel7.Controls.Add(this.DeleteSettingPreset);
			this.panel7.Controls.Add(this.ResetDefaultSettings);
			this.panel7.Controls.Add(this.LoadSettingPreset);
			this.panel7.Controls.Add(this.SettingPresets);
			this.panel7.Controls.Add(this.label9);
			this.panel7.Controls.Add(this.SaveSettingPreset);
			this.panel7.Controls.Add(this.GenerateSettings);
			this.panel7.Controls.Add(this.LoadSettings);
			this.panel7.Controls.Add(this.SettingString);
			this.panel7.Controls.Add(this.label10);
			this.panel7.Location = new System.Drawing.Point(6, 80);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(760, 140);
			this.panel7.TabIndex = 16;
			// 
			// DeleteSettingPreset
			// 
			this.DeleteSettingPreset.Location = new System.Drawing.Point(510, 100);
			this.DeleteSettingPreset.Name = "DeleteSettingPreset";
			this.DeleteSettingPreset.Size = new System.Drawing.Size(240, 23);
			this.DeleteSettingPreset.TabIndex = 30;
			this.DeleteSettingPreset.Text = "Delete Preset";
			this.DeleteSettingPreset.UseVisualStyleBackColor = true;
			this.DeleteSettingPreset.Click += new System.EventHandler(this.DeleteSettingPreset_Click);
			// 
			// ResetDefaultSettings
			// 
			this.ResetDefaultSettings.Location = new System.Drawing.Point(510, 40);
			this.ResetDefaultSettings.Name = "ResetDefaultSettings";
			this.ResetDefaultSettings.Size = new System.Drawing.Size(240, 23);
			this.ResetDefaultSettings.TabIndex = 29;
			this.ResetDefaultSettings.Text = "Reset Defaults";
			this.ResetDefaultSettings.UseVisualStyleBackColor = true;
			this.ResetDefaultSettings.Click += new System.EventHandler(this.ResetDefaultSettings_Click);
			// 
			// LoadSettingPreset
			// 
			this.LoadSettingPreset.Location = new System.Drawing.Point(10, 100);
			this.LoadSettingPreset.Name = "LoadSettingPreset";
			this.LoadSettingPreset.Size = new System.Drawing.Size(240, 23);
			this.LoadSettingPreset.TabIndex = 28;
			this.LoadSettingPreset.Text = "Load Settings Preset";
			this.LoadSettingPreset.UseVisualStyleBackColor = true;
			this.LoadSettingPreset.Click += new System.EventHandler(this.LoadSettingPreset_Click);
			// 
			// SettingPresets
			// 
			this.SettingPresets.FormattingEnabled = true;
			this.SettingPresets.Location = new System.Drawing.Point(153, 72);
			this.SettingPresets.Name = "SettingPresets";
			this.SettingPresets.Size = new System.Drawing.Size(597, 23);
			this.SettingPresets.TabIndex = 27;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label9.Location = new System.Drawing.Point(10, 75);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(133, 15);
			this.label9.TabIndex = 26;
			this.label9.Text = "Settings Presets:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SaveSettingPreset
			// 
			this.SaveSettingPreset.Location = new System.Drawing.Point(260, 100);
			this.SaveSettingPreset.Name = "SaveSettingPreset";
			this.SaveSettingPreset.Size = new System.Drawing.Size(240, 23);
			this.SaveSettingPreset.TabIndex = 25;
			this.SaveSettingPreset.Text = "Save Current As Preset";
			this.SaveSettingPreset.UseVisualStyleBackColor = true;
			this.SaveSettingPreset.Click += new System.EventHandler(this.SaveSettingPreset_Click);
			// 
			// GenerateSettings
			// 
			this.GenerateSettings.Location = new System.Drawing.Point(260, 40);
			this.GenerateSettings.Name = "GenerateSettings";
			this.GenerateSettings.Size = new System.Drawing.Size(240, 23);
			this.GenerateSettings.TabIndex = 24;
			this.GenerateSettings.Text = "Generate String";
			this.GenerateSettings.UseVisualStyleBackColor = true;
			this.GenerateSettings.Click += new System.EventHandler(this.GenerateSettings_Click);
			// 
			// LoadSettings
			// 
			this.LoadSettings.Location = new System.Drawing.Point(10, 40);
			this.LoadSettings.Name = "LoadSettings";
			this.LoadSettings.Size = new System.Drawing.Size(240, 23);
			this.LoadSettings.TabIndex = 23;
			this.LoadSettings.Text = "Load Settings";
			this.LoadSettings.UseVisualStyleBackColor = true;
			this.LoadSettings.Click += new System.EventHandler(this.LoadSettings_Click);
			// 
			// SettingString
			// 
			this.SettingString.Location = new System.Drawing.Point(153, 12);
			this.SettingString.Name = "SettingString";
			this.SettingString.PlaceholderText = "Paste settings string here and click Load Settings to load";
			this.SettingString.Size = new System.Drawing.Size(597, 23);
			this.SettingString.TabIndex = 16;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label10.Location = new System.Drawing.Point(10, 15);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(133, 15);
			this.label10.TabIndex = 16;
			this.label10.Text = "Settings String:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RandomSeed
			// 
			this.RandomSeed.Location = new System.Drawing.Point(676, 36);
			this.RandomSeed.Name = "RandomSeed";
			this.RandomSeed.Size = new System.Drawing.Size(90, 23);
			this.RandomSeed.TabIndex = 13;
			this.RandomSeed.Text = "New Seed";
			this.RandomSeed.UseVisualStyleBackColor = true;
			this.RandomSeed.Click += new System.EventHandler(this.RandomSeed_Click);
			// 
			// Seed
			// 
			this.Seed.Location = new System.Drawing.Point(160, 37);
			this.Seed.MaxLength = 16;
			this.Seed.Name = "Seed";
			this.Seed.Size = new System.Drawing.Size(510, 23);
			this.Seed.TabIndex = 12;
			// 
			// RomPath
			// 
			this.RomPath.Location = new System.Drawing.Point(160, 7);
			this.RomPath.Name = "RomPath";
			this.RomPath.Size = new System.Drawing.Size(510, 23);
			this.RomPath.TabIndex = 10;
			// 
			// BrowseRom
			// 
			this.BrowseRom.Location = new System.Drawing.Point(676, 6);
			this.BrowseRom.Name = "BrowseRom";
			this.BrowseRom.Size = new System.Drawing.Size(90, 23);
			this.BrowseRom.TabIndex = 11;
			this.BrowseRom.Text = "Browse";
			this.BrowseRom.UseVisualStyleBackColor = true;
			this.BrowseRom.Click += new System.EventHandler(this.BrowseRom_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label3.Location = new System.Drawing.Point(4, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 15);
			this.label3.TabIndex = 9;
			this.label3.Text = "European Minish Cap ROM";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(6, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(148, 15);
			this.label4.TabIndex = 8;
			this.label4.Text = "Randomizer Seed";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TabPane
			// 
			this.TabPane.Controls.Add(this.General);
			this.TabPane.Controls.Add(this.Advanced);
			this.TabPane.Controls.Add(this.SeedOutput);
			this.TabPane.Location = new System.Drawing.Point(12, 27);
			this.TabPane.Name = "TabPane";
			this.TabPane.SelectedIndex = 0;
			this.TabPane.Size = new System.Drawing.Size(795, 563);
			this.TabPane.TabIndex = 1;
			// 
			// Advanced
			// 
			this.Advanced.Controls.Add(this.label25);
			this.Advanced.Controls.Add(this.panel4);
			this.Advanced.Controls.Add(this.label19);
			this.Advanced.Controls.Add(this.label5);
			this.Advanced.Controls.Add(this.panel6);
			this.Advanced.Location = new System.Drawing.Point(4, 24);
			this.Advanced.Name = "Advanced";
			this.Advanced.Padding = new System.Windows.Forms.Padding(3);
			this.Advanced.Size = new System.Drawing.Size(787, 535);
			this.Advanced.TabIndex = 3;
			this.Advanced.Text = "Advanced";
			this.Advanced.UseVisualStyleBackColor = true;
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label25.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label25.Location = new System.Drawing.Point(17, 217);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(123, 15);
			this.label25.TabIndex = 20;
			this.label25.Text = "Experimental Features";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel4
			// 
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel4.Controls.Add(this.UseSphereBasedShuffler);
			this.panel4.Location = new System.Drawing.Point(6, 225);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(760, 50);
			this.panel4.TabIndex = 19;
			// 
			// UseSphereBasedShuffler
			// 
			this.UseSphereBasedShuffler.AutoSize = true;
			this.UseSphereBasedShuffler.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseSphereBasedShuffler.Location = new System.Drawing.Point(10, 15);
			this.UseSphereBasedShuffler.Name = "UseSphereBasedShuffler";
			this.UseSphereBasedShuffler.Size = new System.Drawing.Size(245, 19);
			this.UseSphereBasedShuffler.TabIndex = 28;
			this.UseSphereBasedShuffler.Text = "Use Hendrus Seed Shuffler (Experimental)";
			this.UseSphereBasedShuffler.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseSphereBasedShuffler.UseVisualStyleBackColor = true;
			this.UseSphereBasedShuffler.CheckedChanged += new System.EventHandler(this.UseSphereBasedShuffler_CheckedChanged);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(17, 15);
			this.label19.MaximumSize = new System.Drawing.Size(760, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(723, 30);
			this.label19.TabIndex = 18;
			this.label19.Text = "This page contains advanced settings and experimental features for the randomizer" +
    ". Unless you are experienced with the randomizer we recommend not modifying thes" +
    "e settings.";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label5.Location = new System.Drawing.Point(17, 57);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(148, 15);
			this.label5.TabIndex = 17;
			this.label5.Text = "Custom Logic and Patches";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel6.Controls.Add(this.BrowseCustomPatch);
			this.panel6.Controls.Add(this.RomBuildfilePath);
			this.panel6.Controls.Add(this.label7);
			this.panel6.Controls.Add(this.UseCustomPatch);
			this.panel6.Controls.Add(this.BrowseCustomLogicFile);
			this.panel6.Controls.Add(this.LogicFilePath);
			this.panel6.Controls.Add(this.UseCustomLogic);
			this.panel6.Controls.Add(this.label6);
			this.panel6.Location = new System.Drawing.Point(6, 65);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(760, 140);
			this.panel6.TabIndex = 16;
			// 
			// BrowseCustomPatch
			// 
			this.BrowseCustomPatch.Location = new System.Drawing.Point(660, 101);
			this.BrowseCustomPatch.Name = "BrowseCustomPatch";
			this.BrowseCustomPatch.Size = new System.Drawing.Size(90, 23);
			this.BrowseCustomPatch.TabIndex = 20;
			this.BrowseCustomPatch.Text = "Browse";
			this.BrowseCustomPatch.UseVisualStyleBackColor = true;
			this.BrowseCustomPatch.Click += new System.EventHandler(this.BrowseCustomPatch_Click);
			// 
			// RomBuildfilePath
			// 
			this.RomBuildfilePath.Enabled = false;
			this.RomBuildfilePath.Location = new System.Drawing.Point(153, 102);
			this.RomBuildfilePath.Name = "RomBuildfilePath";
			this.RomBuildfilePath.Size = new System.Drawing.Size(501, 23);
			this.RomBuildfilePath.TabIndex = 21;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label7.Location = new System.Drawing.Point(10, 105);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(133, 15);
			this.label7.TabIndex = 22;
			this.label7.Text = "ROM Buildfile Path:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// UseCustomPatch
			// 
			this.UseCustomPatch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomPatch.Location = new System.Drawing.Point(10, 75);
			this.UseCustomPatch.Name = "UseCustomPatch";
			this.UseCustomPatch.Size = new System.Drawing.Size(160, 19);
			this.UseCustomPatch.TabIndex = 19;
			this.UseCustomPatch.Text = "Use Custom Patch Files";
			this.UseCustomPatch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomPatch.UseVisualStyleBackColor = true;
			this.UseCustomPatch.CheckedChanged += new System.EventHandler(this.UseCustomPatch_CheckedChanged);
			// 
			// BrowseCustomLogicFile
			// 
			this.BrowseCustomLogicFile.Location = new System.Drawing.Point(660, 41);
			this.BrowseCustomLogicFile.Name = "BrowseCustomLogicFile";
			this.BrowseCustomLogicFile.Size = new System.Drawing.Size(90, 23);
			this.BrowseCustomLogicFile.TabIndex = 16;
			this.BrowseCustomLogicFile.Text = "Browse";
			this.BrowseCustomLogicFile.UseVisualStyleBackColor = true;
			this.BrowseCustomLogicFile.Click += new System.EventHandler(this.BrowseCustomLogicFile_Click);
			// 
			// LogicFilePath
			// 
			this.LogicFilePath.Enabled = false;
			this.LogicFilePath.Location = new System.Drawing.Point(153, 42);
			this.LogicFilePath.Name = "LogicFilePath";
			this.LogicFilePath.Size = new System.Drawing.Size(501, 23);
			this.LogicFilePath.TabIndex = 16;
			// 
			// UseCustomLogic
			// 
			this.UseCustomLogic.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomLogic.Location = new System.Drawing.Point(10, 15);
			this.UseCustomLogic.Name = "UseCustomLogic";
			this.UseCustomLogic.Size = new System.Drawing.Size(160, 19);
			this.UseCustomLogic.TabIndex = 17;
			this.UseCustomLogic.Text = "Use Custom Logic File";
			this.UseCustomLogic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomLogic.UseVisualStyleBackColor = true;
			this.UseCustomLogic.CheckedChanged += new System.EventHandler(this.UseCustomLogic_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label6.Location = new System.Drawing.Point(10, 45);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(133, 15);
			this.label6.TabIndex = 16;
			this.label6.Text = "Logic File Path:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SeedOutput
			// 
			this.SeedOutput.Controls.Add(this.SaveRom);
			this.SeedOutput.Controls.Add(this.SavePatch);
			this.SeedOutput.Controls.Add(this.SaveSpoiler);
			this.SeedOutput.Controls.Add(this.label17);
			this.SeedOutput.Controls.Add(this.panel3);
			this.SeedOutput.Controls.Add(this.label22);
			this.SeedOutput.Controls.Add(this.panel2);
			this.SeedOutput.Controls.Add(this.label1);
			this.SeedOutput.Controls.Add(this.panel1);
			this.SeedOutput.Location = new System.Drawing.Point(4, 24);
			this.SeedOutput.Name = "SeedOutput";
			this.SeedOutput.Size = new System.Drawing.Size(787, 535);
			this.SeedOutput.TabIndex = 2;
			this.SeedOutput.Text = "Seed Output";
			this.SeedOutput.UseVisualStyleBackColor = true;
			// 
			// SaveRom
			// 
			this.SaveRom.Location = new System.Drawing.Point(519, 500);
			this.SaveRom.Name = "SaveRom";
			this.SaveRom.Size = new System.Drawing.Size(247, 23);
			this.SaveRom.TabIndex = 24;
			this.SaveRom.Text = "Save ROM";
			this.SaveRom.UseVisualStyleBackColor = true;
			this.SaveRom.Click += new System.EventHandler(this.SaveRom_Click);
			// 
			// SavePatch
			// 
			this.SavePatch.Location = new System.Drawing.Point(263, 500);
			this.SavePatch.Name = "SavePatch";
			this.SavePatch.Size = new System.Drawing.Size(246, 23);
			this.SavePatch.TabIndex = 23;
			this.SavePatch.Text = "Save BPS Patch";
			this.SavePatch.UseVisualStyleBackColor = true;
			this.SavePatch.Click += new System.EventHandler(this.SavePatch_Click);
			// 
			// SaveSpoiler
			// 
			this.SaveSpoiler.Location = new System.Drawing.Point(6, 500);
			this.SaveSpoiler.Name = "SaveSpoiler";
			this.SaveSpoiler.Size = new System.Drawing.Size(247, 23);
			this.SaveSpoiler.TabIndex = 22;
			this.SaveSpoiler.Text = "Save Spoiler Log";
			this.SaveSpoiler.UseVisualStyleBackColor = true;
			this.SaveSpoiler.Click += new System.EventHandler(this.SaveSpoiler_Click);
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label17.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label17.Location = new System.Drawing.Point(17, 282);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(128, 15);
			this.label17.TabIndex = 21;
			this.label17.Text = "Cosmetics Information";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.CopyCosmeticsHashToClipboard);
			this.panel3.Controls.Add(this.CosmeticStringLabel);
			this.panel3.Controls.Add(this.label23);
			this.panel3.Controls.Add(this.CosmeticNameLabel);
			this.panel3.Controls.Add(this.label26);
			this.panel3.Location = new System.Drawing.Point(6, 290);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(760, 105);
			this.panel3.TabIndex = 20;
			// 
			// CopyCosmeticsHashToClipboard
			// 
			this.CopyCosmeticsHashToClipboard.Location = new System.Drawing.Point(525, 70);
			this.CopyCosmeticsHashToClipboard.Name = "CopyCosmeticsHashToClipboard";
			this.CopyCosmeticsHashToClipboard.Size = new System.Drawing.Size(225, 23);
			this.CopyCosmeticsHashToClipboard.TabIndex = 24;
			this.CopyCosmeticsHashToClipboard.Text = "Copy Cosmetics String To Clipboard";
			this.CopyCosmeticsHashToClipboard.UseVisualStyleBackColor = true;
			this.CopyCosmeticsHashToClipboard.Click += new System.EventHandler(this.CopyCosmeticsHashToClipboard_Click);
			// 
			// CosmeticStringLabel
			// 
			this.CosmeticStringLabel.AutoEllipsis = true;
			this.CosmeticStringLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticStringLabel.Location = new System.Drawing.Point(198, 45);
			this.CosmeticStringLabel.Name = "CosmeticStringLabel";
			this.CosmeticStringLabel.Size = new System.Drawing.Size(552, 15);
			this.CosmeticStringLabel.TabIndex = 23;
			this.CosmeticStringLabel.Text = "Cosmetics String Goes Here";
			this.CosmeticStringLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label23.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label23.Location = new System.Drawing.Point(10, 45);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(178, 15);
			this.label23.TabIndex = 22;
			this.label23.Text = "Cosmetics String:";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CosmeticNameLabel
			// 
			this.CosmeticNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticNameLabel.Location = new System.Drawing.Point(198, 15);
			this.CosmeticNameLabel.Name = "CosmeticNameLabel";
			this.CosmeticNameLabel.Size = new System.Drawing.Size(552, 15);
			this.CosmeticNameLabel.TabIndex = 19;
			this.CosmeticNameLabel.Text = "None";
			this.CosmeticNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label26.Location = new System.Drawing.Point(10, 15);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(178, 15);
			this.label26.TabIndex = 18;
			this.label26.Text = "Cosmetics Name:";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label22.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label22.Location = new System.Drawing.Point(17, 157);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(115, 15);
			this.label22.TabIndex = 19;
			this.label22.Text = "Settings Information";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.CopySettingsHashToClipboard);
			this.panel2.Controls.Add(this.SettingHashLabel);
			this.panel2.Controls.Add(this.label24);
			this.panel2.Controls.Add(this.SettingNameLabel);
			this.panel2.Controls.Add(this.label28);
			this.panel2.Location = new System.Drawing.Point(6, 165);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(760, 105);
			this.panel2.TabIndex = 18;
			// 
			// CopySettingsHashToClipboard
			// 
			this.CopySettingsHashToClipboard.Location = new System.Drawing.Point(525, 70);
			this.CopySettingsHashToClipboard.Name = "CopySettingsHashToClipboard";
			this.CopySettingsHashToClipboard.Size = new System.Drawing.Size(225, 23);
			this.CopySettingsHashToClipboard.TabIndex = 24;
			this.CopySettingsHashToClipboard.Text = "Copy Settings String To Clipboard";
			this.CopySettingsHashToClipboard.UseVisualStyleBackColor = true;
			this.CopySettingsHashToClipboard.Click += new System.EventHandler(this.CopySettingsHashToClipboard_Click);
			// 
			// SettingHashLabel
			// 
			this.SettingHashLabel.AutoEllipsis = true;
			this.SettingHashLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingHashLabel.Location = new System.Drawing.Point(198, 45);
			this.SettingHashLabel.Name = "SettingHashLabel";
			this.SettingHashLabel.Size = new System.Drawing.Size(552, 15);
			this.SettingHashLabel.TabIndex = 23;
			this.SettingHashLabel.Text = "Settings String Goes Here";
			this.SettingHashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label24.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label24.Location = new System.Drawing.Point(10, 45);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(178, 15);
			this.label24.TabIndex = 22;
			this.label24.Text = "Settings String:";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SettingNameLabel
			// 
			this.SettingNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingNameLabel.Location = new System.Drawing.Point(198, 15);
			this.SettingNameLabel.Name = "SettingNameLabel";
			this.SettingNameLabel.Size = new System.Drawing.Size(552, 15);
			this.SettingNameLabel.TabIndex = 19;
			this.SettingNameLabel.Text = "None";
			this.SettingNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label28.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label28.Location = new System.Drawing.Point(10, 15);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(178, 15);
			this.label28.TabIndex = 18;
			this.label28.Text = "Settings Name:";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label1.Location = new System.Drawing.Point(17, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 15);
			this.label1.TabIndex = 17;
			this.label1.Text = "Seed Information";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.CopyHashToClipboard);
			this.panel1.Controls.Add(this.ImagePanel);
			this.panel1.Controls.Add(this.label20);
			this.panel1.Controls.Add(this.OutputSeedLabel);
			this.panel1.Controls.Add(this.label18);
			this.panel1.Controls.Add(this.InputSeedLabel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Location = new System.Drawing.Point(6, 15);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(760, 135);
			this.panel1.TabIndex = 16;
			// 
			// CopyHashToClipboard
			// 
			this.CopyHashToClipboard.Location = new System.Drawing.Point(525, 100);
			this.CopyHashToClipboard.Name = "CopyHashToClipboard";
			this.CopyHashToClipboard.Size = new System.Drawing.Size(225, 23);
			this.CopyHashToClipboard.TabIndex = 25;
			this.CopyHashToClipboard.Text = "Copy Hash To Clipboard";
			this.CopyHashToClipboard.UseVisualStyleBackColor = true;
			this.CopyHashToClipboard.Click += new System.EventHandler(this.CopyHashToClipboard_Click);
			// 
			// ImagePanel
			// 
			this.ImagePanel.Location = new System.Drawing.Point(230, 42);
			this.ImagePanel.Margin = new System.Windows.Forms.Padding(0);
			this.ImagePanel.Name = "ImagePanel";
			this.ImagePanel.Size = new System.Drawing.Size(384, 48);
			this.ImagePanel.TabIndex = 23;
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label20.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label20.Location = new System.Drawing.Point(10, 59);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(200, 15);
			this.label20.TabIndex = 22;
			this.label20.Text = "ROM Hash:";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// OutputSeedLabel
			// 
			this.OutputSeedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.OutputSeedLabel.Location = new System.Drawing.Point(573, 15);
			this.OutputSeedLabel.Name = "OutputSeedLabel";
			this.OutputSeedLabel.Size = new System.Drawing.Size(177, 15);
			this.OutputSeedLabel.TabIndex = 21;
			this.OutputSeedLabel.Text = "None";
			this.OutputSeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label18.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label18.Location = new System.Drawing.Point(385, 15);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(178, 15);
			this.label18.TabIndex = 20;
			this.label18.Text = "Output Seed Number:";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// InputSeedLabel
			// 
			this.InputSeedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.InputSeedLabel.Location = new System.Drawing.Point(198, 15);
			this.InputSeedLabel.Name = "InputSeedLabel";
			this.InputSeedLabel.Size = new System.Drawing.Size(177, 15);
			this.InputSeedLabel.TabIndex = 19;
			this.InputSeedLabel.Text = "None";
			this.InputSeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label2.Location = new System.Drawing.Point(10, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(178, 15);
			this.label2.TabIndex = 18;
			this.label2.Text = "Input Seed Number:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label16.Location = new System.Drawing.Point(16, 600);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(168, 15);
			this.label16.TabIndex = 26;
			this.label16.Text = "Max Randomization Attempts:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RandomizationAttempts
			// 
			this.RandomizationAttempts.Location = new System.Drawing.Point(190, 596);
			this.RandomizationAttempts.Name = "RandomizationAttempts";
			this.RandomizationAttempts.Size = new System.Drawing.Size(136, 23);
			this.RandomizationAttempts.TabIndex = 27;
			this.RandomizationAttempts.Text = "1";
			this.RandomizationAttempts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// MinishCapRandomizerUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(816, 631);
			this.Controls.Add(this.RandomizationAttempts);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.Randomize);
			this.Controls.Add(this.TabPane);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "MinishCapRandomizerUI";
			this.Text = "Need to make the title dynamic soon™";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.General.ResumeLayout(false);
			this.General.PerformLayout();
			this.panel9.ResumeLayout(false);
			this.panel9.PerformLayout();
			this.panel8.ResumeLayout(false);
			this.panel8.PerformLayout();
			this.panel7.ResumeLayout(false);
			this.panel7.PerformLayout();
			this.TabPane.ResumeLayout(false);
			this.Advanced.ResumeLayout(false);
			this.Advanced.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.SeedOutput.ResumeLayout(false);
			this.SeedOutput.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem logicToolStripMenuItem;
        private Button Randomize;
        private OpenFileDialog openFileDialog1;
        private ColorDialog colorDialog1;
        private ToolStripMenuItem exportDefaultLogicToolStripMenuItem;
        private ToolStripMenuItem loggingToolStripMenuItem;
        private ToolStripMenuItem setLoggerOutputPathToolStripMenuItem;
        private ToolStripMenuItem logAllTransactionsToolStripMenuItem;
        private ToolStripMenuItem localizationToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem changelogToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private TabPage General;
        private Label label14;
        private Panel panel9;
        private Button PatchRomAndGenPatch;
        private RadioButton GeneratePatchButton;
        private Label label15;
        private RadioButton ApplyPatchButton;
        private Button BrowsePatchOrRom;
        private TextBox BpsPatchAndPatchedRomPath;
        private Label BpsPatchAndPatchedRomLabel;
        private Label label11;
        private Panel panel8;
        private Button DeleteCosmeticPreset;
        private Button ResetDefaultCosmetics;
        private Button LoadCosmeticPreset;
        private ComboBox CosmeticsPresets;
        private Label label12;
        private Button SaveCosmeticPreset;
        private Button GenerateCosmetics;
        private Button LoadCosmetics;
        private TextBox CosmeticsString;
        private Label label13;
        private Label label8;
        private Panel panel7;
        private Button DeleteSettingPreset;
        private Button ResetDefaultSettings;
        private Button LoadSettingPreset;
        private ComboBox SettingPresets;
        private Label label9;
        private Button SaveSettingPreset;
        private Button GenerateSettings;
        private Button LoadSettings;
        private TextBox SettingString;
        private Label label10;
        private Button RandomSeed;
        private TextBox Seed;
        private TextBox RomPath;
        private Button BrowseRom;
        private Label label3;
        private Label label4;
        private TabControl TabPane;
        private TabPage SeedOutput;
        private Label label1;
        private Panel panel1;
        private Label label2;
        private Label InputSeedLabel;
        private Label label18;
        private Label OutputSeedLabel;
        private Label label20;
        private Label label22;
        private Panel panel2;
        private Label SettingHashLabel;
        private Label label24;
        private Label SettingNameLabel;
        private Label label28;
        private Button CopySettingsHashToClipboard;
        private Label label17;
        private Panel panel3;
        private Button CopyCosmeticsHashToClipboard;
        private Label CosmeticStringLabel;
        private Label label23;
        private Label CosmeticNameLabel;
        private Label label26;
        private Button SaveSpoiler;
        private Button SaveRom;
        private Button SavePatch;
        private ToolStripMenuItem writeAndFlushLoggerToolStripMenuItem;
        private Label label16;
        private TextBox RandomizationAttempts;
        private CheckBox UseSphereBasedShuffler;
		private TabPage Advanced;
		private Label label5;
		private Panel panel6;
		private Button BrowseCustomPatch;
		private TextBox RomBuildfilePath;
		private Label label7;
		private CheckBox UseCustomPatch;
		private Button BrowseCustomLogicFile;
		private TextBox LogicFilePath;
		private CheckBox UseCustomLogic;
		private Label label6;
		private Label label19;
		private CheckBox UseCustomCosmetics;
		private Label label25;
		private Panel panel4;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem otherToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesOnStartToolStripMenuItem;
        private FlowLayoutPanel ImagePanel;
        private Button CopyHashToClipboard;
    }
}
