using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cards
{
    public partial class Form1 : Form
    { 
        int _index;
        bool _clicflag; // флаг для перетаскивания карты
        Point bclic;
        Point eclic;
        Point _pointdraw;

        List<Point> _pointlist;
        List<Matrix> _matrixlist;
        Matrix m1;
        int n = 10;
        int m = 10;
        
        public Form1()
        {
            InitializeComponent();
            SetDoubleBuffered(pictureBox1); //установка двойной буферизации
            _matrixlist = new List<Matrix>(); //
            this.LoadImage();
            _pointlist = new List<Point>();
            _clicflag = false;
            _pointdraw = new Point(100, 100);
            randomsort();

        }

        public void LoadImage()
        {
            DirectoryInfo directory = new DirectoryInfo("Cards");
            Image texture;
            for (int i = 0; i < 10;i++)
            {
                texture = Image.FromFile("Cards\\Трафарет.png");
                imageList2.Images.Add(texture);
            }

                foreach (FileInfo fi in directory.GetFiles())
                {
                    if (fi.Extension == ".png")
                    {
                        texture = Image.FromFile(fi.FullName);
                        if (fi.Name != "Трафарет.png")
                            imageList1.Images.Add(fi.Name, texture);
                    }
                }
            _index = imageList1.Images.Count - 1;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
            int x = this.Size.Width;
            int y = this.Size.Height;
            LinearGradientBrush gradBrush = new LinearGradientBrush(new Rectangle(0, 0, x, y), Color.DarkGreen,
                Color.LightGreen, LinearGradientMode.BackwardDiagonal);
            e.Graphics.FillRectangle(gradBrush, 0, 0, x, y);
            int k = 750;
            int k1 = 20;
            bclic = new Point(k, k1);
            if (_index < 9 && !_clicflag)
            {
                m--;
            }
            else 
                n--;
            if (!_clicflag)
                n = m;
            for(int i=0;i<n;i++)
            {
                e.Graphics.DrawImage(imageList2.Images[i], new Point(k, k1));
                k += 2;
                k1 += 2;
            }
            n = m;
            eclic = new Point(k + imageList2.ImageSize.Width, k1 + imageList2.ImageSize.Height);
            for (int i = 0; i < imageList1.Images.Count - 1 - _index; i++)
            {
                e.Graphics.Transform = _matrixlist[i];
                e.Graphics.DrawImage(imageList1.Images[imageList1.Images.Count - 1 - i], _pointlist[i]);
                e.Graphics.ResetTransform();
            }
            if(_clicflag)
            {

                e.Graphics.DrawImage(imageList1.Images[_index], _pointdraw);
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (_index != -1)
            {
                if (e.X > bclic.X && e.Y > bclic.Y && e.X < eclic.X && e.Y < eclic.Y)
                {
                    _clicflag = true;
                    _pointdraw = new Point(e.Location.X - imageList1.Images[_index].Size.Width / 2,
                    e.Location.Y - imageList1.Images[_index].Size.Height / 2);
                    pictureBox1.Invalidate();
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(_clicflag)
            {
                _pointdraw = new Point(e.Location.X - imageList1.Images[_index].Size.Width / 2,
                    e.Location.Y - imageList1.Images[_index].Size.Height / 2);
                pictureBox1.Invalidate();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_clicflag)
            {
                _index--;
                _clicflag = false;
                Random rand = new Random();
                int x1 = rand.Next(181);
                m1 = new Matrix();
                m1.RotateAt(x1, e.Location);
                _matrixlist.Add(m1);
                _pointlist.Add(_pointdraw);
                pictureBox1.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                _index = imageList1.Images.Count - 1;
                n = 10; 
                m = 10;
                _pointlist.Clear();
                _matrixlist.Clear();
                pictureBox1.Invalidate();
            }
        }
        private void randomsort()
        {
            Random x = new Random();
            int k;
            Image im;
            
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < imageList1.Images.Count; j++)
                {
                    im = imageList1.Images[j];
                    k = x.Next(imageList1.Images.Count);
                    imageList1.Images[j] = imageList1.Images[k];
                    imageList1.Images[k] = im;
                }
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
    }
}
