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
using test_dotnet_core_migration.Services;
using Newtonsoft.Json.Linq;

namespace test_dotnet_core_migration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private IRoleService _roleService;

        public RolesController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IRoleService roleService
        )
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _roleService = roleService;
        }

        [HttpGet]
        public JsonResult Get(){
            return new JsonResult(_roleService.GetRoles());
        }

        [HttpGet("{id}")]
        public JsonResult GetSingle(int id){
            return new JsonResult(_roleService.getSingleRole(id));
        }

        [HttpPost]
        public JsonResult Post(RegisterRoleRequest role){
            _roleService.addRole(role);

            return new JsonResult("Added role successful");
        }

        [HttpPut("{id}")]
        public JsonResult Put(int id, UpdateRoleRequest model){
            _roleService.updateRole(id, model);

            return new JsonResult("Update role successful");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id){
            _roleService.removeRole(id);

            return new JsonResult("Delete role successful");
        }
    }
}