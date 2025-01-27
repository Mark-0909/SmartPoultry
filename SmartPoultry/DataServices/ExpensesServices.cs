using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmartPoultry.App;

namespace SmartPoultry.DataServices
{
    public class ExpensesServices
    {
        private readonly AppDbContext _context;
        MainWindow mainWindow = UserContext.mainWindow;
        public ExpensesServices(AppDbContext context)
        {
            _context = context;
        }
        public List<Expenses> GeAllExpenses()
        {
            _context.ChangeTracker.Clear();
            List<Expenses> expensesList = _context.Expenses.OrderByDescending(p => p.Added_Date).ToList();
            return expensesList;
            
        }
        public List<Expenses> GetTodaysExpenses()
        {
            try
            {
                DateTime today = DateTime.Now.Date;

                List<Expenses> expenses = _context.Expenses
                    .Where(p => p.Added_Date.Date == today) 
                    .ToList();

                return expenses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Expenses>();
            }
        }

        public bool Create(string name, string category, string status, int employee, string remarks, decimal price, long orderid, DateTime addedDate)
        {
            try
            {
                Expenses expenses = new Expenses()
                {
                    Name = name,
                    Category = category,
                    Status = status,
                    Updated_Time = DateTime.Now,
                    Employee_Incharge = employee,
                    Remarks = remarks,
                    price = price,
                    Order_ID = orderid,
                    Added_Date = addedDate
                };
                _context.Expenses.Add(expenses);
                _context.SaveChanges();

                mainWindow.recordsControl.ExpensesControl.DisplayExpenses("");
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
