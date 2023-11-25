using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piu_Piu
{

    public partial class Form1 : Form
    {
       
        public class Object
        {
            public static Random r = new Random();
            public float x;
            public float y;
            public float speed;
            public float size;
            public float speedX = -10;
            public float speedY = r.Next(-5, 5);
        }
        public class star : Object
        {

            public void update(int Width)
            {
                if (x > 0) { x -= speed; }
                else x = Width;
            }
            public void Init(int width, int height)
            {
                x = r.Next(width);
                y = r.Next(height);
                speed = r.Next(0, 300) / 10f;
                size = r.Next(1, 4);
            }
        }

        public class Aim : Object
        {
            public void InitA(int width, int height)
            {
                x = width;
                y = r.Next(height - 100);
                size = r.Next(50, 70);
                speed = r.Next(1, 5);
            }

            public void UpdateA(int width)
            {
                Random r = new Random();
                x -= speed;
                if (x < 0) { x = width; }
            }
        }

        public class Piu : Object // пуля
        {
            public void InitP(float X, float Y)
            {
                x = X; //Pos.X
                y = Y; //Pos.Y
                size = 3;
                speed = 40;
            }

            public void Updadte()
            {
                x += speed;
            }

            public void UpdatePA()
            {
                x += speedX;
                y += speedY;
            }
        }

        void Init()
        {
            for (int i = 0; i < N; i++)
            {
                stars[i] = new star();
                stars[i].Init(pictureBox1.Width, pictureBox1.Height);
            }
            for (int i = 0; i < n; i++)
            {
                aims[i] = new Aim();
                Aims.Add(aims[i]);
            }
            foreach (var s in Aims)
            {
                s.InitA(pictureBox1.Width, pictureBox1.Height);
            }
        }

        public Form1()
        {
            InitializeComponent();
            Init();
        }
        static DateTime lastFire = DateTime.Now;
        static int n = 10;//количество целей
        Aim[] aims = new Aim[n];
        static List<Aim> Aims = new List<Aim>();
        List<Piu> pius = new List<Piu>();
        static List<Piu> piusA = new List<Piu>();
        const int N = 400;//количество звезд 
        star[] stars = new star[N];
        static int Counter = 3;        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.Clear(Color.Black);
            for (int i = 0; i < N; i++)
            {
                gr.FillEllipse(Brushes.Silver, stars[i].x, stars[i].y, stars[i].size, stars[i].size);
                stars[i].update(pictureBox1.Width);
            }
            var pos = pictureBox1.PointToClient(Cursor.Position);
            gr.FillEllipse(Brushes.LightYellow, pos.X, pos.Y, 50, 20);

            foreach (var s in Aims)
            {
                gr.FillEllipse(Brushes.Gold, s.x, s.y, s.size, s.size);
                s.UpdateA(pictureBox1.Width);
            }
            Aim aimA = Aims[Object.r.Next(Aims.Count)];

            if ((DateTime.Now - lastFire).TotalSeconds >= 1 / 1.8)
            {
                Piu piu = new Piu();
                piu.InitP(aimA.x, aimA.y);
                piusA.Add(piu);
                lastFire = DateTime.Now;
            }

            foreach (var s in piusA)
            {
                gr.FillEllipse(Brushes.Red, s.x, s.y, 10, 10);
                s.UpdatePA();
            }

            foreach (var s in pius)
            {
                gr.FillEllipse(Brushes.Red, s.x, s.y, 30, s.size);
                s.Updadte();
            }

            foreach (var s in pius)
            {
                for (int i = 0; i < Aims.Count; i++)
                {
                    if (s.x < Aims[i].x + Aims[i].size && s.x > Aims[i].x && s.y < Aims[i].y + Aims[i].size && s.y > Aims[i].y)
                    {
                        gr.FillEllipse(Brushes.Red, Aims[i].x - Aims[i].size / 2, Aims[i].y, Aims[i].size * 2, Aims[i].size);
                        Aims.Remove(Aims[i]); pius.Remove(s);
                        Aim aim = new Aim();
                        aim.InitA(pictureBox1.Width, pictureBox1.Height);
                        Aims.Add(aim);
                        return;
                    }
                }
            }

            for (int i = 0; i < piusA.Count; i++)
            {
                if (piusA[i].x + 5 < pos.X + 50 && piusA[i].x + 5 > pos.X && piusA[i].y + 5 < pos.Y + 20 && piusA[i].y + 5 > pos.Y)
                {
                    gr.FillRectangle(Brushes.Red, 0, 0, pictureBox1.Width, pictureBox1.Height);
                    Counter--;
                    piusA.Remove(piusA[i]);
                    return;
                }
            }

            for (int i = 0; i < Counter; i++)
            {
                gr.FillEllipse(Brushes.LightYellow, i * 75 + 25, 20, 50, 20);
            }

            if (Counter <= 0)
            {
                var bmp = Bitmap.FromFile("gover.Png");
                gr.DrawImage(bmp, 400, 200);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < N; i++)
            {
                stars[i] = new star();
                stars[i].Init(pictureBox1.Width, pictureBox1.Height);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var pos = pictureBox1.PointToClient(Cursor.Position);
            Piu piu = new Piu();
            piu.InitP(pos.X, pos.Y);
            pius.Add(piu);
        }
    }
}
