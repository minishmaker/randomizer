using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinishCapRandomizerUI.UI.InputDialog
{
	public partial class InputDialog : Form
	{
		private Action<string> _callback;

		public InputDialog()
		{
			InitializeComponent();
			MessageIcon.Image = SystemIcons.Question.ToBitmap();
		}

		public void Initialize(string title, string message, Action<string> callback)
		{
			Text = title;
			Message.Text = message;
			_callback = callback;
		}

		private void Okay_Click(object sender, EventArgs e)
		{
			_callback.Invoke(UserInput.Text);
			Close();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
