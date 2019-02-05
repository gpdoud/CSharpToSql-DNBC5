﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSql {
    class User {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        public static bool InsertUser(User user) {
            var connStr = @"server=DSI-WORKSTATION\SQLEXPRESS;"
                            + "database=PrsDb;"
                            + "trusted_connection=true;";
            // "uid=sa;pwd=sa;";  // instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if(Connection.State != System.Data.ConnectionState.Open) {
                Console.WriteLine("Connection did not open.");
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

        public static User GetUserByPrimaryKey(int Id) {

            var connStr = @"server=DSI-WORKSTATION\SQLEXPRESS;"
                            + "database=PrsDb;"
                            + "trusted_connection=true;";
            // "uid=sa;pwd=sa;";  // instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if(Connection.State != System.Data.ConnectionState.Open) {
                Console.WriteLine("Connection did not open.");
                return null;
            }
            var sql = $"SELECT * From Users Where Id = {Id}";
            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
            if(reader.HasRows == false) {
                Console.WriteLine("Result set has no rows.");
                Connection.Close();
                return null;
            }
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

            var connStr = @"server=DSI-WORKSTATION\SQLEXPRESS;"
                            + "database=PrsDb;"
                            + "trusted_connection=true;";
            // "uid=sa;pwd=sa;";  // instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if(Connection.State != System.Data.ConnectionState.Open) {
                Console.WriteLine("Connection did not open.");
                return null;
            }
            var sql = "SELECT * From Users;";
            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
            if(reader.HasRows == false) {
                Console.WriteLine("Result set has no rows.");
                Connection.Close();
                return null;
            }
            var users = new User[19];
            var index = 0;
            while(reader.Read()) {
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

                users[index++] = user;

            }

            Connection.Close();
            return users;
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
        public string ToPrint() {
            return $"[ToPrint()] Id={Id}, Username={Username}, Name={Firstname} {Lastname}";
        }
    }
}
