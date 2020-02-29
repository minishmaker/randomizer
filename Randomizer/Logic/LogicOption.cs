using MinishRandomizer.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MinishRandomizer.Randomizer.Logic
{
    public enum LogicOptionType
    {
        Untyped,
        Setting,
        Gimmick
    }

    public class LogicOption
    {
        public string Name;
        public string NiceName;
        public bool Active;
        public LogicOptionType Type;
        public Action ChangeHash;
        public int BitCount;
        protected bool SelfChange = false;

        /// <summary>
        /// Convert a list of options to a byte[] representing their states
        /// </summary>
        /// <param name="options">The options to convert</param>
        /// <returns>The bytes representing the state of the options</returns>
        public static byte[] ToByteArray(List<LogicOption> options)
        {
            int totalBitCount = 0;
            foreach (LogicOption option in options)
            {
                totalBitCount += option.BitCount;
            }

            BitArray bitArray = new BitArray(totalBitCount);
            int arrayOffset = 0;

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AddState(ref bitArray, ref arrayOffset);
            }

            return bitArray.ToByteArray();
        }

        /// <summary>
        /// Apply the settings from a byte[] to a List<LogicOption>
        /// </summary>
        /// <param name="options">The options to change</param>
        /// <param name="settings">The state to put the options in</param>
        public static void ApplySettings(List<LogicOption> options, byte[] settings)
        {
            BitArray bitArray = new BitArray(settings);
            int arrayOffset = 0;

            for (int i = 0; i < options.Count; i++)
            {
                options[i].SetState(ref bitArray, ref arrayOffset);
            }
        }

        public LogicOption(string name, string niceName, bool active, LogicOptionType type = LogicOptionType.Setting)
        {
            Name = name;
            NiceName = niceName;
            Active = active;
            Type = type;
        }

        public virtual Control GetControl()
        {
            throw new NotImplementedException();
        }

        public virtual List<LogicDefine> GetLogicDefines()
        {
            throw new NotImplementedException();
        }

        public virtual byte GetHashByte()
        {
            throw new NotImplementedException();
        }

        public virtual void AddState(ref BitArray bitArray, ref int offset)
        {
            throw new NotImplementedException();
        }

        public virtual void SetState(ref BitArray bitArray, ref int offset)
        {
            throw new NotImplementedException();
        }
    }

    public class LogicFlag : LogicOption
    {
        public LogicFlag(string name, string niceName, bool active, LogicOptionType type) : base(name, niceName, active, type) { BitCount = 1; }

        public override Control GetControl()
        {
            CheckBox flagCheckBox = new CheckBox
            {
                Text = NiceName,
                AutoSize = true,
                Checked = Active
            };

            flagCheckBox.CheckedChanged += (object sender, EventArgs e) => {
                if (!SelfChange)
                {
                    Active = flagCheckBox.Checked;
                    ChangeHash();
                }
            };

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

        public override byte GetHashByte()
        {
            return Active ? (byte)01 : (byte)00;
        }

        public override void AddState(ref BitArray bitArray, ref int offset)
        {
            // Add flag to bitArray, got to next bit
            bitArray[offset] = Active;
            offset++;
        }

        public override void SetState(ref BitArray bitArray, ref int offset)
        {
            // Take flag from bitArray, go to next bit
            Active = bitArray[offset];
            offset++;

            // Set visual component
            SelfChange = false;

        }
    }

    public class LogicColorPicker : LogicOption
    {
        private Color BaseColor;
        private Color DefinedColor;
        private List<Color> InitialColors;

        public LogicColorPicker(string name, string niceName, LogicOptionType type, Color startingColor) : base(name, niceName, true, type)
        {
            BaseColor = startingColor;
            DefinedColor = startingColor;
            InitialColors = new List<Color>(1)
            {
                startingColor
            };

            BitCount = 24;
        }

        // Can specify other colors to be defined relative to the first color
        public LogicColorPicker(string name, string niceName, LogicOptionType type, List<Color> colors) : base(name, niceName, true, type)
        {
            BaseColor = colors[0];
            DefinedColor = colors[0];
            InitialColors = colors;

            BitCount = 24;
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

                ChangeHash();
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

        public override byte GetHashByte()
        {
            // Maybe not a great way to represent, leaves some info out and is likely to cause easy collisions
            return Active ? (byte)(DefinedColor.R ^ DefinedColor.G ^ DefinedColor.B) : (byte)00;
        }

        public override void AddState(ref BitArray bitArray, ref int offset)
        {
            // Use a loop to add the three bytes in cause convenience
            for (int i = 0; i < 8; i++)
            {
                bitArray[offset + i] = ((DefinedColor.R >> i) & 1) > 0;
                bitArray[offset + i + 8] = ((DefinedColor.G >> i) & 1) > 0;
                bitArray[offset + i + 16] = ((DefinedColor.R >> i) & 1) > 0;
            }

            // Increase the offset by 24
            offset += 24;
        }

        public override void SetState(ref BitArray bitArray, ref int offset)
        {
            // Initiate new color values
            byte newR = 0;
            byte newG = 0;
            byte newB = 0;

            // Use a loop to fill the bytes
            for (int i = 0; i < 8; i++)
            {
                if (bitArray[offset + i])
                {
                    newR |= (byte)(1 << i);
                }

                if (bitArray[offset + i + 8])
                {
                    newG |= (byte)(1 << i);
                }

                if (bitArray[offset + i + 16])
                {
                    newB |= (byte)(1 << i);
                }
            }

            // Actually set the new color
            DefinedColor = Color.FromArgb(newR, newG, newB);

            // Increase the offset by 24
            offset += 24;
        }
    }

    public class LogicDropdown : LogicOption
    {
        Dictionary<string, string> Selections;
        string Selection;
        int SelectedNumber;

        public LogicDropdown(string name, LogicOptionType type, Dictionary<string, string> selections) : base(name, name, true, type)
        {
            if (selections.Count > 0xFF)
            {
                throw new ArgumentException("LogicDropdowns are limited to 255 selections!");
            }

            Selections = selections;
            Selection = selections.Keys.First();

            BitCount = 8;
        }

        public override Control GetControl()
        {
            ComboBox comboBox = new ComboBox
            {
                SelectedText = Selection,
                DataSource = Selections.Keys.ToList()
            };

            comboBox.SelectedValueChanged += (object sender, EventArgs e) => { Selection = (string)comboBox.SelectedValue; SelectedNumber = comboBox.SelectedIndex; ChangeHash(); };

            return comboBox;
        }

        public override List<LogicDefine> GetLogicDefines()
        {
            List<LogicDefine> defineList = new List<LogicDefine>(3);

            // Only true if a color has been selected
            if (Active)
            {
                if (Selections.TryGetValue(Selection, out string content))
                {
                    defineList.Add(new LogicDefine(Name, content));
                }
            }

            return defineList;
        }

        public override byte GetHashByte()
        {
            return Active ? (byte)(SelectedNumber & 0xFF) : (byte)0;
        }

        public override void AddState(ref BitArray bitArray, ref int offset)
        {
            if (Active)
            {
                // Use a loop cause I don't want to unroll it
                for (int i = 0; i < BitCount; i++)
                {
                    bitArray[offset + i] = ((SelectedNumber >> i) & 1) > 0;
                }

                offset += 8;
            }
        }

        public override void SetState(ref BitArray bitArray, ref int offset)
        {
            if (Active)
            {
                // Temporarily set selected number to 0
                SelectedNumber = 0;

                // Use a loop to fill the byte
                for (int i = 0; i < 8; i++)
                {
                    if (bitArray[offset + i])
                    {
                        SelectedNumber |= (byte)(1 << i);
                    }
                }

                // Set the visual part as well


                // Increase the offset by 24
                offset += 8;
            }
        }
    }
}
