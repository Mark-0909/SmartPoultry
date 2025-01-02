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
    public class InventoryLogs
    {
        [Key]
        public int Id { get; set; }
        public int product_id {  get; set; }

        public int employee_incharge { get; set; }
        [MaxLength(255)]
        public string action { get; set; }
        [MaxLength(255)]
        public string reason { get; set; }
        public DateTime timestamp { get; set; }
    }
}
