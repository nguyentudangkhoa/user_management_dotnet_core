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
using BCryptNet = BCrypt.Net.BCrypt;
using test_dotnet_core_migration.Authorization;
using AutoMapper;

namespace test_dotnet_core_migration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IUserService _userService;

        private IMapper _mapper;

        public UsersController(
            IConfiguration configuration,
             IUserService userService,
             IWebHostEnvironment webHostEnvironment,
             IMapper mapper
        )
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public JsonResult Get(){
            return new JsonResult(_userService.getUser());
        }

        [HttpGet("{id}")]
        public JsonResult GetSingle(int id){
            return new JsonResult(_userService.getSingleUser(id));
        }

        [HttpPost]
        public JsonResult Post(User user){
            if(user.Password == null) {
                return new JsonResult("Password is required");
            }

            _userService.addUser(user);

            return new JsonResult("Added customer point successful");
        }

        [HttpPut]
        public JsonResult Put(User user){
            _userService.UpdateUser(user);

            return new JsonResult("Update customer point successful");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id){
            _userService.deleteUser(id);

            return new JsonResult("Delete customer point successful");
        }

        [HttpPost]
        [Route("login")]
        public JsonResult Login(User user){
            return new JsonResult(_userService.login(user));
        }

        [HttpGet]
        [Route("login_session")]
        public JsonResult getLoginSession() {
            return new JsonResult(_userService.getLoginUser());
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }
    }
}