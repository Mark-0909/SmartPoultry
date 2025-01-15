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
        public bool Create(string name, string category, string status, DateTime Updated_Date, int employee, string remarks)
        {
            try
            {
                Expenses expenses = new Expenses()
                {
                    Name = name,
                    Category = category,
                    Status = status,
                    Updated_Time = Updated_Date,
                    Employee_Incharge = employee,
                    Remarks = remarks
                };
                _context.Expenses.Add(expenses);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
