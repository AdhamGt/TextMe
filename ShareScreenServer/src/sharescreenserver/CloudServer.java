/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;

/**
 *
 * @author Adham
 */
import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.EOFException;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.lang.ClassNotFoundException;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.Charset;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Random;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;
import static sharescreenserver.MainIntializer.db;
import sun.misc.IOUtils;

public class CloudServer implements Runnable {

    public static ServerSocket server;
    private static int port = 4555;
    public static Socket socket;
    public static  ArrayList<User> users = new ArrayList<User>();
    public static volatile ArrayList<Room> rooms = new ArrayList<Room>();
    public static volatile ArrayList<Thread> roomThreads = new ArrayList<Thread>();
    static int RoomCodeIndex = 1000000;
    public static Random passGen = new Random();
    public static ArrayList<Room> private_rooms = new ArrayList<Room>();
    public static ArrayList<Room> public_rooms = new ArrayList<Room>();
    static volatile boolean serverAlive = true;

    void EndServer() {
        serverAlive = false;
    }

    @Override
    public void run() {
        try {
            try {
                IntializeServer();
            } catch (EOFException ex) {
                Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
            } catch (SQLException ex) {
                Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
            }
        } catch (IOException ex) {
            Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
        } catch (ClassNotFoundException ex) {
            Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
        } catch (InterruptedException ex) {
            Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public void IntializeServer() throws IOException, ClassNotFoundException, EOFException, InterruptedException, SQLException {

        server = new ServerSocket(port);
      Thread Sessions = new Thread(new SessionController());
            Sessions.start();
        while (serverAlive) {
      
            System.out.println("Waiting For User to Join");
            socket = server.accept();
            System.out.println("User Connected");
            Thread.sleep(300);

            DataInputStream dis = new DataInputStream(socket.getInputStream());
            byte[] bytes = new byte[dis.available()];
            dis.readFully(bytes);
            String data = new String(bytes);

            while (true) {
                if (!data.isEmpty()) {
                    System.out.println(data);
                    break;
                }
                System.out.println("Awaiting Data");
                bytes = new byte[dis.available()];
                dis.readFully(bytes);
                data = new String(bytes);
            }

            String[] Data = data.split(",");
            System.out.println("Data Count" + Data.length);
            User u;
            if (Data.length == 2) {
                System.out.println("Quering User");
                try {
                    u = db.GetUser(Data[0], Data[1]);
                    if(u != null)
                    {
                
               
      System.out.println("Found User");
                    if (u != null) {
                        u.status = UserStatus.Online;
                        u.socket = socket;
                        users.add(u);
                        DataOutputStream dos = new DataOutputStream(socket.getOutputStream());
                        String accept = "1";
                        dos.write(accept.getBytes());
                    } else {
                        DataOutputStream dos = new DataOutputStream(socket.getOutputStream());
                        String decline = "0";
                        dos.write(decline.getBytes());
                    }
                    
                    }
                    else
                    {
                    
                            DataOutputStream dos = new DataOutputStream(socket.getOutputStream());
                        String decline = "0";
                        dos.write(decline.getBytes());
                    }
                } catch (SQLException ex) {
                    Logger.getLogger(CloudServer.class.getName()).log(Level.SEVERE, null, ex);
                }
            } else if (Data.length == 3) {
                int result = db.InsertUser(0, Data[0], Data[1], Data[2]);
                if (result <= 0) {
                    DataOutputStream dos = new DataOutputStream(socket.getOutputStream());
                    String decline = "0";
                    System.out.println("User Register Failed");
                    dos.write(decline.getBytes());
                } else if (result > 0) {
                    DataOutputStream dos = new DataOutputStream(socket.getOutputStream());
                    System.out.println("User Register Successfull");
                    String accept = "1";
                    dos.write(accept.getBytes());
                    socket.close();
                }
            }

        }
    }
}
