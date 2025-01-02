using SmartPoultry.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    
    internal class InventoryLogsServices
    {
        AppDbContext _context;
        public InventoryLogsServices(AppDbContext context) 
        {
            _context = context;
        }
    }
}
