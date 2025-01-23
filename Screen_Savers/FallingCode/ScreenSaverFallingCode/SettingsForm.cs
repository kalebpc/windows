using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;


namespace ScreenSaverFallingCode
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
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ScreenSaverFallingCode");
                key.SetValue("backColor", (string)GetBackgroundColor());
                key.SetValue("raindropColor", GetRaindropColor());
                key.SetValue("raintrailColor", GetRaintrailColor());
                key.SetValue("fontType", GetFontType());
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured Saving settings to Registry.",
                    "ScreenSaverFallingCode",
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
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverFallingCode");
                if (key != null)
                {
                    PreSelectBackColor((string)key.GetValue("backColor"));
                    PreSelectRaindropColor((string)key.GetValue("raindropColor"));
                    PreSelectRaintrailColor((string)key.GetValue("raintrailColor"));
                    PreSelectFontType((string)key.GetValue("fontType"));
                }
                else
                {
                    PreSelectBackColor("Black");
                    PreSelectRaindropColor("White");
                    PreSelectRaintrailColor("Green");
                    PreSelectFontType("Segoe UI");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "ScreenSaverFallingCode",
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


        private void PreSelectBackColor(string color)
        {
            BackgroundComboBox.Text = color;
            PreviewPictureBox.BackColor = Color.FromName(color);
        }

        private void PreSelectRaindropColor(string color)
        {
            RaindropComboBox.Text = color;
        }

        private void PreSelectRaintrailColor(string color)
        {
            RainTrailComboBox.Text = color;
        }

        private void PreSelectFontType(string fontName)
        {
            FontLabel.Text = fontName;
        }

        private string GetBackgroundColor()
        {
            return PreviewPictureBox.BackColor.Name;
        }

        private string GetRaindropColor()
        {
            return RaindropComboBox.Text;
        }

        private string GetRaintrailColor()
        {
            return RainTrailComboBox.Text;
        }

        private string GetFontType()
        {
            return FontLabel.Text;
        }

        private void PopulateColorCombos()
        {
            try
            {
                foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
                {
                    BackgroundComboBox.Items.Add(color);
                    RaindropComboBox.Items.Add(color);
                    RainTrailComboBox.Items.Add(color);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ScreenSaverFallingCode", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void OnPreviewPaint(object sender, PaintEventArgs e)
        {
            Font font;
            Color backgroundColor;
            Color raindropColor;
            Color raintrailColor;
            int x = 45;
            int y = 65;
            if (FontLabel.Text != "")
                font = new Font(FontLabel.Text, 20);
            else
                font = new Font("Lucida Console", 20);
            if (BackgroundComboBox.Text != "")
                backgroundColor = Color.FromName(BackgroundComboBox.Text);
            else
                backgroundColor = Color.Black;
            if (RaindropComboBox.Text != "")
                raindropColor = Color.FromName(RaindropComboBox.Text);
            else
                raindropColor = Color.White;
            if (RainTrailComboBox.Text != "")
                raintrailColor = Color.FromName(RainTrailComboBox.Text);
            else
                raintrailColor = Color.Green;
            PreviewPictureBox.BackColor = backgroundColor;
            Graphics g = e.Graphics;
            int decr = y;
            for (int i = 0; i < 4; i++)
            {
                g.DrawString("T", font, new SolidBrush(raintrailColor), x, decr);
                decr -= 20;
            }
            g.DrawString("R", font, new SolidBrush(raindropColor), x, y + 20);
        }

        private void BackgroundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewPictureBox.BackColor = Color.FromName(BackgroundComboBox.Text);
        }

        private void RaindropComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewPictureBox.Invalidate();
        }

        private void RainTrailComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewPictureBox.Invalidate();
        }

        private void FontBrowseButton_Click(object sender, EventArgs e)
        {
            Font font;
            string text;
            if (FontLabel.Text != "")
                text = FontLabel.Text;
            else
                text = "Segoe UI";
            font = new Font(text, 15);
            FontDialog fd = new FontDialog()
            {
                AllowScriptChange = false,
                Font = font,
                MaxSize = 20,
                MinSize = 20,
                ShowEffects = false
            };
            if (fd.ShowDialog() == DialogResult.OK)
                FontLabel.Text = fd.Font.Name;
            PreviewPictureBox.Invalidate();
        }
    }
}
