using System;
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

namespace ScreenSaver
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
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Demo_ScreenSaver");

            key.SetValue("text", enterTextTextbox.Text);
            VerifyColor();
            key.SetValue("color", color);
            key.SetValue("changeTextRate", (int)percentNumericUpDown.Value);
        }

        private void LoadSettings()
        {
            // Get the value stored in the Registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");
            if (key == null)
            {
                enterTextTextbox.Text = "C# Screen Saver";
            }
            else
            {
                enterTextTextbox.Text = (string)key.GetValue("text");
                percentNumericUpDown.Value = (int)key.GetValue("changeTextRate");
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

        private void redRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            color = "red";
        }

        private void greenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            color = "green";
        }

        private void blueRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            color = "blue";
        }

        private void PreSelectColor(string selectedColor)
        {
            // Select current color choice radiobutton
            if (selectedColor == "red")
            {
                redRadioButton.Checked = true;
            }
            else if (selectedColor == "green")
            {
                greenRadioButton.Checked = true;
            }
            else
            {
                blueRadioButton.Checked = true;
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
    }
}
