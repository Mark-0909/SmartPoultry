using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartPoultry.DataServices
{
    public class SupplierServices
    {
        private readonly AppDbContext _context;
        public SupplierServices(AppDbContext context) { 
            _context = context;
        }
        public bool Create(string name, string contact, string location, string productlist)
        {
            try
            {
                var newSupplier = new SupplierList()
                {
                    Name = name,
                    Contact = contact,
                    Location = location,
                    Products = productlist,
                    Added_date = DateTime.Now.ToString(),
                    Status = "active",
                    employee_incharge = 1

                };
                _context.SupplierLists.Add(newSupplier);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
