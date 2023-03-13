using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лабиринт
{
    public partial class Лабиринт : Form
    {
        public Лабиринт()
        {
            InitializeComponent();
            Init();
            Width = cellsize * mapwidth + 17;
            Height = cellsize * mapheight + 40;
        }
        void Init()
        {
            for (int i = 0; i < mapwidth; i++)
            {
                for (int j = 0; j < mapheight; j++)
                {
                    map[i, j] = r.Next(0, 4);
                    map[0, i] = 1;
                    map[j, 0] = 1;
                    map[i, mapwidth - 1] = 1;
                    map[mapheight - 1, j] = 1;

                }
            }
            map[exitX - 1, exitY + 1] = 0;
            map[exitX, exitY] = 0;
            map[enterX + 1, enterY] = 0;
        }
        int cellsize = 50;
        static int mapwidth = 12;
        static int mapheight = 12;
        static int[,] map = new int[mapwidth, mapheight];
        Random r = new Random();
        int playerX = 0, playerY = 10;
        int exitX = 10, exitY = 1;
        int enterX = 0, enterY = 10;
        int counter = 0;
        int level = 1;


        void mapbuild(Graphics gr)
        {
            var bmp = Bitmap.FromFile("shahter.jpg");
            var bmpExit = Bitmap.FromFile("exit.jpg");
            int x = map.GetLength(0);
            int y = map.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    switch (map[i, j])
                    {
                        case 1:
                            wall(gr, i, j);
                            break;
                        case 2:
                            apple(gr, i, j); break;

                    }

                }
            }
            gr.FillRectangle(Brushes.Black, enterX * cellsize, enterY * cellsize, cellsize * 2, cellsize);
            gr.DrawImage(bmp, playerX * cellsize, playerY * cellsize);
            gr.DrawImage(bmpExit, exitX * cellsize, exitY * cellsize);
            gr.DrawString($"Собрано алмазов {counter}", SystemFonts.DefaultFont, Brushes.Black, exitX, exitY);
            gr.DrawString($"Уровень {level}", SystemFonts.DefaultFont, Brushes.Black, exitX, exitY + 15);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            {
                if (keyData == Keys.Up)
                {
                    if (map[playerX, playerY - 1] != 1)
                    {
                        { playerY--; }
                    }
                }
                if (keyData == Keys.Down)
                {
                    if (map[playerX, playerY + 1] != 1)
                    {
                        { playerY++; }
                    }
                }
                if (keyData == Keys.Right)
                {
                    if (map[playerX + 1, playerY] != 1)
                    {
                        { playerX++; }
                    }

                }
                if (keyData == Keys.Left)
                {
                    if (map[playerX - 1, playerY] != 1)
                    {
                        { playerX--; }
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        void wall(Graphics gr, int row, int column)
        {
            var cx = row * cellsize;
            var cy = column * cellsize;
            var bmp = Bitmap.FromFile("brick-wall.jpg");

            gr.DrawImage(bmp, cx, cy);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Лабиринт_Load(object sender, EventArgs e)
        {

        }

        void apple(Graphics gr, int row, int column)
        {
            var cx = row * cellsize;
            var cy = column * cellsize;
            var bmp = Bitmap.FromFile("almaz.jpg");
            gr.DrawImage(bmp, cx, cy);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.Clear(Color.Black);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            mapbuild(gr);
            if (map[playerX, playerY] == 2) { map[playerX, playerY] = 0; counter++; }
            if (playerX == exitX && playerY == exitY)
            {
                Init();
                level++;
                playerX = enterX; playerY = enterY;

            }



        }
    }
}
