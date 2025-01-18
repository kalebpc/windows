using System;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Drawing.Text;
using System.Linq;
//using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
using System.Windows.Forms;
//using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

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
        private int cellSize = 24;
        private int cols;
        private int rows;
        private readonly Color deadColor = Color.Black;
        private Color livingColor;
        private int repeat = 0;
        private List<List<Rectangle>> grid = new List<List<Rectangle>>();
        private int[,] gridState = new int[96, 96];
        private int[,] gridStateNew = new int[96, 96];

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            livingColor = Color.Lime;

            if (previewMode)
            {
                cellSize /= 10;
            }

            Cursor.Hide();
            TopMost = true;

            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ScreenSaverGameofLife");
            if (key != null)
            {
                // Get saved screensaver params
                livingColor = SetCharacterColor((string)key.GetValue("color"));
            }

            // Number of columns and rows
            cols = Math.Max(1, (Bounds.Width / cellSize) + 1);
            rows = Math.Max(1, (Bounds.Height / cellSize) + 1);

            InitializeGrid();
            Background.Paint += new PaintEventHandler(OnPaintBackground);
        }

        private void OnPaintBackground(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (gridState[i, j] == 0)
                    {
                        Brush brush = new SolidBrush(deadColor);
                        g.FillRectangle(brush, grid[i][j]);
                        brush.Dispose();
                    }
                    else
                    {
                        Brush brush = new SolidBrush(livingColor);
                        g.FillRectangle(brush, grid[i][j]);
                        brush.Dispose();
                    }
                }
            }
            g.Dispose();
            CalculateNewLife();
            Background.Invalidate();
        }
        private void InitializeGrid()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    List<Rectangle> list = new List<Rectangle>();
                    grid.Add(list);
                    Rectangle rect = new Rectangle()
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    if (i == 0 && j == 0)
                    {
                        rect.X = 0;
                        rect.Y = 0;
                    }
                    else if (i > 0 && j > 0)
                    {
                        rect.X = grid[i - 1][j].X + cellSize;
                        rect.Y = grid[i][j - 1].Y + cellSize;
                    }
                    // left border
                    else if (i < 1 && j > 0)
                    {
                        rect.X = 0;
                        rect.Y = grid[i][j - 1].Y + cellSize;
                    }
                    // top border
                    else if (i > 0 && j < 1)
                    {
                        rect.X = grid[i - 1][j].X + cellSize;
                        rect.Y = 0;
                    }
                    grid[i].Add(rect);
                    gridState[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void CalculateNewLife()
        {
            for (int i = 0; i < grid.Count(); i++)
            {
                for (int j = 0; j < grid[i].Count(); j++)
                {
                    int numNeighbors = SumNeighbors(i,j);
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
                    gridState[i, j] = gridStateNew[i, j];
                }
            }
            repeat++;
            if (repeat > 400)
            {
                if (rand.Next(0,100) < 50)
                {
                    repeat = 0;
                    ReSeed();
                }
                else
                {
                    repeat = 0;
                }
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

        private Color SetCharacterColor(string keyColor)
        {
            if (keyColor == "red")
            {
                return Color.Red;
            }
            else if (keyColor == "green")
            {
                return Color.Lime;
            }
            else
            {
                return Color.Blue;
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
