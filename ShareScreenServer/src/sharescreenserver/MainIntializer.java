/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;

import com.sun.glass.ui.Application;
import java.sql.SQLException;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Adham
 */
public class MainIntializer {
  public static DatabaseHandler db;
    public static boolean ServerUp = false;
       static CloudServer server =new CloudServer();
    public static void  main(String[]Args) throws SQLException
    {
         db = new DatabaseHandler();
        

        Scanner s = new Scanner(System.in);
            System.out.println("Type Start Server to Intialize Server");
       while(!ServerUp)
       {
        if(s.nextLine().equals("Start Server"))
        {
           System.out.println("Intializing Server");
           ServerUp = true;
           Thread backend = new Thread(server);
           backend.start();
        }
       }
       while(ServerUp)
       {
              if(s.nextLine().equals("End Server"))
              {
                  System.exit(0);
              }
       }
    }
}
