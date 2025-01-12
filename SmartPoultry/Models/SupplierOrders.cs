using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartPoultry.Models
{
    public class SupplierOrders
    {
        [Key]
        public int id { get; set; }

        public int supplierID { get; set; }

        public string productList { get; set; }

        public string orderQty { get; set; }

        public int employee_incharge { get; set; }


    }

}
