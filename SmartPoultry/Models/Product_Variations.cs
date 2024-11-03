using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartPoultry.Models
{
    public class Product_Variations
    {
        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        [MaxLength (100)]
        public string unitName { get; set; }
        public bool isBaseUnit { get; set; }

    }
}
