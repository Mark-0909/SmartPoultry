using System.ComponentModel.DataAnnotations;

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
        public string ContactNo { get; set; }

        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }
    }
}
