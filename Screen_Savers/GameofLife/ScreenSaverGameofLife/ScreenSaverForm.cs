using System;
using System.Collections.Generic;
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

        private readonly bool previewMode = false;
        private readonly static Random rand = new Random();
        private ScreenSaverDict screenSaverDict;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
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

            previewMode = true;
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;
            screenSaverDict = new ScreenSaverDict();

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverGameofLife");
            if (key != null)
            {
                try
                {
                    // Get saved screensaver params
                    screenSaverDict.Shape = (string)key.GetValue("shape");
                    switch ((string)key.GetValue("outline"))
                    {
                        case "true":
                            screenSaverDict.Outline = true;
                            break;
                    }
                    screenSaverDict.ShapeSize = (int)key.GetValue("shapeSize");
                    screenSaverDict.BorderSize = (int)key.GetValue("borderSize");
                    screenSaverDict.ShapeColor = Color.FromName((string)key.GetValue("shapeColor"));
                    screenSaverDict.BackgroundColor = Color.FromName((string)key.GetValue("backColor"));
                    screenSaverDict.StartAngle = (int)key.GetValue("startDegree");
                    screenSaverDict.EndAngle = (int)key.GetValue("endDegree");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "ScreenSaverGameofLife",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    screenSaverDict.Shape = "rectangle";
                    screenSaverDict.ShapeSize = 24;
                    screenSaverDict.BorderSize = 8;
                    screenSaverDict.ShapeColor = Color.Lime;
                    screenSaverDict.BackgroundColor = Color.Black;
                }
            }
            else
            {
                screenSaverDict.Shape = "rectangle";
                screenSaverDict.ShapeSize = 24;
                screenSaverDict.BorderSize = 8;
                screenSaverDict.ShapeColor = Color.Lime;
                screenSaverDict.BackgroundColor = Color.Black;
            }

            screenSaverDict.BackgroundBrush = new SolidBrush(screenSaverDict.BackgroundColor);
            screenSaverDict.ShapeBrush = new SolidBrush(screenSaverDict.ShapeColor);
            screenSaverDict.ShapePen = new Pen(screenSaverDict.ShapeColor, screenSaverDict.BorderSize) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };
            screenSaverDict.Cols = Math.Max(1, (Bounds.Width / screenSaverDict.ShapeSize) + 1);
            screenSaverDict.Rows = Math.Max(1, (Bounds.Height / screenSaverDict.ShapeSize) + 1);
            screenSaverDict.State = new int[screenSaverDict.Cols, screenSaverDict.Rows];
            screenSaverDict.StateNew = new int[screenSaverDict.Cols, screenSaverDict.Rows];

            InitializeGrid();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(screenSaverDict.BackgroundBrush, Bounds);
            for (int i = 0; i < screenSaverDict.Cols; i++)
            {
                for (int j = 0; j < screenSaverDict.Rows; j++)
                {
                    switch (screenSaverDict.Shape)
                    {
                        case "Square":
                            switch (screenSaverDict.Outline)
                            {
                                case true:
                                    switch (screenSaverDict.State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.Grid[i][j]);
                                            break;
                                        case 1:
                                            g.DrawRectangle(screenSaverDict.ShapePen, screenSaverDict.Grid[i][j]);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (screenSaverDict.State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.Grid[i][j]);
                                            break;
                                        case 1:
                                            g.FillRectangle(screenSaverDict.ShapeBrush, screenSaverDict.Grid[i][j]);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "Circle":
                            switch (screenSaverDict.Outline)
                            {
                                case true:
                                    switch (screenSaverDict.State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.GridF[i][j]);
                                            break;
                                        case 1:
                                            g.DrawArc(screenSaverDict.ShapePen, screenSaverDict.GridF[i][j], 0, 360);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (screenSaverDict.State[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.GridF[i][j]);
                                            break;
                                        case 1:
                                            g.FillEllipse(screenSaverDict.ShapeBrush, screenSaverDict.GridF[i][j]);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "Fish Scales":
                            switch (screenSaverDict.State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.GridF[i][j]);
                                    break;
                                case 1:
                                    g.DrawBezier(screenSaverDict.ShapePen,
                                        screenSaverDict.GridF[i][j].X, screenSaverDict.GridF[i][j].Y,
                                        screenSaverDict.GridF[i][j].X + screenSaverDict.ShapeSize - screenSaverDict.BorderSize, screenSaverDict.GridF[i][j].Y,
                                        screenSaverDict.GridF[i][j].X + screenSaverDict.ShapeSize - screenSaverDict.BorderSize, screenSaverDict.GridF[i][j].Y + screenSaverDict.ShapeSize - screenSaverDict.BorderSize,
                                        screenSaverDict.GridF[i][j].X, screenSaverDict.GridF[i][j].Y + screenSaverDict.ShapeSize - screenSaverDict.BorderSize);
                                    break;
                            }
                            break;
                        case "Arc":
                            switch (screenSaverDict.State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.GridF[i][j]);
                                    break;
                                case 1:
                                    g.DrawArc(screenSaverDict.ShapePen, screenSaverDict.GridF[i][j], screenSaverDict.StartAngle, screenSaverDict.EndAngle);
                                    break;
                            }
                            break;
                        case "Stingray":
                            switch (screenSaverDict.State[i, j])
                            {
                                case 0:
                                    g.FillRectangle(screenSaverDict.BackgroundBrush, screenSaverDict.GridF[i][j]);
                                    break;
                                case 1:
                                    g.DrawLine(screenSaverDict.ShapePen, screenSaverDict.GridF[i][j].X + screenSaverDict.ShapeSize, screenSaverDict.GridF[i][j].Y + screenSaverDict.ShapeSize, screenSaverDict.GridF[i][j].X + (screenSaverDict.ShapeSize / 2), screenSaverDict.GridF[i][j].Y + (screenSaverDict.ShapeSize / 2));
                                    g.DrawPie(screenSaverDict.ShapePen, screenSaverDict.GridF[i][j], 90, 270);
                                    break;
                            }
                            break;
                    }
                }
            }
            CalculateNewLife();
            Invalidate();
        }

        private void InitializeGrid()
        {
            switch (screenSaverDict.Shape)
            {
                case "Square":
                    screenSaverDict.Grid = new List<List<Rectangle>>();
                    RectangleShape();
                    break;
                case "Circle":
                    screenSaverDict.GridF = new List<List<RectangleF>>();
                    RectangleFShape();
                    break;
                case "Fish Scales":
                    screenSaverDict.GridF = new List<List<RectangleF>>();
                    RectangleFShape();
                    break;
                case "Arc":
                    screenSaverDict.GridF = new List<List<RectangleF>>();
                    RectangleFShape();
                    break;
                case "Stingray":
                    screenSaverDict.GridF = new List<List<RectangleF>>();
                    RectangleFShape();
                    break;
                default:
                    screenSaverDict.Grid = new List<List<Rectangle>>();
                    RectangleShape();
                    break;
            }
        }

        private void RectangleShape()
        {
            for (int i = 0; i < screenSaverDict.Cols; i++)
            {
                for (int j = 0; j < screenSaverDict.Rows; j++)
                {
                    screenSaverDict.Grid.Add(new List<Rectangle>());
                    Rectangle rect = new Rectangle()
                    {
                        Width = screenSaverDict.ShapeSize - screenSaverDict.BorderSize,
                        Height = screenSaverDict.ShapeSize - screenSaverDict.BorderSize
                    };
                    if (i == 0 && j == 0)
                    {
                        rect.X = 0;
                        rect.Y = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        rect.X = screenSaverDict.Grid[i - 1][j].X + screenSaverDict.ShapeSize;
                        rect.Y = screenSaverDict.Grid[i][j - 1].Y + screenSaverDict.ShapeSize;
                    }
                    // left border
                    else if (i < 1 && j > 0)
                    {
                        rect.X = 0;
                        rect.Y = screenSaverDict.Grid[i][j - 1].Y + screenSaverDict.ShapeSize;
                    }
                    // top border
                    else if (i > 0 && j < 1)
                    {
                        rect.X = screenSaverDict.Grid[i - 1][j].X + screenSaverDict.ShapeSize;
                        rect.Y = 0;
                    }
                    screenSaverDict.Grid[i].Add(rect);
                    screenSaverDict.State[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void RectangleFShape()
        {
            for (int i = 0; i < screenSaverDict.Cols; i++)
            {
                for (int j = 0; j < screenSaverDict.Rows; j++)
                {
                    screenSaverDict.GridF.Add(new List<RectangleF>());
                    RectangleF rect = new RectangleF()
                    {
                        Width = screenSaverDict.ShapeSize - screenSaverDict.BorderSize,
                        Height = screenSaverDict.ShapeSize - screenSaverDict.BorderSize
                    };
                    if (i == 0 && j == 0)
                    {
                        rect.X = 0;
                        rect.Y = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        rect.X = screenSaverDict.GridF[i - 1][j].X + screenSaverDict.ShapeSize;
                        rect.Y = screenSaverDict.GridF[i][j - 1].Y + screenSaverDict.ShapeSize;
                    }
                    // left border
                    else if (i < 1 && j > 0)
                    {
                        rect.X = 0;
                        rect.Y = screenSaverDict.GridF[i][j - 1].Y + screenSaverDict.ShapeSize;
                    }
                    // top border
                    else if (i > 0 && j < 1)
                    {
                        rect.X = screenSaverDict.GridF[i - 1][j].X + screenSaverDict.ShapeSize;
                        rect.Y = 0;
                    }
                    screenSaverDict.GridF[i].Add(rect);
                    screenSaverDict.State[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void CalculateNewLife()
        {
            int countDead = 0;
            int living = 0;
            int livingNew = 0;
            for (int i = 0; i < screenSaverDict.Cols; i++)
            {
                for (int j = 0; j < screenSaverDict.Rows; j++)
                {
                    int numNeighbors = SumNeighbors(i, j);
                    if (numNeighbors > 3 || numNeighbors < 2)
                        screenSaverDict.StateNew[i, j] = 0;
                    else if (numNeighbors == 3)
                        screenSaverDict.StateNew[i, j] = 1;
                    else
                        screenSaverDict.StateNew[i, j] = screenSaverDict.State[i, j];
                }
            }
            for (int i = 0; i < screenSaverDict.StateNew.GetLength(0); i++)
            {
                for (int j = 0; j < screenSaverDict.StateNew.GetLength(1); j++)
                {
                    switch (screenSaverDict.StateNew[i, j])
                    {
                        case 0:
                            countDead++;
                            break;
                        case 1:
                            livingNew++;
                            break;
                    }
                    switch (screenSaverDict.State[i, j])
                    {
                        case 1:
                            living++;
                            break;
                    }
                    screenSaverDict.State[i, j] = screenSaverDict.StateNew[i, j];
                }
            }
            screenSaverDict.Count++;
            if (screenSaverDict.Count > screenSaverDict.ResetCountLimit)
            {
                screenSaverDict.Count = 0;
                ReSeed();
            }
            // ReSeed if screen is all dead
            else if (countDead == screenSaverDict.StateNew.Length)
            {
                screenSaverDict.Count = 0;
                ReSeed();
            }
        }

        private void ReSeed()
        {
            for (int i = 0; i < screenSaverDict.Cols; i++)
            {
                for (int j = 0; j < screenSaverDict.Rows; j++)
                {
                    screenSaverDict.State[i, j] = rand.Next(0, 2);
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
                            switch (screenSaverDict.State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (screenSaverDict.State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
                // bottom
                if (j > screenSaverDict.Rows - 1)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            switch (screenSaverDict.State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (screenSaverDict.State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
            }
            // right corners
            if (i > screenSaverDict.Rows - 1)
            {
                // top
                if (j < 1)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            switch (screenSaverDict.State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (screenSaverDict.State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
                // bottom
                if (j > screenSaverDict.Rows - 1)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            switch (screenSaverDict.State[i + k, j + l])
                            {
                                case 1:
                                    n++;
                                    break;
                            }
                        }
                    }
                    switch (screenSaverDict.State[i, j])
                    {
                        case 1:
                            n--;
                            break;
                    }
                }
            }
            // middle cells
            if (i > 0 && i < screenSaverDict.Cols - 1 && j > 0 && j < screenSaverDict.Rows - 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (screenSaverDict.State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (screenSaverDict.State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // left border
            if (i < 1 && j > 0 && j < screenSaverDict.Rows - 1)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (screenSaverDict.State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (screenSaverDict.State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // right border
            if (i > screenSaverDict.Cols - 2 && j > 0 && j < screenSaverDict.Rows - 1)
            {
                for (int k = -1; k < 1; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        switch (screenSaverDict.State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (screenSaverDict.State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // top border
            if (i > 0 && i < screenSaverDict.Cols - 1 && j < 1)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        switch (screenSaverDict.State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (screenSaverDict.State[i, j])
                {
                    case 1:
                        n--;
                        break;
                }
            }
            // bottom border
            if (i > 0 && i < screenSaverDict.Cols - 1 && j > screenSaverDict.Rows - 2)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 1; l++)
                    {
                        switch (screenSaverDict.State[i + k, j + l])
                        {
                            case 1:
                                n++;
                                break;
                        }
                    }
                }
                switch (screenSaverDict.State[i, j])
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

        private class ScreenSaverDict
        {
            public string Shape { get; set; }
            public Color ShapeColor { get; set; }
            public Color BackgroundColor { get; set; }
            public int Cols { get; set; }
            public int Rows { get; set; }
            public int ShapeSize { get; set; }
            public int BorderSize { get; set; }
            public Brush ShapeBrush { get; set; }
            public Brush BackgroundBrush { get; set; }
            public Pen ShapePen { get; set; }
            public int StartAngle { get; set; }
            public int EndAngle { get; set; }
            public int[,] State { get; set; }
            public int[,] StateNew { get; set; }
            public int Count { get; set; }
            public int ResetCountLimit { get; set; } = 960;
            public bool Outline { get; set; } = false;
            public List<List<Rectangle>> Grid { get; set; }
            public List<List<RectangleF>> GridF { get; set; }
        }
    }
}
