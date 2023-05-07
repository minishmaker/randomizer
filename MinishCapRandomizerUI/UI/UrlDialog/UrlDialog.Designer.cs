namespace MinishCapRandomizerUI.UI.UrlDialog
{
    partial class UrlDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UrlDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.Link = new System.Windows.Forms.LinkLabel();
			this.MessageIcon = new System.Windows.Forms.PictureBox();
			this.Message = new System.Windows.Forms.Label();
			this.Okay = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MessageIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel1.Controls.Add(this.Link);
			this.panel1.Controls.Add(this.MessageIcon);
			this.panel1.Controls.Add(this.Message);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(384, 90);
			this.panel1.TabIndex = 0;
			// 
			// Link
			// 
			this.Link.AutoSize = true;
			this.Link.Location = new System.Drawing.Point(66, 60);
			this.Link.Name = "Link";
			this.Link.Size = new System.Drawing.Size(121, 15);
			this.Link.TabIndex = 3;
			this.Link.TabStop = true;
			this.Link.Text = "Get latest release here";
			this.Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
			// 
			// MessageIcon
			// 
			this.MessageIcon.BackColor = System.Drawing.Color.Transparent;
			this.MessageIcon.Location = new System.Drawing.Point(20, 20);
			this.MessageIcon.Name = "MessageIcon";
			this.MessageIcon.Size = new System.Drawing.Size(40, 40);
			this.MessageIcon.TabIndex = 2;
			this.MessageIcon.TabStop = false;
			// 
			// Message
			// 
			this.Message.AutoSize = true;
			this.Message.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.Message.Location = new System.Drawing.Point(66, 20);
			this.Message.MaximumSize = new System.Drawing.Size(301, 0);
			this.Message.Name = "Message";
			this.Message.Size = new System.Drawing.Size(292, 30);
			this.Message.TabIndex = 0;
			this.Message.Text = "A new version of the Randomizer is available! You can download the new version by" +
    " clicking the link below";
			// 
			// Okay
			// 
			this.Okay.BackColor = System.Drawing.SystemColors.ControlLight;
			this.Okay.Location = new System.Drawing.Point(276, 100);
			this.Okay.Name = "Okay";
			this.Okay.Size = new System.Drawing.Size(96, 23);
			this.Okay.TabIndex = 1;
			this.Okay.Text = "OK";
			this.Okay.UseVisualStyleBackColor = false;
			this.Okay.Click += new System.EventHandler(this.Okay_Click);
			// 
			// UrlDialog
			// 
			this.AcceptButton = this.Okay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(384, 131);
			this.Controls.Add(this.Okay);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UrlDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Version Available!";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MessageIcon)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Button Okay;
        private PictureBox MessageIcon;
        private LinkLabel Link;
        private Label Message;
    }
}
