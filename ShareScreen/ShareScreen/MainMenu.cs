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
  
    public partial class MainMenu : Form
    {
        ShareAccount c = new ShareAccount();
        ClientHandler sr = new ClientHandler();

        public MainMenu()
        {
            InitializeComponent();
         
        }

        private void passwordField_TextChanged(object sender, EventArgs e)
        {

        }

        private void usernameField_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register r = new Register(this);
            r.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
          bool ApplicableUsername =  Validation.ValidateUsernameConstrains(usernameField.Text);
            bool ApplicablePassword = Validation.ValidatePassswordConstrains(passwordField.Text);
            if(ApplicableUsername)
            {
                if (ApplicablePassword)
                {
                
                    Program.User.SharesUsername = usernameField.Text;
                    Program.User.SharesPassword = passwordField.Text;
                    Program.sr.Login(Program.User.SharesUsername, Program.User.SharesPassword,this);
                }
                else
                {
                    MessageBox.Show("Incorrect Password Format");
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
    }
}
