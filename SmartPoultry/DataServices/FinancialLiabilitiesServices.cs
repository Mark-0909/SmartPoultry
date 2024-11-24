using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    public class FinancialLiabilitiesServices
    {
        private readonly AppDbContext _context;

        public FinancialLiabilitiesServices(AppDbContext context)
        {
            _context = context;
        }
        public List<FinancialLiabilities> GetList() { 
            List<FinancialLiabilities> financialLiabilities = _context.FinancialLiabilities.Where(p => p.status != "paid").OrderBy(p => p.due_date).ToList();
            return financialLiabilities;
        }
        public bool Create(string name, long orderid, decimal amount, string type, string mode, DateTime duedate, string contacts)
        {
            try
            {
                var newSched = new FinancialLiabilities
                {
                    name = name,
                    order_id = orderid,
                    amount = amount,
                    type = type,
                    status = "Unpaid",
                    added_date = DateTime.Now,
                    updated_date = DateTime.Now,
                    contacts = contacts,
                    due_date = duedate,
                    payment_mode = mode,
                    employee_incharge = 1
                };
                _context.Add(newSched);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
            
        }
    }
}
