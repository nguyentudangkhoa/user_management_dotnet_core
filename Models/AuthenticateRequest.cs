using System.ComponentModel.DataAnnotations;

namespace test_dotnet_core_migration.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}