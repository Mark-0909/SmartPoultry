using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    
    public class InventoryLogsServices
    {
        AppDbContext _context;
        public InventoryLogsServices(AppDbContext context) 
        {
            _context = context;
        }
        public List<InventoryLogs> GetList()
        {
            var list = _context.InventoryLogs.OrderBy(p => p.timestamp).ToList();
            return list;
        }
        public bool Create(int productid, int employeeid, string action, string reason)
        {
            try
            {
                var row = new InventoryLogs
                {
                    product_id = productid,
                    employee_incharge = employeeid,
                    action = action,
                    reason = reason,
                    timestamp = DateTime.Now,
                };

                _context.Add(row);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return true;
            }
            
        }
    }
}
