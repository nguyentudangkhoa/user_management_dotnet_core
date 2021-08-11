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
            string query = @"INSERT INTO users(name, email, password, roles_id) VALUES(@name, @email, @password, @roles_id);";

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
                    mySqlCommand.Parameters.AddWithValue("password", BCryptNet.HashPassword(user.Password));
                    mySqlCommand.Parameters.AddWithValue("roles_id", user.RolesId);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }
        }
    }
}