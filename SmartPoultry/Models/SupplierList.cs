using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartPoultry.Models
{
    public class SupplierList
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Contact { get; set; }
        [MaxLength(255)]
        public string Location { get; set; }
        [MaxLength(255)]
        public string Products { get; set; }
        [MaxLength(100)]
        public string Added_date { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public int employee_incharge { get; set; }

    }
}
