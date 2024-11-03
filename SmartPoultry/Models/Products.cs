using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartPoultry.Models
{
    public class Products
    {
        [Key]
        public int product_id { get; set; }
        [MaxLength (255)]
        public string product_name { get; set; }
        [MaxLength(255)]
        public string animal_type { get; set; }
        [MaxLength(255)]
        public string product_type { get; set; }
        [MaxLength(100)]
        public int employee_incharge { get; set; }
        public int supplier_id {  get; set; }
        [MaxLength(100)]
        public string status { get; set; }
        [MaxLength(255)]
        public string image {  get; set; }
        [MaxLength(100)]
        public string added_date { get; set; }



    }
}
