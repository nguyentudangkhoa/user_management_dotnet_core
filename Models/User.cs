using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace test_dotnet_core_migration.Models
{
    public class User
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public bool status { get; set; }

        public string password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime created_at { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd HH:mm:ss}")]
        public DateTime updated_at { get; set; }

        [Required]
        public int role_id { get; set; }
    }
}