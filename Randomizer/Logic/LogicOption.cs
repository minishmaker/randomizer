using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer.Logic
{
    public class LogicOption
    {
        public string Name;
        public string NiceName;
        public bool Active;

        public LogicOption(string name, string niceName, bool active)
        {
            Name = name;
            NiceName = niceName;
            Active = active;
        }

        public virtual Control GetControl()
        {
            throw new NotImplementedException();
        }

        public virtual List<LogicDefine> GetLogicDefines()
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

        public override List<LogicDefine> GetLogicDefines()
        {
            List<LogicDefine> defineList = new List<LogicDefine>(1);

            // Only define the new thing if the flag is ticked
            if (Active)
            {
                defineList.Add(new LogicDefine(Name));
            }

            return defineList;
        }
    }

    public class LogicColorPicker : LogicOption
    {
        private Color BaseColor;
        private Color DefinedColor;
        private List<Color> InitialColors;

        public LogicColorPicker(string name, string niceName, Color startingColor) : base(name, niceName, true)
        {
            BaseColor = startingColor;
            DefinedColor = startingColor;
            InitialColors = new List<Color>(1);

            InitialColors.Add(startingColor);
        }

        // Can specify other colors to be defined relative to the first color
        public LogicColorPicker(string name, string niceName, List<Color> colors) : base(name, niceName, true)
        {
            BaseColor = colors[0];
            DefinedColor = colors[0];
            InitialColors = colors;
        }

        public override Control GetControl()
        {
            Button colorSwapButton = new Button
            {
                Text = NiceName,
                AutoSize = true
            };

            colorSwapButton.Click += OpenColors;

            return colorSwapButton;
        }

        private void OpenColors(object sender, EventArgs e)
        {
            ColorDialog colorPicker = new ColorDialog
            {
                Color = DefinedColor,
                FullOpen = true
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                Active = true;
                DefinedColor = new ColorUtil.GBAColor(colorPicker.Color).ToColor();
            }
        }

        public override List<LogicDefine> GetLogicDefines()
        {
            List<LogicDefine> defineList = new List<LogicDefine>(3);
            
            // Only true if a color has been selected
            if (Active)
            {
                defineList.Add(new LogicDefine(Name));

                for (int i = 0; i < InitialColors.Count; i++)
                {
                    // Adjust colors so they work with the selected color, then define
                    ColorUtil.GBAColor newColor = new ColorUtil.GBAColor(ColorUtil.AdjustHue(InitialColors[i], BaseColor, DefinedColor));
                    defineList.Add(new LogicDefine(Name + "_" + i, StringUtil.AsStringHex4(newColor.CombinedValue)));
                }
            }

            return defineList;
        }
    }
}
