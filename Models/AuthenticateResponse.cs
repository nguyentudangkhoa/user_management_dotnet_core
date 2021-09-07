using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_dotnet_core_migration.Models
{
    public class AuthenticateResponse
    {
        public int id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string JwtToken { get; set; }
    }
}