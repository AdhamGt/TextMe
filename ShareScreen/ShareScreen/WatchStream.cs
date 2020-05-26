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
    public partial class WatchStream : Form
    {
     
       public Dashboard_ d;
        public WatchStream(int RID , int Password , Dashboard_ d,int Mode)
        {
            InitializeComponent();
            this.d = d;
            WatchStream Ref = this;
            FormClosing += w_FormClosing;
            if (Mode == 0)
            {
              
                    Program.sr.EnterRoom(ref richTextBox1, 1, Program.User.SharesUsername, Program.User.SharesPassword, RID, Password, ref Ref,0);

               
            }
            else
            {
                      Program.sr.JoinPublicRoom(ref richTextBox1,2, Program.User.SharesUsername, Program.User.SharesPassword, ref Ref);

            }
        }

        private void w_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Program.sr.isCapturing)
            {
                
                    e.Cancel = true;
                    this.Hide();
                
            }
       
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void WatchStream_Load(object sender, EventArgs e)
        {
        

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string x = textBox1.Text;
            x.Replace(" ", "");
            if (!ClientHandler.sending &&  x != "")
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

        private void button2_Click(object sender, EventArgs e)
        {
            d.Show();
            Program.sr.EndSession();
            notifyIcon1.Visible = false;
            this.d = null;
            this.Close();

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
