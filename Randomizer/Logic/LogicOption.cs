using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinishRandomizer.Randomizer.Logic
{
    public class LogicOption : LogicDefine
    {
        public string NiceName;
        public bool Active;

        public LogicOption(string name, string niceName, bool active) : base(name)
        {
            NiceName = niceName;
            Active = active;
        }

        public virtual Control GetControl()
        {
            throw new NotImplementedException();
        }
    }

    public class LogicFlag : LogicOption
    {
        public LogicFlag(string name, string niceName, bool active) : base(name, niceName, active) { }

        public override Control GetControl()
        {
            CheckBox flagCheckBox = new CheckBox
            {
                Text = NiceName,
                AutoSize = true,
                Checked = Active
            };

            flagCheckBox.CheckedChanged += (object sender, EventArgs e) => { Active = flagCheckBox.Checked; };

            return flagCheckBox;
        }
    }

    public class LogicColorPicker : LogicOption
    {
        Color DefinedColor;

        public LogicColorPicker(string name, string niceName, Color startingColor) : base(name, niceName, false)
        {
            DefinedColor = startingColor;
        }

        public override Control GetControl()
        {
            Button colorSwapButton = new Button
            {
                Text = NiceName
            };

            colorSwapButton.Click += OpenColors;

            return colorSwapButton;
        }

        private void OpenColors(object sender, EventArgs e)
        {
            ColorDialog colorPicker = new ColorDialog
            {

            };
        }
    }
}
