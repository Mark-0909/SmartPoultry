using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPoultry.Models
{
    public class Products
    {
        [Key]
        public int product_id { get; set; }

        [MaxLength(255)]
        public string product_name { get; set; }

        [MaxLength(255)]
        public string animal_type { get; set; }

        [MaxLength(255)]
        public string product_type { get; set; }

        // Assuming this is an integer ID for an employee
        public int employee_incharge { get; set; }

        public int supplier_id { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal stocks { get; set; }

        [MaxLength(100)]
        public string status { get; set; }

        [Column(TypeName = "longblob")]
        public byte[] image { get; set; }

        [MaxLength(100)]
        public string added_date { get; set; }
    }
}
