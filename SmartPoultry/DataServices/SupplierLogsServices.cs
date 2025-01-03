using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    public class SupplierLogsServices
    {
        AppDbContext _context;
        public SupplierLogsServices(AppDbContext context)
        {
            _context = context;
        }
        
    }
}
