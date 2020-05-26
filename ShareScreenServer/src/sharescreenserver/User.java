/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;

/**
 *
 * @author Adham
 */
public class User {
    
    public Socket socket;
    public DataInputStream Input;
    public DataOutputStream Output;
    public String Username,Password;
    public Room room;
    public volatile  UserStatus status;
    
    public  UserStatus getStatus()
    {
        return status;
    }
      public  String getUsername()
    {
        return Username;
    }
}
