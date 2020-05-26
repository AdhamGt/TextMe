using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareScreen
{
    public partial class ConnectToUser : Form
    {
        Dashboard_ d;
        public ConnectToUser(Dashboard_ d)
        {
            this.d = d;
            InitializeComponent();
            FormClosing += w_FormClosing;
          
        }

        private void w_FormClosing(object sender, FormClosingEventArgs e)
        {
            d.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool ApplicableUsername = Validation.ValidatePinConstrains(textBox1.Text);
            bool ApplicablePassword = Validation.ValidatePinConstrains(textBox2.Text);
            if (ApplicableUsername)
            {
                if (ApplicablePassword)
                {
                    this.Hide();
                    WatchStream w = new WatchStream(int.Parse(textBox1.Text), int.Parse(textBox2.Text), d,0);
                }
                else
                {
                    MessageBox.Show("Incorrect Room Password Format");
                }
            }
            else
            {
                if (ApplicablePassword)
                {
                    MessageBox.Show("Incorrect Room ID Format");
                }
                else
                {
                    MessageBox.Show("Incorrect Username & Password Format");
                }
            }
          
         
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConnectToUser_Load(object sender, EventArgs e)
        {

        }
    }
}
