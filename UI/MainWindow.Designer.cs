namespace MinishRandomizer
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomize = new System.Windows.Forms.Button();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.seedLabel = new System.Windows.Forms.Label();
            this.seedField = new System.Windows.Forms.TextBox();
            this.customPatchCheckBox = new System.Windows.Forms.CheckBox();
            this.customPatchPath = new System.Windows.Forms.TextBox();
            this.browsePatchButton = new System.Windows.Forms.Button();
            this.customLogicPath = new System.Windows.Forms.TextBox();
            this.browseLogicButton = new System.Windows.Forms.Button();
            this.customLogicCheckBox = new System.Windows.Forms.CheckBox();
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.optionTabPage = new System.Windows.Forms.TabPage();
            this.optionControlLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.gimmicksTabPage = new System.Windows.Forms.TabPage();
            this.gimmickControlLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.generatedTab = new System.Windows.Forms.TabPage();
            this.gimmickHashValue = new System.Windows.Forms.Label();
            this.settingHashValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.generatedLogicLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.generatedSeedValue = new System.Windows.Forms.Label();
            this.generatedSeedLabel = new System.Windows.Forms.Label();
            this.saveSpoilerButton = new System.Windows.Forms.Button();
            this.savePatchButton = new System.Windows.Forms.Button();
            this.saveRomButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.settingsStringBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.mainTabs.SuspendLayout();
            this.optionTabPage.SuspendLayout();
            this.gimmicksTabPage.SuspendLayout();
            this.generatedTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 294);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(390, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(390, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // randomize
            // 
            this.randomize.Location = new System.Drawing.Point(19, 268);
            this.randomize.Name = "randomize";
            this.randomize.Size = new System.Drawing.Size(75, 23);
            this.randomize.TabIndex = 2;
            this.randomize.Text = "Randomize";
            this.randomize.UseVisualStyleBackColor = true;
            this.randomize.Click += new System.EventHandler(this.Randomize_Click);
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.button1);
            this.generalTab.Controls.Add(this.seedLabel);
            this.generalTab.Controls.Add(this.seedField);
            this.generalTab.Controls.Add(this.customPatchCheckBox);
            this.generalTab.Controls.Add(this.customPatchPath);
            this.generalTab.Controls.Add(this.browsePatchButton);
            this.generalTab.Controls.Add(this.customLogicPath);
            this.generalTab.Controls.Add(this.browseLogicButton);
            this.generalTab.Controls.Add(this.customLogicCheckBox);
            this.generalTab.Location = new System.Drawing.Point(4, 22);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(361, 209);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(153, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Random Seed";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point(6, 13);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(35, 13);
            this.seedLabel.TabIndex = 12;
            this.seedLabel.Text = "Seed:";
            // 
            // seedField
            // 
            this.seedField.Location = new System.Drawing.Point(47, 10);
            this.seedField.Name = "seedField";
            this.seedField.Size = new System.Drawing.Size(100, 20);
            this.seedField.TabIndex = 10;
            // 
            // customPatchCheckBox
            // 
            this.customPatchCheckBox.AutoSize = true;
            this.customPatchCheckBox.Location = new System.Drawing.Point(5, 100);
            this.customPatchCheckBox.Name = "customPatchCheckBox";
            this.customPatchCheckBox.Size = new System.Drawing.Size(114, 17);
            this.customPatchCheckBox.TabIndex = 11;
            this.customPatchCheckBox.Text = "Use Custom Patch";
            this.customPatchCheckBox.UseVisualStyleBackColor = true;
            this.customPatchCheckBox.CheckedChanged += new System.EventHandler(this.CustomPatchCheckBox_CheckedChanged);
            // 
            // customPatchPath
            // 
            this.customPatchPath.Enabled = false;
            this.customPatchPath.Location = new System.Drawing.Point(113, 123);
            this.customPatchPath.Name = "customPatchPath";
            this.customPatchPath.Size = new System.Drawing.Size(206, 20);
            this.customPatchPath.TabIndex = 10;
            // 
            // browsePatchButton
            // 
            this.browsePatchButton.Enabled = false;
            this.browsePatchButton.Location = new System.Drawing.Point(32, 123);
            this.browsePatchButton.Name = "browsePatchButton";
            this.browsePatchButton.Size = new System.Drawing.Size(75, 23);
            this.browsePatchButton.TabIndex = 9;
            this.browsePatchButton.Text = "Browse...";
            this.browsePatchButton.UseVisualStyleBackColor = true;
            this.browsePatchButton.Click += new System.EventHandler(this.BrowsePatchButton_Click);
            // 
            // customLogicPath
            // 
            this.customLogicPath.Enabled = false;
            this.customLogicPath.Location = new System.Drawing.Point(113, 175);
            this.customLogicPath.Name = "customLogicPath";
            this.customLogicPath.Size = new System.Drawing.Size(206, 20);
            this.customLogicPath.TabIndex = 8;
            this.customLogicPath.TextChanged += CustomLogicPath_TextChanged;
            // 
            // browseLogicButton
            // 
            this.browseLogicButton.Enabled = false;
            this.browseLogicButton.Location = new System.Drawing.Point(32, 175);
            this.browseLogicButton.Name = "browseLogicButton";
            this.browseLogicButton.Size = new System.Drawing.Size(75, 23);
            this.browseLogicButton.TabIndex = 7;
            this.browseLogicButton.Text = "Browse...";
            this.browseLogicButton.UseVisualStyleBackColor = true;
            this.browseLogicButton.Click += new System.EventHandler(this.BrowseLogicButton_Click);
            // 
            // customLogicCheckBox
            // 
            this.customLogicCheckBox.AutoSize = true;
            this.customLogicCheckBox.Location = new System.Drawing.Point(5, 152);
            this.customLogicCheckBox.Name = "customLogicCheckBox";
            this.customLogicCheckBox.Size = new System.Drawing.Size(112, 17);
            this.customLogicCheckBox.TabIndex = 5;
            this.customLogicCheckBox.Text = "Use Custom Logic";
            this.customLogicCheckBox.UseVisualStyleBackColor = true;
            this.customLogicCheckBox.CheckedChanged += new System.EventHandler(this.CustomLogicCheckBox_CheckedChanged);
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.generalTab);
            this.mainTabs.Controls.Add(this.optionTabPage);
            this.mainTabs.Controls.Add(this.gimmicksTabPage);
            this.mainTabs.Location = new System.Drawing.Point(9, 27);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(369, 235);
            this.mainTabs.TabIndex = 9;
            // 
            // optionTabPage
            // 
            this.optionTabPage.Controls.Add(this.settingsStringBox);
            this.optionTabPage.Controls.Add(this.label4);
            this.optionTabPage.Controls.Add(this.optionControlLayout);
            this.optionTabPage.Location = new System.Drawing.Point(4, 22);
            this.optionTabPage.Name = "optionTabPage";
            this.optionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.optionTabPage.Size = new System.Drawing.Size(361, 209);
            this.optionTabPage.TabIndex = 2;
            this.optionTabPage.Text = "Settings";
            this.optionTabPage.UseVisualStyleBackColor = true;
            // 
            // optionControlLayout
            // 
            this.optionControlLayout.AutoScroll = true;
            this.optionControlLayout.Location = new System.Drawing.Point(0, 30);
            this.optionControlLayout.Name = "optionControlLayout";
            this.optionControlLayout.Size = new System.Drawing.Size(361, 179);
            this.optionControlLayout.TabIndex = 0;
            // 
            // gimmicksTabPage
            // 
            this.gimmicksTabPage.Controls.Add(this.textBox1);
            this.gimmicksTabPage.Controls.Add(this.label5);
            this.gimmicksTabPage.Controls.Add(this.gimmickControlLayout);
            this.gimmicksTabPage.Location = new System.Drawing.Point(4, 22);
            this.gimmicksTabPage.Name = "gimmicksTabPage";
            this.gimmicksTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.gimmicksTabPage.Size = new System.Drawing.Size(361, 209);
            this.gimmicksTabPage.TabIndex = 1;
            this.gimmicksTabPage.Text = "Gimmicks";
            this.gimmicksTabPage.UseVisualStyleBackColor = true;
            // 
            // gimmickControlLayout
            // 
            this.gimmickControlLayout.AutoScroll = true;
            this.gimmickControlLayout.Location = new System.Drawing.Point(0, 30);
            this.gimmickControlLayout.Name = "gimmickControlLayout";
            this.gimmickControlLayout.Size = new System.Drawing.Size(361, 179);
            this.gimmickControlLayout.TabIndex = 0;
            // 
            // generatedTab
            // 
            this.generatedTab.Controls.Add(this.gimmickHashValue);
            this.generatedTab.Controls.Add(this.settingHashValue);
            this.generatedTab.Controls.Add(this.label3);
            this.generatedTab.Controls.Add(this.label2);
            this.generatedTab.Controls.Add(this.generatedLogicLabel);
            this.generatedTab.Controls.Add(this.label1);
            this.generatedTab.Controls.Add(this.generatedSeedValue);
            this.generatedTab.Controls.Add(this.generatedSeedLabel);
            this.generatedTab.Controls.Add(this.saveSpoilerButton);
            this.generatedTab.Controls.Add(this.savePatchButton);
            this.generatedTab.Controls.Add(this.saveRomButton);
            this.generatedTab.Location = new System.Drawing.Point(4, 22);
            this.generatedTab.Name = "generatedTab";
            this.generatedTab.Padding = new System.Windows.Forms.Padding(3);
            this.generatedTab.Size = new System.Drawing.Size(361, 209);
            this.generatedTab.TabIndex = 1;
            this.generatedTab.Text = "Seed";
            this.generatedTab.UseVisualStyleBackColor = true;
            // 
            // gimmickHashValue
            // 
            this.gimmickHashValue.AutoSize = true;
            this.gimmickHashValue.Location = new System.Drawing.Point(100, 83);
            this.gimmickHashValue.Name = "gimmickHashValue";
            this.gimmickHashValue.Size = new System.Drawing.Size(0, 13);
            this.gimmickHashValue.TabIndex = 10;
            // 
            // settingHashValue
            // 
            this.settingHashValue.AutoSize = true;
            this.settingHashValue.Location = new System.Drawing.Point(93, 59);
            this.settingHashValue.Name = "settingHashValue";
            this.settingHashValue.Size = new System.Drawing.Size(0, 13);
            this.settingHashValue.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Gimmick Hash:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Setting Hash:";
            // 
            // generatedLogicLabel
            // 
            this.generatedLogicLabel.AutoSize = true;
            this.generatedLogicLabel.Location = new System.Drawing.Point(54, 36);
            this.generatedLogicLabel.Name = "generatedLogicLabel";
            this.generatedLogicLabel.Size = new System.Drawing.Size(0, 13);
            this.generatedLogicLabel.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Logic: ";
            // 
            // generatedSeedValue
            // 
            this.generatedSeedValue.AutoSize = true;
            this.generatedSeedValue.Location = new System.Drawing.Point(51, 13);
            this.generatedSeedValue.Name = "generatedSeedValue";
            this.generatedSeedValue.Size = new System.Drawing.Size(0, 13);
            this.generatedSeedValue.TabIndex = 4;
            // 
            // generatedSeedLabel
            // 
            this.generatedSeedLabel.AutoSize = true;
            this.generatedSeedLabel.Location = new System.Drawing.Point(16, 13);
            this.generatedSeedLabel.Name = "generatedSeedLabel";
            this.generatedSeedLabel.Size = new System.Drawing.Size(38, 13);
            this.generatedSeedLabel.TabIndex = 3;
            this.generatedSeedLabel.Text = "Seed: ";
            // 
            // saveSpoilerButton
            // 
            this.saveSpoilerButton.Location = new System.Drawing.Point(181, 180);
            this.saveSpoilerButton.Name = "saveSpoilerButton";
            this.saveSpoilerButton.Size = new System.Drawing.Size(75, 23);
            this.saveSpoilerButton.TabIndex = 2;
            this.saveSpoilerButton.Text = "Save Spoiler";
            this.saveSpoilerButton.UseVisualStyleBackColor = true;
            this.saveSpoilerButton.Click += new System.EventHandler(this.SaveSpoilerButton_Click);
            // 
            // savePatchButton
            // 
            this.savePatchButton.Enabled = false;
            this.savePatchButton.Location = new System.Drawing.Point(100, 180);
            this.savePatchButton.Name = "savePatchButton";
            this.savePatchButton.Size = new System.Drawing.Size(75, 23);
            this.savePatchButton.TabIndex = 1;
            this.savePatchButton.Text = "Save Patch";
            this.savePatchButton.UseVisualStyleBackColor = true;
            this.savePatchButton.Click += new System.EventHandler(this.SavePatchButton_Click);
            // 
            // saveRomButton
            // 
            this.saveRomButton.Location = new System.Drawing.Point(19, 180);
            this.saveRomButton.Name = "saveRomButton";
            this.saveRomButton.Size = new System.Drawing.Size(75, 23);
            this.saveRomButton.TabIndex = 0;
            this.saveRomButton.Text = "Save ROM";
            this.saveRomButton.UseVisualStyleBackColor = true;
            this.saveRomButton.Click += new System.EventHandler(this.SaveRomButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Settings String:";
            // 
            // settingsStringBox
            // 
            this.settingsStringBox.Location = new System.Drawing.Point(91, 6);
            this.settingsStringBox.Name = "settingsStringBox";
            this.settingsStringBox.Size = new System.Drawing.Size(100, 20);
            this.settingsStringBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Gimmicks String:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(98, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 316);
            this.Controls.Add(this.mainTabs);
            this.Controls.Add(this.randomize);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Minish Cap Randomizer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.mainTabs.ResumeLayout(false);
            this.optionTabPage.ResumeLayout(false);
            this.optionTabPage.PerformLayout();
            this.gimmicksTabPage.ResumeLayout(false);
            this.gimmicksTabPage.PerformLayout();
            this.generatedTab.ResumeLayout(false);
            this.generatedTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button randomize;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TextBox customLogicPath;
        private System.Windows.Forms.Button browseLogicButton;
        private System.Windows.Forms.CheckBox customLogicCheckBox;
        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.CheckBox customPatchCheckBox;
        private System.Windows.Forms.TextBox customPatchPath;
        private System.Windows.Forms.Button browsePatchButton;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.TextBox seedField;
        private System.Windows.Forms.TabPage generatedTab;
        private System.Windows.Forms.Button saveSpoilerButton;
        private System.Windows.Forms.Button savePatchButton;
        private System.Windows.Forms.Button saveRomButton;
        private System.Windows.Forms.Label generatedSeedLabel;
        private System.Windows.Forms.Label generatedSeedValue;
        private System.Windows.Forms.TabPage gimmicksTabPage;
        private System.Windows.Forms.TabPage optionTabPage;
        private System.Windows.Forms.FlowLayoutPanel optionControlLayout;
        private System.Windows.Forms.FlowLayoutPanel gimmickControlLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label generatedLogicLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label gimmickHashValue;
        private System.Windows.Forms.Label settingHashValue;
        private System.Windows.Forms.TextBox settingsStringBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}

