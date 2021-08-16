using System;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using test_dotnet_core_migration.Models;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;
using test_dotnet_core_migration.Authorization;
using test_dotnet_core_migration.Helpers;
using AutoMapper;


namespace test_dotnet_core_migration.Services
{
    public interface IUserService
    {
        IEnumerable<GetUser> getUser();

        void addUser(RegisterUserRequest model);

        void UpdateUser(int id, UpdateUserRequest model);

        void deleteUser(int id);

        GetUser getSingleUser(int id);

        GetUser getLoginUser();

        User GetById(int id);

        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private DataContext _context;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly IMapper _mapper;

        private IJwtUtils _jwtUtils;

        public UserService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            DataContext context,
            IMapper mapper,
            IJwtUtils jwtUtils
        )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public IEnumerable<GetUser> getUser()
        {
            var getUser = (from user in _context.users
                 join role in _context.roles on user.Role_Id equals role.Id
                 select new GetUser {
                     Id = user.Id,
                     Name = user.Name,
                     Email = user.Email,
                     FirstName = user.FirstName,
                     LastName = user.LastName,
                     Password = user.Password,
                     Status = user.Status,
                     Role_Id = user.Role_Id,
                     Permission = role.Permission,
                     created_at = user.created_at,
                     updated_at = user.updated_at
                 });

            return getUser;
        }

        public GetUser getSingleUser(int id)
        {
             var getUser = (from user in _context.users
                 join role in _context.roles on user.Role_Id equals role.Id
                 where user.Id == id
                 select new GetUser {
                     Id = user.Id,
                     Name = user.Name,
                     Email = user.Email,
                     FirstName = user.FirstName,
                     LastName = user.LastName,
                     Password = user.Password,
                     Status = user.Status,
                     Role_Id = user.Role_Id,
                     Permission = role.Permission,
                     created_at = user.created_at,
                     updated_at = user.updated_at
                 }).SingleOrDefault();

            return getUser;
        }

        public void addUser(RegisterUserRequest model) {
            if (_context.users.Any(x => x.Name == model.Name))
                throw new AppException("Username '" + model.Name + "' is already taken");
 
            if (_context.users.Any(x => x.Email == model.Email))
                throw new AppException("Email '" + model.Email + "' is already taken");
 
            if (! _context.roles.Any(x=>x.Id == model.Role_Id))
                throw new AppException("Role id '" + model.Role_Id + "' is not existed");
 
            // map model to new user object
            var user = _mapper.Map<User>(model);
 
            // hash password
            user.Password = BCryptNet.HashPassword(model.Password);
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;
            // save user
            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(int id, UpdateUserRequest model){
            var user = getUser(id);
 
            // validate
            if (model.Name != user.Name && _context.users.Any(x => x.Name == model.Name))
                throw new AppException("Username '" + model.Name + "' is already taken");
 
            if (model.Email != user.Email && _context.users.Any(x => x.Email == model.Email))
                throw new AppException("Email '" + model.Email + "' is already taken");
 
            if (! _context.roles.Any(x=>x.Id == model.Role_Id))
                throw new AppException("Role id '" + model.Role_Id + "' is not existed");
 
            // copy model to user and save
            _mapper.Map(model, user);
 
            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = BCryptNet.HashPassword(model.Password);
            
            user.updated_at = DateTime.Now;
 
            _context.users.Update(user);
            _context.SaveChanges();
        }

        public void deleteUser(int id) 
        {
             var user = getUser(id);
            _context.users.Remove(user);
            _context.SaveChanges();
        }

        public GetUser getUserByEmail(string email) {
            var getUser = (from user in _context.users
                 join role in _context.roles on user.Role_Id equals role.Id
                 where user.Email == email
                 select new GetUser {
                     Id = user.Id,
                     Name = user.Name,
                     Email = user.Email,
                     FirstName = user.FirstName,
                     LastName = user.LastName,
                     Password = user.Password,
                     Status = user.Status,
                     Role_Id = user.Role_Id,
                     Permission = role.Permission,
                 }).SingleOrDefault();

            return getUser;
        }

        public GetUser getLoginUser() {
            if (_session.GetString("email") == null) {
                return this.getUserByEmail("");
            }

            return this.getUserByEmail(_session.GetString("email"));
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.users.SingleOrDefault(x => x.Email == model.Email);
            var permissions = _context.roles.SingleOrDefault(x => x.Id == user.Role_Id);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.JwtToken = _jwtUtils.GenerateToken(user);
            response.Permission = permissions.Permission;

            _session.SetString("user_name", user.Name);
            _session.SetString("email", user.Email);

            return response;
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        private User getUser(int id)
        {
            var user = _context.users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}