using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;


namespace ScreenSaverSpace
{
    public partial class ScreenSaverSettings : Form
    {
        public ScreenSaverSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverSpace");
                if (key != null)
                {
                    // TODO
                    key.SetValue("backgroundColor", BackgroundColorBox.BackColor.Name);
                    key.SetValue("starColor", StarColorBox.BackColor.Name);
                    key.SetValue("starSize", (int)StarSizeNumericUpDown.Value);
                    key.SetValue("starSpeed", StarSpeedNumericUpDown.Value.ToString());
                    key.SetValue("starCount", (int)StarCountNumericUpDown.Value);
                    key.SetValue("followMouse", MouseMoveCheckBox.Checked.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured Saving settings.",
                    "ScreenSaverSpace",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void LoadSettings()
        {
            PopulateColorCombos();
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverSpace");
            if (key != null)
            {
                if (key.GetValue("backgroundColor") != null)
                {
                    string s = (string)key.GetValue("backgroundColor");
                    BackgroundColorComboBox.Text = s;
                    BackgroundColorBox.BackColor = Color.FromName(s);
                }
                if (key.GetValue("starColor") != null)
                {
                    string s = (string)key.GetValue("starColor");
                    StarColorComboBox.Text = s;
                    StarColorBox.BackColor = Color.FromName(s);
                }
                if (key.GetValue("starSize") != null)
                    StarSizeNumericUpDown.Value = (int)key.GetValue("starSize");
                if (key.GetValue("starSpeed") != null)
                    StarSpeedNumericUpDown.Value = (decimal)Convert.ToDouble(key.GetValue("starSpeed"));
                if (key.GetValue("starCount") != null)
                    StarCountNumericUpDown.Value = (decimal)Convert.ToDouble(key.GetValue("starCount"));
                if (key.GetValue("followMouse") != null)
                {
                    string s = (string)key.GetValue("followMouse");
                    if (s == "True")
                        MouseMoveCheckBox.Checked = true;
                    else
                        MouseMoveCheckBox.Checked = false;
                }
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

        private void PopulateColorCombos()
        {
            try
            {
                foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
                {
                    BackgroundColorComboBox.Items.Add(color);
                    StarColorComboBox.Items.Add(color);
                }
                BackgroundColorComboBox.SelectedItem = BackgroundColorComboBox.Items[34];
                StarColorComboBox.SelectedItem = StarColorComboBox.Items[79];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ScreenSaverSpace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BackgroundColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BackgroundColorBox.BackColor = Color.FromName(BackgroundColorComboBox.Text);
        }

        private void StarColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StarColorBox.BackColor = Color.FromName(StarColorComboBox.Text);
        }
    }
}
