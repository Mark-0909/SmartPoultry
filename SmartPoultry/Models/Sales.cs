using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPoultry.Models
{
    public class Sales
    {
        [Key]
        public int id {  get; set; }
        [MaxLength(255)]
        public string product_list { get; set; }
        [MaxLength(255)]
        public string price_list { get; set; }
        [MaxLength(255)]
        public string quantity_list { get; set; }
        [MaxLength(255)]
        public string purchase_date { get; set; }
        [MaxLength(255)]
        public string variation_list { get; set; }
        [MaxLength(100)]
        public string payment_mode { get; set; }
        [MaxLength(100)]
        public string status { get; set; }
        [MaxLength(100)]
        public string purchase_method { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal total_price { get; set; }
        public int employee_incharge { get; set; }

    }
}
