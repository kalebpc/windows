using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
using System.Windows.Forms;
//using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
//using ScreenSaverFallingCode;


namespace ScreenSaverFallingCode
{
    public partial class ScreenSaverForm : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private readonly bool previewMode = false;
        private readonly static Random rand = new Random();
        private readonly List<string> characters = new List<string>
        {
            "\u30A0","\u30A1","\u30A2","\u30A3","\u30A4","\u30A5","\u30A6","\u30A7","\u30A8","\u30A9","\u30AA","\u30AB","\u30AC","\u30AD","\u30AE","\u30AF",
            "\u30B0","\u30B1","\u30B2","\u30B3","\u30B4","\u30B5","\u30B6","\u30B7","\u30B8","\u30B9","\u30BA","\u30BB","\u30BC","\u30BD","\u30BE","\u30BF",
            "\u30C0","\u30C1","\u30C2","\u30C3","\u30C4","\u30C5","\u30C6","\u30C7","\u30C8","\u30C9","\u30CA","\u30CB","\u30CC","\u30CD","\u30CE","\u30CF",
            "\u30D0","\u30D1","\u30D2","\u30D3","\u30D4","\u30D5","\u30D6","\u30D7","\u30D8","\u30D9","\u30DA","\u30DB","\u30DC","\u30DD","\u30DE","\u30DF",
            "\u30E0","\u30E1","\u30E2","\u30E3","\u30E4","\u30E5","\u30E6","\u30E7","\u30E8","\u30E9","\u30EA","\u30EB","\u30EC","\u30ED","\u30EE","\u30EF",
            "\u30F0","\u30F1","\u30F2","\u30F3","\u30F4","\u30F5","\u30F6","\u30F7","\u30F8","\u30F9","\u30FA","\u30FB","\u30FC","\u30FD","\u30FE","\u30FF",
            "\u2160","\u2161","\u2162","\u2163","\u2164","\u2165","\u2166","\u2167","\u2168","\u2169","\u216A","\u216B","\u216C","\u216D","\u216E","\u216F",
            "0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
        };
        private readonly List<Label> rainDrops = new List<Label>();
        private readonly List<Label> rainDropsColumns = new List<Label>();
        private readonly List<Color> redColors = new List<Color>
        {
            Color.Red,
            //Color.Crimson,
            //Color.OrangeRed,
            //Color.Tomato,
            //Color.IndianRed,
            //Color.Firebrick,
            //Color.Brown,
            //Color.Maroon,
            //Color.DarkRed,
            //Color.DimGray
        };
        private readonly List<Color> greenColors = new List<Color>
        {
            //Color.Lime,
            //Color.GreenYellow,
            //Color.PaleGreen,
            //Color.LightGreen,
            Color.LimeGreen,
            //Color.MediumSeaGreen,
            //Color.DarkSeaGreen,
            //Color.Gray,
            //Color.DimGray,
            //Color.DarkSlateGray
        };
        private readonly List<Color> blueColors = new List<Color>
        {
            //Color.LightSkyBlue,
            //Color.DeepSkyBlue,
            //Color.DodgerBlue,
            //Color.CornflowerBlue,
            //Color.RoyalBlue,
            Color.Blue,
            //Color.MediumBlue,
            //Color.DarkBlue,
            //Color.Navy,
            //Color.MidnightBlue
        };

        private readonly int maxCharWidth = 32;
        private readonly int fontSize = 16;
        private readonly int columnWidth = 50;
        private readonly Font font = new Font("Segoe UI", 16);

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            this.StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            // Initialize colors
            List<Color> colors = greenColors;

            Cursor.Hide();
            TopMost = true;

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverFallingCode");
            if (key != null)
            {
                // Get saved screensaver params
                TextLabel.Text = (string)key.GetValue("text");
                colors = SetCharacterColor((string)key.GetValue("color"));
            }
            // Set Center Text to a bright color
            TextLabel.ForeColor = colors[0];
            // Center TextLabel on screen
            TextLabel.Top = (Bounds.Height / 2) - (TextLabel.Height / 2);
            TextLabel.Left = (Bounds.Width / 2) - (TextLabel.Width / 2);

            // Create list of label raindrops
            for (int i = 0; i < (Bounds.Width / columnWidth); i++)
            {
                string character = characters[rand.Next(0, characters.Count())];
                int startHeight = rand.Next(-Bounds.Height*2, 0);
                Label label = new Label
                {
                    Text = character,
                    Font = font,
                    ForeColor = Color.White,
                    Top = startHeight,
                    AutoSize = true
                };
                Label label1 = new Label
                {
                    Top = 2,
                    Font = new Font("Segoe UI", fontSize),
                    ForeColor = colors[0],
                    Width = columnWidth,
                    AutoSize=true
                };
                if (i < 1)
                {
                    label.Left = maxCharWidth/4;
                    label1.Left = 5;
                }
                else
                {
                    int left = rainDrops[i - 1].Left + columnWidth;
                    label.Left = left;
                    label1.Left = left;
                }
                if (previewMode)
                {
                    Font font1 = new Font("Segoe UI", 2);
                    label.Font = font1;
                    label1.Font = font1;
                }
                rainDrops.Add(label);
                rainDropsColumns.Add(label1);
                this.Controls.Add(label);
                this.Controls.Add(label1);
            }

            moveTimer.Interval = 125;
            moveTimer.Tick += new EventHandler(Timer_Tick);
            moveTimer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            for (int i = 0; i < rainDrops.Count(); i++)
            {
                rainDrops[i].Text = characters[rand.Next(0, characters.Count())];
                // Move raindrops down
                if (rainDrops[i].Top < Bounds.Height)
                {
                    rainDrops[i].Top += rainDrops[i].Size.Height;
                    // update columns
                    if (rainDrops[i].Top > 0)
                    {
                        if (rainDrops[i].Top < fontSize*2)
                        {
                            rainDropsColumns[i].Text = "";
                            rainDropsColumns[i].Top = 0;
                        }
                        rainDropsColumns[i].Text += Environment.NewLine;
                        rainDropsColumns[i].Text += rainDrops[i].Text;
                        rainDropsColumns[i].Top = rainDrops[i].Top - rainDropsColumns[i].Height;
                    }
                    else
                    {
                        rainDropsColumns[i].Top += rainDrops[i].Size.Height;
                    }
                }
                // Else move back to top
                else
                {
                    rainDrops[i].Top = rand.Next(-Bounds.Height * 2, 0);
                    // empty column
                    if (rand.Next(0, 100) < 79)
                    {
                        rainDropsColumns[i].Text = "";
                        rainDropsColumns[i].Top = 0;
                    }

                }
            }
        }

        private List<Color> SetCharacterColor(string keyColor)
        {
            if (keyColor == "red")
            {
                return redColors;
            }
            else if (keyColor == "green")
            {
                return greenColors;
            }
            else
            {
                return blueColors;
            }
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MousePosition.IsEmpty)
            {
                int mouseLocationX = MousePosition.X;
                int mouseLocationY = MousePosition.Y;
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocationX - e.X) > 10 || Math.Abs(mouseLocationY - e.Y) > 10)
                {
                    if (!previewMode)
                    {
                        Application.Exit();
                    }
                }
            }
        }
        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            GetClientRect(PreviewWndHandle, out Rectangle ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            // Make text smaller
            TextLabel.Font = new System.Drawing.Font("Segoe UI", 2);

            previewMode = true;
        }
    }
}
