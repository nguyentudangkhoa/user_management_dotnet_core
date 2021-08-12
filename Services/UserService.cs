using System;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using test_dotnet_core_migration.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace test_dotnet_core_migration.Services
{
    public interface IUserService
    {
        void Register(User user);
        String login(User user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }



        public void Register(User user) { 
            string query = @"INSERT INTO users(name, email, firstname, lastname, status, password, role_id) VALUES(@name, @email, @firstname, @lastname, @status, @password, @role_id);";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon))
                {
                    mySqlCommand.Parameters.AddWithValue("name", user.Name);
                    mySqlCommand.Parameters.AddWithValue("email", user.Email);
                    mySqlCommand.Parameters.AddWithValue("firstname", user.FirstName);
                    mySqlCommand.Parameters.AddWithValue("lastname", user.LastName);
                    mySqlCommand.Parameters.AddWithValue("status", user.Status);
                    mySqlCommand.Parameters.AddWithValue("password", BCryptNet.HashPassword(user.Password));
                    mySqlCommand.Parameters.AddWithValue("role_id", user.RoleId);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }
        }

        public DataTable getUserByName(string name) {
            string query = @"SELECT * FROM users where name=@name limit 1;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource)){
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon)){
                    mySqlCommand.Parameters.AddWithValue("name", name);
                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }

            return table;
        }

        public String login(User user) 
        {
            var dbUser = this.getUserByName(user.Name).Rows[0]["password"];
            bool checkPassword = BCryptNet.Verify(user.Password, dbUser.ToString());

            if(this.getUserByName(user.Name) == null) {
                return "Sai tai khoan";
            }

            if(! checkPassword) {
                return "Sai tai khoan mat khau";
            }



            return "Ban da dang nhap thanh cong";
        }
    }
}