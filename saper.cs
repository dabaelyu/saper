using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Minesweeper
{
  //ПРОЕКТ ДЕНИСА
  //НИЧЕГО НЕ ТРОГАЙ

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
        }
        //ПРОЕКТ ДЕНИСА
        //НИЧЕГО НЕ ТРОГАЙ
        private void InitializMenu()
        {
            menuStrip = new MenuStrip();
            gameToolStripMenuItem = new ToolStripMenuItem("игра");
            newGameToolStripMenuItem = new ToolStripMenuItem("новая игра", null,  newGame_Click);
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

