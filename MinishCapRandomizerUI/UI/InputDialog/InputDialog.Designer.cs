namespace MinishCapRandomizerUI.UI.InputDialog
{
	partial class InputDialog
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.MessageIcon = new System.Windows.Forms.PictureBox();
			this.UserInput = new System.Windows.Forms.TextBox();
			this.Message = new System.Windows.Forms.Label();
			this.Okay = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MessageIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel1.Controls.Add(this.MessageIcon);
			this.panel1.Controls.Add(this.UserInput);
			this.panel1.Controls.Add(this.Message);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(384, 125);
			this.panel1.TabIndex = 0;
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
			// UserInput
			// 
			this.UserInput.Location = new System.Drawing.Point(66, 90);
			this.UserInput.Name = "UserInput";
			this.UserInput.Size = new System.Drawing.Size(306, 23);
			this.UserInput.TabIndex = 1;
			// 
			// Message
			// 
			this.Message.AutoSize = true;
			this.Message.Location = new System.Drawing.Point(66, 9);
			this.Message.MaximumSize = new System.Drawing.Size(301, 0);
			this.Message.Name = "Message";
			this.Message.Size = new System.Drawing.Size(110, 15);
			this.Message.TabIndex = 0;
			this.Message.Text = "Message Goes Here";
			// 
			// Okay
			// 
			this.Okay.BackColor = System.Drawing.SystemColors.ControlLight;
			this.Okay.Location = new System.Drawing.Point(276, 135);
			this.Okay.Name = "Okay";
			this.Okay.Size = new System.Drawing.Size(96, 23);
			this.Okay.TabIndex = 1;
			this.Okay.Text = "OK";
			this.Okay.UseVisualStyleBackColor = false;
			this.Okay.Click += new System.EventHandler(this.Okay_Click);
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(170, 135);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(96, 23);
			this.Cancel.TabIndex = 2;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// InputDialog
			// 
			this.AcceptButton = this.Okay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(384, 166);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Okay);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "InputDialog";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MessageIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Panel panel1;
		private Label Message;
		private Button Okay;
		private Button Cancel;
		private TextBox UserInput;
		private PictureBox MessageIcon;
	}
}