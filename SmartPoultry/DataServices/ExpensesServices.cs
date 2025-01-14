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
    public class ExpensesServices
    {
        private readonly AppDbContext _context;
        public ExpensesServices(AppDbContext context)
        {
            _context = context;
        }
        
    }
}
