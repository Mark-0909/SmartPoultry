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
    public class SupplierLogs
    {
        [Key]
        public int Id { get; set; }
        public int order_id { get; set; }
        public int Supplier_id { get; set; }
        public string action { get; set; }
        [MaxLength(255)]
        public string remarks { get; set; }
        public int employee_incharge { get; set; }
        public DateTime timestamp { get; set; }
    }
}
