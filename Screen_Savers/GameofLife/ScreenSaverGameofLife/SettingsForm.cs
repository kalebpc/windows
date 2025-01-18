using System;
//using System.Drawing;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScreenSaverGameofLife
{
    public partial class SettingsForm : Form
    {
        private string color;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings()
        {
            // Create or get existing Registry subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ScreenSaverGameofLife");

            VerifyColor();
            key.SetValue("color", color);
        }

        private void LoadSettings()
        {
            // Get the value stored in the Registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverGameofLife");
            if (key == null)
            {
                // nothing.
            }
            else
            {
                PreSelectColor((string)key.GetValue("color"));
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreSelectColor(string selectedColor)
        {
            // Select current color choice radiobutton
            if (selectedColor == "red")
            {
                RedRadioButton.Checked = true;
            }
            else if (selectedColor == "green")
            {
                GreenRadioButton.Checked = true;
            }
            else
            {
                BlueRadioButton.Checked = true;
            }
        }

        private void VerifyColor()
        {
            if (color == "red" || color == "green" || color == "blue")
            {
                // nothing.
            }
            else
            {
                color = "green";
            }
        }

        private void RedRadioButton_CheckedChanged_1(object sender, EventArgs e)
        {
            color = "red";
        }

        private void GreenRadioButton_CheckedChanged_1(object sender, EventArgs e)
        {
            color = "green";
        }

        private void BlueRadioButton_CheckedChanged_1(object sender, EventArgs e)
        {
            color = "blue";
        }
    }
}
