using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScreenSaverGameofLife
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
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ScreenSaverGameofLife");
                key.SetValue("shapeColor", (string)GetShapeColor());
                key.SetValue("backColor", (string)GetBackgroundColor());
                key.SetValue("outline", (string)BoolOutline());
                key.SetValue("shape", (string)GetShape());
                key.SetValue("shapeSize", (int)GetShapeSize());
                key.SetValue("borderSize", (int)GetBorderSize());
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured Saving settings to Registry.");
            }
        }

        private void LoadSettings()
        {
            try
            {
                PopulateColorCombos();
                // Get the value stored in the Registry
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverGameofLife");
                if (key != null)
                {
                    PreSelectShapeColor((string)key.GetValue("shapeColor"));
                    PreSelectBackColor((string)key.GetValue("backColor"));
                    PreSelectOutline((string)key.GetValue("outline"));
                    PreSelectShape((string)key.GetValue("shape"));
                    PreSelectShapeSize((int)key.GetValue("shapeSize"));
                    PreSelectBorderSize((int)key.GetValue("borderSize"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void PreSelectShapeColor(string color)
        {
            ShapeColorComboBox.Text = color;
            ShapeColorBox.BackColor = Color.FromName(color);
        }

        private void PreSelectBackColor(string color)
        {
            BackgroundColorComboBox.Text = color;
            BackgroundColorBox.BackColor = Color.FromName(color);
        }

        private void PreSelectOutline(string outline)
        {
            if (outline.Equals("true"))
                OutlineCheckBox.Checked = true;
        }

        private void PreSelectShape(string shape)
        {
            switch (shape)
            {
                case "rectangle":
                    ShapeComboBox.Text = "Square";
                    break;
                case "ellipse":
                    ShapeComboBox.Text = "Circle";
                    break;
                case "bezier":
                    ShapeComboBox.Text = "FishScales";
                    break;
                case "line":
                    ShapeComboBox.Text = "X";
                    break;
            }
        }


        private string BoolOutline()
        {
            if (OutlineCheckBox.Checked)
                return "true";
            else
                return "false";
        }

        private string GetShape()
        {
            switch (ShapeComboBox.Text)
            {
                case "Square":
                    return "rectangle";
                case "Circle":
                    return "ellipse";
                case "FishScales":
                    return "bezier";
                case "X":
                    return "line";
                default:
                    return "rectangle";
            }
        }

        private int GetShapeSize()
        {
            return (int)ShapeNumericUpDown.Value;
        }

        private void PreSelectShapeSize(int size)
        {
            ShapeNumericUpDown.Value = size;
        }

        private int GetBorderSize()
        {
            return (int)BorderNumericUpDown.Value;
        }

        private void PreSelectBorderSize(int size)
        {
            BorderNumericUpDown.Value = size;
        }

        private string GetBackgroundColor()
        {
            return BackgroundColorBox.BackColor.Name;
        }

        private string GetShapeColor()
        {
            return ShapeColorBox.BackColor.Name;
        }

        private void PopulateColorCombos()
        {
            try
            {
                foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
                {
                    BackgroundColorComboBox.Items.Add(color);
                    ShapeColorComboBox.Items.Add(color);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void ShapeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (BorderNumericUpDown.Value > ShapeNumericUpDown.Value - 2)
                BorderNumericUpDown.Value = ShapeNumericUpDown.Value - 2;
        }

        private void BorderNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (BorderNumericUpDown.Value > ShapeNumericUpDown.Value - 2)
                BorderNumericUpDown.Value = ShapeNumericUpDown.Value - 2;
        }
    }
}
