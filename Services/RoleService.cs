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
    public interface IRoleService
    {
        IEnumerable<Role> GetRoles();

        Role getSingleRole(int id);

        void addRole(RegisterRoleRequest model); 

        void updateRole(int id, UpdateRoleRequest model);

        void removeRole(int id);
    }
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private DataContext _context;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly IMapper _mapper;

        private IJwtUtils _jwtUtils;

        public RoleService(
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

        public IEnumerable<Role> GetRoles() {
            return _context.roles;
        }

        public Role getSingleRole(int id)
        {
            return _context.roles.Find(id);
        }        

        public void addRole(RegisterRoleRequest model)
        {
            var role = _mapper.Map<Role>(model);

            role.created_at = DateTime.Now;
            role.updated_at = DateTime.Now;
            _context.roles.Add(role);
            _context.SaveChanges();
        }

        public void updateRole(int id,UpdateRoleRequest model)
        {
            var role = this.getSingleRole(id);
            
            _mapper.Map(model, role);

            role.updated_at = DateTime.Now;
 
            _context.roles.Update(role);
            _context.SaveChanges();
        }

        public void removeRole(int id)
        {
            var role = this.getSingleRole(id);
            _context.roles.Remove(role);
            _context.SaveChanges();
        }
    }
}