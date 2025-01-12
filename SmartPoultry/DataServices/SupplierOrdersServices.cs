using SmartPoultry.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    public class SupplierOrdersServices
    {
        public AppDbContext _context;
        public SupplierOrdersServices(AppDbContext context) 
        {
            _context = context;
        }


    }
}
