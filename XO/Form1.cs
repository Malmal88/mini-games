using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace XO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[,] board = new int[3, 3];
        int cellsize = 190;
        int gap = 25;
        Pen pen = new Pen(Color.Black, 5);
        Random rand = new Random();
        bool rowx()
        {
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, j] != 1) return false;
            }
            return true;
        }
        bool row0()
        {
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, j] != 1) return false;
            }
            return true;
        }

        bool free()
        {
           
            for (int i = 0; i < board.GetLength(0);i++)
            {
                for (int j = 0; j < board.GetLength(1);j++)
                {
                    if (board[i, j] == 0) return true;                    
                }
            }
            return false; 
        }
        void zeroturn()
        {
            int px, py;
           
            do
            {
                px = rand.Next(3);
                py = rand.Next(3);
            }
            while (board[px, py] != 0);                       
            board[px, py] = 2;                      
                                             
                       
        }
        
        
    void field(Graphics gr)
           
        {
            for (int i = cellsize; i <= cellsize*2; i += cellsize)
            {
                gr.DrawLine(pen, gap, i,pictureBox1.Width- gap, i);
                gr.DrawLine(pen, i, gap, i, pictureBox1.Height - gap);
            }
        }
    void zero(Graphics gr, int row, int column)
        {
            var cx = row * cellsize;
            var cy = column * cellsize;
            gr.DrawEllipse(pen,cx+gap/2,cy+gap/2,cellsize-gap,cellsize-gap );
        }

    void cross(Graphics gr, int row, int column)
        {
            var cx=row*cellsize;
            var cy=column*cellsize;
            gr.DrawLine(pen,cx+gap, cy+gap, cx+cellsize-gap,cy+cellsize-gap);
            gr.DrawLine(pen, cx + cellsize - gap , cy + gap , cx + gap, cy + cellsize - gap );
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.Clear(Color.White);
            field(gr);
            int x = board.GetLength(0);
            int y = board.GetLength(1);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    switch (board[i, j])
                    {

                        case 1:
                            cross(gr, i, j); break;
                        case 2:
                            zero(gr, i, j); break;

                    }                   
                }
            }                         
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var pos= pictureBox1.PointToClient(Cursor.Position);
            var cx = pos.X / cellsize;
            var cy = pos.Y / cellsize;
         
                if (e.Button == MouseButtons.Left)
                {
              
                  {
                    if (board[cx, cy] == 0)
                    {
                        board[cx, cy] = 1;
                        if (free())
                        {
                            zeroturn();
                        }
                        
                    }    
                              
                  }
                
                }   
                
        }
    }
}
