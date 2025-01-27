using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Windows;
using static SmartPoultry.App;

namespace SmartPoultry.DataServices
{
    
    public class SalesServices
    {
        private readonly AppDbContext _context;
        MainWindow mainWindow = UserContext.mainWindow;

        public SalesServices(AppDbContext context)
        {
            _context = context;
        }
        public bool MarkAsVoided(long receiptid, string remarks)
        {
            try
            {
                if (receiptid == 0)
                {
                    return true;
                }

                var sale = _context.Sales.FirstOrDefault(p => p.receipt_id == receiptid);
                sale.purchase_method = "voided";
                sale.status = "voided";
                sale.payment_mode = "voided";
                sale.Remarks = remarks;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool MarkAsPaid(long orderid, string remarks)
        {
            try
            {
                if (orderid == 0 || orderid.ToString().Length != 11)
                {
                    return true;
                }
                var itemrow = _context.Sales.FirstOrDefault(p => p.receipt_id == orderid);
                itemrow.status = "paid";
                itemrow.Remarks = remarks;
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool UpdateDelivered(long id, string remarks)
        {
            try
            {
                if (id == 0)
                {
                    return true;
                }

                var row = _context.Sales.FirstOrDefault(x => x.receipt_id == id);
                row.purchase_method = "delivered";
                row.Remarks = remarks;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Sales GetSales(long id)
        {
            try
            {
                Sales sales = _context.Sales.FirstOrDefault(p => p.receipt_id == id);
                return sales;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        
        public List<Sales> GetSalesList()
        {
            try
            {
                DateTime today = DateTime.Today;

                DateTime tomorrow = today.AddDays(1);

                return _context.Sales
                    .AsEnumerable()
                    .Where(p =>
                    {
                        DateTime purchaseDate;
                        bool isValidDate = DateTime.TryParse(p.purchase_date.ToString(), out purchaseDate);

                        return isValidDate && purchaseDate >= today && purchaseDate < tomorrow;
                    })
                    .OrderByDescending(p => p.purchase_date)
                    .ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Fetching Sales: " + e.Message);
                return null;
            }
        }

        public List<Sales> GetAllSales()
        {
            try
            {
                _context.ChangeTracker.Clear();
                List<Sales> sales = _context.Sales.OrderByDescending(p => p.purchase_date).ToList();
                return sales;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                return null;
            }
        }

        public long Create(string productList, string priceList, string quantityList, string varList, string paymentMode, string status, decimal totalPrice, string purchaseMethod)
        {
            try
            {
                long refid = CreateReferenceNumber();
                var newSales = new Sales()
                {
                    receipt_id = refid,
                    product_list = productList,
                    price_list = priceList,
                    quantity_list = quantityList,
                    purchase_date = DateTime.Now,
                    variation_list = varList,
                    payment_mode = paymentMode,
                    status = status,
                    purchase_method = purchaseMethod,
                    total_price = totalPrice,
                    employee_incharge = 1,
                    Remarks = "Order Processed."
                };

                
                _context.Sales.Add(newSales);
                _context.SaveChanges();

                mainWindow.recordsControl.SalesControl.FetchSales();
                return newSales.receipt_id;
            }
            catch (Exception e)
            {
                
                Console.WriteLine($"Error in SalesServices.Create: {e.Message}");
                return -1;
            }
        }

        
        public long CreateReferenceNumber()
        {
            try
            {
                string salesNumber = $"{DateTime.Now:yyMMdd}{new Random().Next(10000, 99999)}";

                long convertsalesnumber = Convert.ToInt64(salesNumber);

                bool isPresent = _context.Sales.Any(p => p.receipt_id == convertsalesnumber);

                if (isPresent)
                {
                    return CreateReferenceNumber();
                }

                return convertsalesnumber;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while creating sales record: {e.Message}");
                return 0;
            }
        }


    }
}
