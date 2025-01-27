using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;


namespace ScreenSaverBlank
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
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ScreenSaverBlank");

                // TODO
                //
                //key.SetValue("", "");
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured Saving settings to Registry.",
                    "ScreenSaverBlank",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void LoadSettings()
        {
            // Get the value stored in the Registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverBlank");
            if (key != null)
            {
                // TODO
                //
                //if (key.GetValue("") != null)
            }
            else
            {
                // TODO
                //
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
    }
}
