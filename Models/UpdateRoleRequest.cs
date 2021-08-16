using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace test_dotnet_core_migration.Models
{
    public class UpdateRoleRequest
    {
        public string name { get; set; }
        public string displayname { get; set; }

        public string permission { get; set; }
    }
}