using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartPoultry.App;
namespace SmartPoultry.DataServices
{
    
    public class InventoryLogsServices
    {
        AppDbContext _context;
        MainWindow mainWindow = UserContext.mainWindow;
        public InventoryLogsServices(AppDbContext context) 
        {
            _context = context;
        }
        public List<InventoryLogs> GetList()
        {
            _context.ChangeTracker.Clear();
            var list = _context.InventoryLogs.OrderByDescending(p => p.timestamp).ToList();
            return list;

        }
        public bool Create(int productid, int employeeid, string action, string reason, int qty)
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

                mainWindow.recordsControl.InventoryControl.FetchInventoryLogs("");
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
