using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace test_dotnet_core_migration.Models
{
    public class User
    {
         private DateTime _date = DateTime.Now;
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

        public string Password { get; set; }

        public DateTime created_at { get; set; }
        
        public DateTime updated_at { get; set; }

        [Required]
        public int Role_Id { get; set; }
    }
}