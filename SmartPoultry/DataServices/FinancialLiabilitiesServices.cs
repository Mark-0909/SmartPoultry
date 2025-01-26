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
    public class FinancialLiabilitiesServices
    {
        private readonly AppDbContext _context;

        public FinancialLiabilitiesServices(AppDbContext context)
        {
            _context = context;
        }

        public List<FinancialLiabilities> GetAllPayments()
        {
            var lists = _context.FinancialLiabilities.OrderByDescending(p => p.added_date).ToList();

            return lists;
        }
        public bool MarkAsVoided(long receiptid, string remarks)
        {
            try
            {
                var finance = _context.FinancialLiabilities.FirstOrDefault(p => p.order_id == receiptid);

                if (finance == null)
                {
                    return true;
                }

                finance.status = "voided";
                finance.Remarks = remarks;

                

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool EditPayment(int id, string name, decimal price, string type, string paymentmode, DateTime date, string contacts, string remarks)
        {
            try
            {
                var payment = _context.FinancialLiabilities.FirstOrDefault(p => p.Id == id);
                payment.name = name;
                payment.amount = price;
                payment.type = type;
                payment.payment_mode = paymentmode;
                payment.due_date = date;
                payment.contacts = contacts;
                payment.Remarks = remarks;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool MarkAsPaid(int id, string remarks)
        {
            try
            {
                var itemrow = _context.FinancialLiabilities.FirstOrDefault(p => p.Id == id);
                itemrow.status = "paid";
                itemrow.Remarks = remarks;
                itemrow.updated_date = DateTime.Now;
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;      
            }
        }
        public FinancialLiabilities GetByReceipt(long orderId)
        {
            return _context.FinancialLiabilities.FirstOrDefault(f => f.order_id == orderId);
        }


        public FinancialLiabilities GetById(int id)
        {
            var itemrow = _context.FinancialLiabilities.FirstOrDefault(x => x.Id == id);
            return itemrow;
        }
        public List<FinancialLiabilities> GetList(string filter)
        {
            _context.ChangeTracker.Clear(); 
            List<FinancialLiabilities> financialLiabilities = _context.FinancialLiabilities
                .Where(p => p.status != "paid" && p.status != "voided" && p.type == filter)
                .OrderBy(p => p.due_date)
                .ToList();

            return financialLiabilities;
           
        }

        public int CountPayments()
        {
            try 
            {
                DateTime dateTime = DateTime.Now;
                int count = _context.FinancialLiabilities.Count(p => p.due_date <= dateTime && p.type == "To Pay" && p.status == "Unpaid");
                return count;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public bool Create(string name, long orderid, decimal amount, string type, string mode, DateTime duedate, string contacts, string remarks)
        {
            try
            {
                var newSched = new FinancialLiabilities
                {
                    name = name,
                    order_id = orderid,
                    amount = amount,
                    type = type.Trim(),
                    status = "Unpaid",
                    added_date = DateTime.Now,
                    updated_date = DateTime.Now,
                    contacts = contacts,
                    due_date = duedate,
                    payment_mode = mode,
                    employee_incharge = UserContext.CurrentUserId,
                    Remarks = remarks
                };
                _context.Add(newSched);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }
    }
}
