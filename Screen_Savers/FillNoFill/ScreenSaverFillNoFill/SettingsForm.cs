using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;


namespace ScreenSaverFillNoFill
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings()
        {
            try
            {
                // Create or get existing Registry subkey
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverFillNoFill");
                key.SetValue("backgroundColor", BackgroundColorComboBox.Text);
                key.SetValue("shapeColor", ShapeColorComboBox.Text);
                key.SetValue("gridColor", GridColorComboBox.Text);
                key.SetValue("showGrid", ShowGridCheckBox.Checked.ToString());
                key.SetValue("size", (int)(SizeTrackBar.Value - 4) * 5);
                key.SetValue("speed", GetSpeed());
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured Saving settings to Registry.",
                    "ScreenSaverFillNoFill",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void LoadSettings()
        {
            try
            {
                PopulateColorCombos();
                // Get the value stored in the Registry
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverFillNoFill");
                if (key != null)
                {
                    if (key.GetValue("backgroundColor") != null)
                    {
                        string s = (string)key.GetValue("backgroundColor");
                        BackgroundColorComboBox.Text = s;
                        BackgroundColorBox.BackColor = Color.FromName(s);
                    }
                    if (key.GetValue("shapeColor") != null)
                    {
                        string s = (string)key.GetValue("shapeColor");
                        ShapeColorComboBox.Text = s;
                        ShapeColorBox.BackColor = Color.FromName(s);
                    }
                    if (key.GetValue("gridColor") != null)
                    {
                        string s = (string)key.GetValue("gridColor");
                        GridColorComboBox.Text = s;
                        GridColorBox.BackColor = Color.FromName(s);
                    }
                    if (key.GetValue("showGrid") != null)
                    {
                        if ((string)key.GetValue("showGrid") == "True")
                            ShowGridCheckBox.Checked = true;
                    }
                    if (key.GetValue("size") != null)
                    {
                        SizeTrackBar.Value = ((int)key.GetValue("size") / 5) + 4;
                    }
                    if (key.GetValue("speed") != null)
                    {
                        SpeedTrackBar.Value = 3500 / (5 * (int)key.GetValue("speed"));
                    }
                }
                else
                {
                    BackgroundColorComboBox.SelectedItem = BackgroundColorComboBox.Items[34];
                    ShapeColorComboBox.SelectedItem = ShapeColorComboBox.Items[140];
                    GridColorComboBox.SelectedItem = GridColorComboBox.Items[140];
                    SizeTrackBar.Value = 14;
                    SpeedTrackBar.Value = 21;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "ScreenSaverFillNoFill",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
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
                    ShapeColorComboBox.Items.Add(color);
                    GridColorComboBox.Items.Add(color);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ScreenSaverFillNoFill", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void BackgroundColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BackgroundColorBox.BackColor = Color.FromName(BackgroundColorComboBox.Text);
        }

        private void ShapeColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShapeColorBox.BackColor = Color.FromName(ShapeColorComboBox.Text);
        }

        private void GridColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridColorBox.BackColor = Color.FromName(GridColorComboBox.Text);
        }

        private int GetSpeed()
        {
            return 3500 / (SpeedTrackBar.Value * 5);
        }
    }
}
