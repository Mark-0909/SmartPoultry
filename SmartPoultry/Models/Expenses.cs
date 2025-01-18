using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Org.BouncyCastle.Asn1.Mozilla;

namespace SmartPoultry.Models
{
    public class Expenses
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order_ID { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }
        public DateTime Added_Date { get; set; }
        [MaxLength(100)]
        public string Category {  get; set; }
        [MaxLength(100)]
        public string Status { get; set; }

        public DateTime Updated_Time { get; set; }
        public int Employee_Incharge { get; set; }
        [MaxLength(255)]
        public string Remarks { get; set; }
    }
}
