using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSql {
    public class User {

        private static string CONN_STRING = @"server=DSI-WORKSTATION\SQLEXPRESS;"
                           + "database=PrsDb;"
                           + "trusted_connection=true;";

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        private static SqlConnection CreateAndCheckConnection() {
            var Connection = new SqlConnection(CONN_STRING);
            Connection.Open();
            if(Connection.State != System.Data.ConnectionState.Open) {
                Console.WriteLine("Connection did not open.");
                return null;
            }
            return Connection;
        }

        public static bool UpdateUser(User user) {
            var Connection = CreateAndCheckConnection();
            if(Connection == null) {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = new StringBuilder("UPDATE Users Set ");
            sql.Append($"Username = '{user.Username}',");
            sql.Append($"Password = '{user.Password}',");
            sql.Append($"Firstname = '{user.Firstname}',");
            sql.Append($"Lastname = '{user.Lastname}',");
            sql.Append($"Phone = '{user.Phone}',");
            sql.Append($"Email = '{user.Email}',");
            sql.AppendFormat("IsReviewer = {0}", (user.IsReviewer ? 1 : 0));
            sql.AppendFormat("IsAdmin = {0}", (user.IsAdmin ? 1 : 0));
            sql.Append($" Where Id = {user.Id}");
            var Command = new SqlCommand(sql.ToString(), Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;

        }

        public static bool DeleteUser(int Id) {
            var Connection = CreateAndCheckConnection();
            if(Connection == null) {
                return false;
            }
            var sql = $"DELETE From Users Where Id = {Id}";
            var Command = new SqlCommand(sql, Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }

        public static bool InsertUser(User user) {
            var Connection = CreateAndCheckConnection();
            if(Connection == null) {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = $"INSERT into Users (Username, Password, Firstname, Lastname, Phone, Email, IsReviewer, IsAdmin)" 
                + $"values ('{user.Username}', '{user.Password}', '{user.Firstname}', '{user.Lastname}', '{user.Phone}', '{user.Email}', {isReviewer}, {isAdmin})";
            var Command = new SqlCommand(sql, Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }

        private static SqlDataReader CreateSqlReaderAndCheck(string sql, SqlConnection Connection) {
            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
            if(reader.HasRows == false) {
                Console.WriteLine("Result set has no rows.");
                Connection.Close();
                return null;
            }
            return reader;
        }

        public static User GetUserByPrimaryKey(int Id) {
            var Connection = CreateAndCheckConnection();
            if(Connection == null) {
                return null;
            }
            var sql = $"SELECT * From Users Where Id = {Id}";
            var reader = CreateSqlReaderAndCheck(sql, Connection);
            reader.Read();
            var user = new User();
            user.Id = (int)reader["Id"];
            user.Username = (string)reader["Username"];
            user.Firstname = (string)reader["Firstname"];
            user.Lastname = (string)reader["Lastname"];
            //user.Fullname = $"{Firstname} {Lastname}";
            user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
            user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
            user.IsReviewer = (bool)reader["IsReviewer"];
            user.IsAdmin = (bool)reader["IsAdmin"];



            Connection.Close();
            return user;
        }

        public static User[] GetAllUsers() {
            var Connection = CreateAndCheckConnection();
            if(Connection == null) {
                return null;
            }
            var sql = "SELECT * From Users;";
            var reader = CreateSqlReaderAndCheck(sql, Connection);

            var users = new List<User>();
            var index = 0;
            while(reader.Read()) {
                var user = new User();
                user.Id = (int)reader["Id"];
                user.Username = (string)reader["Username"];
                user.Password = (string)reader["Password"];
                user.Firstname = (string)reader["Firstname"];
                user.Lastname = (string)reader["Lastname"];
                //user.Fullname = $"{Firstname} {Lastname}";
                user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
                user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
                user.IsReviewer = (bool)reader["IsReviewer"];
                user.IsAdmin = (bool)reader["IsAdmin"];

                users.Add(user);

            }
            reader.Close();
            Connection.Close();
            return users.ToArray();
        }

        public User() { }

        public User(int id, string username, string password, string firstname, string lastname,
                        string phone, string email, bool isReviewer, bool isAdmin) {
            Id = id;
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            Phone = phone;
            Email = email;
            IsReviewer = isReviewer;
            IsAdmin = isAdmin;
        }
        public override string ToString() {
            return $"[ToString()] Id={Id}, Username={Username}, Password={Password}, Name={Firstname} {Lastname}";
        }
    }
}
