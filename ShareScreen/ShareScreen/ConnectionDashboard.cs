using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareScreen
{
    public partial class Dashboard_ : Form
    {
        Color Orgin;
        public Dashboard_()
        {
           
            InitializeComponent();
           Bitmap b =  (Bitmap)pictureBox1.Image;
            b.MakeTransparent(b.GetPixel(0, 0));
            pictureBox1.Image = b;
         b = (Bitmap)pictureBox2.Image;
            b.MakeTransparent(b.GetPixel(0, 0));
            pictureBox2.Image = b;
            FormClosing += Dashboard__FormClosing;
            label1.Text = "Hello " + Program.User.SharesUsername;
            Orgin = checkBox1.BackColor; 
        }

        private void Dashboard__FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;  
            Bitmap img = (Bitmap)p.Image;

            p.Image = ReplaceColor(img, img.GetPixel(0, 0), Color.CadetBlue);

        }
        private  Bitmap ReplaceColor(Bitmap bmp, Color oldColor, Color newColor)
        {
            var lockedBitmap = new Bitmap(bmp);
     

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    if (lockedBitmap.GetPixel(x, y) == oldColor || lockedBitmap.GetPixel(x, y) == Color.White)
                    {
                        lockedBitmap.SetPixel(x, y, newColor);
                    }
                }
            }
            return lockedBitmap;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            Bitmap img = (Bitmap)p.Image;
        
            img.MakeTransparent(img.GetPixel(0, 0));
            p.Image = img;
            var image = ScreenCapture.CaptureDesktop();
  

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!Program.sr.isCapturing)
            {
                notifyIcon1.Visible = false;
                ConnectToUser c = new ConnectToUser(this);
                this.Hide();

                c.Show();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!Program.sr.isCapturing)
            {
                Stream st = new Stream(this);
                this.Hide();
                notifyIcon1.Visible = false;
                st.Show();
            }
        }

        private void Dashboard__Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Program.sr.isCapturing)
            {
                this.Hide();
                WatchStream w = new WatchStream(0, 0, this, 2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if((cb.Checked))
                {

                cb.BackColor = Color.Green;
            }
            else
            {
                cb.BackColor = Orgin;   
            }
        }
    }
}
