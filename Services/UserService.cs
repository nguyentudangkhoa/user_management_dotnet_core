using System;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using test_dotnet_core_migration.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace test_dotnet_core_migration.Services
{
    public interface IUserService
    {
        DataTable getUser();

        void addUser(User user);

        void UpdateUser(User user);

        void deleteUser(int id);

        DataTable getSingleUser(int id);

        String login(User user);

        DataTable getLoginUser();
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public UserService(IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public DataTable getUser()
        {
            string query = @"select u.id, u.name,u.email,u.firstname,u.lastname,u.password,u.status,u.created_at,u.updated_at,r.displayname as role, r.permission as permission
                            from users as u join roles as r on r.id = u.role_id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource)){
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon)){
                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }

            return table;
        }

        public DataTable getSingleUser(int id)
        {
            string query = @"select u.id, u.name,u.email,u.firstname,u.lastname,u.password,u.status,u.created_at,u.updated_at,r.displayname as role, r.permission as permission
                            from users as u join roles as r on r.id = u.role_id where u.id=@id limit 1;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource)){
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon)){
                    mySqlCommand.Parameters.AddWithValue("id", id);
                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }
            return table;
        }

        public void addUser(User user) {
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

        public void UpdateUser(User user){
            string query = "";

            if (user.Password == null) {
                query = @"UPDATE users
                        SET name=@name,
                            email=@email,
                            firstname=@firstname,
                            lastname=@lastname,
                            status=@status,
                            role_id=@role_id
                        WHERE id=@id";
            } else {
                query = @"UPDATE users
                        SET name=@name,
                            email=@email,
                            firstname=@firstname,
                            lastname=@lastname,
                            status=@status,
                            password=@password,
                            role_id=@role_id
                        WHERE id=@id";
            }

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon))
                {
                    mySqlCommand.Parameters.AddWithValue("id", user.Id);
                    mySqlCommand.Parameters.AddWithValue("name", user.Name);
                    mySqlCommand.Parameters.AddWithValue("email", user.Email);
                    mySqlCommand.Parameters.AddWithValue("firstname", user.FirstName);
                    mySqlCommand.Parameters.AddWithValue("lastname", user.LastName);
                    mySqlCommand.Parameters.AddWithValue("status", user.Status);
                    if (user.Password != null){
                        mySqlCommand.Parameters.AddWithValue("password", BCryptNet.HashPassword(user.Password));
                    }
                    mySqlCommand.Parameters.AddWithValue("role_id", user.RoleId);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }
        }

        public void deleteUser(int id) 
        {
             string query = @"DELETE FROM users
                            WHERE id=@id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon))
                {
                    mySqlCommand.Parameters.AddWithValue("id", id);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }
        }

        public DataTable getUserByEmail(string email) {
            string query = @"select u.id, u.name,u.email,u.firstname,u.lastname,u.password,u.status,u.created_at,u.updated_at,r.displayname as role
                            from users as u join roles as r on r.id = u.role_id
                            where email=@email limit 1;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource)){
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon)){
                    mySqlCommand.Parameters.AddWithValue("email", email);
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
            if(this.getUserByEmail(user.Email).Rows.Count == 0) {
                return "Wrong email or the user not exist";
            }

            var dbUser = this.getUserByEmail(user.Email).Rows[0]["password"];
            bool checkPassword = BCryptNet.Verify(user.Password, dbUser.ToString());

            if(! checkPassword) {
                return "Wrong password";
            }

            _session.SetString("user_name", user.Name);
            _session.SetString("email", this.getUserByEmail(user.Email).Rows[0]["email"].ToString());

            return "Login successful";
        }

        public DataTable getLoginUser() {
            if (_session.GetString("email") == null) {
                return this.getUserByEmail("");
            }

            return this.getUserByEmail(_session.GetString("email"));
        }
    }
}