using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareScreen
{
    public partial class Stream : Form
    {
        Dashboard_ d;
       
        public Stream(Dashboard_ d)
        {
            this.d = d;
            InitializeComponent();
            FormClosing += Stream_FormClosing;


        }

        private void Stream_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.sr.isCapturing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void Stream_Load(object sender, EventArgs e)
        {
            if (d.checkBox1.Checked)
            {
                Program.sr.StartRoom(ref richTextBox1, 0, Program.User.SharesUsername, Program.User.SharesPassword, 0, 0, ref label1,1);
            }
            else
            {
                Program.sr.StartRoom(ref richTextBox1, 0, Program.User.SharesUsername, Program.User.SharesPassword, 0, 0, ref label1,0);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            d.Show();
            notifyIcon1.Visible = false;
            Program.sr.EndSession();
            d = null;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ClientHandler.sending && textBox1.Text != "")
            {
                Program.sr.Send("t/" + textBox1.Text);

                richTextBox1.Text += "You : " + textBox1.Text + "\n";
                string txt1 = richTextBox1.Text;

                var pattern = "(\\n){2,}";
                var replacePattern = "\n";

                richTextBox1.Text = Regex.Replace(txt1, pattern, replacePattern, RegexOptions.IgnoreCase);
                textBox1.Text = "";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
