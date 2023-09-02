using Microsoft.VisualBasic.Logging;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace Jumping_Cat_Pet
{
    public partial class Form1 : Form
    {
        bool mouse = false;
        bool gravity = true;
        Point mouseOffset = new Point(0, 0);
        PointF velocity = new Point(0, 0);
        PointF position = new Point(0, 0);
        Rectangle resolution;
        float gravityForce = 25;
        PointF resistance = new PointF(300, 300);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.TopMost = true;
            resolution = Screen.PrimaryScreen.WorkingArea;
            this.Left = resolution.Size.Width - 160;
            this.Top = resolution.Size.Height - 160;
            position = new PointF(resolution.Size.Width - 160, resolution.Size.Height - 160);
            Thread cat = new Thread(Cat);
            cat.Start();
        }

        private void Cat()
        {
            while (true)
            {
                if (mouse)
                {
                    velocity.X += (System.Windows.Forms.Cursor.Position.X - this.Left - 80);
                    velocity.Y += (System.Windows.Forms.Cursor.Position.Y - this.Top - 130);
                }
                position.X += velocity.X / 2000;
                position.Y += velocity.Y / 2000;
                velocity.X += -(velocity.X / resistance.X);
                velocity.Y += -(velocity.Y / resistance.Y);
                this.Invoke((MethodInvoker)delegate
                {
                    this.Left = (int)Math.Round(position.X);
                    this.Top = (int)Math.Round(position.Y);
                    // Streching
                    this.Width = (int)Math.Round(Math.Abs(velocity.X) / 1000 + 160);
                    this.Height = (int)Math.Round(Math.Abs(velocity.Y) / 1000 + 160);
                });
                if (position.Y + this.Height > resolution.Size.Height)
                    gravity = false;
                else
                    gravity = true;
                // Gravity
                if (gravity)
                {
                    velocity.Y += gravityForce;
                }
                // Collisions
                if (position.X < 0 || position.X + this.Width > resolution.Size.Width)
                {
                    velocity.X = -velocity.X;
                }
                if (position.Y < 0 || position.Y + this.Height > resolution.Size.Height)
                {
                    velocity.Y = -velocity.Y;
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = true;
            gravity = false;
            mouseOffset = new Point(System.Windows.Forms.Cursor.Position.X - this.Left, System.Windows.Forms.Cursor.Position.Y - this.Top);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse = false;
            gravity = true;
        }
    }
}