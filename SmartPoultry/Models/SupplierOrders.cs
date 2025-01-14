using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPoultry.Models
{
    public class SupplierOrders
    {
        [Key]
        public int id { get; set; }

        public int supplierID { get; set; }
        [MaxLength(255)]
        public string productList { get; set; }
        [MaxLength(255)]
        public string orderQty { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        public DateTime Added_Date { get; set; }

        public DateTime Delivery_Date { get; set; }

        public DateTime Delivered_Date { get; set; }

        public int employee_incharge { get; set; }
    }

}
