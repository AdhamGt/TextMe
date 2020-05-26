using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareScreen
{
   public class ClientHandler
    {    Thread CaptureThread;
       private delegate void UpdateGUIdel(RichTextBox t);
        private delegate void UpdateGUIdel2(Label t);
        private delegate void UpdateGUIdel3(WatchStream t);
        PictureBox p;
        byte[] img;
        volatile bool isSent;
        volatile string SentMessage="";
        public static volatile bool isRecieving;
        volatile Bitmap UpdatedScreen;
        Label infol;
        Dashboard_ d;
        WatchStream w;
        RichTextBox textb;
        int roomid, roompass;
        public static volatile bool sending = false;
       volatile string Message = "";
        SocketClient Socket = new SocketClient();
        public volatile bool isCapturing = false;
       public volatile Bitmap Screen;
        Thread Recieving;
        Thread Sending;
        public void EndRecieving()
        {
           // Socket.CloseConnection();
        }
       public void Login(string Username , string Password,MainMenu m)
        {
            byte[] bytes = Socket.ApplyConnection(Username,Password);
            if (bytes != null)
            {
                string info = Encoding.ASCII.GetString(bytes);
                if (info == "0")
                {
                    MessageBox.Show("Incorrect Username or Password");
                }
                else if (info == "1")
                {
                    Dashboard_ d = new Dashboard_();
                    d.Show();
                    m.Hide();
                    MessageBox.Show("Successfully Logged In");
                }
                else if ( info == "2")
                {
                    Dashboard_ d = new Dashboard_();
                    d.Show();
                    m.Hide();

                    MessageBox.Show("User Already Logged in");
                }
            }
        }
        public void Register(string Username, string Password,string email ,  Register m,MainMenu m2)
        {
            byte[] bytes = Socket.ApplyConnection(Username, Password,email);
            if (bytes != null)
            {
                string info = Encoding.ASCII.GetString(bytes);
                if (info == "0")
                {
                  
                    MessageBox.Show("Account Already Exists");
                    Socket.sender.Close();
                }
                else if (info == "1")
                {

                    m.Close();
                    m2.Show();
                    MessageBox.Show("Successfully Registered");
                    Socket.sender.Close();
                }
            }
        }
        void filterMessage()
        {
           if(Message.Length > 2)
            {
                string header = "";
                header += Message[0];
                header += Message[1];
                if (Message == "bye")
                {

                    isCapturing = false;
                    sending = false;
                    Message = "";
                    Sending.Abort();
                    Recieving.Abort();



                }
                else
                {
             
                    if (header == "t/")
                    {
                        Message = Message.Substring(2);
                        UpdateUI(textb);
                    }
                    else
                    {
                        Message = "";
                    }
                }
            }
            
        }
        void RecieveText()
        {
            while (isCapturing)
            {
                isRecieving = true;
               byte[] bytes= Socket.RecieveMessage();
                isRecieving = false;
                Message = Encoding.ASCII.GetString(bytes);
                if(Message != "")
                {
                    if (Message != "h/")
                    {
                        filterMessage();
                       
                    }
                    Message = "";
                }

                Thread.Sleep(100);
            }

            }
        public void JoinPublicRoom(ref RichTextBox t, int mode, string username, string password,ref WatchStream w)
        {
            textb = t;
            this.w = w;
        
            byte[] bytes = Socket.ApplyConnection(username, password,mode);
     
            string info = Encoding.ASCII.GetString(bytes);
            if (info == "1")
            {
                isCapturing = true;
                w.Show();
                StartRecieving(ref t);
         
                    
                MessageBox.Show("Successfully Entered Room");
                Sending = new Thread(SendText);
                Sending.Start();
                sending = false;
            }
            else
            {
                w.Close();
                w.d.Show();
              
                MessageBox.Show("Cannot Find Public Room right now, try again later");
            }
         
        }
        public void StartRoom(ref RichTextBox t, int mode, string username, string password, int rid, int rpassword,ref Label infol,int privacy)
        {
           textb = t;
            this.infol = infol;
            isCapturing = true;
   
    
            if (mode == 1)
            {
                byte[] bytes = Socket.ApplyConnection(username, password, 1, rid, rpassword);
                StartRecieving(ref t);
                string info = Encoding.ASCII.GetString(bytes);

            }
            else
            {
                byte[] roomInfo = Socket.ApplyConnection(username, password, 0,privacy);
                string info = Encoding.ASCII.GetString(roomInfo);
                string[] data = info.Split(',');
                StartRecieving(ref t);
                roomid = int.Parse(data[0]);
                roompass = int.Parse(data[1]);
                UpdateUI(infol);
            }
             Sending = new Thread(SendText);
            Sending.Start();
            sending = false;
        }
        public   void EnterRoom(ref RichTextBox t,int mode, string username, string password, int rid, int rpassword,ref WatchStream w, int privacy)
        {
     
            textb = t;
 
            this.w = w;
            if (mode == 1)
            {
             byte[] bytes =   Socket.ApplyConnection(username, password, 1, rid, rpassword);
                 string info = Encoding.ASCII.GetString(bytes);
                if(info == "1")
                {
                    w.Show();
                    StartRecieving(ref t);
                    isCapturing = true;
                    MessageBox.Show("Successfully Entered Room");
                    Sending = new Thread(SendText);
                    Sending.Start();
                    sending = false;
                }
                else
                {
                    w.Close();
                    w.d.Show();
                    isCapturing = false;
                    MessageBox.Show("Incorrect Room ID or Password");
                }

            }
            else
            {
                byte[] roomInfo = Socket.ApplyConnection(username, password, 0,privacy);
                string info = Encoding.ASCII.GetString(roomInfo);
                string[] data = info.Split(',');
                StartRecieving(ref t);
            }
     
        }
        public void StartRecieving(ref RichTextBox t)
        {
       
            textb = t;
        
            isCapturing = true;
      
            Recieving = new Thread(RecieveText);
            Recieving.Start();
     
        }
        public void EndSession()
        {
            Send("e/");
          while(sending)
            {
                Thread.Sleep(100);
            }
        
  
        }
        public void Send(string text)
        {
            SentMessage = text;
            sending = true;
          
        }
   void SendText()
        {
            while (isCapturing)
            {
                if (sending)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(SentMessage);
                    Socket.SendMessage(bytes);
                    sending = false;
                    Thread.Sleep(100);
                    UpdateUI(textb);
                }
            }
        }
    
        void UpdateUI(RichTextBox t)
        {
            if (t.InvokeRequired)
            {
                var d = new UpdateGUIdel(UpdateUI);
                t.Invoke(d, new object[] { t });
            }
            else
            {
                string newText = Message + Environment.NewLine;
             
                t.Text += newText;
                string txt1 = t.Text;

                var pattern = "(\\n){2,}";
                var patt1 = "t/";
                var replacePattern = "\n";
            

                t.Text = Regex.Replace(txt1, pattern, replacePattern, RegexOptions.IgnoreCase);

                string NewString = t.Text;
                t.Text = Regex.Replace(NewString, patt1, replacePattern, RegexOptions.IgnoreCase);

                Message = "";
            }
        }
        void UpdateUI(Label t)
        {
            if (t.InvokeRequired)
            {
                var d = new UpdateGUIdel2(UpdateUI);
                t.Invoke(d, new object[] { t });
            }
            else
            {
                t.Text = "Room ID : " + roomid +"\n"+ "Room Pass : " + roompass;

            }
        }
        void UpdateUI(WatchStream t)
        {
            if (t.InvokeRequired)
            {
                var d = new UpdateGUIdel3(UpdateUI);
                t.Invoke(d, new object[] { t });
            }
            else
            {
                t.d.Show();
                t.Close();

            }
        }

    }
}
