using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper_www
{
    public partial class Form1 : Form
    {
        private int row = 8;
        private int col = 8;
        private int mineCount = 10;
        private bool[,] mine;
        private bool[,] open;
        private bool[,] flag;
        private Button[,] buttons;
        private bool gameOver;
        private int safeCells;

        private enum Slojnost { ez, norm, hard };
        private Slojnost curretSlojnost = Slojnost.ez;

        private MenuStrip menuStrip;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem slojnostToolStripMenuItem;
        private ToolStripMenuItem ezToolStripMenuItem;
        private ToolStripMenuItem normToolStripMenuItem;
        private ToolStripMenuItem hardToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Label statusLabel;
        public Form1()
        {
            InitializeComponent();
            InitializMenu();
            InitializGame();
        }
        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void InitializGame()
        {
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    this.Controls.Remove(button);
                }
            }

            int buttonSize = 30;
            this.ClientSize = new Size(col * buttonSize, row * buttonSize + menuStrip.Height);

            buttons = new Button[col, row];
            mine = new bool[col, row];
            open = new bool[col, row];
            flag = new bool[col, row];
            gameOver = false;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    buttons[i, j] = new Button()
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(i * buttonSize, j * buttonSize + menuStrip.Height),
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        Tag = new Point(i, j)
                    };
                    buttons[i, j].MouseDown += Button_MouseDown;
                    this.Controls.Add(buttons[i, j]);
                }
            }
            PlaceMine();

            UpdateStatus($"Игра началась. Осталось безопасных клеток: {safeCells}");
        }
        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void PlaceMine()
        {
            Random rnd = new Random();
            int minePlaced = 0;

            while (minePlaced < mineCount)
            {
                int i = rnd.Next(col);
                int j = rnd.Next(row);

                if (!mine[i, j])
                {
                    mine[i, j] = true;
                    minePlaced++;
                }
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (gameOver) return;

            Button button = (Button)sender;
            Point positiion = (Point)button.Tag;
            int x = positiion.X;
            int y = positiion.Y;

            if (e.Button == MouseButtons.Left && !flag[x, y])
            {
                if (mine[x, y])
                {
                    GameOver(false);
                }
                else
                {
                    OpenCells(x, y);

                    if (safeCells == 0)
                    {
                        GameOver(true);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!open[x, y])
                {
                    flag[x, y] = !flag[x, y];
                    button.Text = flag[x, y] ? "|>" : "";
                }
            }
        }

        private void OpenCells(int x, int y)
        {
            if (x < 0 || x >= col || y < 0 || y >= row || open[x, y] || flag[x, y])
            {
                return;
            }

            open[x, y] = true;
            safeCells--;
            buttons[x, y].Enabled = false;
            buttons[x, y].BackColor = SystemColors.ControlLight;

            int Around = minesAround(x, y);

            if (Around > 0)
            {
                buttons[x, y].Text = Around.ToString();
                buttons[x, y].ForeColor = GetNumberColor(Around);
            }
            else
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        OpenCells(x + i, y + j);
                    }
                }
            }
            UpdateStatus($"Осталось безопасных клеток: {safeCells}");
            return;
        }

        private int minesAround(int x, int y)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    if (nx >= 0 && nx < col && ny >= 0 && ny < row && mine[nx, ny])
                        count++;
                }
            }

            return count;
        }

        private Color GetNumberColor(int number)
        {
            switch (number)
            {
                case 1: return Color.Green;
                case 2: return Color.Blue;
                case 3: return Color.Red;
                case 4: return Color.Magenta;
                case 5: return Color.Yellow;
                case 6: return Color.Orange;
                case 7: return Color.Orchid;
                case 8: return Color.PaleGreen;
                default: return Color.Black;
            }
        }

        private void GameOver(bool isWin)
        {
            gameOver = true;

            // Показываем все мины
            for (int x = 0; x < col; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    if (mine[x, y])
                    {
                        buttons[x, y].Text = "`o";
                        buttons[x, y].BackColor = Color.Red;
                    }
                }
            }

            string message = isWin ? "Поздравляем! Вы победили!" : "Игра окончена! Вы проиграли!";

            UpdateStatus(message);
            MessageBox.Show(message, "Игра окончена");
        }

        private void UpdateStatus(string text)
        {
            statusLabel.Text = text;
        }

        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void InitializMenu()
        {
            menuStrip = new MenuStrip();
            gameToolStripMenuItem = new ToolStripMenuItem("игра");
            newGameToolStripMenuItem = new ToolStripMenuItem("новая игра", null, newGame_Click);
            slojnostToolStripMenuItem = new ToolStripMenuItem("сложность", null, Slojnost_Click);
            ezToolStripMenuItem = new ToolStripMenuItem("легкая", null, Slojnost_Click);
            normToolStripMenuItem = new ToolStripMenuItem("средняя", null, Slojnost_Click);
            hardToolStripMenuItem = new ToolStripMenuItem("тяжелая", null, Slojnost_Click);
            exitToolStripMenuItem = new ToolStripMenuItem("выход", null, Exit_Click);

            menuStrip.Items.Add(gameToolStripMenuItem);
            gameToolStripMenuItem.DropDownItems.AddRange(new[] { newGameToolStripMenuItem, slojnostToolStripMenuItem, exitToolStripMenuItem });
            slojnostToolStripMenuItem.DropDownItems.AddRange(new[] { ezToolStripMenuItem, normToolStripMenuItem, hardToolStripMenuItem });

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 25,
                BackColor = Color.LightGray,
                Text = "Выберите сложность и начните игру"
            };
            this.Controls.Add(statusLabel);
        }
        private void newGame_Click(object sender, EventArgs e)
        {
            SetSlojnost(curretSlojnost);
            InitializGame();
        }
        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void Slojnost_Click(object sender, EventArgs e)
        {
            if (sender == ezToolStripMenuItem)
            {
                SetSlojnost(Slojnost.ez);
            }
            else if (sender == normToolStripMenuItem)
            {
                SetSlojnost(Slojnost.norm);
            }
            else if (sender == hardToolStripMenuItem)
            {
                SetSlojnost(Slojnost.hard);
            }
            InitializGame();
        }

        private void SetSlojnost(Slojnost slojnost)
        {
            curretSlojnost = slojnost;

            switch (slojnost)
            {
                case Slojnost.ez:
                    row = 8;
                    col = 8;
                    mineCount = 10;
                    break;
                case Slojnost.norm:
                    row = 16;
                    col = 16;
                    mineCount = 45;
                    break;
                case Slojnost.hard:
                    row = 32;
                    col = 32;
                    mineCount = 99;
                    break;

            }
        }
        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
