using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;


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

        private readonly List<string> characters = new List<string>
        {
            "\u30A0","\u30A1","\u30A2","\u30A3","\u30A4","\u30A5","\u30A6","\u30A7","\u30A8","\u30A9","\u30AA","\u30AB","\u30AC","\u30AD","\u30AE","\u30AF",
            "\u30B0","\u30B1","\u30B2","\u30B3","\u30B4","\u30B5","\u30B6","\u30B7","\u30B8","\u30B9","\u30BA","\u30BB","\u30BC","\u30BD","\u30BE","\u30BF",
            "\u30C0","\u30C1","\u30C2","\u30C3","\u30C4","\u30C5","\u30C6","\u30C7","\u30C8","\u30C9","\u30CA","\u30CB","\u30CC","\u30CD","\u30CE","\u30CF",
            "\u30D0","\u30D1","\u30D2","\u30D3","\u30D4","\u30D5","\u30D6","\u30D7","\u30D8","\u30D9","\u30DA","\u30DB","\u30DC","\u30DD","\u30DE","\u30DF",
            "\u30E0","\u30E1","\u30E2","\u30E3","\u30E4","\u30E5","\u30E6","\u30E7","\u30E8","\u30E9","\u30EA","\u30EB","\u30EC","\u30ED","\u30EE","\u30EF",
            "\u30F0","\u30F1","\u30F2","\u30F3","\u30F4","\u30F5","\u30F6","\u30F7","\u30F8","\u30F9","\u30FA","\u30FB","\u30FC","\u30FD","\u30FE","\u30FF",
            "0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
        };
        private List<List<string>> RaintrailList = new List<List<string>>();
        private List<List<int>> RaindropListSXY = new List<List<int>>();
        private List<string> RaindropList = new List<string>();
        private readonly static Random rand = new Random();
        private readonly bool previewMode = false;
        private int minTrailLength = 24;
        private int maxTrailLength = 32;
        private Color BackgroundColor;
        private Color RaintrailColor;
        private Brush RaintrailBrush;
        private Color RaindropColor;
        private Brush RaindropBrush;
        private Font RaindropFont;
        private int maxSpeed = 20;
        private int FontSize = 20;
        private int Gap = 6;
        private int minSpeed = 2;
        private int WindowHeight;
        private string FontType;
        private int UpperBound;
        private int LowerBound;
        private int Cols;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint , true);
            this.Bounds = Bounds;
            WindowHeight = Bounds.Height;
            UpperBound = -WindowHeight;
            LowerBound = WindowHeight * 2;
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

            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            FontSize = 3;
            Gap = 3 / 3;
            minSpeed = 1;
            maxSpeed = 5;
            WindowHeight = Size.Height;
            UpperBound = -WindowHeight * 2;
            LowerBound = WindowHeight * 3;
            previewMode = true;
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverFallingCode");
            if (key != null)
            {
                try
                {
                    BackgroundColor = Color.FromName((string)key.GetValue("backColor"));
                    RaindropColor = Color.FromName((string)key.GetValue("raindropColor"));
                    RaintrailColor = Color.FromName((string)key.GetValue("raintrailColor"));
                    FontType = (string)key.GetValue("fontType");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "ScreenSaverFallingCode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    SetDefaultSettings();
                }
            }
            else
                SetDefaultSettings();
            RaindropFont = new Font(FontType, FontSize);
            RaindropBrush = new SolidBrush(RaindropColor);
            RaintrailBrush = new SolidBrush(RaintrailColor);
            Cols = Math.Max(1, (Bounds.Width / FontSize) + 1);
            BackColor = BackgroundColor;
            InitializeRaindrops();
        }

        private void SetDefaultSettings()
        {
            FontType = "Segoe UI";
            RaindropColor = Color.White;
            RaintrailColor = Color.Red;
            BackgroundColor = Color.Black;
        }

        private void InitializeRaindrops()
        {
            for (int i = 0; i < Cols; i++)
            {
                RaindropList.Add(characters[rand.Next(0, characters.Count)]);
                int trailLength = rand.Next(minTrailLength, maxTrailLength);
                List<string> list = new List<string>();
                for (int j = 0; j < trailLength; j++)
                    list.Add(characters[rand.Next(0, characters.Count)]);
                RaintrailList.Add(list);
                List<int> sxy = new List<int>()
                {
                    rand.Next(minSpeed, maxSpeed)
                };
                // x
                if (i < 1)
                    sxy.Add(0);
                else
                    sxy.Add(RaindropListSXY[i - 1][1] + FontSize + Gap*3);
                // y
                sxy.Add(rand.Next(UpperBound, -FontSize));
                RaindropListSXY.Add(sxy);
            }
        }

        private void MoveDown()
        {
            for (int i = 0; i < Cols; i++)
            {
                if (rand.Next(0, 200) < 1)
                    RaindropListSXY[i][0] = rand.Next(minSpeed, maxSpeed);
                if (RaindropListSXY[i][2] + RaindropListSXY[i][0] < LowerBound)
                    RaindropListSXY[i][2] += RaindropListSXY[i][0];
                else
                {
                    RaindropListSXY[i][2] = -FontSize;
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < Cols; i++)
            {
                if (rand.Next(0, 100) < 1)
                    RaindropList[i] = characters[rand.Next(0, characters.Count)];
                g.DrawString(RaindropList[i], RaindropFont, RaindropBrush, RaindropListSXY[i][1], RaindropListSXY[i][2]);
                for (int j = 0; j < RaintrailList[i].Count; j++)
                {
                    if (rand.Next(0, 100) < 1)
                        RaintrailList[i][j] = characters[rand.Next(0, characters.Count)];
                    if (j < 1)
                        g.DrawString(RaintrailList[i][j], RaindropFont, RaintrailBrush, RaindropListSXY[i][1], RaindropListSXY[i][2] - FontSize - Gap);
                    else
                        g.DrawString(RaintrailList[i][j], RaindropFont, RaintrailBrush, RaindropListSXY[i][1], RaindropListSXY[i][2] - (FontSize + Gap * 5 * j));
                }
            }
            MoveDown();
            Invalidate();
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
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
                        Application.Exit();
                }
            }
        }
    }
}

