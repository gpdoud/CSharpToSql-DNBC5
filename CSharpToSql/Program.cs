using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CSharpToSql {
    class Program {
        // DSI-WORKSTATION\SQLEXPRESS
        static void Main(string[] args) {
            try {
                //var user = new User();
                var user = new User(0, "xxx9", "xxx2", "UserX", "UserX", "5135552323", "info@user.com", true, true);
                //var returnCode = User.InsertUser(user);

                User[] users = User.GetAllUsers();
                foreach(var u in users) {
                    if(u == null) {
                        continue;
                    }
                    Console.WriteLine(u);
                }
                int ID = users[0].Id;
                User userpk = User.GetUserByPrimaryKey(ID);
                Console.WriteLine(userpk);

                userpk.Password = "ABCXYZ";
                var updateSuccess = User.UpdateUser(userpk);
                if(updateSuccess) {
                    Console.WriteLine("Update successful");
                } else {
                    Console.WriteLine("Update failed");
                }

                var deleteSuccess = User.DeleteUser(ID);
                if(!deleteSuccess) {     // (deleteSuccess == false) {
                    Console.WriteLine("Delete failed");
                }
                deleteSuccess = User.DeleteUser(6);
                if(!deleteSuccess) {     // (deleteSuccess == false) {
                    Console.WriteLine("Delete failed on non-existent Id");
                }
            } catch (Exception ex) {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
