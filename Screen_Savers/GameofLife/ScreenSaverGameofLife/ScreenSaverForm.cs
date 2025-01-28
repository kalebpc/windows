using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace ScreenSaverGameofLife
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

        private Point MouseXY = new Point() { X = 0, Y = 0};
        private readonly static Random rand = new Random();
        private Color BackgroundColor = Color.Black;
        private readonly bool previewMode = false;
        private Color ShapeColor = Color.Lime;
        private string Shape = "Square";
        private Brush BackgroundBrush;
        private bool Outline= false;
        private int ShapeSize = 24;
        private int BorderSize = 8;
        private Brush ShapeBrush;
        private RectangleF RectF;
        private int[,] StateNew;
        private int[,] StateOld;
        private Rectangle Rect;
        private int StartAngle;
        private Pen ShapePen;
        private int EndAngle;
        private int[,] State;
        private int Count;
        private int Cols;
        private int Rows;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            WindowState = FormWindowState.Maximized;
            this.Bounds = Bounds;
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
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            ShapeSize = 2;
            BorderSize = 0;
            Cols = Math.Max(1, Size.Width / ShapeSize);
            Rows = Math.Max(1, Size.Height / ShapeSize);
            previewMode = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverGameofLife");
            if (key != null)
            {
                // Get saved screensaver params
                if (!previewMode)
                {
                    if (key.GetValue("shapeSize") != null)
                        ShapeSize = (int)key.GetValue("shapeSize");
                    if (key.GetValue("borderSize") != null)
                        BorderSize = (int)key.GetValue("borderSize");
                }
                if (key.GetValue("shape") != null)
                    Shape = (string)key.GetValue("shape");
                string s = (string)key.GetValue("outline");
                if (s != null)
                {
                    if (s == "true")
                    {
                        Outline = true;
                        if (BorderSize < 1)
                            BorderSize = 1;
                    }   
                }
                if (key.GetValue("shapeColor") != null)
                    ShapeColor = Color.FromName((string)key.GetValue("shapeColor"));
                if (key.GetValue("backColor") != null)
                    BackgroundColor = Color.FromName((string)key.GetValue("backColor"));
                if (key.GetValue("startDegree") != null)
                    StartAngle = (int)key.GetValue("startDegree");
                if (key.GetValue("endDegree") != null)
                    EndAngle = (int)key.GetValue("endDegree");
                if (Shape == "Arc" && BorderSize < 1)
                    BorderSize = 1;
            }
            BackgroundBrush = new SolidBrush(BackgroundColor);
            ShapeBrush = new SolidBrush(ShapeColor);
            ShapePen = new Pen(ShapeColor, BorderSize) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };
            if (!previewMode)
            {
                Cols = Math.Max(1, Bounds.Width / ShapeSize);
                Rows = Math.Max(1, Bounds.Height / ShapeSize);
            }
            State = new int[Cols, Rows];
            StateNew = new int[Cols, Rows];
            StateOld = new int[Cols, Rows];
            if (Shape == "Square" && Outline == true)
            {
                Rect = new Rectangle()
                {
                    X = (int)Math.Round((Bounds.Width % ShapeSize) * .5),
                    Y = (int)Math.Round((Bounds.Height % ShapeSize) * .5),
                    Width = ShapeSize - BorderSize,
                    Height = ShapeSize - BorderSize
                };
            }
            RectF = new RectangleF()
            {
                X = (int)Math.Round((Bounds.Width % ShapeSize) * .5),
                Y = (int)Math.Round((Bounds.Height % ShapeSize) * .5),
                Width = ShapeSize - BorderSize,
                Height = ShapeSize - BorderSize
            };
            BackColor = BackgroundColor;
            PaintPictureBox.Width = Bounds.Width;
            PaintPictureBox.Height = Bounds.Height;
            ReSeed();
            MoveTimer.Enabled = true;
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            CalculateNewLife();
            PaintPictureBox.Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < Cols; i++)
            {
                if (i > 0)
                {
                    RectF.X = ShapeSize * i;
                    RectF.Y = (int)Math.Round((Bounds.Height % ShapeSize) * .5);
                }
                for (int j = 0; j < Rows; j++)
                {
                    if (j > 0)
                        RectF.Y = ShapeSize * j;
                    switch (Shape)
                    {
                        case "Square":
                            switch (Outline)
                            {
                                case true:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, RectF);
                                            break;
                                        case 1:
                                            Rect.X = (int)RectF.X;
                                            Rect.Y = (int)RectF.Y;
                                            g.DrawRectangle(ShapePen, Rect);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, RectF);
                                            break;
                                        case 1:
                                            g.FillRectangle(ShapeBrush, RectF);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "Circle":
                            switch (Outline)
                            {
                                case true:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, RectF);
                                            break;
                                        case 1:
                                            g.DrawArc(ShapePen, RectF, 0, 360);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, RectF);
                                            break;
                                        case 1:
                                            g.FillEllipse(ShapeBrush, RectF);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "Fish Scales":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, RectF);
                                    break;
                                case 1:
                                    g.DrawBezier(ShapePen,
                                        RectF.X + ShapeSize - BorderSize, RectF.Y,
                                        RectF.X + ShapeSize - BorderSize, RectF.Y + ShapeSize - BorderSize,
                                        RectF.X, RectF.Y,
                                        RectF.X, RectF.Y + ShapeSize - BorderSize);
                                    break;
                            }
                            break;
                        case "Arc":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, RectF);
                                    break;
                                case 1:
                                    g.DrawArc(ShapePen, RectF, StartAngle, EndAngle);
                                    break;
                            }
                            break;
                        case "Stingray":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, RectF);
                                    break;
                                case 1:
                                    g.DrawLine(ShapePen, RectF.X + ShapeSize, RectF.Y + ShapeSize, RectF.X + (int)Math.Round(ShapeSize * .5), RectF.Y + (int)Math.Round(ShapeSize * .5));
                                    g.DrawPie(ShapePen, RectF, 90, 270);
                                    break;
                            }
                            break;
                    }
                }
            }
            // reset RectF location
            RectF.X = (int)Math.Round((Bounds.Width % ShapeSize) * .5);
            RectF.Y = (int)Math.Round((Bounds.Height % ShapeSize) * .5);
        }

        private void CalculateNewLife()
        {
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    int numNeighbors = SumNeighbors(i, j);
                    if (numNeighbors > 3 || numNeighbors < 2)
                        StateNew[i, j] = 0;
                    else if (numNeighbors == 3)
                        StateNew[i, j] = 1;
                    else
                        StateNew[i, j] = State[i, j];
                }
            }
            bool result = CheckStagnant();
            if (result)
                Count++;
            if (Count > 4)
            {
                Count = 0;
                ReSeed();
            }
            else
            {
                // copy over values
                for (int i = 0; i < State.GetLength(0); i++)
                {
                    for (int j = 0; j < State.GetLength(1); j++)
                    {
                        StateOld[i, j] = State[i, j];
                        State[i, j] = StateNew[i, j];
                    }
                }
            }
        }

        private bool CheckStagnant()
        {
            for (int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {
                    if (StateNew[i, j] != StateOld[i, j])
                        return false;
                }
            }
            return true;
        }

        private void ReSeed()
        {
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    State[i, j] = rand.Next(0, 2);
                }
            }
        }

        private int SumNeighbors(int i, int j)
        {
            int n = 0;
            // left corners
            if (i < 1)
            {
                // top
                if (j < 1)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            if (State[i + k, j + l] == 1)
                                n++;
                        }
                    }
                    if (State[i, j] == 1)
                        n--;
                }
                // bottom
                if (j > Rows - 1)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            if (State[i + k, j + l] == 1)
                                n++;
                        }
                    }
                    if (State[i, j] == 1)
                        n--;
                }
            }
            // right corners
            if (i > Rows - 1)
            {
                // top
                if (j < 1)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            if (State[i + k, j + l] == 1)
                                n++;
                        }
                    }
                    if (State[i, j] == 1)
                        n--;
                }
                // bottom
                if (j > Rows - 1)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            if (State[i + k, j + l] == 1)
                                n++;
                        }
                    }
                    if (State[i, j] == 1)
                        n--;
                }
            }
            // middle cells
            if (i > 0 && i < Cols - 1 && j > 0 && j < Rows - 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        if (State[i + k, j + l] == 1)
                            n++;
                    }
                }
                if (State[i, j] == 1)
                    n--;
            }
            // left border
            if (i < 1 && j > 0 && j < Rows - 1)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        if (State[i + k, j + l] == 1)
                            n++;
                    }
                }
                if (State[i, j] == 1)
                    n--;
            }
            // right border
            if (i > Cols - 2 && j > 0 && j < Rows - 1)
            {
                for (int k = -1; k < 1; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        if (State[i + k, j + l] == 1)
                            n++;
                    }
                }
                if (State[i, j] == 1)
                    n--;
            }
            // top border
            if (i > 0 && i < Cols - 1 && j < 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        if (State[i + k, j + l] == 1)
                            n++;
                    }
                }
                if (State[i, j] == 1)
                    n--;
            }
            // bottom border
            if (i > 0 && i < Cols - 1 && j > Rows - 2)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 1; l++)
                    {
                        if (State[i + k, j + l] == 1)
                            n++;
                    }
                }
                if (State[i, j] == 1)
                    n--;
            }
            return n;
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
            if (!MouseXY.IsEmpty)
            {
                if (MouseXY != new Point(e.X, e.Y))
                    Application.Exit();
            }
            MouseXY = new Point(e.X, e.Y);
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }
    }
}
