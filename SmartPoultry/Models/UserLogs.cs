using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace SmartPoultry.Models
{
    public class UserLogs
    {
        [Key]
        public int Id { get; set; }
        public long user_id { get; set; }
        
        public string action { get; set; }

        public DateTime timestamp { get; set; }
    }
}
