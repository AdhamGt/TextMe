/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.EOFException;
import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Adham
 */
public class Room implements Runnable {
     public   int RoomID;
     public  int RoomPassword;
     public boolean isPrivate;
     public volatile ArrayList<User> Listeners = new ArrayList<User>();
    public  volatile User Owner = new  User();
    public  volatile ArrayList< User>All = new  ArrayList< User>();
       volatile boolean Alive =true;
    @Override
    public void run() {
   
        while(Alive && CloudServer.serverAlive)
        {
         
               Listen();
               try {
            Thread.sleep(100);
            } catch (InterruptedException ex) {
                Logger.getLogger(Room.class.getName()).log(Level.SEVERE, null, ex);
            }
            
        
        }
       System.out.println("Room "+RoomID+" Meeting Ended");
    CloudServer.rooms.remove(this);
    if(!isPrivate)
    {
    CloudServer.public_rooms.remove(this);
  
    }   
        }
    
    void Listen() 
    {
        
      for(int i = 0 ; i  < All.size();i++)
      {
          if(All.get(i).status == UserStatus.inRoom)
          {
          byte[]bytes;
          try {
              bytes = new byte[ All.get(i).Input.available()];
                 All.get(i).Input.readFully(bytes);
         
          String data =  new String(bytes);
           if(!data.isEmpty())
       {
               if(data.length() >= 2 )
               {
                   String header = "";
                   header=data.substring(0, 2); 
                   data = data.substring(2);
                   switch (header)
                   {
                       case("t/"):
                           SendText(All.get(i),data);
                           break;
                       case ("e/"):
                             All.get(i).status = UserStatus.Offline;
                           
                
                           break;
                   
               }
         
              
       
               }   
       }
                      Check_Connection();
          } catch (IOException ex) {
               Logger.getLogger(Room.class.getName()).log(Level.SEVERE, null, ex);
          }
       
          
      }
      
      }  
    }
   
    void Check_Connection() throws IOException 
    {
        for(int i =0 ; i < All.size() ;i++)
        {
              if(All.get(i).status.equals(UserStatus.Offline))
              {
                 
              byte[]bytes;
                String UserDis = All.get(i).Username;
   
                     bytes = new byte[ All.get(i).Input.available()];
                 All.get(i).Input.readFully(bytes);
                  if(All.get(i).Input.available() == 0)
                  {
                All.get(i).status = UserStatus.Online;
                 if(Listeners.contains(All.get(i)))
                {
             Listeners.remove(All.get(i));
                }
                 BroadCastAllListners("t/"+UserDis + " Has Disconnected "); 
                      System.out.println(" User Disconnected");
                
                 String bye = "bye";
               
                 All.get(i).Output.write(bye.getBytes());
                 All.remove(All.get(i));
                
           
                     i--;
                  }
              }
            
            }
          if(All.isEmpty())
          {
               System.out.println("Room Closed");
               Alive = false;
          }
        
            
              
    }
    public void Attach_User(User user)
    {
         try {
             user.Input = new DataInputStream(user.socket.getInputStream());
        
        user.Output = new DataOutputStream(user.socket.getOutputStream());
        All.add(user);
        System.out.print("User Added");
   } catch (IOException ex) {
             Logger.getLogger(Room.class.getName()).log(Level.SEVERE, null, ex);
         }
    }
     void BroadCastListners(byte[]Message) throws IOException
        {
         
            for(int i = 0 ; i < Listeners.size() ;i++)
            {
                try {
                    
                    Listeners.get(i).Output.write(Message);
                } catch (IOException ex) {
           
                        Listeners.get(i).Output.write(Message);
                    
                }
                
            }
 }
          void BroadCastListners(User u, byte[]Message) throws IOException
        {
           
           
           // System.out.println("BroadCasting to All Users");
            for(int i = 0 ; i < All.size() ;i++)
            {
                if(All.get(i) != u)
                {
           
                    try {
                        All.get(i).Output.write(Message);
                    } catch (IOException ex) {
            
            All.get(i).Output.write(Message);
                      
                      }
                    }
      
                }
            
        }
             void BroadCastAllListners(String Message) throws IOException 
        {
              byte[] b = Message.getBytes();
            System.out.println("BroadCasting to All Users");
            for(int i = 0 ; i < All.size() ;i++)
            {
           
                  try {
                      All.get(i).Output.write(b);
                  } catch (IOException ex) {
                   
                         All.get(i).Output.write(b);
                      }
                  }
      
                
            
        }
             void SendText(User u ,String data) throws IOException
             {
                   if(u == Owner)
           {
           
                 String message ="t/"+ u.Username + " : "+data;
               byte[] b = message.getBytes();
               System.out.println(u.Username+" : "+data);
               BroadCastListners(b);
               }
           
           else
           {
       
               String message ="t/"+ u.Username + " : "+data;
               
             byte[] b = message.getBytes();
                 System.out.println(u.Username+" : "+data);
               BroadCastListners(u,b);
               }
             }
           
}
