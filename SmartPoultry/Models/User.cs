using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SmartPoultry.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string Q1 { get; set; }
        [MaxLength(255)]
        public string Q2 { get; set; }
        [MaxLength(255)]
        public string Q3 { get; set; }

        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        
    }
}
