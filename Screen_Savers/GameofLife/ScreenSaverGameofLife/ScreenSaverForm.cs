using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using ScreenSaverGameofLife;

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

        private readonly static Random rand = new Random();
        private readonly int ResetCountLimit = 1250;
        private readonly bool previewMode = false;
        private Color BackgroundColor;
        private Brush BackgroundBrush;
        private bool Outline= false;
        private Color ShapeColor;
        private Brush ShapeBrush;
        private RectangleF RectF;
        private int[,] StateNew;
        private int BorderSize;
        private int StartAngle;
        private int ShapeSize;
        private string Shape;
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
                try
                {
                    // Get saved screensaver params
                    Shape = (string)key.GetValue("shape");
                    switch ((string)key.GetValue("outline"))
                    {
                        case "true":
                            Outline = true;
                            break;
                    }
                    if (!previewMode)
                    {
                        ShapeSize = (int)key.GetValue("shapeSize");
                        BorderSize = (int)key.GetValue("borderSize");
                    }
                    ShapeColor = Color.FromName((string)key.GetValue("shapeColor"));
                    BackgroundColor = Color.FromName((string)key.GetValue("backColor"));
                    StartAngle = (int)key.GetValue("startDegree");
                    EndAngle = (int)key.GetValue("endDegree");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "ScreenSaverGameofLife",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    Shape = "rectangle";
                    if (!previewMode)
                    {
                        ShapeSize = 24;
                        BorderSize = 8;
                    }
                    ShapeColor = Color.Lime;
                    BackgroundColor = Color.Black;
                }
            }
            else
            {
                Shape = "rectangle";
                if (!previewMode)
                {
                    ShapeSize = 24;
                    BorderSize = 8;
                }
                ShapeColor = Color.Lime;
                BackgroundColor = Color.Black;
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
            RectF = new RectangleF()
            {
                X = (Bounds.Width % ShapeSize) / 2,
                Y = (Bounds.Height % ShapeSize) / 2,
                Width = ShapeSize - BorderSize,
                Height = ShapeSize - BorderSize
            };
            BackColor = BackgroundColor;
            ReSeed();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < Cols; i++)
            {
                RectangleF rectF = new RectangleF() { X = RectF.X, Y = RectF.Y, Width = RectF.Width, Height = RectF.Height };
                if (i > 0)
                    rectF.X = ShapeSize * i;
                for (int j = 0; j < Rows; j++)
                {
                    if (j > 0)
                        rectF.Y = ShapeSize * j;
                    switch (Shape)
                    {
                        case "Square":
                            switch (Outline)
                            {
                                case true:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, rectF);
                                            break;
                                        case 1:
                                            g.DrawRectangle(ShapePen, new Rectangle() { X = (int)rectF.X, Y = (int)rectF.Y, Width = (int)rectF.Width, Height = (int)rectF.Height });
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, rectF);
                                            break;
                                        case 1:
                                            g.FillRectangle(ShapeBrush, rectF);
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
                                            g.FillRectangle(BackgroundBrush, rectF);
                                            break;
                                        case 1:
                                            g.DrawArc(ShapePen, rectF, 0, 360);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(BackgroundBrush, rectF);
                                            break;
                                        case 1:
                                            g.FillEllipse(ShapeBrush, rectF);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "Fish Scales":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, rectF);
                                    break;
                                case 1:
                                    g.DrawBezier(ShapePen,
                                        rectF.X, rectF.Y,
                                        rectF.X + ShapeSize - BorderSize, rectF.Y,
                                        rectF.X + ShapeSize - BorderSize, rectF.Y + ShapeSize - BorderSize,
                                        rectF.X, rectF.Y + ShapeSize - BorderSize);
                                    break;
                            }
                            break;
                        case "Arc":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, rectF);
                                    break;
                                case 1:
                                    g.DrawArc(ShapePen, rectF, StartAngle, EndAngle);
                                    break;
                            }
                            break;
                        case "Stingray":
                            switch (State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(BackgroundBrush, rectF);
                                    break;
                                case 1:
                                    g.DrawLine(ShapePen, rectF.X + ShapeSize, rectF.Y + ShapeSize, rectF.X + (ShapeSize / 2), rectF.Y + (ShapeSize / 2));
                                    g.DrawPie(ShapePen, rectF, 90, 270);
                                    break;
                            }
                            break;
                    }
                }
            }
            CalculateNewLife();
            Invalidate();
        }

        private void CalculateNewLife()
        {
            int countDead = 0;
            int living = 0;
            int livingNew = 0;
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
            for (int i = 0; i < StateNew.GetLength(0); i++)
            {
                for (int j = 0; j < StateNew.GetLength(1); j++)
                {
                    switch (StateNew[i, j])
                    {
                        case 0:
                            countDead++;
                            break;
                        case 1:
                            livingNew++;
                            break;
                    }
                    switch (State[i, j])
                    {
                        case 1:
                            living++;
                            break;
                    }
                    State[i, j] = StateNew[i, j];
                }
            }
            Count++;
            if (Count > ResetCountLimit)
            {
                Count = 0;
                ReSeed();
            }
            // ReSeed if screen is all dead
            else if (countDead == StateNew.Length)
            {
                Count = 0;
                ReSeed();
            }
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
                            switch (State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
                // bottom
                if (j > Rows - 1)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            switch (State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
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
                            switch (State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
                // bottom
                if (j > Rows - 1)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            switch (State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
            }
            // middle cells
            if (i > 0 && i < Cols - 1 && j > 0 && j < Rows - 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // left border
            if (i < 1 && j > 0 && j < Rows - 1)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // right border
            if (i > Cols - 2 && j > 0 && j < Rows - 1)
            {
                for (int k = -1; k < 1; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // top border
            if (i > 0 && i < Cols - 1 && j < 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        switch (State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // bottom border
            if (i > 0 && i < Cols - 1 && j > Rows - 2)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 1; l++)
                    {
                        switch (State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            return n;
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
    }
}
