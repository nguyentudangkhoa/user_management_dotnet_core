using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace test_dotnet_core_migration.Models
{
    public class RegisterUserRequest
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public bool status { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public int role_id { get; set; }
    }
}