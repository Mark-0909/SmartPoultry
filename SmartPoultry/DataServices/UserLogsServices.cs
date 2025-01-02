using SmartPoultry.DataAccess;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    internal class UserLogsServices
    {
        AppDbContext _context;
        public UserLogsServices(AppDbContext context) 
        {
            _context = context;
        }
    }
}
