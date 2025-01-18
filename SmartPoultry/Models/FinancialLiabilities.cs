using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPoultry.Models
{
    public class FinancialLiabilities
    {
        [Key]
        public int Id { get; set;}
        [MaxLength(255)]
        public string name { get; set;}

        public long order_id { get; set;}

        [Column(TypeName = "decimal(10,2)")]
        public decimal amount { get; set;}
        [MaxLength(100)]
        public string type { get; set;}
        [MaxLength(100)]
        public string status { get; set;}
        public DateTime added_date { get; set;}
        public DateTime due_date { get; set;}
        public DateTime updated_date { get; set;}
        [MaxLength(255)]
        public string contacts { get; set;}
        [MaxLength(100)]
        public string payment_mode { get; set;}
        public int employee_incharge { get; set;}

        [MaxLength(255)]
        public string Remarks { get; set; }
    }
}
