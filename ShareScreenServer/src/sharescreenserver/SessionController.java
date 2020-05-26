/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;
import static sharescreenserver.CloudServer.RoomCodeIndex;
import static sharescreenserver.CloudServer.rooms;
import static sharescreenserver.CloudServer.socket;

/**
 *
 * @author Adham
 */
public class SessionController implements Runnable {

    public SessionController() {

    }

    @Override
    public void run() {

        while (CloudServer.serverAlive) {
            MatchRequests();
         try  {
                Thread.sleep(10);
            } catch (InterruptedException ex) {
                Logger.getLogger(SessionController.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }

    void MatchRequests()  {
        for (int i = 0; i < CloudServer.users.size(); i++) {

            
            if (CloudServer.users.get(i).getStatus().equals(UserStatus.Online)) {
                byte[] bytes;
               try
               {
                DataInputStream dis = new DataInputStream(CloudServer.users.get(i).socket.getInputStream());
                CloudServer.users.get(i).socket.setKeepAlive(true);
                if(dis.available() > 0)
                {bytes = new byte[dis.available()];
                dis.readFully(bytes);
                String data = new String(bytes);
                String[] Data = data.split(",");
                System.out.println("Data at lenght"+Data.length+data+ CloudServer.users.get(i).Username + CloudServer.users.get(i).status.toString());
                if(Data.length > 3 && Data[2].equals("0") )
                {
                     CloudServer.users.get(i).status = UserStatus.inRoom;
                    Room r = new Room();
                    r.Owner = CloudServer.users.get(i);
                    r.Attach_User(CloudServer.users.get(i));
                    r.RoomID = RoomCodeIndex;
                    int pass = CloudServer.passGen.nextInt() * 10;
                    if (pass < 0) {
                        pass *= -1;
                    }
                    if(Data[3].equals("0"))
                    {
                       r.isPrivate = false;
                      CloudServer.public_rooms.add(r);
                        System.out.println("Creating Public Rooom");
                    }
                    else
                    {
                            r.isPrivate = true;
                    CloudServer.private_rooms.add(r);
                        System.out.println("Creating Private Rooom");
                    }
                    
                    r.RoomPassword = pass;
                   
                    CloudServer.rooms.add(r);
                    String RoomInfo = r.RoomID + "," + r.RoomPassword;
                    System.out.println(RoomInfo);
                    byte[] b = RoomInfo.getBytes();    
                    Thread rt = new Thread(r);
                    CloudServer.roomThreads.add(rt);
                    rt.start();
                    DataOutputStream out = new DataOutputStream(CloudServer.users.get(i).socket.getOutputStream());
                    out.write(b);
                    RoomCodeIndex++;
            
                        System.out.println("User Entered Room");
                } else if ( Data.length > 4 && Data[2].equals("1") ) {

                    System.out.println("Connecting to Room");
                    int ID = Integer.parseInt(Data[3]);
                    int Password = Integer.parseInt(Data[4]);
                    System.out.println("ID : " + ID + " Password :" + Password);
                    boolean res = FindActiveRoom(CloudServer.users.get(i), ID, Password);

                    if (res) {
                        System.out.println("Successfully Found Room");
                        String qbit = "1";
                        byte[] b = qbit.getBytes();
                        DataOutputStream out = new DataOutputStream(CloudServer.users.get(i).socket.getOutputStream());
                        out.write(b);
                        CloudServer.users.get(i).status = UserStatus.inRoom;
                    } else {
                        System.out.println("Didn't Find Room");
                        String qbit = "0";
                        byte[] b = qbit.getBytes();
                        DataOutputStream out = new DataOutputStream(CloudServer.users.get(i).socket.getOutputStream());
                        out.write(b);
                    }
                } else if (Data.length > 2 && Data[2].equals("2")  ) {

                    System.out.println("Connecting to Random Room");
      
                   String qbit = "0"; 
                   if(CloudServer.public_rooms.size() > 0)
                   {
                   
                       int Rnd = CloudServer.passGen.nextInt(CloudServer.public_rooms.size());
                        qbit = "1";
                        byte[] b = qbit.getBytes();
                        DataOutputStream out = new DataOutputStream(CloudServer.users.get(i).socket.getOutputStream());
                        out.write(b);
                        rooms.get(Rnd).Listeners.add(CloudServer.users.get(i));
               
                        System.out.println("User Added to "+rooms.get(Rnd).RoomID);
                                   CloudServer.users.get(i).status = UserStatus.inRoom;
                        rooms.get(Rnd).BroadCastAllListners("t/" + CloudServer.users.get(i).Username + " Has Joined ");
               rooms.get(Rnd).Attach_User(CloudServer.users.get(i));
          
                        CloudServer.users.get(i).status = UserStatus.inRoom;
                    } else {
                        System.out.println("Didn't Find Room");
                         qbit = "0";
                       byte[] b = qbit.getBytes();
                        DataOutputStream out = new DataOutputStream(CloudServer.users.get(i).socket.getOutputStream());
                        out.write(b);
                    }
                }
                  
                

            }
            }catch(IOException ex)
                    {
                    CloudServer.users.get(i).status = UserStatus.Offline;
                    System.out.println("User Disconnected");
                    }
            }
        
                  
        }
    
    }
    boolean FindActiveRoom(User u, int RoomID, int Password) throws IOException {
        for (int i = 0; i < rooms.size(); i++) {
            if (RoomID == rooms.get(i).RoomID && Password == rooms.get(i).RoomPassword) {
                
                rooms.get(i).Listeners.add(u);
        
    
         rooms.get(i).BroadCastAllListners("t/" + u.Username + " Has Joined ");
                     rooms.get(i).Attach_User(u);
                System.out.println("User Added");

                return true;
            }
        }

        return false;
    }

}
