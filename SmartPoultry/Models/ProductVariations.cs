using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartPoultry.Models
{
    public class ProductVariations
    {
        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        [MaxLength (100)]
        public string? variant_type { get; set; }
        public bool isBaseUnit { get; set; }

        public int price { get; set; }

        public int conversion_rate { get; set; }
    }
}
