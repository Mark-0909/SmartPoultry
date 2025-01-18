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
    public class Deliveries
    {
        [Key]
        public int Id { get; set; }
        public long order_id { get; set; }
        [MaxLength(100)]
        public string type { get; set; }
        [MaxLength(255)]
        public string name { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }
        [MaxLength(255)]
        public string address { get; set; }
        [MaxLength(100)]
        public string payment_status { get; set; }
        [MaxLength(100)]
        public string delivery_status { get; set; }
        [MaxLength(100)]
        public string contact_no { get; set; }
        public DateTime added_date { get; set; }
        public DateTime delivery_date { get; set; }
        [MaxLength(100)]
        public string delivery_man {  get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal charges { get; set; }
        public int employee_incharge { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }
    }
}
