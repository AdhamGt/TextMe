/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sharescreenserver;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.logging.Level;
import java.util.logging.Logger;
import static sharescreenserver.MainIntializer.db;
/**
 *
 * @author Adham
 */
public class DatabaseHandler {
  Connection con;
public DatabaseHandler()
{
      try {
          Connect();
      } catch (SQLException ex) {
          Logger.getLogger(DatabaseHandler.class.getName()).log(Level.SEVERE, null, ex);
      } catch (ClassNotFoundException ex) {
          Logger.getLogger(DatabaseHandler.class.getName()).log(Level.SEVERE, null, ex);
      }
}
  public void Connect() throws SQLException, ClassNotFoundException {

Class.forName("com.mysql.jdbc.Driver");  
System.out.println("Connecting to Database");
 con=DriverManager.getConnection("jdbc:mysql://groupmeet.cytvkbexhefi.us-east-1.rds.amazonaws.com:3306/groupmeet","adham","12345678");  
 System.out.println("Successfully Connected to Database");
 
//con.close();  
       
    }
     User GetUser(String Username , String Password) throws SQLException
     {
         Statement stmt=con.createStatement(); 
         String Query = "select * FROM user WHERE groupname = '"+Username+"' and Password = '"+Password+"'";
ResultSet rs=stmt.executeQuery(Query);  
if(rs.next())  
{       User un = new User();
         un.Username = rs.getString(5);
         un.Password =rs.getString(4);
         return un;
    }
else
{
   return null;   
}
     }
         public  int InsertUser(int state  , String name, String password , String email) throws SQLException
     {
            Statement stmt=con.createStatement(); 
         String Query = "INSERT INTO user (email, Econfirm, password, groupname) " +
"VALUES ('"+email+"', '"+state+"','"+password+"','"+name+"');" ;
           int rs=stmt.executeUpdate(Query);
           return rs;
     }
}
