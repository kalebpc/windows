using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;


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
            PopulateColorCombos();
            // Get the value stored in the Registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverFallingCode");
            if (key != null)
            {
                if (key.GetValue("backColor") != null)
                    PreSelectBackColor((string)key.GetValue("backColor"));
                if (key.GetValue("raindropColor") != null)
                    PreSelectRaindropColor((string)key.GetValue("raindropColor"));
                if (key.GetValue("fontType") != null)
                    PreSelectFontType((string)key.GetValue("fontType"));
            }
            else
            {
                PreSelectBackColor("Black");
                PreSelectRaindropColor("White");
                PreSelectFontType("Segoe UI");
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
            int x = 45;
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
            PreviewPictureBox.BackColor = backgroundColor;
            Graphics g = e.Graphics;
            Random rand = new Random();
            List<string> characters = new List<string>
            {
                "\u30A0","\u30A1","\u30A2","\u30A3","\u30A4","\u30A5","\u30A6","\u30A7","\u30A8","\u30A9","\u30AA","\u30AB","\u30AC","\u30AD","\u30AE","\u30AF",
                "\u30B0","\u30B1","\u30B2","\u30B3","\u30B4","\u30B5","\u30B6","\u30B7","\u30B8","\u30B9","\u30BA","\u30BB","\u30BC","\u30BD","\u30BE","\u30BF",
                "\u30C0","\u30C1","\u30C2","\u30C3","\u30C4","\u30C5","\u30C6","\u30C7","\u30C8","\u30C9","\u30CA","\u30CB","\u30CC","\u30CD","\u30CE","\u30CF",
                "\u30D0","\u30D1","\u30D2","\u30D3","\u30D4","\u30D5","\u30D6","\u30D7","\u30D8","\u30D9","\u30DA","\u30DB","\u30DC","\u30DD","\u30DE","\u30DF",
                "\u30E0","\u30E1","\u30E2","\u30E3","\u30E4","\u30E5","\u30E6","\u30E7","\u30E8","\u30E9","\u30EA","\u30EB","\u30EC","\u30ED","\u30EE","\u30EF",
                "\u30F0","\u30F1","\u30F2","\u30F3","\u30F4","\u30F5","\u30F6","\u30F7","\u30F8","\u30F9","\u30FA","\u30FB","\u30FC","\u30FD","\u30FE","\u30FF",
                "0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
            };
            for (int i = 5; i < 100; i+=20)
            {
                g.DrawString(characters[rand.Next(0, characters.Count)], font, new SolidBrush(raindropColor), x, i + 3);
            }
            Rectangle rect = new Rectangle()
            {
                X = PreviewPictureBox.Left,
                Y = PreviewPictureBox.Top - 10, Width = PreviewPictureBox.Width,
                Height = (int)Math.Round(PreviewPictureBox.Height * .6666667)
            };
            g.FillRectangle(new LinearGradientBrush(new Point(rect.Left, rect.Bottom), new Point(rect.Left, rect.Top), Color.Transparent, backgroundColor), rect);
        }

        private void BackgroundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewPictureBox.BackColor = Color.FromName(BackgroundComboBox.Text);
        }

        private void RaindropComboBox_SelectedIndexChanged(object sender, EventArgs e)
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
