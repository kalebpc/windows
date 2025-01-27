using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;


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

        public readonly List<string> characters = new List<string>
        {
            "\u30A0","\u30A1","\u30A2","\u30A3","\u30A4","\u30A5","\u30A6","\u30A7","\u30A8","\u30A9","\u30AA","\u30AB","\u30AC","\u30AD","\u30AE","\u30AF",
            "\u30B0","\u30B1","\u30B2","\u30B3","\u30B4","\u30B5","\u30B6","\u30B7","\u30B8","\u30B9","\u30BA","\u30BB","\u30BC","\u30BD","\u30BE","\u30BF",
            "\u30C0","\u30C1","\u30C2","\u30C3","\u30C4","\u30C5","\u30C6","\u30C7","\u30C8","\u30C9","\u30CA","\u30CB","\u30CC","\u30CD","\u30CE","\u30CF",
            "\u30D0","\u30D1","\u30D2","\u30D3","\u30D4","\u30D5","\u30D6","\u30D7","\u30D8","\u30D9","\u30DA","\u30DB","\u30DC","\u30DD","\u30DE","\u30DF",
            "\u30E0","\u30E1","\u30E2","\u30E3","\u30E4","\u30E5","\u30E6","\u30E7","\u30E8","\u30E9","\u30EA","\u30EB","\u30EC","\u30ED","\u30EE","\u30EF",
            "\u30F0","\u30F1","\u30F2","\u30F3","\u30F4","\u30F5","\u30F6","\u30F7","\u30F8","\u30F9","\u30FA","\u30FB","\u30FC","\u30FD","\u30FE","\u30FF",
            "0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
        };
        private List<List<int>> RaindropListXY = new List<List<int>>();
        private readonly static Random Rand = new Random();
        Point MouseXY = new Point() { X = 0, Y = 0 };
        private readonly bool previewMode = false;
        private Color BackgroundColor;
        private Color RaindropColor;
        private Font RaindropFont;
        private Bitmap DrawBitmap;
        private readonly int FontSize = 12;
        private int WindowHeight;
        private int WindowWidth;
        private string FontType;
        private Rectangle Rect;
        private int UpperBound;
        private int Gap;
        private Graphics G;
        private int Cols;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint , true);
            this.Bounds = Bounds;
            WindowHeight = Bounds.Height;
            WindowWidth = Bounds.Width;
            Gap = FontSize / 4;
            UpperBound = -WindowHeight * 2;
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
            FontSize = 1;
            Gap = 0;
            WindowHeight = Size.Height;
            WindowWidth = Size.Width;
            UpperBound = -WindowHeight * 2;
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
                if (key.GetValue("backColor") != null)
                    BackgroundColor = Color.FromName((string)key.GetValue("backColor"));
                if (key.GetValue("raindropColor") != null)
                    RaindropColor = Color.FromName((string)key.GetValue("raindropColor"));
                if (key.GetValue("fontType") != null)
                    FontType = (string)key.GetValue("fontType");
            }
            else
            {
                FontType = "Segoe UI";
                RaindropColor = Color.Green;
                BackgroundColor = Color.Black;
            }
            RaindropFont = new Font(FontType, FontSize);
            Rect = new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = FontSize * 2,
                Height = WindowHeight * 2
            };
            if (previewMode)
                Rect.Height = WindowHeight;
            Cols = Math.Max(1, (WindowWidth / (FontSize + Gap)) + 1);
            DrawBitmap = new Bitmap(WindowWidth, WindowHeight);
            G = Graphics.FromImage(DrawBitmap);
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            G.FillRectangle(new SolidBrush(BackgroundColor), new Rectangle(0,0,DrawBitmap.Width,DrawBitmap.Height));
            PaintPictureBox.Image = DrawBitmap;
            MoveTimer.Enabled = true;
            InitializeRaindrops();
        }

        private void InitializeRaindrops()
        {
            for (int i = 0; i < Cols; i++)
            {
                List<int> xy = new List<int>();
                // x
                if (i < 1)
                    xy.Add(0);
                else
                    xy.Add(RaindropListXY[i - 1][0] + FontSize + Gap * 2);
                // y
                xy.Add(Rand.Next(UpperBound, -FontSize));
                RaindropListXY.Add(xy);
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            MoveDown();
            PaintPictureBox.Invalidate();
        }

        private void MoveDown()
        {
            for (int i = 0; i < Cols; i++)
            {
                if (RaindropListXY[i][1] + FontSize + Gap < DrawBitmap.Height + DrawBitmap.Height)
                    RaindropListXY[i][1] += FontSize + Gap;
                else
                {
                    RaindropListXY[i][1] = Rand.Next(-(int)Math.Round(WindowHeight * .3333333), -FontSize);
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Cols; i++)
            {
                G.DrawString(characters[Rand.Next(0, characters.Count)], RaindropFont, new SolidBrush(RaindropColor), RaindropListXY[i][0], RaindropListXY[i][1]);
                Rect.X = RaindropListXY[i][0] - Gap;
                Rect.Y = RaindropListXY[i][1] - Rect.Height - FontSize * 10;
                G.FillRectangle(new LinearGradientBrush(new Point(Rect.Left, Rect.Bottom), new Point(Rect.Left, Rect.Top), Color.FromArgb(0, BackgroundColor), BackgroundColor), Rect);
            }
        }

        private void PaintPictureBox_MouseClick(object sender, MouseEventArgs e)
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

        private void PaintPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MouseXY.IsEmpty)
            {
                if (MouseXY != new Point(e.X, e.Y))
                    Application.Exit();
            }
            MouseXY = new Point(e.X, e.Y);
        }
    }
}

