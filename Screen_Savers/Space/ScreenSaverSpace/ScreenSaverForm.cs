using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace ScreenSaverSpace
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

        private PointF MouseXY = new Point() { X = 0, Y = 0 };
        private readonly bool previewMode = false;
        private Bitmap DrawBitmap;
        private int WindowHeight;
        private int WindowWidth;
        private Graphics G;

        private readonly Random Rand = new Random();
        private List<PointF> StarsPreviousPos;
        private List<PointF> StarsStartPos;
        private List<PointF> StarsNextPos;
        private bool FollowMouse = false;
        private List<float> StarsSize;
        private Color BackgroundColor;
        private Brush BackgroundBrush;
        private PointF CenterPoint;
        private RectangleF RectF;
        private Color StarColor;
        private Brush StarBrush;
        private int StarMinSize;
        private int StarMaxSize;
        private int NumOfStars;
        private int EdgeBuff;
        private float Speed;
        private Pen StarPen;

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
            CenterPoint = new PointF(Size.Width * .5F, Size.Height * .5F);
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();

            /// <remarks>
            /// <para>Place screensaver on top of all windows</para>
            /// </remarks>
            TopMost = true;

            /// <remarks>
            /// <para>Retrieve user settings if key exists</para>
            /// </remarks>
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverSpace");
            if (key != null)
            {
                if (key.GetValue("backgroundColor") != null)
                    BackgroundColor = Color.FromName((string)key.GetValue("backgroundColor"));
                if (key.GetValue("starColor") != null)
                    StarColor = Color.FromName((string)key.GetValue("starColor"));
                if (key.GetValue("starSize") != null)
                    StarMinSize = (int)key.GetValue("starSize");
                if (key.GetValue("starSpeed") != null)
                    Speed = (float)Convert.ToDouble(key.GetValue("starSpeed"));
                if (key.GetValue("starCount") != null)
                    NumOfStars = (int)key.GetValue("starCount");
                if (key.GetValue("followMouse") != null)
                {
                    string s = (string)key.GetValue("followMouse");
                    if (s == "True")
                        FollowMouse = true;
                }
            }
            else
            {
                NumOfStars = 500;
                BackgroundColor = Color.Black;
                StarColor = Color.Red;
                Speed = .33F;
                FollowMouse = true;
                StarMinSize = 2;
            }
            if (previewMode)
                StarMinSize = 1;
            else
                CenterPoint = new PointF(MousePosition.X, MousePosition.Y);
            StarMaxSize = StarMinSize + 10;
            EdgeBuff = (int)Math.Round(555 * Speed);
            DrawBitmap = new Bitmap(WindowWidth, WindowHeight);

            /// <remarks>
            /// <para>Create graphics object to draw on</para>
            /// </remarks>
            G = Graphics.FromImage(DrawBitmap);

            PaintPictureBox.Image = DrawBitmap;

            /// <remarks>
            /// <para>Eliminate weird glitch when first painting bitmap to screen</para>
            /// </remarks>
            StarPen = new Pen(StarColor);
            StarBrush = new SolidBrush(StarColor);
            BackgroundBrush = new SolidBrush(BackgroundColor);
            G.FillRectangle(BackgroundBrush, new Rectangle(0, 0, DrawBitmap.Width, DrawBitmap.Height));

            CreateStars();
            RectF = new RectangleF();
            /// <remarks>
            /// <para>Start Timer</para>
            /// </remarks>
            MoveTimer.Enabled = true;
        }

        private void CreateStars()
        {
            StarsSize = new List<float>();
            StarsNextPos = new List<PointF>();
            StarsStartPos = new List<PointF>();
            StarsPreviousPos = new List<PointF>();
            for (int i = 0; i < NumOfStars; i++)
            {
                // Star Size
                StarsSize.Add(StarMinSize);
                // Star X
                float randX = Rand.Next(0, DrawBitmap.Width);
                // Star Y
                float randY = Rand.Next(0, DrawBitmap.Height);
                StarsNextPos.Add(new PointF(randX, randY));
                StarsStartPos.Add(new PointF(randX, randY));
                StarsPreviousPos.Add(new PointF(randX, randY));
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            /// <remarks>
            /// <para>Do Calculations here</para>
            /// </remarks>
            MoveStarsOut();

            /// <remarks>
            /// <para>Trigger re-paint event</para>
            /// </remarks>
            PaintPictureBox.Invalidate();
        }

        private void PaintPictureBox_Paint(object sender, PaintEventArgs e)
        {
            /// <remarks>
            /// <para>Do Drawing here with graphics object(s)</para>
            /// </remarks>
            G.FillRectangle(BackgroundBrush, new RectangleF(0, 0, DrawBitmap.Width, DrawBitmap.Height));
            for (int i = 0; i < NumOfStars; i++)
            {
                RectF.Width = StarsSize[i];
                RectF.Height = StarsSize[i];
                RectF.X = StarsNextPos[i].X - RectF.Width * .5F;
                RectF.Y = StarsNextPos[i].Y - RectF.Height * .5F;
                StarPen.Width = StarsSize[i];
                G.DrawLine(StarPen, StarsPreviousPos[i], StarsNextPos[i]);
                G.FillEllipse(StarBrush, RectF);
            }
        }

        private void MoveStarsOut()
        {
            for (int i = 0; i < NumOfStars; i++)
            {
                // Store current pos as previous position
                StarsPreviousPos[i] = StarsNextPos[i];

                // Calculate next x, y
                //StarsSXY[i][1] = StarsSXY[i][1] + ((StarsSXY[i][1] - CenterPoint.X) * Speed);
                float x = StarsPreviousPos[i].X + ((StarsPreviousPos[i].X - CenterPoint.X) * Speed);
                //StarsSXY[i][2] = StarsSXY[i][2] + ((StarsSXY[i][2] - CenterPoint.Y) * Speed);
                float y = StarsPreviousPos[i].Y + ((StarsPreviousPos[i].Y - CenterPoint.Y) * Speed);
                StarsNextPos[i] = new PointF(x, y);

                // Chance of size increase up to MaxSize
                if (Rand.Next(0, 100) < 3 && StarsSize[i] < StarMaxSize)
                    StarsSize[i] += Speed;

                // Within screen bounds checks, next x, y is startpos if off screen
                if (StarsNextPos[i].X > DrawBitmap.Width + EdgeBuff || StarsNextPos[i].X < -EdgeBuff)
                    ResetStar(i);
                else if (StarsNextPos[i].Y > DrawBitmap.Height + EdgeBuff || StarsNextPos[i].Y < -EdgeBuff)
                    ResetStar(i);
            }
        }

        private void ResetStar(int i)
        {
            float x = Rand.Next(0, DrawBitmap.Width);
            float y = Rand.Next(0, DrawBitmap.Height);
            StarsNextPos[i] = new PointF(x, y);
            StarsSize[i] = StarMinSize;
            StarsPreviousPos[i] = StarsNextPos[i];
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void PaintPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void PaintPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // if not following mouse close if can
            if (MouseXY.IsEmpty)
                MouseXY = new PointF(e.X, e.Y);
            if (!FollowMouse)
            {
                if (MouseXY != new PointF(e.X, e.Y))
                    if (!previewMode)
                        Application.Exit();
            }
            else if (FollowMouse)
            {
                // update centerpoint to new mouse position
                if (e.X > 0 && e.X < DrawBitmap.Width + EdgeBuff && e.Y > 0 && e.Y < DrawBitmap.Height + EdgeBuff)
                {
                    CenterPoint.X = e.X;
                    CenterPoint.Y = e.Y;
                }
            }
        }
    }
}
