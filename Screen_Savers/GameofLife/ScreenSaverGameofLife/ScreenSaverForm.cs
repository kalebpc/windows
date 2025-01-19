using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
//using System.Diagnostics;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing.Text;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Windows.Forms.VisualStyles;
//using ScreenSaverGameofLife;


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
        private bool initialized = false;
        private readonly int countStagnantReSeed = 100;
        private int countStagnant = 0;
        private int count = 0;
        private int cellSize;
        private int borderSize;
        private int cols;
        private int rows;
        private string shape;
        private Color shapeColor;
        private Color backgroundColor;
        Brush brush;
        Brush brush1;
        Pen pen1;
        private bool outLine = false;
        private List<List<RectangleF>> rectGridF = new List<List<RectangleF>>();
        private List<List<Rectangle>> rectGrid = new List<List<Rectangle>>();
        // sized for up to 4k monitor (3840/24px + 1)
        private int[,] gridState = new int[161, 161];
        private int[,] gridStateNew = new int[161, 161];


        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
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
                shape = (string)key.GetValue("shape");
                switch ((string)key.GetValue("outline"))
                {
                    case "True":
                        outLine = true;
                        break;
                    case "False":
                        outLine = false;
                        break;
                }
                cellSize = (int)key.GetValue("shapeSize");
                borderSize = (int)key.GetValue("borderSize");
                shapeColor = Color.FromName((string)key.GetValue("shapeColor"));
                backgroundColor = Color.FromName((string)key.GetValue("backColor"));
            }
            else
            {
                shape = "rectangle";
                cellSize = 24;
                borderSize = 8;
                shapeColor = Color.Lime;
                backgroundColor = Color.Black;
            }
            switch (outLine)
            {
                case true:
                    if (cellSize < 40)
                        cellSize = 40;
                    if (borderSize < 8)
                        borderSize = 8;
                    break;
            }
            if (cellSize < 24)
                cellSize = 24;
            if (borderSize > cellSize)
                borderSize = 0;

            brush = new SolidBrush(backgroundColor);
            brush1 = new SolidBrush(shapeColor);
            pen1 = new Pen(shapeColor, borderSize) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };

            // Number of columns and rows
            cols = Math.Max(1, (Bounds.Width / cellSize) + 1);
            rows = Math.Max(1, (Bounds.Height / cellSize) + 1);

            if (previewMode)
            {
                cellSize = 2;
                borderSize = 0;
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(brush, Bounds);
            if (!initialized)
            {
                initialized = true;
                InitializeGrid();
            }
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    switch (shape)
                    {
                        case "rectangle":
                            switch (outLine)
                            {
                                case true:
                                    switch (gridState[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(brush, rectGrid[i][j]);
                                            break;
                                        case 1:
                                            g.DrawRectangle(pen1, rectGrid[i][j]);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (gridState[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(brush, rectGrid[i][j]);
                                            break;
                                        case 1:
                                            g.FillRectangle(brush1, rectGrid[i][j]);
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "ellipse":
                            switch (outLine)
                            {
                                case true:
                                    switch (gridState[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(brush, rectGridF[i][j]);
                                            break;
                                        case 1:
                                            g.DrawEllipse(pen1, rectGridF[i][j]);
                                            break;
                                    }
                                    break;
                                case false:
                                    switch (gridState[i, j])
                                    {
                                        case 0:
                                            g.FillRectangle(brush, rectGridF[i][j]);
                                            break;
                                        case 1:
                                            g.FillEllipse(brush1, rectGridF[i][j]);
                                            break;
                                    }
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
            switch (shape)
            {
                case "rectangle":
                    RectangleShape();
                    break;
                case "ellipse":
                    RectangleFShape();
                    break;
                default:
                    RectangleShape();
                    break;
            }
        }

        private void RectangleShape()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    List<Rectangle> list = new List<Rectangle>();
                    rectGrid.Add(list);
                    Rectangle rect = new Rectangle()
                    {
                        Width = cellSize - borderSize,
                        Height = cellSize - borderSize
                    };
                    if (i == 0 && j == 0)
                    {
                        rect.X = 0;
                        rect.Y = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        rect.X = rectGrid[i - 1][j].X + cellSize;
                        rect.Y = rectGrid[i][j - 1].Y + cellSize;
                    }
                    // left border
                    else if (i < 1 && j > 0)
                    {
                        rect.X = 0;
                        rect.Y = rectGrid[i][j - 1].Y + cellSize;
                    }
                    // top border
                    else if (i > 0 && j < 1)
                    {
                        rect.X = rectGrid[i - 1][j].X + cellSize;
                        rect.Y = 0;
                    }
                    rectGrid[i].Add(rect);
                    gridState[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void RectangleFShape()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    List<RectangleF> list = new List<RectangleF>();
                    rectGridF.Add(list);
                    RectangleF rect = new RectangleF()
                    {
                        Width = cellSize - borderSize,
                        Height = cellSize - borderSize
                    };
                    if (i == 0 && j == 0)
                    {
                        rect.X = 0;
                        rect.Y = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        rect.X = rectGridF[i - 1][j].X + cellSize;
                        rect.Y = rectGridF[i][j - 1].Y + cellSize;
                    }
                    // left border
                    else if (i < 1 && j > 0)
                    {
                        rect.X = 0;
                        rect.Y = rectGridF[i][j - 1].Y + cellSize;
                    }
                    // top border
                    else if (i > 0 && j < 1)
                    {
                        rect.X = rectGridF[i - 1][j].X + cellSize;
                        rect.Y = 0;
                    }
                    rectGridF[i].Add(rect);
                    gridState[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void CalculateNewLife()
        {
            int countDead = 0;
            int living = 0;
            int livingNew = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int numNeighbors = SumNeighbors(i, j);
                    if (numNeighbors > 3 || numNeighbors < 2)
                        gridStateNew[i, j] = 0;
                    else if (numNeighbors == 3)
                        gridStateNew[i, j] = 1;
                    else
                        gridStateNew[i, j] = gridState[i, j];
                }
            }
            for (int i = 0; i < gridStateNew.GetLength(0); i++)
            {
                for (int j = 0; j < gridStateNew.GetLength(1); j++)
                {
                    switch (gridStateNew[i, j])
                    {
                        case 0:
                            countDead++;
                            break;
                        case 1:
                            livingNew++;
                            break;
                    }
                    switch (gridState[i, j])
                    {
                        case 1:
                            living++;
                            break;
                    }
                    gridState[i, j] = gridStateNew[i, j];
                }
            }
            count++;
            if (countStagnant > countStagnantReSeed)
            {
                countStagnant = 0;
                ReSeed();
            }
            // ReSeed if screen is all dead
            else if (countDead == gridStateNew.Length)
            {
                countStagnant = 0;
                ReSeed();
            }
            // Set repaint false if screen is stagnant
            else if (living == livingNew)
            {
                countStagnant++;
            }
            // Force reseed after 500 counts
            else if (count > 500)
            {
                count = 0;
                ReSeed();
            }
        }

        private void ReSeed()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    gridState[i, j] = rand.Next(0, 2);
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
                    if (gridState[(i - 1 + cols) % cols, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i + 1, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[(i - 1 + cols) % cols, j] != 0)
                        n++;
                    if (gridState[i + 1, j] != 0)
                        n++;
                    if (gridState[(i - 1 + cols) % cols, j + 1] != 0)
                        n++;
                    if (gridState[i, j + 1] != 0)
                        n++;
                    if (gridState[i + 1, j + 1] != 0)
                        n++;
                }
                // bottom
                if (j > gridState.GetLength(1) - 1)
                {
                    if (gridState[(i - 1 + cols) % cols, j - 1] != 0)
                        n++;
                    if (gridState[i, j - 1] != 0)
                        n++;
                    if (gridState[i + 1, j - 1] != 0)
                        n++;
                    if (gridState[(i - 1 + cols) % cols, j] != 0)
                        n++;
                    if (gridState[i + 1, j] != 0)
                        n++;
                    if (gridState[(i - 1 + cols) % cols, (j + 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i, (j + 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i + 1, (j + 1 + rows) % rows] != 0)
                        n++;
                }
            }
            // right corners
            if (i > gridState.GetLength(0) - 1)
            {
                // top
                if (j < 1)
                {
                    if (gridState[i - 1, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, (j - 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i - 1, j] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, j] != 0)
                        n++;
                    if (gridState[i - 1, j + 1] != 0)
                        n++;
                    if (gridState[i, j + 1] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, j + 1] != 0)
                        n++;
                }
                // bottom
                if (j > gridState.GetLength(1) - 1)
                {
                    if (gridState[i - 1, j - 1] != 0)
                        n++;
                    if (gridState[i, j - 1] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, j - 1] != 0)
                        n++;
                    if (gridState[i - 1, j] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, j] != 0)
                        n++;
                    if (gridState[i - 1, (j + 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[i, (j + 1 + rows) % rows] != 0)
                        n++;
                    if (gridState[(i + 1 + cols) % cols, (j + 1 + rows) % rows] != 0)
                        n++;
                }
            }
            // middle cells
            if (i > 0 && i < gridState.GetLength(0) - 1 && j > 0 && j < gridState.GetLength(1) - 1)
            {
                if (gridState[i - 1, j - 1] != 0)
                    n++;
                if (gridState[i - 1, j - 1] != 0)
                    n++;
                if (gridState[i, j - 1] != 0)
                    n++;
                if (gridState[i + 1, j - 1] != 0)
                    n++;
                if (gridState[i - 1, j] != 0)
                    n++;
                if (gridState[i + 1, j] != 0)
                    n++;
                if (gridState[i - 1, j + 1] != 0)
                    n++;
                if (gridState[i, j + 1] != 0)
                    n++;
                if (gridState[i + 1, j + 1] != 0)
                    n++;
            }
            // left border
            if (i < 1 && j > 0 && j < gridState.GetLength(1) - 1)
            {
                if (gridState[(i - 1 + cols) % cols, j - 1] != 0)
                    n++;
                if (gridState[i, j - 1] != 0)
                    n++;
                if (gridState[i + 1, j - 1] != 0)
                    n++;
                if (gridState[(i - 1 + cols) % cols, j] != 0)
                    n++;
                if (gridState[i + 1, j] != 0)
                    n++;
                if (gridState[(i - 1 + cols) % cols, j + 1] != 0)
                    n++;
                if (gridState[i, j + 1] != 0)
                    n++;
                if (gridState[i + 1, j + 1] != 0)
                    n++;
            }
            // right border
            if (i > gridState.GetLength(0) - 1 && j > 0 && j < gridState.GetLength(1) - 2)
            {
                if (gridState[i - 1, j - 1] != 0)
                    n++;
                if (gridState[i, j - 1] != 0)
                    n++;
                if (gridState[(i + 1 + cols) % cols, j - 1] != 0)
                    n++;
                if (gridState[i - 1, j] != 0)
                    n++;
                if (gridState[(i + 1 + cols) % cols, j] != 0)
                    n++;
                if (gridState[i - 1, j + 1] != 0)
                    n++;
                if (gridState[i, j + 1] != 0)
                    n++;
                if (gridState[(i + 1 + cols) % cols, j + 1] != 0)
                    n++;
            }
            // top border
            if (i > 0 && i < gridState.GetLength(0) - 1 && j < 1)
            {
                if (gridState[i - 1, (j - 1 + rows) % rows] != 0)
                    n++;
                if (gridState[i, (j - 1 + rows) % rows] != 0)
                    n++;
                if (gridState[i + 1, (j - 1 + rows) % rows] != 0)
                    n++;
                if (gridState[i - 1, j] != 0)
                    n++;
                if (gridState[i + 1, j] != 0)
                    n++;
                if (gridState[i - 1, j + 1] != 0)
                    n++;
                if (gridState[i, j + 1] != 0)
                    n++;
                if (gridState[i + 1, j + 1] != 0)
                    n++;
            }
            // bottom border
            if (i > 0 && i < gridState.GetLength(0) - 1 && j > gridState.GetLength(1) - 2)
            {
                if (gridState[i - 1, j - 1] != 0)
                    n++;
                if (gridState[i, j - 1] != 0)
                    n++;
                if (gridState[i + 1, j - 1] != 0)
                    n++;
                if (gridState[i - 1, j] != 0)
                    n++;
                if (gridState[i + 1, j] != 0)
                    n++;
                if (gridState[i - 1, (j + 1 + rows) % rows] != 0)
                    n++;
                if (gridState[i, (j + 1 + rows) % rows] != 0)
                    n++;
                if (gridState[i + 1, (j + 1 + rows) % rows] != 0)
                    n++;
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
    }
}
