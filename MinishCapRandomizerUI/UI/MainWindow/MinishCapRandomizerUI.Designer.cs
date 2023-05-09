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
            menuStrip1 = new MenuStrip();
            logicToolStripMenuItem = new ToolStripMenuItem();
            exportDefaultLogicToolStripMenuItem = new ToolStripMenuItem();
            savePresetYAMLMenuItem = new ToolStripMenuItem();
            saveMysteryYAMLMenuItem = new ToolStripMenuItem();
            loggingToolStripMenuItem = new ToolStripMenuItem();
            setLoggerOutputPathToolStripMenuItem = new ToolStripMenuItem();
            logAllTransactionsToolStripMenuItem = new ToolStripMenuItem();
            writeAndFlushLoggerToolStripMenuItem = new ToolStripMenuItem();
            localizationToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            otherToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesOnStartToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            changelogToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            Randomize = new Button();
            openFileDialog1 = new OpenFileDialog();
            colorDialog1 = new ColorDialog();
            General = new TabPage();
            PatcherAndPatchGeneratorLabel = new Label();
            PatcherAndPatchGeneratorPanel = new Panel();
            UseCustomCosmetics = new CheckBox();
            PatchRomAndGenPatch = new Button();
            GeneratePatchButton = new RadioButton();
            PatcherModeLabel = new Label();
            ApplyPatchButton = new RadioButton();
            BrowsePatchOrRom = new Button();
            BpsPatchAndPatchedRomPath = new TextBox();
            BpsPatchAndPatchedRomLabel = new Label();
            CosmeticsPanelLabel = new Label();
            CosmeticsPanel = new Panel();
            DeleteCosmeticPreset = new Button();
            ResetDefaultCosmetics = new Button();
            LoadCosmeticPreset = new Button();
            CosmeticsPresets = new ComboBox();
            CosmeticsPresetLabel = new Label();
            SaveCosmeticPreset = new Button();
            GenerateCosmetics = new Button();
            LoadCosmetics = new Button();
            CosmeticsString = new TextBox();
            CosmeticsStringLabel = new Label();
            SettingsPaneLabel = new Label();
            SettingsPanel = new Panel();
            DeleteSettingPreset = new Button();
            ResetDefaultSettings = new Button();
            LoadSettingPreset = new Button();
            SettingPresets = new ComboBox();
            SettingsPresetLabel = new Label();
            SaveSettingPreset = new Button();
            GenerateSettings = new Button();
            LoadSettings = new Button();
            SettingString = new TextBox();
            SettingsStringLabel = new Label();
            RandomSeed = new Button();
            Seed = new TextBox();
            RomPath = new TextBox();
            BrowseRom = new Button();
            EuTmcRomLabel = new Label();
            RandoSeedLabel = new Label();
            TabPane = new TabControl();
            Advanced = new TabPage();
            AlternativeShufflersPanelLabel = new Label();
            AlternativeShufflerPanel = new Panel();
            UseSphereBasedShuffler = new CheckBox();
            AdvancedSettingsLabel = new Label();
            LogicPatchesYamlLabel = new Label();
            LogicPatchesYamlPanel = new Panel();
            BrowseCustomYAML = new Button();
            YAMLPath = new TextBox();
            YamlFilePathLabel = new Label();
            UseCustomYAML = new CheckBox();
            BrowseCustomPatch = new Button();
            RomBuildfilePath = new TextBox();
            RomBuildfinePathLabel = new Label();
            UseCustomPatch = new CheckBox();
            BrowseCustomLogicFile = new Button();
            LogicFilePath = new TextBox();
            UseCustomLogic = new CheckBox();
            LogicFilePathLabel = new Label();
            MysterySettingsLabel = new Label();
            MysterySettingsPanel = new Panel();
            UseMysteryCosmetics = new CheckBox();
            LoadCosmeticSample = new Button();
            CosmeticsWeights = new ComboBox();
            UseMysterySettings = new CheckBox();
            LoadSettingSample = new Button();
            SettingsWeights = new ComboBox();
            SeedOutput = new TabPage();
            SaveRom = new Button();
            SavePatch = new Button();
            SaveSpoiler = new Button();
            CosmeticsInformationPanelLabel = new Label();
            CosmeticsInformationPanel = new Panel();
            CopyCosmeticsHashToClipboard = new Button();
            CosmeticStringLabel = new Label();
            CosmeticsStringSeedOutputLabel = new Label();
            CosmeticNameLabel = new Label();
            CosmeticsNameLabel = new Label();
            SettingsInformationPanelLabel = new Label();
            SettingsInformationPanel = new Panel();
            CopySettingsHashToClipboard = new Button();
            SettingHashLabel = new Label();
            SettingsStringSeedOutputLabel = new Label();
            SettingNameLabel = new Label();
            SettingsNameLabel = new Label();
            SeedInformationPanelLabel = new Label();
            SeedInformationPanel = new Panel();
            CopyHashToClipboard = new Button();
            RomHashPanel = new FlowLayoutPanel();
            RomHashLabel = new Label();
            OutputSeedLabel = new Label();
            OutputSeedNumberLabel = new Label();
            InputSeedLabel = new Label();
            InputSeedNumberLabel = new Label();
            MaxRandomizationAttemptsLabel = new Label();
            RandomizationAttempts = new TextBox();
            menuStrip1.SuspendLayout();
            General.SuspendLayout();
            PatcherAndPatchGeneratorPanel.SuspendLayout();
            CosmeticsPanel.SuspendLayout();
            SettingsPanel.SuspendLayout();
            TabPane.SuspendLayout();
            Advanced.SuspendLayout();
            AlternativeShufflerPanel.SuspendLayout();
            LogicPatchesYamlPanel.SuspendLayout();
            MysterySettingsPanel.SuspendLayout();
            SeedOutput.SuspendLayout();
            CosmeticsInformationPanel.SuspendLayout();
            SettingsInformationPanel.SuspendLayout();
            SeedInformationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { logicToolStripMenuItem, loggingToolStripMenuItem, localizationToolStripMenuItem, otherToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(816, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // logicToolStripMenuItem
            // 
            logicToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportDefaultLogicToolStripMenuItem, savePresetYAMLMenuItem, saveMysteryYAMLMenuItem });
            logicToolStripMenuItem.Name = "logicToolStripMenuItem";
            logicToolStripMenuItem.Size = new Size(48, 20);
            logicToolStripMenuItem.Text = "Logic";
            // 
            // exportDefaultLogicToolStripMenuItem
            // 
            exportDefaultLogicToolStripMenuItem.Name = "exportDefaultLogicToolStripMenuItem";
            exportDefaultLogicToolStripMenuItem.Size = new Size(239, 22);
            exportDefaultLogicToolStripMenuItem.Text = "Export Default Logic";
            exportDefaultLogicToolStripMenuItem.Click += exportDefaultLogicToolStripMenuItem_Click;
            // 
            // savePresetYAMLMenuItem
            // 
            savePresetYAMLMenuItem.Name = "savePresetYAMLMenuItem";
            savePresetYAMLMenuItem.Size = new Size(239, 22);
            savePresetYAMLMenuItem.Text = "Save all options as Preset YAML";
            savePresetYAMLMenuItem.Click += savePresetYAMLMenuItem_Click;
            // 
            // saveMysteryYAMLMenuItem
            // 
            saveMysteryYAMLMenuItem.Name = "saveMysteryYAMLMenuItem";
            saveMysteryYAMLMenuItem.Size = new Size(239, 22);
            saveMysteryYAMLMenuItem.Text = "Save Mystery YAML template";
            saveMysteryYAMLMenuItem.Click += saveMysteryYAMLMenuItem_Click;
            // 
            // loggingToolStripMenuItem
            // 
            loggingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { setLoggerOutputPathToolStripMenuItem, logAllTransactionsToolStripMenuItem, writeAndFlushLoggerToolStripMenuItem });
            loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            loggingToolStripMenuItem.Size = new Size(63, 20);
            loggingToolStripMenuItem.Text = "Logging";
            // 
            // setLoggerOutputPathToolStripMenuItem
            // 
            setLoggerOutputPathToolStripMenuItem.Name = "setLoggerOutputPathToolStripMenuItem";
            setLoggerOutputPathToolStripMenuItem.Size = new Size(198, 22);
            setLoggerOutputPathToolStripMenuItem.Text = "Set Logger Output Path";
            setLoggerOutputPathToolStripMenuItem.Click += setLoggerOutputPathToolStripMenuItem_Click;
            // 
            // logAllTransactionsToolStripMenuItem
            // 
            logAllTransactionsToolStripMenuItem.Name = "logAllTransactionsToolStripMenuItem";
            logAllTransactionsToolStripMenuItem.Size = new Size(198, 22);
            logAllTransactionsToolStripMenuItem.Text = "Log All Transactions";
            logAllTransactionsToolStripMenuItem.Click += logAllTransactionsToolStripMenuItem_Click;
            // 
            // writeAndFlushLoggerToolStripMenuItem
            // 
            writeAndFlushLoggerToolStripMenuItem.Name = "writeAndFlushLoggerToolStripMenuItem";
            writeAndFlushLoggerToolStripMenuItem.Size = new Size(198, 22);
            writeAndFlushLoggerToolStripMenuItem.Text = "Write and Flush Logger";
            writeAndFlushLoggerToolStripMenuItem.Click += writeAndFlushLoggerToolStripMenuItem_Click;
            // 
            // localizationToolStripMenuItem
            // 
            localizationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { englishToolStripMenuItem });
            localizationToolStripMenuItem.Name = "localizationToolStripMenuItem";
            localizationToolStripMenuItem.Size = new Size(82, 20);
            localizationToolStripMenuItem.Text = "Localization";
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(112, 22);
            englishToolStripMenuItem.Text = "English";
            // 
            // otherToolStripMenuItem
            // 
            otherToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkForUpdatesOnStartToolStripMenuItem });
            otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            otherToolStripMenuItem.Size = new Size(49, 20);
            otherToolStripMenuItem.Text = "Other";
            // 
            // checkForUpdatesOnStartToolStripMenuItem
            // 
            checkForUpdatesOnStartToolStripMenuItem.Name = "checkForUpdatesOnStartToolStripMenuItem";
            checkForUpdatesOnStartToolStripMenuItem.Size = new Size(217, 22);
            checkForUpdatesOnStartToolStripMenuItem.Text = "Check for Updates On Start";
            checkForUpdatesOnStartToolStripMenuItem.Click += checkForUpdatesOnStartToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changelogToolStripMenuItem, aboutToolStripMenuItem1, checkForUpdatesToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            // 
            // changelogToolStripMenuItem
            // 
            changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            changelogToolStripMenuItem.Size = new Size(171, 22);
            changelogToolStripMenuItem.Text = "Changelog";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(171, 22);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem1_Click;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new Size(171, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            // 
            // Randomize
            // 
            Randomize.Location = new Point(533, 596);
            Randomize.Name = "Randomize";
            Randomize.Size = new Size(255, 23);
            Randomize.TabIndex = 3;
            Randomize.Text = "Randomize";
            Randomize.UseVisualStyleBackColor = true;
            Randomize.Click += Randomize_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // General
            // 
            General.Controls.Add(PatcherAndPatchGeneratorLabel);
            General.Controls.Add(PatcherAndPatchGeneratorPanel);
            General.Controls.Add(CosmeticsPanelLabel);
            General.Controls.Add(CosmeticsPanel);
            General.Controls.Add(SettingsPaneLabel);
            General.Controls.Add(SettingsPanel);
            General.Controls.Add(RandomSeed);
            General.Controls.Add(Seed);
            General.Controls.Add(RomPath);
            General.Controls.Add(BrowseRom);
            General.Controls.Add(EuTmcRomLabel);
            General.Controls.Add(RandoSeedLabel);
            General.Location = new Point(4, 24);
            General.Name = "General";
            General.Size = new Size(787, 535);
            General.TabIndex = 1;
            General.Text = "General";
            General.UseVisualStyleBackColor = true;
            // 
            // PatcherAndPatchGeneratorLabel
            // 
            PatcherAndPatchGeneratorLabel.AutoSize = true;
            PatcherAndPatchGeneratorLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PatcherAndPatchGeneratorLabel.ImageAlign = ContentAlignment.MiddleRight;
            PatcherAndPatchGeneratorLabel.Location = new Point(17, 393);
            PatcherAndPatchGeneratorLabel.Name = "PatcherAndPatchGeneratorLabel";
            PatcherAndPatchGeneratorLabel.Size = new Size(181, 15);
            PatcherAndPatchGeneratorLabel.TabIndex = 21;
            PatcherAndPatchGeneratorLabel.Text = "BPS Patcher and Patch Generator";
            PatcherAndPatchGeneratorLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PatcherAndPatchGeneratorPanel
            // 
            PatcherAndPatchGeneratorPanel.BorderStyle = BorderStyle.FixedSingle;
            PatcherAndPatchGeneratorPanel.Controls.Add(UseCustomCosmetics);
            PatcherAndPatchGeneratorPanel.Controls.Add(PatchRomAndGenPatch);
            PatcherAndPatchGeneratorPanel.Controls.Add(GeneratePatchButton);
            PatcherAndPatchGeneratorPanel.Controls.Add(PatcherModeLabel);
            PatcherAndPatchGeneratorPanel.Controls.Add(ApplyPatchButton);
            PatcherAndPatchGeneratorPanel.Controls.Add(BrowsePatchOrRom);
            PatcherAndPatchGeneratorPanel.Controls.Add(BpsPatchAndPatchedRomPath);
            PatcherAndPatchGeneratorPanel.Controls.Add(BpsPatchAndPatchedRomLabel);
            PatcherAndPatchGeneratorPanel.Location = new Point(6, 400);
            PatcherAndPatchGeneratorPanel.Name = "PatcherAndPatchGeneratorPanel";
            PatcherAndPatchGeneratorPanel.Size = new Size(760, 110);
            PatcherAndPatchGeneratorPanel.TabIndex = 20;
            // 
            // UseCustomCosmetics
            // 
            UseCustomCosmetics.AutoSize = true;
            UseCustomCosmetics.Location = new Point(510, 14);
            UseCustomCosmetics.Name = "UseCustomCosmetics";
            UseCustomCosmetics.Size = new Size(231, 19);
            UseCustomCosmetics.TabIndex = 26;
            UseCustomCosmetics.Text = "Use Custom Cosmetics (Disabled, WIP)";
            UseCustomCosmetics.UseVisualStyleBackColor = true;
            // 
            // PatchRomAndGenPatch
            // 
            PatchRomAndGenPatch.Location = new Point(510, 75);
            PatchRomAndGenPatch.Name = "PatchRomAndGenPatch";
            PatchRomAndGenPatch.Size = new Size(240, 23);
            PatchRomAndGenPatch.TabIndex = 25;
            PatchRomAndGenPatch.Text = "Patch Rom";
            PatchRomAndGenPatch.UseVisualStyleBackColor = true;
            PatchRomAndGenPatch.Click += PatchRomAndGenPatch_Click;
            // 
            // GeneratePatchButton
            // 
            GeneratePatchButton.Location = new Point(331, 13);
            GeneratePatchButton.Name = "GeneratePatchButton";
            GeneratePatchButton.Size = new Size(168, 19);
            GeneratePatchButton.TabIndex = 24;
            GeneratePatchButton.Text = "Generate Patch Mode";
            GeneratePatchButton.UseVisualStyleBackColor = true;
            // 
            // PatcherModeLabel
            // 
            PatcherModeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PatcherModeLabel.ImageAlign = ContentAlignment.MiddleRight;
            PatcherModeLabel.Location = new Point(10, 15);
            PatcherModeLabel.Name = "PatcherModeLabel";
            PatcherModeLabel.Size = new Size(133, 15);
            PatcherModeLabel.TabIndex = 23;
            PatcherModeLabel.Text = "Patcher Mode:";
            PatcherModeLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ApplyPatchButton
            // 
            ApplyPatchButton.Checked = true;
            ApplyPatchButton.Location = new Point(153, 13);
            ApplyPatchButton.Name = "ApplyPatchButton";
            ApplyPatchButton.Size = new Size(168, 19);
            ApplyPatchButton.TabIndex = 22;
            ApplyPatchButton.TabStop = true;
            ApplyPatchButton.Text = "Apply Patch Mode";
            ApplyPatchButton.UseVisualStyleBackColor = true;
            ApplyPatchButton.CheckedChanged += ApplyPatchButton_CheckedChanged;
            // 
            // BrowsePatchOrRom
            // 
            BrowsePatchOrRom.Location = new Point(660, 41);
            BrowsePatchOrRom.Name = "BrowsePatchOrRom";
            BrowsePatchOrRom.Size = new Size(90, 23);
            BrowsePatchOrRom.TabIndex = 21;
            BrowsePatchOrRom.Text = "Browse";
            BrowsePatchOrRom.UseVisualStyleBackColor = true;
            BrowsePatchOrRom.Click += BrowsePatchOrRom_Click;
            // 
            // BpsPatchAndPatchedRomPath
            // 
            BpsPatchAndPatchedRomPath.Location = new Point(153, 42);
            BpsPatchAndPatchedRomPath.Name = "BpsPatchAndPatchedRomPath";
            BpsPatchAndPatchedRomPath.Size = new Size(501, 23);
            BpsPatchAndPatchedRomPath.TabIndex = 16;
            // 
            // BpsPatchAndPatchedRomLabel
            // 
            BpsPatchAndPatchedRomLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            BpsPatchAndPatchedRomLabel.ImageAlign = ContentAlignment.MiddleRight;
            BpsPatchAndPatchedRomLabel.Location = new Point(10, 45);
            BpsPatchAndPatchedRomLabel.Name = "BpsPatchAndPatchedRomLabel";
            BpsPatchAndPatchedRomLabel.Size = new Size(133, 15);
            BpsPatchAndPatchedRomLabel.TabIndex = 16;
            BpsPatchAndPatchedRomLabel.Text = "BPS Patch File Path:";
            BpsPatchAndPatchedRomLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CosmeticsPanelLabel
            // 
            CosmeticsPanelLabel.AutoSize = true;
            CosmeticsPanelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsPanelLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsPanelLabel.Location = new Point(17, 232);
            CosmeticsPanelLabel.Name = "CosmeticsPanelLabel";
            CosmeticsPanelLabel.Size = new Size(62, 15);
            CosmeticsPanelLabel.TabIndex = 19;
            CosmeticsPanelLabel.Text = "Cosmetics";
            CosmeticsPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CosmeticsPanel
            // 
            CosmeticsPanel.BorderStyle = BorderStyle.FixedSingle;
            CosmeticsPanel.Controls.Add(DeleteCosmeticPreset);
            CosmeticsPanel.Controls.Add(ResetDefaultCosmetics);
            CosmeticsPanel.Controls.Add(LoadCosmeticPreset);
            CosmeticsPanel.Controls.Add(CosmeticsPresets);
            CosmeticsPanel.Controls.Add(CosmeticsPresetLabel);
            CosmeticsPanel.Controls.Add(SaveCosmeticPreset);
            CosmeticsPanel.Controls.Add(GenerateCosmetics);
            CosmeticsPanel.Controls.Add(LoadCosmetics);
            CosmeticsPanel.Controls.Add(CosmeticsString);
            CosmeticsPanel.Controls.Add(CosmeticsStringLabel);
            CosmeticsPanel.Location = new Point(6, 240);
            CosmeticsPanel.Name = "CosmeticsPanel";
            CosmeticsPanel.Size = new Size(760, 140);
            CosmeticsPanel.TabIndex = 18;
            // 
            // DeleteCosmeticPreset
            // 
            DeleteCosmeticPreset.Location = new Point(510, 100);
            DeleteCosmeticPreset.Name = "DeleteCosmeticPreset";
            DeleteCosmeticPreset.Size = new Size(240, 23);
            DeleteCosmeticPreset.TabIndex = 30;
            DeleteCosmeticPreset.Text = "Delete Preset";
            DeleteCosmeticPreset.UseVisualStyleBackColor = true;
            DeleteCosmeticPreset.Click += DeleteCosmeticPreset_Click;
            // 
            // ResetDefaultCosmetics
            // 
            ResetDefaultCosmetics.Location = new Point(510, 40);
            ResetDefaultCosmetics.Name = "ResetDefaultCosmetics";
            ResetDefaultCosmetics.Size = new Size(240, 23);
            ResetDefaultCosmetics.TabIndex = 29;
            ResetDefaultCosmetics.Text = "Reset Defaults";
            ResetDefaultCosmetics.UseVisualStyleBackColor = true;
            ResetDefaultCosmetics.Click += ResetDefaultCosmetics_Click;
            // 
            // LoadCosmeticPreset
            // 
            LoadCosmeticPreset.Location = new Point(10, 100);
            LoadCosmeticPreset.Name = "LoadCosmeticPreset";
            LoadCosmeticPreset.Size = new Size(240, 23);
            LoadCosmeticPreset.TabIndex = 28;
            LoadCosmeticPreset.Text = "Load Cosmetics Preset";
            LoadCosmeticPreset.UseVisualStyleBackColor = true;
            LoadCosmeticPreset.Click += LoadCosmeticPreset_Click;
            // 
            // CosmeticsPresets
            // 
            CosmeticsPresets.FormattingEnabled = true;
            CosmeticsPresets.Location = new Point(153, 72);
            CosmeticsPresets.Name = "CosmeticsPresets";
            CosmeticsPresets.Size = new Size(597, 23);
            CosmeticsPresets.TabIndex = 27;
            // 
            // CosmeticsPresetLabel
            // 
            CosmeticsPresetLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsPresetLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsPresetLabel.Location = new Point(10, 75);
            CosmeticsPresetLabel.Name = "CosmeticsPresetLabel";
            CosmeticsPresetLabel.Size = new Size(133, 15);
            CosmeticsPresetLabel.TabIndex = 26;
            CosmeticsPresetLabel.Text = "Cosmetics Presets:";
            CosmeticsPresetLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SaveCosmeticPreset
            // 
            SaveCosmeticPreset.Location = new Point(260, 100);
            SaveCosmeticPreset.Name = "SaveCosmeticPreset";
            SaveCosmeticPreset.Size = new Size(240, 23);
            SaveCosmeticPreset.TabIndex = 25;
            SaveCosmeticPreset.Text = "Save Current As Preset";
            SaveCosmeticPreset.UseVisualStyleBackColor = true;
            SaveCosmeticPreset.Click += SaveCosmeticPreset_Click;
            // 
            // GenerateCosmetics
            // 
            GenerateCosmetics.Location = new Point(260, 40);
            GenerateCosmetics.Name = "GenerateCosmetics";
            GenerateCosmetics.Size = new Size(240, 23);
            GenerateCosmetics.TabIndex = 24;
            GenerateCosmetics.Text = "Generate String";
            GenerateCosmetics.UseVisualStyleBackColor = true;
            GenerateCosmetics.Click += GenerateCosmetics_Click;
            // 
            // LoadCosmetics
            // 
            LoadCosmetics.Location = new Point(10, 40);
            LoadCosmetics.Name = "LoadCosmetics";
            LoadCosmetics.Size = new Size(240, 23);
            LoadCosmetics.TabIndex = 23;
            LoadCosmetics.Text = "Load Cosmetics";
            LoadCosmetics.UseVisualStyleBackColor = true;
            LoadCosmetics.Click += LoadCosmetics_Click;
            // 
            // CosmeticsString
            // 
            CosmeticsString.Location = new Point(153, 12);
            CosmeticsString.Name = "CosmeticsString";
            CosmeticsString.PlaceholderText = "Paste cosmetics string here and click Load Cosmetics to load";
            CosmeticsString.Size = new Size(597, 23);
            CosmeticsString.TabIndex = 16;
            // 
            // CosmeticsStringLabel
            // 
            CosmeticsStringLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsStringLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsStringLabel.Location = new Point(10, 15);
            CosmeticsStringLabel.Name = "CosmeticsStringLabel";
            CosmeticsStringLabel.Size = new Size(133, 15);
            CosmeticsStringLabel.TabIndex = 16;
            CosmeticsStringLabel.Text = "Cosmetics String:";
            CosmeticsStringLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SettingsPaneLabel
            // 
            SettingsPaneLabel.AutoSize = true;
            SettingsPaneLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsPaneLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsPaneLabel.Location = new Point(17, 72);
            SettingsPaneLabel.Name = "SettingsPaneLabel";
            SettingsPaneLabel.Size = new Size(49, 15);
            SettingsPaneLabel.TabIndex = 17;
            SettingsPaneLabel.Text = "Settings";
            SettingsPaneLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsPanel
            // 
            SettingsPanel.BorderStyle = BorderStyle.FixedSingle;
            SettingsPanel.Controls.Add(DeleteSettingPreset);
            SettingsPanel.Controls.Add(ResetDefaultSettings);
            SettingsPanel.Controls.Add(LoadSettingPreset);
            SettingsPanel.Controls.Add(SettingPresets);
            SettingsPanel.Controls.Add(SettingsPresetLabel);
            SettingsPanel.Controls.Add(SaveSettingPreset);
            SettingsPanel.Controls.Add(GenerateSettings);
            SettingsPanel.Controls.Add(LoadSettings);
            SettingsPanel.Controls.Add(SettingString);
            SettingsPanel.Controls.Add(SettingsStringLabel);
            SettingsPanel.Location = new Point(6, 80);
            SettingsPanel.Name = "SettingsPanel";
            SettingsPanel.Size = new Size(760, 140);
            SettingsPanel.TabIndex = 16;
            // 
            // DeleteSettingPreset
            // 
            DeleteSettingPreset.Location = new Point(510, 100);
            DeleteSettingPreset.Name = "DeleteSettingPreset";
            DeleteSettingPreset.Size = new Size(240, 23);
            DeleteSettingPreset.TabIndex = 30;
            DeleteSettingPreset.Text = "Delete Preset";
            DeleteSettingPreset.UseVisualStyleBackColor = true;
            DeleteSettingPreset.Click += DeleteSettingPreset_Click;
            // 
            // ResetDefaultSettings
            // 
            ResetDefaultSettings.Location = new Point(510, 40);
            ResetDefaultSettings.Name = "ResetDefaultSettings";
            ResetDefaultSettings.Size = new Size(240, 23);
            ResetDefaultSettings.TabIndex = 29;
            ResetDefaultSettings.Text = "Reset Defaults";
            ResetDefaultSettings.UseVisualStyleBackColor = true;
            ResetDefaultSettings.Click += ResetDefaultSettings_Click;
            // 
            // LoadSettingPreset
            // 
            LoadSettingPreset.Location = new Point(10, 100);
            LoadSettingPreset.Name = "LoadSettingPreset";
            LoadSettingPreset.Size = new Size(240, 23);
            LoadSettingPreset.TabIndex = 28;
            LoadSettingPreset.Text = "Load Settings Preset";
            LoadSettingPreset.UseVisualStyleBackColor = true;
            LoadSettingPreset.Click += LoadSettingPreset_Click;
            // 
            // SettingPresets
            // 
            SettingPresets.FormattingEnabled = true;
            SettingPresets.Location = new Point(153, 72);
            SettingPresets.Name = "SettingPresets";
            SettingPresets.Size = new Size(597, 23);
            SettingPresets.TabIndex = 27;
            // 
            // SettingsPresetLabel
            // 
            SettingsPresetLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsPresetLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsPresetLabel.Location = new Point(10, 75);
            SettingsPresetLabel.Name = "SettingsPresetLabel";
            SettingsPresetLabel.Size = new Size(133, 15);
            SettingsPresetLabel.TabIndex = 26;
            SettingsPresetLabel.Text = "Settings Presets:";
            SettingsPresetLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SaveSettingPreset
            // 
            SaveSettingPreset.Location = new Point(260, 100);
            SaveSettingPreset.Name = "SaveSettingPreset";
            SaveSettingPreset.Size = new Size(240, 23);
            SaveSettingPreset.TabIndex = 25;
            SaveSettingPreset.Text = "Save Current As Preset";
            SaveSettingPreset.UseVisualStyleBackColor = true;
            SaveSettingPreset.Click += SaveSettingPreset_Click;
            // 
            // GenerateSettings
            // 
            GenerateSettings.Location = new Point(260, 40);
            GenerateSettings.Name = "GenerateSettings";
            GenerateSettings.Size = new Size(240, 23);
            GenerateSettings.TabIndex = 24;
            GenerateSettings.Text = "Generate String";
            GenerateSettings.UseVisualStyleBackColor = true;
            GenerateSettings.Click += GenerateSettings_Click;
            // 
            // LoadSettings
            // 
            LoadSettings.Location = new Point(10, 40);
            LoadSettings.Name = "LoadSettings";
            LoadSettings.Size = new Size(240, 23);
            LoadSettings.TabIndex = 23;
            LoadSettings.Text = "Load Settings";
            LoadSettings.UseVisualStyleBackColor = true;
            LoadSettings.Click += LoadSettings_Click;
            // 
            // SettingString
            // 
            SettingString.Location = new Point(153, 12);
            SettingString.Name = "SettingString";
            SettingString.PlaceholderText = "Paste settings string here and click Load Settings to load";
            SettingString.Size = new Size(597, 23);
            SettingString.TabIndex = 16;
            // 
            // SettingsStringLabel
            // 
            SettingsStringLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsStringLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsStringLabel.Location = new Point(10, 15);
            SettingsStringLabel.Name = "SettingsStringLabel";
            SettingsStringLabel.Size = new Size(133, 15);
            SettingsStringLabel.TabIndex = 16;
            SettingsStringLabel.Text = "Settings String:";
            SettingsStringLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RandomSeed
            // 
            RandomSeed.Location = new Point(676, 36);
            RandomSeed.Name = "RandomSeed";
            RandomSeed.Size = new Size(90, 23);
            RandomSeed.TabIndex = 13;
            RandomSeed.Text = "New Seed";
            RandomSeed.UseVisualStyleBackColor = true;
            RandomSeed.Click += RandomSeed_Click;
            // 
            // Seed
            // 
            Seed.Location = new Point(160, 37);
            Seed.MaxLength = 16;
            Seed.Name = "Seed";
            Seed.Size = new Size(510, 23);
            Seed.TabIndex = 12;
            // 
            // RomPath
            // 
            RomPath.Location = new Point(160, 7);
            RomPath.Name = "RomPath";
            RomPath.Size = new Size(510, 23);
            RomPath.TabIndex = 10;
            // 
            // BrowseRom
            // 
            BrowseRom.Location = new Point(676, 6);
            BrowseRom.Name = "BrowseRom";
            BrowseRom.Size = new Size(90, 23);
            BrowseRom.TabIndex = 11;
            BrowseRom.Text = "Browse";
            BrowseRom.UseVisualStyleBackColor = true;
            BrowseRom.Click += BrowseRom_Click;
            // 
            // EuTmcRomLabel
            // 
            EuTmcRomLabel.AutoSize = true;
            EuTmcRomLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            EuTmcRomLabel.ImageAlign = ContentAlignment.MiddleRight;
            EuTmcRomLabel.Location = new Point(4, 10);
            EuTmcRomLabel.Name = "EuTmcRomLabel";
            EuTmcRomLabel.Size = new Size(150, 15);
            EuTmcRomLabel.TabIndex = 9;
            EuTmcRomLabel.Text = "European Minish Cap ROM";
            EuTmcRomLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RandoSeedLabel
            // 
            RandoSeedLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            RandoSeedLabel.Location = new Point(6, 40);
            RandoSeedLabel.Name = "RandoSeedLabel";
            RandoSeedLabel.Size = new Size(148, 15);
            RandoSeedLabel.TabIndex = 8;
            RandoSeedLabel.Text = "Randomizer Seed";
            RandoSeedLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TabPane
            // 
            TabPane.Controls.Add(General);
            TabPane.Controls.Add(Advanced);
            TabPane.Controls.Add(SeedOutput);
            TabPane.Location = new Point(12, 27);
            TabPane.Name = "TabPane";
            TabPane.SelectedIndex = 0;
            TabPane.Size = new Size(795, 563);
            TabPane.TabIndex = 1;
            // 
            // Advanced
            // 
            Advanced.Controls.Add(AlternativeShufflersPanelLabel);
            Advanced.Controls.Add(AlternativeShufflerPanel);
            Advanced.Controls.Add(AdvancedSettingsLabel);
            Advanced.Controls.Add(LogicPatchesYamlLabel);
            Advanced.Controls.Add(LogicPatchesYamlPanel);
            Advanced.Controls.Add(MysterySettingsLabel);
            Advanced.Controls.Add(MysterySettingsPanel);
            Advanced.Location = new Point(4, 24);
            Advanced.Name = "Advanced";
            Advanced.Padding = new Padding(3);
            Advanced.Size = new Size(787, 535);
            Advanced.TabIndex = 3;
            Advanced.Text = "Advanced";
            Advanced.UseVisualStyleBackColor = true;
            // 
            // AlternativeShufflersPanelLabel
            // 
            AlternativeShufflersPanelLabel.AutoSize = true;
            AlternativeShufflersPanelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            AlternativeShufflersPanelLabel.ImageAlign = ContentAlignment.MiddleRight;
            AlternativeShufflersPanelLabel.Location = new Point(17, 387);
            AlternativeShufflersPanelLabel.Name = "AlternativeShufflersPanelLabel";
            AlternativeShufflersPanelLabel.Size = new Size(113, 15);
            AlternativeShufflersPanelLabel.TabIndex = 20;
            AlternativeShufflersPanelLabel.Text = "Alternative Shufflers";
            AlternativeShufflersPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlternativeShufflerPanel
            // 
            AlternativeShufflerPanel.BorderStyle = BorderStyle.FixedSingle;
            AlternativeShufflerPanel.Controls.Add(UseSphereBasedShuffler);
            AlternativeShufflerPanel.Location = new Point(6, 395);
            AlternativeShufflerPanel.Name = "AlternativeShufflerPanel";
            AlternativeShufflerPanel.Size = new Size(760, 50);
            AlternativeShufflerPanel.TabIndex = 19;
            // 
            // UseSphereBasedShuffler
            // 
            UseSphereBasedShuffler.AutoSize = true;
            UseSphereBasedShuffler.CheckAlign = ContentAlignment.MiddleRight;
            UseSphereBasedShuffler.Location = new Point(10, 15);
            UseSphereBasedShuffler.Name = "UseSphereBasedShuffler";
            UseSphereBasedShuffler.Size = new Size(165, 19);
            UseSphereBasedShuffler.TabIndex = 28;
            UseSphereBasedShuffler.Text = "Use Hendrus Seed Shuffler";
            UseSphereBasedShuffler.TextAlign = ContentAlignment.MiddleRight;
            UseSphereBasedShuffler.UseVisualStyleBackColor = true;
            UseSphereBasedShuffler.CheckedChanged += UseSphereBasedShuffler_CheckedChanged;
            // 
            // AdvancedSettingsLabel
            // 
            AdvancedSettingsLabel.AutoSize = true;
            AdvancedSettingsLabel.Location = new Point(17, 15);
            AdvancedSettingsLabel.MaximumSize = new Size(760, 0);
            AdvancedSettingsLabel.Name = "AdvancedSettingsLabel";
            AdvancedSettingsLabel.Size = new Size(723, 30);
            AdvancedSettingsLabel.TabIndex = 18;
            AdvancedSettingsLabel.Text = "This page contains advanced settings and experimental features for the randomizer. Unless you are experienced with the randomizer we recommend not modifying these settings.";
            // 
            // LogicPatchesYamlLabel
            // 
            LogicPatchesYamlLabel.AutoSize = true;
            LogicPatchesYamlLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            LogicPatchesYamlLabel.ImageAlign = ContentAlignment.MiddleRight;
            LogicPatchesYamlLabel.Location = new Point(17, 57);
            LogicPatchesYamlLabel.Name = "LogicPatchesYamlLabel";
            LogicPatchesYamlLabel.Size = new Size(243, 15);
            LogicPatchesYamlLabel.TabIndex = 17;
            LogicPatchesYamlLabel.Text = "Custom Logic, Patches and Global YAML File";
            LogicPatchesYamlLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LogicPatchesYamlPanel
            // 
            LogicPatchesYamlPanel.BorderStyle = BorderStyle.FixedSingle;
            LogicPatchesYamlPanel.Controls.Add(BrowseCustomYAML);
            LogicPatchesYamlPanel.Controls.Add(YAMLPath);
            LogicPatchesYamlPanel.Controls.Add(YamlFilePathLabel);
            LogicPatchesYamlPanel.Controls.Add(UseCustomYAML);
            LogicPatchesYamlPanel.Controls.Add(BrowseCustomPatch);
            LogicPatchesYamlPanel.Controls.Add(RomBuildfilePath);
            LogicPatchesYamlPanel.Controls.Add(RomBuildfinePathLabel);
            LogicPatchesYamlPanel.Controls.Add(UseCustomPatch);
            LogicPatchesYamlPanel.Controls.Add(BrowseCustomLogicFile);
            LogicPatchesYamlPanel.Controls.Add(LogicFilePath);
            LogicPatchesYamlPanel.Controls.Add(UseCustomLogic);
            LogicPatchesYamlPanel.Controls.Add(LogicFilePathLabel);
            LogicPatchesYamlPanel.Location = new Point(6, 65);
            LogicPatchesYamlPanel.Name = "LogicPatchesYamlPanel";
            LogicPatchesYamlPanel.Size = new Size(760, 200);
            LogicPatchesYamlPanel.TabIndex = 16;
            // 
            // BrowseCustomYAML
            // 
            BrowseCustomYAML.Location = new Point(660, 161);
            BrowseCustomYAML.Name = "BrowseCustomYAML";
            BrowseCustomYAML.Size = new Size(90, 23);
            BrowseCustomYAML.TabIndex = 20;
            BrowseCustomYAML.Text = "Browse";
            BrowseCustomYAML.UseVisualStyleBackColor = true;
            BrowseCustomYAML.Click += BrowseCustomYAML_Click;
            // 
            // YAMLPath
            // 
            YAMLPath.Enabled = false;
            YAMLPath.Location = new Point(153, 162);
            YAMLPath.Name = "YAMLPath";
            YAMLPath.Size = new Size(501, 23);
            YAMLPath.TabIndex = 21;
            // 
            // YamlFilePathLabel
            // 
            YamlFilePathLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            YamlFilePathLabel.ImageAlign = ContentAlignment.MiddleRight;
            YamlFilePathLabel.Location = new Point(10, 165);
            YamlFilePathLabel.Name = "YamlFilePathLabel";
            YamlFilePathLabel.Size = new Size(133, 15);
            YamlFilePathLabel.TabIndex = 22;
            YamlFilePathLabel.Text = "YAML File Path:";
            YamlFilePathLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // UseCustomYAML
            // 
            UseCustomYAML.CheckAlign = ContentAlignment.MiddleRight;
            UseCustomYAML.Location = new Point(10, 135);
            UseCustomYAML.Name = "UseCustomYAML";
            UseCustomYAML.Size = new Size(160, 19);
            UseCustomYAML.TabIndex = 19;
            UseCustomYAML.Text = "Use Global YAML File";
            UseCustomYAML.TextAlign = ContentAlignment.MiddleRight;
            UseCustomYAML.UseVisualStyleBackColor = true;
            UseCustomYAML.CheckedChanged += UseCustomYAML_CheckedChanged;
            // 
            // BrowseCustomPatch
            // 
            BrowseCustomPatch.Location = new Point(660, 101);
            BrowseCustomPatch.Name = "BrowseCustomPatch";
            BrowseCustomPatch.Size = new Size(90, 23);
            BrowseCustomPatch.TabIndex = 20;
            BrowseCustomPatch.Text = "Browse";
            BrowseCustomPatch.UseVisualStyleBackColor = true;
            BrowseCustomPatch.Click += BrowseCustomPatch_Click;
            // 
            // RomBuildfilePath
            // 
            RomBuildfilePath.Enabled = false;
            RomBuildfilePath.Location = new Point(153, 102);
            RomBuildfilePath.Name = "RomBuildfilePath";
            RomBuildfilePath.Size = new Size(501, 23);
            RomBuildfilePath.TabIndex = 21;
            // 
            // RomBuildfinePathLabel
            // 
            RomBuildfinePathLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            RomBuildfinePathLabel.ImageAlign = ContentAlignment.MiddleRight;
            RomBuildfinePathLabel.Location = new Point(10, 105);
            RomBuildfinePathLabel.Name = "RomBuildfinePathLabel";
            RomBuildfinePathLabel.Size = new Size(133, 15);
            RomBuildfinePathLabel.TabIndex = 22;
            RomBuildfinePathLabel.Text = "ROM Buildfile Path:";
            RomBuildfinePathLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // UseCustomPatch
            // 
            UseCustomPatch.CheckAlign = ContentAlignment.MiddleRight;
            UseCustomPatch.Location = new Point(10, 75);
            UseCustomPatch.Name = "UseCustomPatch";
            UseCustomPatch.Size = new Size(160, 19);
            UseCustomPatch.TabIndex = 19;
            UseCustomPatch.Text = "Use Custom Patch Files";
            UseCustomPatch.TextAlign = ContentAlignment.MiddleRight;
            UseCustomPatch.UseVisualStyleBackColor = true;
            UseCustomPatch.CheckedChanged += UseCustomPatch_CheckedChanged;
            // 
            // BrowseCustomLogicFile
            // 
            BrowseCustomLogicFile.Location = new Point(660, 41);
            BrowseCustomLogicFile.Name = "BrowseCustomLogicFile";
            BrowseCustomLogicFile.Size = new Size(90, 23);
            BrowseCustomLogicFile.TabIndex = 16;
            BrowseCustomLogicFile.Text = "Browse";
            BrowseCustomLogicFile.UseVisualStyleBackColor = true;
            BrowseCustomLogicFile.Click += BrowseCustomLogicFile_Click;
            // 
            // LogicFilePath
            // 
            LogicFilePath.Enabled = false;
            LogicFilePath.Location = new Point(153, 42);
            LogicFilePath.Name = "LogicFilePath";
            LogicFilePath.Size = new Size(501, 23);
            LogicFilePath.TabIndex = 16;
            // 
            // UseCustomLogic
            // 
            UseCustomLogic.CheckAlign = ContentAlignment.MiddleRight;
            UseCustomLogic.Location = new Point(10, 15);
            UseCustomLogic.Name = "UseCustomLogic";
            UseCustomLogic.Size = new Size(160, 19);
            UseCustomLogic.TabIndex = 17;
            UseCustomLogic.Text = "Use Custom Logic File";
            UseCustomLogic.TextAlign = ContentAlignment.MiddleRight;
            UseCustomLogic.UseVisualStyleBackColor = true;
            UseCustomLogic.CheckedChanged += UseCustomLogic_CheckedChanged;
            // 
            // LogicFilePathLabel
            // 
            LogicFilePathLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            LogicFilePathLabel.ImageAlign = ContentAlignment.MiddleRight;
            LogicFilePathLabel.Location = new Point(10, 45);
            LogicFilePathLabel.Name = "LogicFilePathLabel";
            LogicFilePathLabel.Size = new Size(133, 15);
            LogicFilePathLabel.TabIndex = 16;
            LogicFilePathLabel.Text = "Logic File Path:";
            LogicFilePathLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MysterySettingsLabel
            // 
            MysterySettingsLabel.AutoSize = true;
            MysterySettingsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MysterySettingsLabel.ImageAlign = ContentAlignment.MiddleRight;
            MysterySettingsLabel.Location = new Point(17, 277);
            MysterySettingsLabel.Name = "MysterySettingsLabel";
            MysterySettingsLabel.Size = new Size(49, 15);
            MysterySettingsLabel.TabIndex = 22;
            MysterySettingsLabel.Text = "Mystery";
            MysterySettingsLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MysterySettingsPanel
            // 
            MysterySettingsPanel.BorderStyle = BorderStyle.FixedSingle;
            MysterySettingsPanel.Controls.Add(UseMysteryCosmetics);
            MysterySettingsPanel.Controls.Add(LoadCosmeticSample);
            MysterySettingsPanel.Controls.Add(CosmeticsWeights);
            MysterySettingsPanel.Controls.Add(UseMysterySettings);
            MysterySettingsPanel.Controls.Add(LoadSettingSample);
            MysterySettingsPanel.Controls.Add(SettingsWeights);
            MysterySettingsPanel.Location = new Point(6, 285);
            MysterySettingsPanel.Name = "MysterySettingsPanel";
            MysterySettingsPanel.Size = new Size(760, 90);
            MysterySettingsPanel.TabIndex = 21;
            // 
            // UseMysteryCosmetics
            // 
            UseMysteryCosmetics.CheckAlign = ContentAlignment.MiddleRight;
            UseMysteryCosmetics.Location = new Point(10, 49);
            UseMysteryCosmetics.Name = "UseMysteryCosmetics";
            UseMysteryCosmetics.Size = new Size(160, 24);
            UseMysteryCosmetics.TabIndex = 34;
            UseMysteryCosmetics.Text = "Use Mystery Cosmetics";
            UseMysteryCosmetics.TextAlign = ContentAlignment.MiddleRight;
            UseMysteryCosmetics.UseVisualStyleBackColor = true;
            UseMysteryCosmetics.CheckedChanged += UseMysteryCosmetics_CheckedChanged;
            // 
            // LoadCosmeticSample
            // 
            LoadCosmeticSample.Location = new Point(540, 48);
            LoadCosmeticSample.Name = "LoadCosmeticSample";
            LoadCosmeticSample.Size = new Size(210, 23);
            LoadCosmeticSample.TabIndex = 33;
            LoadCosmeticSample.Text = "Load Sample Options";
            LoadCosmeticSample.UseVisualStyleBackColor = true;
            LoadCosmeticSample.Click += LoadCosmeticSample_Click;
            // 
            // CosmeticsWeights
            // 
            CosmeticsWeights.FormattingEnabled = true;
            CosmeticsWeights.Location = new Point(180, 49);
            CosmeticsWeights.Name = "CosmeticsWeights";
            CosmeticsWeights.Size = new Size(350, 23);
            CosmeticsWeights.TabIndex = 32;
            // 
            // UseMysterySettings
            // 
            UseMysterySettings.CheckAlign = ContentAlignment.MiddleRight;
            UseMysterySettings.Location = new Point(10, 14);
            UseMysterySettings.Name = "UseMysterySettings";
            UseMysterySettings.Size = new Size(160, 24);
            UseMysterySettings.TabIndex = 31;
            UseMysterySettings.Text = "Use Mystery Settings";
            UseMysterySettings.TextAlign = ContentAlignment.MiddleRight;
            UseMysterySettings.UseVisualStyleBackColor = true;
            UseMysterySettings.CheckedChanged += UseMysterySettings_CheckedChanged;
            // 
            // LoadSettingSample
            // 
            LoadSettingSample.Location = new Point(540, 13);
            LoadSettingSample.Name = "LoadSettingSample";
            LoadSettingSample.Size = new Size(210, 23);
            LoadSettingSample.TabIndex = 28;
            LoadSettingSample.Text = "Load Sample Options";
            LoadSettingSample.UseVisualStyleBackColor = true;
            LoadSettingSample.Click += LoadSettingSample_Click;
            // 
            // SettingsWeights
            // 
            SettingsWeights.FormattingEnabled = true;
            SettingsWeights.Location = new Point(180, 14);
            SettingsWeights.Name = "SettingsWeights";
            SettingsWeights.Size = new Size(350, 23);
            SettingsWeights.TabIndex = 27;
            // 
            // SeedOutput
            // 
            SeedOutput.Controls.Add(SaveRom);
            SeedOutput.Controls.Add(SavePatch);
            SeedOutput.Controls.Add(SaveSpoiler);
            SeedOutput.Controls.Add(CosmeticsInformationPanelLabel);
            SeedOutput.Controls.Add(CosmeticsInformationPanel);
            SeedOutput.Controls.Add(SettingsInformationPanelLabel);
            SeedOutput.Controls.Add(SettingsInformationPanel);
            SeedOutput.Controls.Add(SeedInformationPanelLabel);
            SeedOutput.Controls.Add(SeedInformationPanel);
            SeedOutput.Location = new Point(4, 24);
            SeedOutput.Name = "SeedOutput";
            SeedOutput.Size = new Size(787, 535);
            SeedOutput.TabIndex = 2;
            SeedOutput.Text = "Seed Output";
            SeedOutput.UseVisualStyleBackColor = true;
            // 
            // SaveRom
            // 
            SaveRom.Location = new Point(519, 500);
            SaveRom.Name = "SaveRom";
            SaveRom.Size = new Size(247, 23);
            SaveRom.TabIndex = 24;
            SaveRom.Text = "Save ROM";
            SaveRom.UseVisualStyleBackColor = true;
            SaveRom.Click += SaveRom_Click;
            // 
            // SavePatch
            // 
            SavePatch.Location = new Point(263, 500);
            SavePatch.Name = "SavePatch";
            SavePatch.Size = new Size(246, 23);
            SavePatch.TabIndex = 23;
            SavePatch.Text = "Save BPS Patch";
            SavePatch.UseVisualStyleBackColor = true;
            SavePatch.Click += SavePatch_Click;
            // 
            // SaveSpoiler
            // 
            SaveSpoiler.Location = new Point(6, 500);
            SaveSpoiler.Name = "SaveSpoiler";
            SaveSpoiler.Size = new Size(247, 23);
            SaveSpoiler.TabIndex = 22;
            SaveSpoiler.Text = "Save Spoiler Log";
            SaveSpoiler.UseVisualStyleBackColor = true;
            SaveSpoiler.Click += SaveSpoiler_Click;
            // 
            // CosmeticsInformationPanelLabel
            // 
            CosmeticsInformationPanelLabel.AutoSize = true;
            CosmeticsInformationPanelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsInformationPanelLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsInformationPanelLabel.Location = new Point(17, 282);
            CosmeticsInformationPanelLabel.Name = "CosmeticsInformationPanelLabel";
            CosmeticsInformationPanelLabel.Size = new Size(128, 15);
            CosmeticsInformationPanelLabel.TabIndex = 21;
            CosmeticsInformationPanelLabel.Text = "Cosmetics Information";
            CosmeticsInformationPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CosmeticsInformationPanel
            // 
            CosmeticsInformationPanel.BorderStyle = BorderStyle.FixedSingle;
            CosmeticsInformationPanel.Controls.Add(CopyCosmeticsHashToClipboard);
            CosmeticsInformationPanel.Controls.Add(CosmeticStringLabel);
            CosmeticsInformationPanel.Controls.Add(CosmeticsStringSeedOutputLabel);
            CosmeticsInformationPanel.Controls.Add(CosmeticNameLabel);
            CosmeticsInformationPanel.Controls.Add(CosmeticsNameLabel);
            CosmeticsInformationPanel.Location = new Point(6, 290);
            CosmeticsInformationPanel.Name = "CosmeticsInformationPanel";
            CosmeticsInformationPanel.Size = new Size(760, 105);
            CosmeticsInformationPanel.TabIndex = 20;
            // 
            // CopyCosmeticsHashToClipboard
            // 
            CopyCosmeticsHashToClipboard.Location = new Point(525, 70);
            CopyCosmeticsHashToClipboard.Name = "CopyCosmeticsHashToClipboard";
            CopyCosmeticsHashToClipboard.Size = new Size(225, 23);
            CopyCosmeticsHashToClipboard.TabIndex = 24;
            CopyCosmeticsHashToClipboard.Text = "Copy Cosmetics String To Clipboard";
            CopyCosmeticsHashToClipboard.UseVisualStyleBackColor = true;
            CopyCosmeticsHashToClipboard.Click += CopyCosmeticsHashToClipboard_Click;
            // 
            // CosmeticStringLabel
            // 
            CosmeticStringLabel.AutoEllipsis = true;
            CosmeticStringLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticStringLabel.Location = new Point(198, 45);
            CosmeticStringLabel.Name = "CosmeticStringLabel";
            CosmeticStringLabel.Size = new Size(552, 15);
            CosmeticStringLabel.TabIndex = 23;
            CosmeticStringLabel.Text = "Cosmetics String Goes Here";
            CosmeticStringLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CosmeticsStringSeedOutputLabel
            // 
            CosmeticsStringSeedOutputLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsStringSeedOutputLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsStringSeedOutputLabel.Location = new Point(10, 45);
            CosmeticsStringSeedOutputLabel.Name = "CosmeticsStringSeedOutputLabel";
            CosmeticsStringSeedOutputLabel.Size = new Size(178, 15);
            CosmeticsStringSeedOutputLabel.TabIndex = 22;
            CosmeticsStringSeedOutputLabel.Text = "Cosmetics String:";
            CosmeticsStringSeedOutputLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CosmeticNameLabel
            // 
            CosmeticNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticNameLabel.Location = new Point(198, 15);
            CosmeticNameLabel.Name = "CosmeticNameLabel";
            CosmeticNameLabel.Size = new Size(552, 15);
            CosmeticNameLabel.TabIndex = 19;
            CosmeticNameLabel.Text = "None";
            CosmeticNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CosmeticsNameLabel
            // 
            CosmeticsNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CosmeticsNameLabel.ImageAlign = ContentAlignment.MiddleRight;
            CosmeticsNameLabel.Location = new Point(10, 15);
            CosmeticsNameLabel.Name = "CosmeticsNameLabel";
            CosmeticsNameLabel.Size = new Size(178, 15);
            CosmeticsNameLabel.TabIndex = 18;
            CosmeticsNameLabel.Text = "Cosmetics Name:";
            CosmeticsNameLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SettingsInformationPanelLabel
            // 
            SettingsInformationPanelLabel.AutoSize = true;
            SettingsInformationPanelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsInformationPanelLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsInformationPanelLabel.Location = new Point(17, 157);
            SettingsInformationPanelLabel.Name = "SettingsInformationPanelLabel";
            SettingsInformationPanelLabel.Size = new Size(115, 15);
            SettingsInformationPanelLabel.TabIndex = 19;
            SettingsInformationPanelLabel.Text = "Settings Information";
            SettingsInformationPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsInformationPanel
            // 
            SettingsInformationPanel.BorderStyle = BorderStyle.FixedSingle;
            SettingsInformationPanel.Controls.Add(CopySettingsHashToClipboard);
            SettingsInformationPanel.Controls.Add(SettingHashLabel);
            SettingsInformationPanel.Controls.Add(SettingsStringSeedOutputLabel);
            SettingsInformationPanel.Controls.Add(SettingNameLabel);
            SettingsInformationPanel.Controls.Add(SettingsNameLabel);
            SettingsInformationPanel.Location = new Point(6, 165);
            SettingsInformationPanel.Name = "SettingsInformationPanel";
            SettingsInformationPanel.Size = new Size(760, 105);
            SettingsInformationPanel.TabIndex = 18;
            // 
            // CopySettingsHashToClipboard
            // 
            CopySettingsHashToClipboard.Location = new Point(525, 70);
            CopySettingsHashToClipboard.Name = "CopySettingsHashToClipboard";
            CopySettingsHashToClipboard.Size = new Size(225, 23);
            CopySettingsHashToClipboard.TabIndex = 24;
            CopySettingsHashToClipboard.Text = "Copy Settings String To Clipboard";
            CopySettingsHashToClipboard.UseVisualStyleBackColor = true;
            CopySettingsHashToClipboard.Click += CopySettingsHashToClipboard_Click;
            // 
            // SettingHashLabel
            // 
            SettingHashLabel.AutoEllipsis = true;
            SettingHashLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingHashLabel.Location = new Point(198, 45);
            SettingHashLabel.Name = "SettingHashLabel";
            SettingHashLabel.Size = new Size(552, 15);
            SettingHashLabel.TabIndex = 23;
            SettingHashLabel.Text = "Settings String Goes Here";
            SettingHashLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SettingsStringSeedOutputLabel
            // 
            SettingsStringSeedOutputLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsStringSeedOutputLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsStringSeedOutputLabel.Location = new Point(10, 45);
            SettingsStringSeedOutputLabel.Name = "SettingsStringSeedOutputLabel";
            SettingsStringSeedOutputLabel.Size = new Size(178, 15);
            SettingsStringSeedOutputLabel.TabIndex = 22;
            SettingsStringSeedOutputLabel.Text = "Settings String:";
            SettingsStringSeedOutputLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SettingNameLabel
            // 
            SettingNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingNameLabel.Location = new Point(198, 15);
            SettingNameLabel.Name = "SettingNameLabel";
            SettingNameLabel.Size = new Size(552, 15);
            SettingNameLabel.TabIndex = 19;
            SettingNameLabel.Text = "None";
            SettingNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SettingsNameLabel
            // 
            SettingsNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsNameLabel.ImageAlign = ContentAlignment.MiddleRight;
            SettingsNameLabel.Location = new Point(10, 15);
            SettingsNameLabel.Name = "SettingsNameLabel";
            SettingsNameLabel.Size = new Size(178, 15);
            SettingsNameLabel.TabIndex = 18;
            SettingsNameLabel.Text = "Settings Name:";
            SettingsNameLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SeedInformationPanelLabel
            // 
            SeedInformationPanelLabel.AutoSize = true;
            SeedInformationPanelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SeedInformationPanelLabel.ImageAlign = ContentAlignment.MiddleRight;
            SeedInformationPanelLabel.Location = new Point(17, 7);
            SeedInformationPanelLabel.Name = "SeedInformationPanelLabel";
            SeedInformationPanelLabel.Size = new Size(98, 15);
            SeedInformationPanelLabel.TabIndex = 17;
            SeedInformationPanelLabel.Text = "Seed Information";
            SeedInformationPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SeedInformationPanel
            // 
            SeedInformationPanel.BorderStyle = BorderStyle.FixedSingle;
            SeedInformationPanel.Controls.Add(CopyHashToClipboard);
            SeedInformationPanel.Controls.Add(RomHashPanel);
            SeedInformationPanel.Controls.Add(RomHashLabel);
            SeedInformationPanel.Controls.Add(OutputSeedLabel);
            SeedInformationPanel.Controls.Add(OutputSeedNumberLabel);
            SeedInformationPanel.Controls.Add(InputSeedLabel);
            SeedInformationPanel.Controls.Add(InputSeedNumberLabel);
            SeedInformationPanel.Location = new Point(6, 15);
            SeedInformationPanel.Name = "SeedInformationPanel";
            SeedInformationPanel.Size = new Size(760, 135);
            SeedInformationPanel.TabIndex = 16;
            // 
            // CopyHashToClipboard
            // 
            CopyHashToClipboard.Location = new Point(525, 100);
            CopyHashToClipboard.Name = "CopyHashToClipboard";
            CopyHashToClipboard.Size = new Size(225, 23);
            CopyHashToClipboard.TabIndex = 25;
            CopyHashToClipboard.Text = "Copy Hash To Clipboard";
            CopyHashToClipboard.UseVisualStyleBackColor = true;
            CopyHashToClipboard.Click += CopyHashToClipboard_Click;
            // 
            // RomHashPanel
            // 
            RomHashPanel.Location = new Point(230, 42);
            RomHashPanel.Margin = new Padding(0);
            RomHashPanel.Name = "RomHashPanel";
            RomHashPanel.Size = new Size(384, 48);
            RomHashPanel.TabIndex = 23;
            // 
            // RomHashLabel
            // 
            RomHashLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            RomHashLabel.ImageAlign = ContentAlignment.MiddleRight;
            RomHashLabel.Location = new Point(10, 59);
            RomHashLabel.Name = "RomHashLabel";
            RomHashLabel.Size = new Size(200, 15);
            RomHashLabel.TabIndex = 22;
            RomHashLabel.Text = "ROM Hash:";
            RomHashLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // OutputSeedLabel
            // 
            OutputSeedLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            OutputSeedLabel.Location = new Point(573, 15);
            OutputSeedLabel.Name = "OutputSeedLabel";
            OutputSeedLabel.Size = new Size(177, 15);
            OutputSeedLabel.TabIndex = 21;
            OutputSeedLabel.Text = "None";
            OutputSeedLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // OutputSeedNumberLabel
            // 
            OutputSeedNumberLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            OutputSeedNumberLabel.ImageAlign = ContentAlignment.MiddleRight;
            OutputSeedNumberLabel.Location = new Point(385, 15);
            OutputSeedNumberLabel.Name = "OutputSeedNumberLabel";
            OutputSeedNumberLabel.Size = new Size(178, 15);
            OutputSeedNumberLabel.TabIndex = 20;
            OutputSeedNumberLabel.Text = "Output Seed Number:";
            OutputSeedNumberLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // InputSeedLabel
            // 
            InputSeedLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            InputSeedLabel.Location = new Point(198, 15);
            InputSeedLabel.Name = "InputSeedLabel";
            InputSeedLabel.Size = new Size(177, 15);
            InputSeedLabel.TabIndex = 19;
            InputSeedLabel.Text = "None";
            InputSeedLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // InputSeedNumberLabel
            // 
            InputSeedNumberLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            InputSeedNumberLabel.ImageAlign = ContentAlignment.MiddleRight;
            InputSeedNumberLabel.Location = new Point(10, 15);
            InputSeedNumberLabel.Name = "InputSeedNumberLabel";
            InputSeedNumberLabel.Size = new Size(178, 15);
            InputSeedNumberLabel.TabIndex = 18;
            InputSeedNumberLabel.Text = "Input Seed Number:";
            InputSeedNumberLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MaxRandomizationAttemptsLabel
            // 
            MaxRandomizationAttemptsLabel.AutoSize = true;
            MaxRandomizationAttemptsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MaxRandomizationAttemptsLabel.ImageAlign = ContentAlignment.MiddleRight;
            MaxRandomizationAttemptsLabel.Location = new Point(16, 600);
            MaxRandomizationAttemptsLabel.Name = "MaxRandomizationAttemptsLabel";
            MaxRandomizationAttemptsLabel.Size = new Size(168, 15);
            MaxRandomizationAttemptsLabel.TabIndex = 26;
            MaxRandomizationAttemptsLabel.Text = "Max Randomization Attempts:";
            MaxRandomizationAttemptsLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RandomizationAttempts
            // 
            RandomizationAttempts.Location = new Point(190, 596);
            RandomizationAttempts.Name = "RandomizationAttempts";
            RandomizationAttempts.Size = new Size(136, 23);
            RandomizationAttempts.TabIndex = 27;
            RandomizationAttempts.Text = "1";
            RandomizationAttempts.TextAlign = HorizontalAlignment.Right;
            // 
            // MinishCapRandomizerUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(816, 631);
            Controls.Add(RandomizationAttempts);
            Controls.Add(MaxRandomizationAttemptsLabel);
            Controls.Add(Randomize);
            Controls.Add(TabPane);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MinishCapRandomizerUI";
            Text = "=O the title is dynamic now! =O";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            General.ResumeLayout(false);
            General.PerformLayout();
            PatcherAndPatchGeneratorPanel.ResumeLayout(false);
            PatcherAndPatchGeneratorPanel.PerformLayout();
            CosmeticsPanel.ResumeLayout(false);
            CosmeticsPanel.PerformLayout();
            SettingsPanel.ResumeLayout(false);
            SettingsPanel.PerformLayout();
            TabPane.ResumeLayout(false);
            Advanced.ResumeLayout(false);
            Advanced.PerformLayout();
            AlternativeShufflerPanel.ResumeLayout(false);
            AlternativeShufflerPanel.PerformLayout();
            LogicPatchesYamlPanel.ResumeLayout(false);
            LogicPatchesYamlPanel.PerformLayout();
            MysterySettingsPanel.ResumeLayout(false);
            SeedOutput.ResumeLayout(false);
            SeedOutput.PerformLayout();
            CosmeticsInformationPanel.ResumeLayout(false);
            SettingsInformationPanel.ResumeLayout(false);
            SeedInformationPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStripMenuItem changelogToolStripMenuItem;
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
    }
}
