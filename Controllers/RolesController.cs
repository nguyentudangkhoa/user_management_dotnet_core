using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using test_dotnet_core_migration.Models;
using Newtonsoft.Json.Linq;

namespace test_dotnet_core_migration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RolesController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment){
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get(){
            string query = @"SELECT * FROM roles";

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

            return new JsonResult(table);
        }

        [HttpGet("{id}")]
        public JsonResult GetSingle(int id){
            string query = @"SELECT * FROM roles where id=@id limit 1;";

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

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Role role){
            string query = @"INSERT INTO roles(name, displayname,permission) VALUES(@name, @displayname, @permission)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon))
                {
                    mySqlCommand.Parameters.AddWithValue("name", role.Name);
                    mySqlCommand.Parameters.AddWithValue("displayname", role.DisplayName);
                    mySqlCommand.Parameters.AddWithValue("permission", role.Permission);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added customer point successful");
        }

        [HttpPut]
        public JsonResult Put(Role role){
            string query = @"UPDATE roles
                            SET name=@name, permission=@permission, displayname=@displayname
                            WHERE id=@id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            MySqlDataReader reader;
            using(MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand mySqlCommand = new MySqlCommand(query, mycon))
                {
                    mySqlCommand.Parameters.AddWithValue("id", role.Id);
                    mySqlCommand.Parameters.AddWithValue("name", role.Name);
                    mySqlCommand.Parameters.AddWithValue("displayname", role.DisplayName);
                    mySqlCommand.Parameters.AddWithValue("permission", role.Permission);

                    reader = mySqlCommand.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Update customer point successful");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id){
            string query = @"DELETE FROM roles
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

            return new JsonResult("Delete customer point successful");
        }
    }
}