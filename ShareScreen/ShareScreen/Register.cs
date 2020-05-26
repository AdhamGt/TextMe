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
    public partial class Register : Form
    {
        public MainMenu m;
        public Register(MainMenu m)
        {
            InitializeComponent();
            this.m = m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ApplicableUsername = Validation.ValidateUsernameConstrains(usernameField.Text);
            bool ApplicablePassword = Validation.ValidatePassswordConstrains(passwordField.Text);
            bool ApplicablePassword2 = Validation.ValidatePassswordConstrains(textBox1.Text);
            if (ApplicableUsername)
            {
                if (ApplicablePassword && ApplicablePassword2 && passwordField.Text == textBox1.Text)
                {

                    Program.User.SharesUsername = usernameField.Text;
                    Program.User.SharesPassword = passwordField.Text;
                    Program.sr.Register(Program.User.SharesUsername, Program.User.SharesPassword, textBox2.Text,this,m);
                }
                else if (passwordField.Text == textBox1.Text)
                {
                    MessageBox.Show("Incorrect Password Format");
                }
                else
                {
                    MessageBox.Show("Password And Confirm Password Don't Match");
                }
            }
            else
            {
                if (ApplicablePassword)
                {
                    MessageBox.Show("Incorrect Username Format");
                }
                else
                {
                    MessageBox.Show("Incorrect Username & Password Format");
                }
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
