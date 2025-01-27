using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;
using ScreenSaverBlank;


namespace ScreenSaverBlank
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

        /// <remarks>
        /// <para>Essential to detecting mouse move</para>
        /// </remarks>
        private Point MouseXY = new Point() { X = 0, Y = 0 };

        private readonly bool previewMode = false;
        private int WindowHeight;
        private int WindowWidth;
        private Graphics G;
        Bitmap DrawBitmap;

        /// <summary>
        /// Create screen saver form size of Screen (Monitor).
        /// </summary>
        /// <param name="Bounds">ScreenBounds represented as a Rectangle</param>
        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;

            /// <remarks>
            /// <para>Ensure new forms are displayed on corresponding monitors</para>
            /// </remarks>
            StartPosition = FormStartPosition.Manual;

            /// <remarks>
            /// <para>Flicker free painting to screen</para>
            /// </remarks>
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            /// <remarks>
            /// <para>Cover whole window and let windows know covering entire desktop surface</para>
            /// </remarks>
            WindowState = FormWindowState.Maximized;

            /// <remarks>
            /// <para>Set WindowWidth to the entire screen width</para>
            /// </remarks>
            WindowWidth = this.Bounds.Width;
            WindowHeight = this.Bounds.Height;

            /// <remarks>
            /// <para>Set other 'non preview mode' settings here</para>
            /// </remarks>
        }

        /// <summary>
        /// Create screen saver form size of 'Screen Savers settings' Peek Window.
        /// </summary>
        /// <param name="PreviewWndHandle">Pointer to 'Screen Savers settings' Peek Window</param>
        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            /// <remarks>
            /// <para>Set the preview window as the parent of this window</para>
            /// </remarks>
            SetParent(this.Handle, PreviewWndHandle);

            /// <remarks>
            /// <para>Make this a child window so it will close when the parent dialog closes</para>
            /// <para>GWL_STYLE = -16, WS_CHILD = 0x40000000</para>
            /// </remarks>
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            /// <remarks>
            /// <para>Place our window inside the parent</para>
            /// </remarks>
            GetClientRect(PreviewWndHandle, out Rectangle ParentRect);

            /// <remarks>
            /// <para>Set Size and Location of preview peek window</para>
            /// </remarks>
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            previewMode = true;

            /// <remarks>
            /// <para>Ensure peek view displayed in correct location</para>
            /// </remarks>
            StartPosition = FormStartPosition.Manual;

            /// <remarks>
            /// <para>Flicker free painting to screen in peek view window</para>
            /// </remarks>
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            /// <remarks>
            /// <para>Set WindowWidth to the screen savers peek view window</para>
            /// </remarks>
            WindowWidth = Size.Width;
            WindowHeight = Size.Height;

            /// <remarks>
            /// <para>Set other 'preview mode' dependent settings here</para>
            /// </remarks>
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();

            /// <remarks>
            /// <para>Place screensaver on top of all windows</para>
            /// </remarks>
            TopMost = true;

            /// <remarks>
            /// <para>Retrieve user settings if it exists</para>
            /// </remarks>
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverBlank");
            if (key != null)
            {
                /// <remarks>
                /// <para>if (key.GetValue("") != null)</para>
                /// </remarks>
            }
            else
            {
                /// <remarks>
                /// <para>Settings if registry entry does not exist</para>
                /// </remarks>
            }
            DrawBitmap = new Bitmap(WindowWidth, WindowHeight);

            /// <remarks>
            /// <para>Create graphics object to draw on</para>
            /// </remarks>
            G = Graphics.FromImage(DrawBitmap);

            PaintPictureBox.Image = DrawBitmap;

            /// <remarks>
            /// <para>Eliminate weird glitch when first painting bitmap to screen</para>
            /// </remarks>
            G.FillRectangle(Brushes.Black, new Rectangle(0, 0, DrawBitmap.Width, DrawBitmap.Height));

            /// <remarks>
            /// <para>Start Timer</para>
            /// </remarks>
            MoveTimer.Enabled = true;
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            /// <remarks>
            /// <para>Do Calculations here</para>
            /// </remarks>

            /// <remarks>
            /// <para>Trigger re-paint event</para>
            /// </remarks>
            PaintPictureBox.Invalidate();
        }

        private void PaintPictureBox_Paint(object sender, PaintEventArgs e)
        {
            /// <remarks>
            /// <para>Do Drawing here with graphics object(s)</para>
            /// <para>G.DrawEllipse(...</para>
            /// </remarks>
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void PaintPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void PaintPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            /// <remarks>
            /// <para>If MouseXY is not (0, 0), exit program</para>
            /// </remarks>
            if (!MouseXY.IsEmpty)
            {
                if (MouseXY != new Point(e.X, e.Y))
                    Application.Exit();
            }

            /// <remarks>
            /// <para>If MouseXY is (0, 0) set to current mouse position</para>
            /// </remarks>
            MouseXY = new Point(e.X, e.Y);
        }
    }
}
