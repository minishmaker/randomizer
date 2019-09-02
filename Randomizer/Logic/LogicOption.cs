using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                Checked = Active
            };

            flagCheckBox.CheckedChanged += (object sender, EventArgs e) => { Active = flagCheckBox.Checked; };

            return flagCheckBox;
        }
    }
}
