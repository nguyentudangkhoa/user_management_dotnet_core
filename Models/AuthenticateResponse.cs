using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_dotnet_core_migration.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JwtToken { get; set; }

        public string Permission { get; set; }
    }
}