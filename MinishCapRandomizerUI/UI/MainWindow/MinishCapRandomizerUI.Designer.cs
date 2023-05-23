namespace MinishCapRandomizerUI.UI.MainWindow
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
			this.savePresetYAMLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveMysteryYAMLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setLoggerOutputPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logAllTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeAndFlushLoggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.localizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.françaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deutschToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.espanolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.italianoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesOnStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Randomize = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.General = new System.Windows.Forms.TabPage();
			this.PatcherAndPatchGeneratorLabel = new System.Windows.Forms.Label();
			this.PatcherAndPatchGeneratorPanel = new System.Windows.Forms.Panel();
			this.UseCustomCosmetics = new System.Windows.Forms.CheckBox();
			this.PatchRomAndGenPatch = new System.Windows.Forms.Button();
			this.GeneratePatchButton = new System.Windows.Forms.RadioButton();
			this.PatcherModeLabel = new System.Windows.Forms.Label();
			this.ApplyPatchButton = new System.Windows.Forms.RadioButton();
			this.BrowsePatchOrRom = new System.Windows.Forms.Button();
			this.BpsPatchAndPatchedRomPath = new System.Windows.Forms.TextBox();
			this.BpsPatchAndPatchedRomLabel = new System.Windows.Forms.Label();
			this.CosmeticsPanelLabel = new System.Windows.Forms.Label();
			this.CosmeticsPanel = new System.Windows.Forms.Panel();
			this.DeleteCosmeticPreset = new System.Windows.Forms.Button();
			this.ResetDefaultCosmetics = new System.Windows.Forms.Button();
			this.LoadCosmeticPreset = new System.Windows.Forms.Button();
			this.CosmeticsPresets = new System.Windows.Forms.ComboBox();
			this.CosmeticsPresetLabel = new System.Windows.Forms.Label();
			this.SaveCosmeticPreset = new System.Windows.Forms.Button();
			this.GenerateCosmetics = new System.Windows.Forms.Button();
			this.LoadCosmetics = new System.Windows.Forms.Button();
			this.CosmeticsString = new System.Windows.Forms.TextBox();
			this.CosmeticsStringLabel = new System.Windows.Forms.Label();
			this.SettingsPaneLabel = new System.Windows.Forms.Label();
			this.SettingsPanel = new System.Windows.Forms.Panel();
			this.DeleteSettingPreset = new System.Windows.Forms.Button();
			this.ResetDefaultSettings = new System.Windows.Forms.Button();
			this.LoadSettingPreset = new System.Windows.Forms.Button();
			this.SettingPresets = new System.Windows.Forms.ComboBox();
			this.SettingsPresetLabel = new System.Windows.Forms.Label();
			this.SaveSettingPreset = new System.Windows.Forms.Button();
			this.GenerateSettings = new System.Windows.Forms.Button();
			this.LoadSettings = new System.Windows.Forms.Button();
			this.SettingString = new System.Windows.Forms.TextBox();
			this.SettingsStringLabel = new System.Windows.Forms.Label();
			this.RandomSeed = new System.Windows.Forms.Button();
			this.Seed = new System.Windows.Forms.TextBox();
			this.RomPath = new System.Windows.Forms.TextBox();
			this.BrowseRom = new System.Windows.Forms.Button();
			this.EuTmcRomLabel = new System.Windows.Forms.Label();
			this.RandoSeedLabel = new System.Windows.Forms.Label();
			this.TabPane = new System.Windows.Forms.TabControl();
			this.Advanced = new System.Windows.Forms.TabPage();
			this.AlternativeShufflersPanelLabel = new System.Windows.Forms.Label();
			this.AlternativeShufflerPanel = new System.Windows.Forms.Panel();
			this.UseSphereBasedShuffler = new System.Windows.Forms.CheckBox();
			this.AdvancedSettingsLabel = new System.Windows.Forms.Label();
			this.LogicPatchesYamlLabel = new System.Windows.Forms.Label();
			this.LogicPatchesYamlPanel = new System.Windows.Forms.Panel();
			this.BrowseCustomYAML = new System.Windows.Forms.Button();
			this.YAMLPath = new System.Windows.Forms.TextBox();
			this.YamlFilePathLabel = new System.Windows.Forms.Label();
			this.UseCustomYAML = new System.Windows.Forms.CheckBox();
			this.BrowseCustomPatch = new System.Windows.Forms.Button();
			this.RomBuildfilePath = new System.Windows.Forms.TextBox();
			this.RomBuildfinePathLabel = new System.Windows.Forms.Label();
			this.UseCustomPatch = new System.Windows.Forms.CheckBox();
			this.BrowseCustomLogicFile = new System.Windows.Forms.Button();
			this.LogicFilePath = new System.Windows.Forms.TextBox();
			this.UseCustomLogic = new System.Windows.Forms.CheckBox();
			this.LogicFilePathLabel = new System.Windows.Forms.Label();
			this.MysterySettingsLabel = new System.Windows.Forms.Label();
			this.MysterySettingsPanel = new System.Windows.Forms.Panel();
			this.UseMysteryCosmetics = new System.Windows.Forms.CheckBox();
			this.LoadCosmeticSample = new System.Windows.Forms.Button();
			this.CosmeticsWeights = new System.Windows.Forms.ComboBox();
			this.UseMysterySettings = new System.Windows.Forms.CheckBox();
			this.LoadSettingSample = new System.Windows.Forms.Button();
			this.SettingsWeights = new System.Windows.Forms.ComboBox();
			this.SeedOutput = new System.Windows.Forms.TabPage();
			this.SaveRom = new System.Windows.Forms.Button();
			this.SavePatch = new System.Windows.Forms.Button();
			this.SaveSpoiler = new System.Windows.Forms.Button();
			this.CosmeticsInformationPanelLabel = new System.Windows.Forms.Label();
			this.CosmeticsInformationPanel = new System.Windows.Forms.Panel();
			this.CopyCosmeticsHashToClipboard = new System.Windows.Forms.Button();
			this.CosmeticStringLabel = new System.Windows.Forms.Label();
			this.CosmeticsStringSeedOutputLabel = new System.Windows.Forms.Label();
			this.CosmeticNameLabel = new System.Windows.Forms.Label();
			this.CosmeticsNameLabel = new System.Windows.Forms.Label();
			this.SettingsInformationPanelLabel = new System.Windows.Forms.Label();
			this.SettingsInformationPanel = new System.Windows.Forms.Panel();
			this.CopySettingsHashToClipboard = new System.Windows.Forms.Button();
			this.SettingHashLabel = new System.Windows.Forms.Label();
			this.SettingsStringSeedOutputLabel = new System.Windows.Forms.Label();
			this.SettingNameLabel = new System.Windows.Forms.Label();
			this.SettingsNameLabel = new System.Windows.Forms.Label();
			this.SeedInformationPanelLabel = new System.Windows.Forms.Label();
			this.SeedInformationPanel = new System.Windows.Forms.Panel();
			this.YamlHashNotShownLabel = new System.Windows.Forms.Label();
			this.CopyHashToClipboard = new System.Windows.Forms.Button();
			this.RomHashPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.RomHashLabel = new System.Windows.Forms.Label();
			this.OutputSeedLabel = new System.Windows.Forms.Label();
			this.OutputSeedNumberLabel = new System.Windows.Forms.Label();
			this.InputSeedLabel = new System.Windows.Forms.Label();
			this.InputSeedNumberLabel = new System.Windows.Forms.Label();
			this.MaxRandomizationAttemptsLabel = new System.Windows.Forms.Label();
			this.RandomizationAttempts = new System.Windows.Forms.TextBox();
			this.menuStrip1.SuspendLayout();
			this.General.SuspendLayout();
			this.PatcherAndPatchGeneratorPanel.SuspendLayout();
			this.CosmeticsPanel.SuspendLayout();
			this.SettingsPanel.SuspendLayout();
			this.TabPane.SuspendLayout();
			this.Advanced.SuspendLayout();
			this.AlternativeShufflerPanel.SuspendLayout();
			this.LogicPatchesYamlPanel.SuspendLayout();
			this.MysterySettingsPanel.SuspendLayout();
			this.SeedOutput.SuspendLayout();
			this.CosmeticsInformationPanel.SuspendLayout();
			this.SettingsInformationPanel.SuspendLayout();
			this.SeedInformationPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logicToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.localizationToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.otherToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(816, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// logicToolStripMenuItem
			// 
			this.logicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDefaultLogicToolStripMenuItem,
            this.savePresetYAMLMenuItem,
            this.saveMysteryYAMLMenuItem});
			this.logicToolStripMenuItem.Name = "logicToolStripMenuItem";
			this.logicToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.logicToolStripMenuItem.Text = "Logic";
			// 
			// exportDefaultLogicToolStripMenuItem
			// 
			this.exportDefaultLogicToolStripMenuItem.Name = "exportDefaultLogicToolStripMenuItem";
			this.exportDefaultLogicToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
			this.exportDefaultLogicToolStripMenuItem.Text = "Export Default Logic";
			this.exportDefaultLogicToolStripMenuItem.Click += new System.EventHandler(this.exportDefaultLogicToolStripMenuItem_Click);
			// 
			// savePresetYAMLMenuItem
			// 
			this.savePresetYAMLMenuItem.Name = "savePresetYAMLMenuItem";
			this.savePresetYAMLMenuItem.Size = new System.Drawing.Size(273, 22);
			this.savePresetYAMLMenuItem.Text = "Save Selected Options as YAML Preset";
			this.savePresetYAMLMenuItem.Click += new System.EventHandler(this.savePresetYAMLMenuItem_Click);
			// 
			// saveMysteryYAMLMenuItem
			// 
			this.saveMysteryYAMLMenuItem.Name = "saveMysteryYAMLMenuItem";
			this.saveMysteryYAMLMenuItem.Size = new System.Drawing.Size(273, 22);
			this.saveMysteryYAMLMenuItem.Text = "Save Mystery YAML template";
			this.saveMysteryYAMLMenuItem.Click += new System.EventHandler(this.saveMysteryYAMLMenuItem_Click);
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
            this.englishToolStripMenuItem,
            this.françaisToolStripMenuItem,
            this.deutschToolStripMenuItem,
            this.espanolToolStripMenuItem,
            this.italianoToolStripMenuItem,
            this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem});
			this.localizationToolStripMenuItem.Name = "localizationToolStripMenuItem";
			this.localizationToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
			this.localizationToolStripMenuItem.Text = "Localization";
			// 
			// englishToolStripMenuItem
			// 
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			this.englishToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.englishToolStripMenuItem.Text = "English";
			// 
			// françaisToolStripMenuItem
			// 
			this.françaisToolStripMenuItem.Enabled = false;
			this.françaisToolStripMenuItem.Name = "françaisToolStripMenuItem";
			this.françaisToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.françaisToolStripMenuItem.Text = "Français";
			// 
			// deutschToolStripMenuItem
			// 
			this.deutschToolStripMenuItem.Enabled = false;
			this.deutschToolStripMenuItem.Name = "deutschToolStripMenuItem";
			this.deutschToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.deutschToolStripMenuItem.Text = "Deutsch";
			// 
			// espanolToolStripMenuItem
			// 
			this.espanolToolStripMenuItem.Enabled = false;
			this.espanolToolStripMenuItem.Name = "espanolToolStripMenuItem";
			this.espanolToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.espanolToolStripMenuItem.Text = "Espanol";
			// 
			// italianoToolStripMenuItem
			// 
			this.italianoToolStripMenuItem.Enabled = false;
			this.italianoToolStripMenuItem.Name = "italianoToolStripMenuItem";
			this.italianoToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.italianoToolStripMenuItem.Text = "Italiano";
			// 
			// pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem
			// 
			this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem.Enabled = false;
			this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem.Name = "pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem";
			this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem.Size = new System.Drawing.Size(406, 22);
			this.pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem.Text = "Please contact the devs if you wish to do Localization for the UI";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1,
            this.checkForUpdatesToolStripMenuItem});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// aboutToolStripMenuItem1
			// 
			this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
			this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.aboutToolStripMenuItem1.Text = "About";
			this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
			this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
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
			this.General.Controls.Add(this.PatcherAndPatchGeneratorLabel);
			this.General.Controls.Add(this.PatcherAndPatchGeneratorPanel);
			this.General.Controls.Add(this.CosmeticsPanelLabel);
			this.General.Controls.Add(this.CosmeticsPanel);
			this.General.Controls.Add(this.SettingsPaneLabel);
			this.General.Controls.Add(this.SettingsPanel);
			this.General.Controls.Add(this.RandomSeed);
			this.General.Controls.Add(this.Seed);
			this.General.Controls.Add(this.RomPath);
			this.General.Controls.Add(this.BrowseRom);
			this.General.Controls.Add(this.EuTmcRomLabel);
			this.General.Controls.Add(this.RandoSeedLabel);
			this.General.Location = new System.Drawing.Point(4, 24);
			this.General.Name = "General";
			this.General.Size = new System.Drawing.Size(787, 535);
			this.General.TabIndex = 1;
			this.General.Text = "General";
			this.General.UseVisualStyleBackColor = true;
			// 
			// PatcherAndPatchGeneratorLabel
			// 
			this.PatcherAndPatchGeneratorLabel.AutoSize = true;
			this.PatcherAndPatchGeneratorLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PatcherAndPatchGeneratorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.PatcherAndPatchGeneratorLabel.Location = new System.Drawing.Point(17, 393);
			this.PatcherAndPatchGeneratorLabel.Name = "PatcherAndPatchGeneratorLabel";
			this.PatcherAndPatchGeneratorLabel.Size = new System.Drawing.Size(181, 15);
			this.PatcherAndPatchGeneratorLabel.TabIndex = 21;
			this.PatcherAndPatchGeneratorLabel.Text = "BPS Patcher and Patch Generator";
			this.PatcherAndPatchGeneratorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PatcherAndPatchGeneratorPanel
			// 
			this.PatcherAndPatchGeneratorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.UseCustomCosmetics);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.PatchRomAndGenPatch);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.GeneratePatchButton);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.PatcherModeLabel);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.ApplyPatchButton);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.BrowsePatchOrRom);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.BpsPatchAndPatchedRomPath);
			this.PatcherAndPatchGeneratorPanel.Controls.Add(this.BpsPatchAndPatchedRomLabel);
			this.PatcherAndPatchGeneratorPanel.Location = new System.Drawing.Point(6, 400);
			this.PatcherAndPatchGeneratorPanel.Name = "PatcherAndPatchGeneratorPanel";
			this.PatcherAndPatchGeneratorPanel.Size = new System.Drawing.Size(760, 110);
			this.PatcherAndPatchGeneratorPanel.TabIndex = 20;
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
			// PatcherModeLabel
			// 
			this.PatcherModeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PatcherModeLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.PatcherModeLabel.Location = new System.Drawing.Point(10, 15);
			this.PatcherModeLabel.Name = "PatcherModeLabel";
			this.PatcherModeLabel.Size = new System.Drawing.Size(133, 15);
			this.PatcherModeLabel.TabIndex = 23;
			this.PatcherModeLabel.Text = "Patcher Mode:";
			this.PatcherModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// CosmeticsPanelLabel
			// 
			this.CosmeticsPanelLabel.AutoSize = true;
			this.CosmeticsPanelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsPanelLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsPanelLabel.Location = new System.Drawing.Point(17, 232);
			this.CosmeticsPanelLabel.Name = "CosmeticsPanelLabel";
			this.CosmeticsPanelLabel.Size = new System.Drawing.Size(62, 15);
			this.CosmeticsPanelLabel.TabIndex = 19;
			this.CosmeticsPanelLabel.Text = "Cosmetics";
			this.CosmeticsPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CosmeticsPanel
			// 
			this.CosmeticsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CosmeticsPanel.Controls.Add(this.DeleteCosmeticPreset);
			this.CosmeticsPanel.Controls.Add(this.ResetDefaultCosmetics);
			this.CosmeticsPanel.Controls.Add(this.LoadCosmeticPreset);
			this.CosmeticsPanel.Controls.Add(this.CosmeticsPresets);
			this.CosmeticsPanel.Controls.Add(this.CosmeticsPresetLabel);
			this.CosmeticsPanel.Controls.Add(this.SaveCosmeticPreset);
			this.CosmeticsPanel.Controls.Add(this.GenerateCosmetics);
			this.CosmeticsPanel.Controls.Add(this.LoadCosmetics);
			this.CosmeticsPanel.Controls.Add(this.CosmeticsString);
			this.CosmeticsPanel.Controls.Add(this.CosmeticsStringLabel);
			this.CosmeticsPanel.Location = new System.Drawing.Point(6, 240);
			this.CosmeticsPanel.Name = "CosmeticsPanel";
			this.CosmeticsPanel.Size = new System.Drawing.Size(760, 140);
			this.CosmeticsPanel.TabIndex = 18;
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
			this.CosmeticsPresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.CosmeticsPresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.CosmeticsPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CosmeticsPresets.FormattingEnabled = true;
			this.CosmeticsPresets.Location = new System.Drawing.Point(153, 72);
			this.CosmeticsPresets.Name = "CosmeticsPresets";
			this.CosmeticsPresets.Size = new System.Drawing.Size(597, 23);
			this.CosmeticsPresets.TabIndex = 27;
			// 
			// CosmeticsPresetLabel
			// 
			this.CosmeticsPresetLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsPresetLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsPresetLabel.Location = new System.Drawing.Point(10, 75);
			this.CosmeticsPresetLabel.Name = "CosmeticsPresetLabel";
			this.CosmeticsPresetLabel.Size = new System.Drawing.Size(133, 15);
			this.CosmeticsPresetLabel.TabIndex = 26;
			this.CosmeticsPresetLabel.Text = "Cosmetics Presets:";
			this.CosmeticsPresetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// CosmeticsStringLabel
			// 
			this.CosmeticsStringLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsStringLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsStringLabel.Location = new System.Drawing.Point(10, 15);
			this.CosmeticsStringLabel.Name = "CosmeticsStringLabel";
			this.CosmeticsStringLabel.Size = new System.Drawing.Size(133, 15);
			this.CosmeticsStringLabel.TabIndex = 16;
			this.CosmeticsStringLabel.Text = "Cosmetics String:";
			this.CosmeticsStringLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SettingsPaneLabel
			// 
			this.SettingsPaneLabel.AutoSize = true;
			this.SettingsPaneLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsPaneLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsPaneLabel.Location = new System.Drawing.Point(17, 72);
			this.SettingsPaneLabel.Name = "SettingsPaneLabel";
			this.SettingsPaneLabel.Size = new System.Drawing.Size(49, 15);
			this.SettingsPaneLabel.TabIndex = 17;
			this.SettingsPaneLabel.Text = "Settings";
			this.SettingsPaneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SettingsPanel
			// 
			this.SettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SettingsPanel.Controls.Add(this.DeleteSettingPreset);
			this.SettingsPanel.Controls.Add(this.ResetDefaultSettings);
			this.SettingsPanel.Controls.Add(this.LoadSettingPreset);
			this.SettingsPanel.Controls.Add(this.SettingPresets);
			this.SettingsPanel.Controls.Add(this.SettingsPresetLabel);
			this.SettingsPanel.Controls.Add(this.SaveSettingPreset);
			this.SettingsPanel.Controls.Add(this.GenerateSettings);
			this.SettingsPanel.Controls.Add(this.LoadSettings);
			this.SettingsPanel.Controls.Add(this.SettingString);
			this.SettingsPanel.Controls.Add(this.SettingsStringLabel);
			this.SettingsPanel.Location = new System.Drawing.Point(6, 80);
			this.SettingsPanel.Name = "SettingsPanel";
			this.SettingsPanel.Size = new System.Drawing.Size(760, 140);
			this.SettingsPanel.TabIndex = 16;
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
			this.SettingPresets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.SettingPresets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.SettingPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SettingPresets.FormattingEnabled = true;
			this.SettingPresets.Location = new System.Drawing.Point(153, 72);
			this.SettingPresets.Name = "SettingPresets";
			this.SettingPresets.Size = new System.Drawing.Size(597, 23);
			this.SettingPresets.TabIndex = 27;
			// 
			// SettingsPresetLabel
			// 
			this.SettingsPresetLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsPresetLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsPresetLabel.Location = new System.Drawing.Point(10, 75);
			this.SettingsPresetLabel.Name = "SettingsPresetLabel";
			this.SettingsPresetLabel.Size = new System.Drawing.Size(133, 15);
			this.SettingsPresetLabel.TabIndex = 26;
			this.SettingsPresetLabel.Text = "Settings Presets:";
			this.SettingsPresetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// SettingsStringLabel
			// 
			this.SettingsStringLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsStringLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsStringLabel.Location = new System.Drawing.Point(10, 15);
			this.SettingsStringLabel.Name = "SettingsStringLabel";
			this.SettingsStringLabel.Size = new System.Drawing.Size(133, 15);
			this.SettingsStringLabel.TabIndex = 16;
			this.SettingsStringLabel.Text = "Settings String:";
			this.SettingsStringLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// EuTmcRomLabel
			// 
			this.EuTmcRomLabel.AutoSize = true;
			this.EuTmcRomLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.EuTmcRomLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.EuTmcRomLabel.Location = new System.Drawing.Point(4, 10);
			this.EuTmcRomLabel.Name = "EuTmcRomLabel";
			this.EuTmcRomLabel.Size = new System.Drawing.Size(150, 15);
			this.EuTmcRomLabel.TabIndex = 9;
			this.EuTmcRomLabel.Text = "European Minish Cap ROM";
			this.EuTmcRomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RandoSeedLabel
			// 
			this.RandoSeedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.RandoSeedLabel.Location = new System.Drawing.Point(6, 40);
			this.RandoSeedLabel.Name = "RandoSeedLabel";
			this.RandoSeedLabel.Size = new System.Drawing.Size(148, 15);
			this.RandoSeedLabel.TabIndex = 8;
			this.RandoSeedLabel.Text = "Randomizer Seed";
			this.RandoSeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.Advanced.Controls.Add(this.AlternativeShufflersPanelLabel);
			this.Advanced.Controls.Add(this.AlternativeShufflerPanel);
			this.Advanced.Controls.Add(this.AdvancedSettingsLabel);
			this.Advanced.Controls.Add(this.LogicPatchesYamlLabel);
			this.Advanced.Controls.Add(this.LogicPatchesYamlPanel);
			this.Advanced.Controls.Add(this.MysterySettingsLabel);
			this.Advanced.Controls.Add(this.MysterySettingsPanel);
			this.Advanced.Location = new System.Drawing.Point(4, 24);
			this.Advanced.Name = "Advanced";
			this.Advanced.Padding = new System.Windows.Forms.Padding(3);
			this.Advanced.Size = new System.Drawing.Size(787, 535);
			this.Advanced.TabIndex = 3;
			this.Advanced.Text = "Advanced";
			this.Advanced.UseVisualStyleBackColor = true;
			// 
			// AlternativeShufflersPanelLabel
			// 
			this.AlternativeShufflersPanelLabel.AutoSize = true;
			this.AlternativeShufflersPanelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AlternativeShufflersPanelLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.AlternativeShufflersPanelLabel.Location = new System.Drawing.Point(17, 387);
			this.AlternativeShufflersPanelLabel.Name = "AlternativeShufflersPanelLabel";
			this.AlternativeShufflersPanelLabel.Size = new System.Drawing.Size(113, 15);
			this.AlternativeShufflersPanelLabel.TabIndex = 20;
			this.AlternativeShufflersPanelLabel.Text = "Alternative Shufflers";
			this.AlternativeShufflersPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AlternativeShufflerPanel
			// 
			this.AlternativeShufflerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AlternativeShufflerPanel.Controls.Add(this.UseSphereBasedShuffler);
			this.AlternativeShufflerPanel.Location = new System.Drawing.Point(6, 395);
			this.AlternativeShufflerPanel.Name = "AlternativeShufflerPanel";
			this.AlternativeShufflerPanel.Size = new System.Drawing.Size(760, 50);
			this.AlternativeShufflerPanel.TabIndex = 19;
			// 
			// UseSphereBasedShuffler
			// 
			this.UseSphereBasedShuffler.AutoSize = true;
			this.UseSphereBasedShuffler.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseSphereBasedShuffler.Location = new System.Drawing.Point(10, 15);
			this.UseSphereBasedShuffler.Name = "UseSphereBasedShuffler";
			this.UseSphereBasedShuffler.Size = new System.Drawing.Size(165, 19);
			this.UseSphereBasedShuffler.TabIndex = 28;
			this.UseSphereBasedShuffler.Text = "Use Hendrus Seed Shuffler";
			this.UseSphereBasedShuffler.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseSphereBasedShuffler.UseVisualStyleBackColor = true;
			this.UseSphereBasedShuffler.CheckedChanged += new System.EventHandler(this.UseSphereBasedShuffler_CheckedChanged);
			// 
			// AdvancedSettingsLabel
			// 
			this.AdvancedSettingsLabel.AutoSize = true;
			this.AdvancedSettingsLabel.Location = new System.Drawing.Point(17, 15);
			this.AdvancedSettingsLabel.MaximumSize = new System.Drawing.Size(760, 0);
			this.AdvancedSettingsLabel.Name = "AdvancedSettingsLabel";
			this.AdvancedSettingsLabel.Size = new System.Drawing.Size(723, 30);
			this.AdvancedSettingsLabel.TabIndex = 18;
			this.AdvancedSettingsLabel.Text = "This page contains advanced settings and experimental features for the randomizer" +
    ". Unless you are experienced with the randomizer we recommend not modifying thes" +
    "e settings.";
			// 
			// LogicPatchesYamlLabel
			// 
			this.LogicPatchesYamlLabel.AutoSize = true;
			this.LogicPatchesYamlLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.LogicPatchesYamlLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.LogicPatchesYamlLabel.Location = new System.Drawing.Point(17, 57);
			this.LogicPatchesYamlLabel.Name = "LogicPatchesYamlLabel";
			this.LogicPatchesYamlLabel.Size = new System.Drawing.Size(243, 15);
			this.LogicPatchesYamlLabel.TabIndex = 17;
			this.LogicPatchesYamlLabel.Text = "Custom Logic, Patches and Global YAML File";
			this.LogicPatchesYamlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LogicPatchesYamlPanel
			// 
			this.LogicPatchesYamlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LogicPatchesYamlPanel.Controls.Add(this.BrowseCustomYAML);
			this.LogicPatchesYamlPanel.Controls.Add(this.YAMLPath);
			this.LogicPatchesYamlPanel.Controls.Add(this.YamlFilePathLabel);
			this.LogicPatchesYamlPanel.Controls.Add(this.UseCustomYAML);
			this.LogicPatchesYamlPanel.Controls.Add(this.BrowseCustomPatch);
			this.LogicPatchesYamlPanel.Controls.Add(this.RomBuildfilePath);
			this.LogicPatchesYamlPanel.Controls.Add(this.RomBuildfinePathLabel);
			this.LogicPatchesYamlPanel.Controls.Add(this.UseCustomPatch);
			this.LogicPatchesYamlPanel.Controls.Add(this.BrowseCustomLogicFile);
			this.LogicPatchesYamlPanel.Controls.Add(this.LogicFilePath);
			this.LogicPatchesYamlPanel.Controls.Add(this.UseCustomLogic);
			this.LogicPatchesYamlPanel.Controls.Add(this.LogicFilePathLabel);
			this.LogicPatchesYamlPanel.Location = new System.Drawing.Point(6, 65);
			this.LogicPatchesYamlPanel.Name = "LogicPatchesYamlPanel";
			this.LogicPatchesYamlPanel.Size = new System.Drawing.Size(760, 200);
			this.LogicPatchesYamlPanel.TabIndex = 16;
			// 
			// BrowseCustomYAML
			// 
			this.BrowseCustomYAML.Location = new System.Drawing.Point(660, 161);
			this.BrowseCustomYAML.Name = "BrowseCustomYAML";
			this.BrowseCustomYAML.Size = new System.Drawing.Size(90, 23);
			this.BrowseCustomYAML.TabIndex = 20;
			this.BrowseCustomYAML.Text = "Browse";
			this.BrowseCustomYAML.UseVisualStyleBackColor = true;
			this.BrowseCustomYAML.Click += new System.EventHandler(this.BrowseCustomYAML_Click);
			// 
			// YAMLPath
			// 
			this.YAMLPath.Enabled = false;
			this.YAMLPath.Location = new System.Drawing.Point(153, 162);
			this.YAMLPath.Name = "YAMLPath";
			this.YAMLPath.Size = new System.Drawing.Size(501, 23);
			this.YAMLPath.TabIndex = 21;
			// 
			// YamlFilePathLabel
			// 
			this.YamlFilePathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.YamlFilePathLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.YamlFilePathLabel.Location = new System.Drawing.Point(10, 165);
			this.YamlFilePathLabel.Name = "YamlFilePathLabel";
			this.YamlFilePathLabel.Size = new System.Drawing.Size(133, 15);
			this.YamlFilePathLabel.TabIndex = 22;
			this.YamlFilePathLabel.Text = "YAML File Path:";
			this.YamlFilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// UseCustomYAML
			// 
			this.UseCustomYAML.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomYAML.Location = new System.Drawing.Point(10, 135);
			this.UseCustomYAML.Name = "UseCustomYAML";
			this.UseCustomYAML.Size = new System.Drawing.Size(160, 19);
			this.UseCustomYAML.TabIndex = 19;
			this.UseCustomYAML.Text = "Use Global YAML File";
			this.UseCustomYAML.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseCustomYAML.UseVisualStyleBackColor = true;
			this.UseCustomYAML.CheckedChanged += new System.EventHandler(this.UseCustomYAML_CheckedChanged);
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
			// RomBuildfinePathLabel
			// 
			this.RomBuildfinePathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.RomBuildfinePathLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.RomBuildfinePathLabel.Location = new System.Drawing.Point(10, 105);
			this.RomBuildfinePathLabel.Name = "RomBuildfinePathLabel";
			this.RomBuildfinePathLabel.Size = new System.Drawing.Size(133, 15);
			this.RomBuildfinePathLabel.TabIndex = 22;
			this.RomBuildfinePathLabel.Text = "ROM Buildfile Path:";
			this.RomBuildfinePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// LogicFilePathLabel
			// 
			this.LogicFilePathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.LogicFilePathLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.LogicFilePathLabel.Location = new System.Drawing.Point(10, 45);
			this.LogicFilePathLabel.Name = "LogicFilePathLabel";
			this.LogicFilePathLabel.Size = new System.Drawing.Size(133, 15);
			this.LogicFilePathLabel.TabIndex = 16;
			this.LogicFilePathLabel.Text = "Logic File Path:";
			this.LogicFilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MysterySettingsLabel
			// 
			this.MysterySettingsLabel.AutoSize = true;
			this.MysterySettingsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.MysterySettingsLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.MysterySettingsLabel.Location = new System.Drawing.Point(17, 277);
			this.MysterySettingsLabel.Name = "MysterySettingsLabel";
			this.MysterySettingsLabel.Size = new System.Drawing.Size(49, 15);
			this.MysterySettingsLabel.TabIndex = 22;
			this.MysterySettingsLabel.Text = "Mystery";
			this.MysterySettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MysterySettingsPanel
			// 
			this.MysterySettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MysterySettingsPanel.Controls.Add(this.UseMysteryCosmetics);
			this.MysterySettingsPanel.Controls.Add(this.LoadCosmeticSample);
			this.MysterySettingsPanel.Controls.Add(this.CosmeticsWeights);
			this.MysterySettingsPanel.Controls.Add(this.UseMysterySettings);
			this.MysterySettingsPanel.Controls.Add(this.LoadSettingSample);
			this.MysterySettingsPanel.Controls.Add(this.SettingsWeights);
			this.MysterySettingsPanel.Location = new System.Drawing.Point(6, 285);
			this.MysterySettingsPanel.Name = "MysterySettingsPanel";
			this.MysterySettingsPanel.Size = new System.Drawing.Size(760, 90);
			this.MysterySettingsPanel.TabIndex = 21;
			// 
			// UseMysteryCosmetics
			// 
			this.UseMysteryCosmetics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseMysteryCosmetics.Location = new System.Drawing.Point(10, 49);
			this.UseMysteryCosmetics.Name = "UseMysteryCosmetics";
			this.UseMysteryCosmetics.Size = new System.Drawing.Size(160, 24);
			this.UseMysteryCosmetics.TabIndex = 34;
			this.UseMysteryCosmetics.Text = "Use Mystery Cosmetics";
			this.UseMysteryCosmetics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseMysteryCosmetics.UseVisualStyleBackColor = true;
			this.UseMysteryCosmetics.CheckedChanged += new System.EventHandler(this.UseMysteryCosmetics_CheckedChanged);
			// 
			// LoadCosmeticSample
			// 
			this.LoadCosmeticSample.Location = new System.Drawing.Point(540, 48);
			this.LoadCosmeticSample.Name = "LoadCosmeticSample";
			this.LoadCosmeticSample.Size = new System.Drawing.Size(210, 23);
			this.LoadCosmeticSample.TabIndex = 33;
			this.LoadCosmeticSample.Text = "Load Sample Options";
			this.LoadCosmeticSample.UseVisualStyleBackColor = true;
			this.LoadCosmeticSample.Click += new System.EventHandler(this.LoadCosmeticSample_Click);
			// 
			// CosmeticsWeights
			// 
			this.CosmeticsWeights.FormattingEnabled = true;
			this.CosmeticsWeights.Location = new System.Drawing.Point(180, 49);
			this.CosmeticsWeights.Name = "CosmeticsWeights";
			this.CosmeticsWeights.Size = new System.Drawing.Size(350, 23);
			this.CosmeticsWeights.TabIndex = 32;
			// 
			// UseMysterySettings
			// 
			this.UseMysterySettings.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseMysterySettings.Location = new System.Drawing.Point(10, 14);
			this.UseMysterySettings.Name = "UseMysterySettings";
			this.UseMysterySettings.Size = new System.Drawing.Size(160, 24);
			this.UseMysterySettings.TabIndex = 31;
			this.UseMysterySettings.Text = "Use Mystery Settings";
			this.UseMysterySettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseMysterySettings.UseVisualStyleBackColor = true;
			this.UseMysterySettings.CheckedChanged += new System.EventHandler(this.UseMysterySettings_CheckedChanged);
			// 
			// LoadSettingSample
			// 
			this.LoadSettingSample.Location = new System.Drawing.Point(540, 13);
			this.LoadSettingSample.Name = "LoadSettingSample";
			this.LoadSettingSample.Size = new System.Drawing.Size(210, 23);
			this.LoadSettingSample.TabIndex = 28;
			this.LoadSettingSample.Text = "Load Sample Options";
			this.LoadSettingSample.UseVisualStyleBackColor = true;
			this.LoadSettingSample.Click += new System.EventHandler(this.LoadSettingSample_Click);
			// 
			// SettingsWeights
			// 
			this.SettingsWeights.FormattingEnabled = true;
			this.SettingsWeights.Location = new System.Drawing.Point(180, 14);
			this.SettingsWeights.Name = "SettingsWeights";
			this.SettingsWeights.Size = new System.Drawing.Size(350, 23);
			this.SettingsWeights.TabIndex = 27;
			// 
			// SeedOutput
			// 
			this.SeedOutput.Controls.Add(this.SaveRom);
			this.SeedOutput.Controls.Add(this.SavePatch);
			this.SeedOutput.Controls.Add(this.SaveSpoiler);
			this.SeedOutput.Controls.Add(this.CosmeticsInformationPanelLabel);
			this.SeedOutput.Controls.Add(this.CosmeticsInformationPanel);
			this.SeedOutput.Controls.Add(this.SettingsInformationPanelLabel);
			this.SeedOutput.Controls.Add(this.SettingsInformationPanel);
			this.SeedOutput.Controls.Add(this.SeedInformationPanelLabel);
			this.SeedOutput.Controls.Add(this.SeedInformationPanel);
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
			// CosmeticsInformationPanelLabel
			// 
			this.CosmeticsInformationPanelLabel.AutoSize = true;
			this.CosmeticsInformationPanelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsInformationPanelLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsInformationPanelLabel.Location = new System.Drawing.Point(17, 282);
			this.CosmeticsInformationPanelLabel.Name = "CosmeticsInformationPanelLabel";
			this.CosmeticsInformationPanelLabel.Size = new System.Drawing.Size(128, 15);
			this.CosmeticsInformationPanelLabel.TabIndex = 21;
			this.CosmeticsInformationPanelLabel.Text = "Cosmetics Information";
			this.CosmeticsInformationPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CosmeticsInformationPanel
			// 
			this.CosmeticsInformationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CosmeticsInformationPanel.Controls.Add(this.CopyCosmeticsHashToClipboard);
			this.CosmeticsInformationPanel.Controls.Add(this.CosmeticStringLabel);
			this.CosmeticsInformationPanel.Controls.Add(this.CosmeticsStringSeedOutputLabel);
			this.CosmeticsInformationPanel.Controls.Add(this.CosmeticNameLabel);
			this.CosmeticsInformationPanel.Controls.Add(this.CosmeticsNameLabel);
			this.CosmeticsInformationPanel.Location = new System.Drawing.Point(6, 290);
			this.CosmeticsInformationPanel.Name = "CosmeticsInformationPanel";
			this.CosmeticsInformationPanel.Size = new System.Drawing.Size(760, 105);
			this.CosmeticsInformationPanel.TabIndex = 20;
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
			// CosmeticsStringSeedOutputLabel
			// 
			this.CosmeticsStringSeedOutputLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsStringSeedOutputLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsStringSeedOutputLabel.Location = new System.Drawing.Point(10, 45);
			this.CosmeticsStringSeedOutputLabel.Name = "CosmeticsStringSeedOutputLabel";
			this.CosmeticsStringSeedOutputLabel.Size = new System.Drawing.Size(178, 15);
			this.CosmeticsStringSeedOutputLabel.TabIndex = 22;
			this.CosmeticsStringSeedOutputLabel.Text = "Cosmetics String:";
			this.CosmeticsStringSeedOutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// CosmeticsNameLabel
			// 
			this.CosmeticsNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CosmeticsNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CosmeticsNameLabel.Location = new System.Drawing.Point(10, 15);
			this.CosmeticsNameLabel.Name = "CosmeticsNameLabel";
			this.CosmeticsNameLabel.Size = new System.Drawing.Size(178, 15);
			this.CosmeticsNameLabel.TabIndex = 18;
			this.CosmeticsNameLabel.Text = "Cosmetics Name:";
			this.CosmeticsNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SettingsInformationPanelLabel
			// 
			this.SettingsInformationPanelLabel.AutoSize = true;
			this.SettingsInformationPanelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsInformationPanelLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsInformationPanelLabel.Location = new System.Drawing.Point(17, 157);
			this.SettingsInformationPanelLabel.Name = "SettingsInformationPanelLabel";
			this.SettingsInformationPanelLabel.Size = new System.Drawing.Size(115, 15);
			this.SettingsInformationPanelLabel.TabIndex = 19;
			this.SettingsInformationPanelLabel.Text = "Settings Information";
			this.SettingsInformationPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SettingsInformationPanel
			// 
			this.SettingsInformationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SettingsInformationPanel.Controls.Add(this.CopySettingsHashToClipboard);
			this.SettingsInformationPanel.Controls.Add(this.SettingHashLabel);
			this.SettingsInformationPanel.Controls.Add(this.SettingsStringSeedOutputLabel);
			this.SettingsInformationPanel.Controls.Add(this.SettingNameLabel);
			this.SettingsInformationPanel.Controls.Add(this.SettingsNameLabel);
			this.SettingsInformationPanel.Location = new System.Drawing.Point(6, 165);
			this.SettingsInformationPanel.Name = "SettingsInformationPanel";
			this.SettingsInformationPanel.Size = new System.Drawing.Size(760, 105);
			this.SettingsInformationPanel.TabIndex = 18;
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
			// SettingsStringSeedOutputLabel
			// 
			this.SettingsStringSeedOutputLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsStringSeedOutputLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsStringSeedOutputLabel.Location = new System.Drawing.Point(10, 45);
			this.SettingsStringSeedOutputLabel.Name = "SettingsStringSeedOutputLabel";
			this.SettingsStringSeedOutputLabel.Size = new System.Drawing.Size(178, 15);
			this.SettingsStringSeedOutputLabel.TabIndex = 22;
			this.SettingsStringSeedOutputLabel.Text = "Settings String:";
			this.SettingsStringSeedOutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// SettingsNameLabel
			// 
			this.SettingsNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SettingsNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SettingsNameLabel.Location = new System.Drawing.Point(10, 15);
			this.SettingsNameLabel.Name = "SettingsNameLabel";
			this.SettingsNameLabel.Size = new System.Drawing.Size(178, 15);
			this.SettingsNameLabel.TabIndex = 18;
			this.SettingsNameLabel.Text = "Settings Name:";
			this.SettingsNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SeedInformationPanelLabel
			// 
			this.SeedInformationPanelLabel.AutoSize = true;
			this.SeedInformationPanelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SeedInformationPanelLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SeedInformationPanelLabel.Location = new System.Drawing.Point(17, 7);
			this.SeedInformationPanelLabel.Name = "SeedInformationPanelLabel";
			this.SeedInformationPanelLabel.Size = new System.Drawing.Size(98, 15);
			this.SeedInformationPanelLabel.TabIndex = 17;
			this.SeedInformationPanelLabel.Text = "Seed Information";
			this.SeedInformationPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SeedInformationPanel
			// 
			this.SeedInformationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SeedInformationPanel.Controls.Add(this.YamlHashNotShownLabel);
			this.SeedInformationPanel.Controls.Add(this.CopyHashToClipboard);
			this.SeedInformationPanel.Controls.Add(this.RomHashPanel);
			this.SeedInformationPanel.Controls.Add(this.RomHashLabel);
			this.SeedInformationPanel.Controls.Add(this.OutputSeedLabel);
			this.SeedInformationPanel.Controls.Add(this.OutputSeedNumberLabel);
			this.SeedInformationPanel.Controls.Add(this.InputSeedLabel);
			this.SeedInformationPanel.Controls.Add(this.InputSeedNumberLabel);
			this.SeedInformationPanel.Location = new System.Drawing.Point(6, 15);
			this.SeedInformationPanel.Name = "SeedInformationPanel";
			this.SeedInformationPanel.Size = new System.Drawing.Size(760, 135);
			this.SeedInformationPanel.TabIndex = 16;
			// 
			// YamlHashNotShownLabel
			// 
			this.YamlHashNotShownLabel.AutoSize = true;
			this.YamlHashNotShownLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.YamlHashNotShownLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.YamlHashNotShownLabel.Location = new System.Drawing.Point(230, 59);
			this.YamlHashNotShownLabel.Name = "YamlHashNotShownLabel";
			this.YamlHashNotShownLabel.Size = new System.Drawing.Size(322, 15);
			this.YamlHashNotShownLabel.TabIndex = 26;
			this.YamlHashNotShownLabel.Text = "Hash is not displayed when YAML is being used for settings.";
			this.YamlHashNotShownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.YamlHashNotShownLabel.Visible = false;
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
			// RomHashPanel
			// 
			this.RomHashPanel.Location = new System.Drawing.Point(230, 42);
			this.RomHashPanel.Margin = new System.Windows.Forms.Padding(0);
			this.RomHashPanel.Name = "RomHashPanel";
			this.RomHashPanel.Size = new System.Drawing.Size(384, 48);
			this.RomHashPanel.TabIndex = 23;
			// 
			// RomHashLabel
			// 
			this.RomHashLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.RomHashLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.RomHashLabel.Location = new System.Drawing.Point(10, 59);
			this.RomHashLabel.Name = "RomHashLabel";
			this.RomHashLabel.Size = new System.Drawing.Size(200, 15);
			this.RomHashLabel.TabIndex = 22;
			this.RomHashLabel.Text = "ROM Hash:";
			this.RomHashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// OutputSeedNumberLabel
			// 
			this.OutputSeedNumberLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.OutputSeedNumberLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.OutputSeedNumberLabel.Location = new System.Drawing.Point(385, 15);
			this.OutputSeedNumberLabel.Name = "OutputSeedNumberLabel";
			this.OutputSeedNumberLabel.Size = new System.Drawing.Size(178, 15);
			this.OutputSeedNumberLabel.TabIndex = 20;
			this.OutputSeedNumberLabel.Text = "Output Seed Number:";
			this.OutputSeedNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// InputSeedNumberLabel
			// 
			this.InputSeedNumberLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.InputSeedNumberLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.InputSeedNumberLabel.Location = new System.Drawing.Point(10, 15);
			this.InputSeedNumberLabel.Name = "InputSeedNumberLabel";
			this.InputSeedNumberLabel.Size = new System.Drawing.Size(178, 15);
			this.InputSeedNumberLabel.TabIndex = 18;
			this.InputSeedNumberLabel.Text = "Input Seed Number:";
			this.InputSeedNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MaxRandomizationAttemptsLabel
			// 
			this.MaxRandomizationAttemptsLabel.AutoSize = true;
			this.MaxRandomizationAttemptsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.MaxRandomizationAttemptsLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.MaxRandomizationAttemptsLabel.Location = new System.Drawing.Point(16, 600);
			this.MaxRandomizationAttemptsLabel.Name = "MaxRandomizationAttemptsLabel";
			this.MaxRandomizationAttemptsLabel.Size = new System.Drawing.Size(168, 15);
			this.MaxRandomizationAttemptsLabel.TabIndex = 26;
			this.MaxRandomizationAttemptsLabel.Text = "Max Randomization Attempts:";
			this.MaxRandomizationAttemptsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.Controls.Add(this.MaxRandomizationAttemptsLabel);
			this.Controls.Add(this.Randomize);
			this.Controls.Add(this.TabPane);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "MinishCapRandomizerUI";
			this.Text = "=O the title is dynamic now! =O";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.General.ResumeLayout(false);
			this.General.PerformLayout();
			this.PatcherAndPatchGeneratorPanel.ResumeLayout(false);
			this.PatcherAndPatchGeneratorPanel.PerformLayout();
			this.CosmeticsPanel.ResumeLayout(false);
			this.CosmeticsPanel.PerformLayout();
			this.SettingsPanel.ResumeLayout(false);
			this.SettingsPanel.PerformLayout();
			this.TabPane.ResumeLayout(false);
			this.Advanced.ResumeLayout(false);
			this.Advanced.PerformLayout();
			this.AlternativeShufflerPanel.ResumeLayout(false);
			this.AlternativeShufflerPanel.PerformLayout();
			this.LogicPatchesYamlPanel.ResumeLayout(false);
			this.LogicPatchesYamlPanel.PerformLayout();
			this.MysterySettingsPanel.ResumeLayout(false);
			this.SeedOutput.ResumeLayout(false);
			this.SeedOutput.PerformLayout();
			this.CosmeticsInformationPanel.ResumeLayout(false);
			this.SettingsInformationPanel.ResumeLayout(false);
			this.SeedInformationPanel.ResumeLayout(false);
			this.SeedInformationPanel.PerformLayout();
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
        private ToolStripMenuItem savePresetYAMLMenuItem;
        private ToolStripMenuItem saveMysteryYAMLMenuItem;
        private ToolStripMenuItem loggingToolStripMenuItem;
        private ToolStripMenuItem setLoggerOutputPathToolStripMenuItem;
        private ToolStripMenuItem logAllTransactionsToolStripMenuItem;
        private ToolStripMenuItem localizationToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private TabPage General;
        private Label PatcherAndPatchGeneratorLabel;
        private Panel PatcherAndPatchGeneratorPanel;
        private Button PatchRomAndGenPatch;
        private RadioButton GeneratePatchButton;
        private Label PatcherModeLabel;
        private RadioButton ApplyPatchButton;
        private Button BrowsePatchOrRom;
        private TextBox BpsPatchAndPatchedRomPath;
        private Label BpsPatchAndPatchedRomLabel;
        private Label CosmeticsPanelLabel;
        private Panel CosmeticsPanel;
        private Button DeleteCosmeticPreset;
        private Button ResetDefaultCosmetics;
        private Button LoadCosmeticPreset;
        private ComboBox CosmeticsPresets;
        private Label CosmeticsPresetLabel;
        private Button SaveCosmeticPreset;
        private Button GenerateCosmetics;
        private Button LoadCosmetics;
        private TextBox CosmeticsString;
        private Label CosmeticsStringLabel;
        private Label SettingsPaneLabel;
        private Panel SettingsPanel;
        private Button DeleteSettingPreset;
        private Button ResetDefaultSettings;
        private Button LoadSettingPreset;
        private ComboBox SettingPresets;
        private Label SettingsPresetLabel;
        private Button SaveSettingPreset;
        private Button GenerateSettings;
        private Button LoadSettings;
        private TextBox SettingString;
        private Label SettingsStringLabel;
        private Button RandomSeed;
        private TextBox Seed;
        private TextBox RomPath;
        private Button BrowseRom;
        private Label EuTmcRomLabel;
        private Label RandoSeedLabel;
        private TabControl TabPane;
        private TabPage SeedOutput;
        private Label SeedInformationPanelLabel;
        private Panel SeedInformationPanel;
        private Label InputSeedNumberLabel;
        private Label InputSeedLabel;
        private Label OutputSeedNumberLabel;
        private Label OutputSeedLabel;
        private Label RomHashLabel;
        private Label SettingsInformationPanelLabel;
        private Panel SettingsInformationPanel;
        private Label SettingHashLabel;
        private Label SettingsStringSeedOutputLabel;
        private Label SettingNameLabel;
        private Label SettingsNameLabel;
        private Button CopySettingsHashToClipboard;
        private Label CosmeticsInformationPanelLabel;
        private Panel CosmeticsInformationPanel;
        private Button CopyCosmeticsHashToClipboard;
        private Label CosmeticStringLabel;
        private Label CosmeticsStringSeedOutputLabel;
        private Label CosmeticNameLabel;
        private Label CosmeticsNameLabel;
        private Button SaveSpoiler;
        private Button SaveRom;
        private Button SavePatch;
        private ToolStripMenuItem writeAndFlushLoggerToolStripMenuItem;
        private Label MaxRandomizationAttemptsLabel;
        private TextBox RandomizationAttempts;
        private CheckBox UseSphereBasedShuffler;
        private TabPage Advanced;
        private Label LogicPatchesYamlLabel;
        private Panel LogicPatchesYamlPanel;
        private Button BrowseCustomPatch;
        private TextBox RomBuildfilePath;
        private Label RomBuildfinePathLabel;
        private CheckBox UseCustomPatch;
        private Button BrowseCustomYAML;
        private TextBox YAMLPath;
        private Label YamlFilePathLabel;
        private CheckBox UseCustomYAML;
        private Button BrowseCustomLogicFile;
        private TextBox LogicFilePath;
        private CheckBox UseCustomLogic;
        private Label LogicFilePathLabel;
        private Label AdvancedSettingsLabel;
        private CheckBox UseCustomCosmetics;
        private Label AlternativeShufflersPanelLabel;
        private Panel AlternativeShufflerPanel;
        private Label MysterySettingsLabel;
        private Panel MysterySettingsPanel;
        private Button LoadSettingSample;
        private ComboBox SettingsWeights;
        private CheckBox UseMysteryCosmetics;
        private Button LoadCosmeticSample;
        private ComboBox CosmeticsWeights;
        private CheckBox UseMysterySettings;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem otherToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesOnStartToolStripMenuItem;
        private FlowLayoutPanel RomHashPanel;
        private Button CopyHashToClipboard;
        private Label YamlHashNotShownLabel;
        private ToolStripMenuItem françaisToolStripMenuItem;
        private ToolStripMenuItem deutschToolStripMenuItem;
        private ToolStripMenuItem espanolToolStripMenuItem;
        private ToolStripMenuItem italianoToolStripMenuItem;
        private ToolStripMenuItem pleaseContactTheDevsIfYouWishToDoLocalizationForTheUIToolStripMenuItem;
    }
}
