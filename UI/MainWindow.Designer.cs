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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomize = new System.Windows.Forms.Button();
            this.logicTab = new System.Windows.Forms.TabPage();
            this.customLogicCheckBox = new System.Windows.Forms.CheckBox();
            this.customLocationPath = new System.Windows.Forms.TextBox();
            this.browseLogicButton = new System.Windows.Forms.Button();
            this.browseLocationButton = new System.Windows.Forms.Button();
            this.customLogicPath = new System.Windows.Forms.TextBox();
            this.customLocationCheckBox = new System.Windows.Forms.CheckBox();
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.locationsTab = new System.Windows.Forms.TabPage();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.logicTab.SuspendLayout();
            this.mainTabs.SuspendLayout();
            this.locationsTab.SuspendLayout();
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
            // logicTab
            // 
            this.logicTab.Controls.Add(this.customLogicPath);
            this.logicTab.Controls.Add(this.browseLogicButton);
            this.logicTab.Controls.Add(this.customLogicCheckBox);
            this.logicTab.Location = new System.Drawing.Point(4, 22);
            this.logicTab.Name = "logicTab";
            this.logicTab.Padding = new System.Windows.Forms.Padding(3);
            this.logicTab.Size = new System.Drawing.Size(361, 209);
            this.logicTab.TabIndex = 0;
            this.logicTab.Text = "Logic";
            this.logicTab.UseVisualStyleBackColor = true;
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
            // customLocationPath
            // 
            this.customLocationPath.Enabled = false;
            this.customLocationPath.Location = new System.Drawing.Point(113, 175);
            this.customLocationPath.Name = "customLocationPath";
            this.customLocationPath.Size = new System.Drawing.Size(206, 20);
            this.customLocationPath.TabIndex = 3;
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
            // browseLocationButton
            // 
            this.browseLocationButton.Enabled = false;
            this.browseLocationButton.Location = new System.Drawing.Point(32, 175);
            this.browseLocationButton.Name = "browseLocationButton";
            this.browseLocationButton.Size = new System.Drawing.Size(75, 23);
            this.browseLocationButton.TabIndex = 4;
            this.browseLocationButton.Text = "Browse...";
            this.browseLocationButton.UseVisualStyleBackColor = true;
            this.browseLocationButton.Click += new System.EventHandler(this.BrowseLocationButton_Click);
            // 
            // customLogicPath
            // 
            this.customLogicPath.Enabled = false;
            this.customLogicPath.Location = new System.Drawing.Point(113, 175);
            this.customLogicPath.Name = "customLogicPath";
            this.customLogicPath.Size = new System.Drawing.Size(206, 20);
            this.customLogicPath.TabIndex = 8;
            // 
            // customLocationCheckBox
            // 
            this.customLocationCheckBox.AutoSize = true;
            this.customLocationCheckBox.Location = new System.Drawing.Point(5, 152);
            this.customLocationCheckBox.Name = "customLocationCheckBox";
            this.customLocationCheckBox.Size = new System.Drawing.Size(146, 17);
            this.customLocationCheckBox.TabIndex = 6;
            this.customLocationCheckBox.Text = "Use Custom Location List";
            this.customLocationCheckBox.UseVisualStyleBackColor = true;
            this.customLocationCheckBox.CheckedChanged += new System.EventHandler(this.CustomLocationCheckBox_CheckedChanged);
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.logicTab);
            this.mainTabs.Controls.Add(this.locationsTab);
            this.mainTabs.Location = new System.Drawing.Point(9, 27);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(369, 235);
            this.mainTabs.TabIndex = 9;
            // 
            // locationsTab
            // 
            this.locationsTab.Controls.Add(this.customLocationCheckBox);
            this.locationsTab.Controls.Add(this.customLocationPath);
            this.locationsTab.Controls.Add(this.browseLocationButton);
            this.locationsTab.Location = new System.Drawing.Point(4, 22);
            this.locationsTab.Name = "locationsTab";
            this.locationsTab.Padding = new System.Windows.Forms.Padding(3);
            this.locationsTab.Size = new System.Drawing.Size(361, 209);
            this.locationsTab.TabIndex = 1;
            this.locationsTab.Text = "Locations";
            this.locationsTab.UseVisualStyleBackColor = true;
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
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Minish Cap Randomizer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.logicTab.ResumeLayout(false);
            this.logicTab.PerformLayout();
            this.mainTabs.ResumeLayout(false);
            this.locationsTab.ResumeLayout(false);
            this.locationsTab.PerformLayout();
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
        private System.Windows.Forms.TabPage logicTab;
        private System.Windows.Forms.CheckBox customLocationCheckBox;
        private System.Windows.Forms.TextBox customLogicPath;
        private System.Windows.Forms.TextBox customLocationPath;
        private System.Windows.Forms.Button browseLocationButton;
        private System.Windows.Forms.Button browseLogicButton;
        private System.Windows.Forms.CheckBox customLogicCheckBox;
        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.TabPage locationsTab;
    }
}

