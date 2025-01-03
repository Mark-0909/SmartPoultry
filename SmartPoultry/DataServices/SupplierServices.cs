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
        public int FindSupplierByName(string name)
        {
            try
            {
                int supplierId = _context.SupplierLists.FirstOrDefault(p => p.Name == name).Id;
                return supplierId;
            }
            catch (Exception ex) { 
                return -1;
            }
            
        }
        public List<SupplierList> ListSuppliers()
        {
            try
            {
                return _context.SupplierLists.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching suppliers: " + ex.Message);
                return new List<SupplierList>(); 
            }
        }
        public SupplierList FindSupplier(int id)
        {
            try
            {
                return _context.SupplierLists.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                // Log exception for debugging (example: log to a file or console)
                Console.WriteLine("Error finding supplier: " + ex.Message);
                return null;
            }


        }

        public bool Create(string name, string contactperson, string contact, string email, string location)
        {
            try
            {
                var newSupplier = new SupplierList()
                {
                    Name = name,
                    Contact_Person = contactperson,
                    Contact = contact,
                    Location = location,
                    Email = email,
                    Added_date = DateTime.Now.ToString(),
                    Status = "active",
                    employee_incharge = 1

                };
                _context.SupplierLists.Add(newSupplier);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show($"{ex}");
                return false;
            }
        }
        
    }
}
