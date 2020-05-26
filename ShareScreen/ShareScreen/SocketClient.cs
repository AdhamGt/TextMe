using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareScreen
{
    public class SocketClient
    {
        IPHostEntry host;
        IPAddress ipAddress;
        IPEndPoint remoteEP;
      public  Socket sender;
        byte[] bytes;
      public byte[] ApplyConnection(string Username,string Password, int Mode, int privacy)
        {
           string messege = Username + "," + Password + "," + Mode + "," + privacy;
            byte[] mes = Encoding.ASCII.GetBytes(messege);


            SendMessage(mes);
          return RecieveMessage();
        }
        public byte[] ApplyConnection(string Username, string Password, int Mode)
        {
            string messege = Username + "," + Password + "," + Mode;
            byte[] mes = Encoding.ASCII.GetBytes(messege);


            SendMessage(mes);
            return RecieveMessage();
        }
        public byte[] ApplyConnection(string Username, string Password,string email)
        {
            try
            {
                bytes = new byte[90000000];
                host = Dns.GetHostEntry("ec2-3-83-80-93.compute-1.amazonaws.com");
                ipAddress = host.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, 4555);
                sender = new Socket(ipAddress.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
                string messege = Username + "," + Password+","+email;
                byte[] mes = Encoding.ASCII.GetBytes(messege);
                sender.Connect(remoteEP);
                SendMessage(mes);
            }
            catch
            {
                MessageBox.Show("Connection Timeout");
                return null;
            }

            return RecieveMessage();
        }
        public byte[] ApplyConnection(string Username, string Password)
        {
            try
            {
                //
                bytes = new byte[90000000];
                host = Dns.GetHostEntry("ec2-3-83-80-93.compute-1.amazonaws.com");
                ipAddress = host.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, 4555);
                sender = new Socket(ipAddress.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
                string messege = Username + "," + Password;
                byte[] mes = Encoding.ASCII.GetBytes(messege);
                sender.Connect(remoteEP);
                SendMessage(mes);
            }
            catch
            {
                MessageBox.Show("Connection Timeout");
                return null;
            }
         
            return RecieveMessage();
        }
        public byte[] ApplyConnection(string Username, string Password, int Mode,int RoomID , int Roompassword)
        {
            string messege = Username + "," + Password + "," + Mode + "," + RoomID + "," + Roompassword;
            byte[] mes = Encoding.ASCII.GetBytes(messege);
            SendMessage(mes);
            
           return RecieveMessage();
        }
        public byte[] RecieveMessage()
        {
            while (true)
            {
                if (sender.Available > 0)
                {
                    int bytesRec = sender.Receive(bytes);
                    if (bytesRec > 0)
                    {
                        byte[] nbt = bytes.ToList().GetRange(0, bytesRec).ToArray();
                        string res = Encoding.ASCII.GetString(bytes);
                        if (res != "")
                        {
                            return nbt;
                        }
                    }
                    else
                    {
                        MessageBox.Show("no result");
                        return null;
                    }
                }
            }
           
        }
    
        public void CloseConnection()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
             
        }
     
        public  void SendMessage(byte[] byts)
        {
         
            int bytesSent = sender.Send(byts);
           
       
        }
        public byte[] ImageToByteArray(System.Drawing.Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, System.Drawing.Imaging.ImageFormat.Bmp);
                return mStream.ToArray();
            }
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
