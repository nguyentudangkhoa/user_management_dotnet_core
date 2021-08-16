using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace test_dotnet_core_migration.Models
{
    public class RegisterRoleRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [Required]
        public string Permission { get; set; }
    }
}