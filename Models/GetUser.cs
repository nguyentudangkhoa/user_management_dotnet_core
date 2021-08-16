using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace test_dotnet_core_migration.Models
{
    public class GetUser
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

        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime created_at { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime updated_at { get; set; }

        [Required]
        public int Role_Id { get; set; }
        
        [JsonIgnore]        
        public string Permission { get; set; }
    }
}