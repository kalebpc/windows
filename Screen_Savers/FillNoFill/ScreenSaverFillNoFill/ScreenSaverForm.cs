using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;


namespace ScreenSaverFillNoFill
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

        private readonly static Random rand = new Random();
        private Color BackgroundColor = Color.Black;
        private readonly bool previewMode = false;
        private Color GridFillColor = Color.Red;
        private Point MouseXY = new Point(0, 0);
        private Color GridColor = Color.Red;
        private bool ShowGrid = false;
        private Brush GridFillBrush;
        private float RightMovement;
        private float DownMovement;
        private int CountDown = 0;
        private int Interval = 35;
        private int WindowHeight;
        private PointF[] Polygon;
        private float GridStroke;
        private int WindowWidth;
        private int CountUp = 0;
        private int Radius = 50;
        private float OffsetY;
        private float OffsetX;
        private int[,] State;
        private Pen GridPen;
        private int Theta;
        private int Cols;
        private int Rows;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.Bounds = Bounds;
            WindowWidth = Bounds.Width;
            WindowHeight = Bounds.Height;
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
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            Radius = 10;
            WindowWidth = Size.Width;
            WindowHeight = Size.Height;
            
            previewMode = true;
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Screensavers\\ScreenSaverFillNoFill");
            if (key != null)
            {
                if (key.GetValue("backgroundColor") != null)
                {
                    BackgroundColor = Color.FromName((string)key.GetValue("backgroundColor"));
                }
                if (key.GetValue("shapeColor") != null)
                {
                    GridFillColor = Color.FromName((string)key.GetValue("shapeColor"));
                }
                if (key.GetValue("gridColor") != null)
                {
                    GridColor = Color.FromName((string)key.GetValue("gridColor"));
                }
                if (key.GetValue("showGrid") != null)
                {
                    if (!previewMode)
                    {

                    }
                    if ((string)key.GetValue("showGrid") == "True")
                        ShowGrid = true;
                }
                if (key.GetValue("size") != null)
                {
                    if (!previewMode)
                        Radius = (int)key.GetValue("size");
                }
                if (key.GetValue("speed") != null)
                {
                    Interval = (int)key.GetValue("speed");
                }
            }

            GridStroke = Radius / 20;
            Theta = 60;
            BackColor = BackgroundColor;
            GridFillBrush = new SolidBrush(GridFillColor);
            GridPen = new Pen(new SolidBrush(GridColor), GridStroke);
            RightMovement = (float)(Radius * Math.Cos(Theta) * 1.8111111);
            Cols = (int)Math.Abs(WindowWidth / RightMovement);
            OffsetX = (WindowWidth % RightMovement) / 8;
            DownMovement = Radius + Radius;
            Rows = (int)Math.Abs(WindowHeight / DownMovement);
            OffsetY = (WindowHeight % DownMovement) / 8;
            State = DefaultState(State);
            CreatePolygon();
            MoveTimer.Interval = Interval;
            MoveTimer.Enabled = true;
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (CountUp < State.Length)
                State = AddOneState(State);
            else
            {
                if (CountDown < State.Length)
                    State = RemoveOneState(State);
                else
                {
                    CountUp = 0;
                    CountDown = 0;
                }
            }
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int i = 0; i < Rows; i++)
            {
                PointF[] xPolygon = CopyPolygon(Polygon);
                if (i > 0)
                    xPolygon = MoveDown(xPolygon, DownMovement * i);
                for (int j = 0; j < Cols; j++)
                {
                    if (j % 2 == 0)
                    {
                        switch (ShowGrid)
                        {
                            case true:
                                switch (State[j, i])
                                {
                                    case 0:
                                        g.DrawPolygon(GridPen, xPolygon);
                                        break;
                                    case 1:
                                        g.FillPolygon(GridFillBrush, xPolygon);
                                        break;
                                }
                                break;
                            case false:
                                switch (State[j, i])
                                {
                                    case 1:
                                        g.FillPolygon(GridFillBrush, xPolygon);
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        PointF[] yPolygon = CopyPolygon(xPolygon);
                        yPolygon = MoveDown(yPolygon, DownMovement / 2);
                        switch (ShowGrid)
                        {
                            case true:
                                switch (State[j, i])
                                {
                                    case 0:
                                        g.DrawPolygon(GridPen, yPolygon);
                                        break;
                                    case 1:
                                        g.FillPolygon(GridFillBrush, yPolygon);
                                        break;
                                }
                                break;
                            case false:
                                switch (State[j, i])
                                {
                                    case 1:
                                        g.FillPolygon(GridFillBrush, yPolygon);
                                        break;
                                }
                                break;
                        }
                    }
                    xPolygon = MoveRight(xPolygon, -RightMovement);
                }
            }
        }

        private void CreatePolygon()
        {
            int thetaLocal = Theta;
            Polygon = new PointF[360 / Theta];
            PointF centerPoint = new PointF(Radius + OffsetX*(OffsetX*.8111111F), Radius + OffsetY);
            for (int i = 0; i < Polygon.Length; i++)
            {
                double radians = thetaLocal * (Math.PI / 180);
                Polygon[i] = new PointF(centerPoint.X + (float)(Radius * Math.Cos(radians)), centerPoint.Y + (float)(Radius * Math.Sin(radians)));
                thetaLocal += Theta;
            }
        }

        private PointF[] MoveRight(PointF[] polygon, float n)
        {
            for (int j = 0; j < polygon.Length; j++)
            {
                polygon[j].X += n;
            }
            return polygon;
        }

        private PointF[] MoveDown(PointF[] polygon, float n)
        {
            for (int j = 0; j < polygon.Length; j++)
            {
                polygon[j].Y += n;
            }
            return polygon;
        }

        private PointF[] CopyPolygon(PointF[] polygon)
        {
            PointF[] pcopy = new PointF[polygon.Length];
            for (int j = 0; j < polygon.Length; j++)
            {
                pcopy[j] = polygon[j];
            }
            return pcopy;
        }

        private int[,] DefaultState(int[,] state)
        {
            state = new int[Cols, Rows];
            for (int j = 0; j < Cols; j++)
            {
                for (int k = 0; k < Rows; k++)
                {
                    state[j, k] = 0;
                }
            }
            return state;
        }

        private int[,] AddOneState(int[,] state)
        {
            int x = rand.Next(0, Cols);
            int y = rand.Next(0, Rows);
            while (state[x, y] != 0)
            {
                x = rand.Next(0, Cols);
                y = rand.Next(0, Rows);
            }
            state[x, y] = 1;
            CountUp++;
            return state;
        }

        private int[,] RemoveOneState(int[,] state)
        {
            int x = rand.Next(0, Cols);
            int y = rand.Next(0, Rows);
            while (state[x, y] != 1)
            {
                x = rand.Next(0, Cols);
                y = rand.Next(0, Rows);
            }
            state[x, y] = 0;
            CountDown++;
            return state;
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
            if (!MouseXY.IsEmpty)
            {
                if (MouseXY != new Point(e.X, e.Y))
                {
                    if (!previewMode)
                        Application.Exit();
                }
            }
            MouseXY = new Point(e.X, e.Y);
        }
    }
}
