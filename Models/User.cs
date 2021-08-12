using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace test_dotnet_core_migration.Models
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Status { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}