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

        public bool UpdateSupplier(int id, string name, string contactPerson, string contact, string location, string email)
        {
            try
            {
                var supp = _context.SupplierLists.FirstOrDefault(s => s.Id == id);
                supp.Name = name;
                supp.Contact_Person = contactPerson;
                supp.Contact = contact;
                supp.Location = location;
                supp.Email = email;
                
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public string DeleteSupplier(int id)
        {
            try
            {
                var supplier = _context.SupplierLists.FirstOrDefault(s => s.Id == id);
                if (supplier == null)
                {
                    return "Supplier not found.";
                }

                _context.SupplierLists.Remove(supplier);
                _context.SaveChanges();
                return "Supplier deleted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting supplier: " + ex.Message);
                return "An error occurred while deleting the supplier.";
            }
        }

        public int FindSupplierByName(string name)
        {
            try
            {
                int supplierId = _context.SupplierLists.FirstOrDefault(p => p.Name == name).Id;
                return supplierId;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
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
