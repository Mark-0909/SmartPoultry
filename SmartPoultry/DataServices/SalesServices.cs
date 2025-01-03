using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Windows;

namespace SmartPoultry.DataServices
{
    public class SalesServices
    {
        private readonly AppDbContext _context;

        public SalesServices(AppDbContext context)
        {
            _context = context;
        }
        public bool MarkAsPaid(long orderid)
        {
            try
            {
                var itemrow = _context.Sales.FirstOrDefault(p => p.receipt_id == orderid);
                itemrow.status = "paid";
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool UpdateDelivered(long id)
        {
            try
            {
                if (id == 0)
                {
                    return true;
                }

                var row = _context.Sales.FirstOrDefault(x => x.receipt_id == id);
                row.purchase_method = "delivered";
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
                        bool isValidDate = DateTime.TryParse(p.purchase_date, out purchaseDate);

                        return isValidDate && purchaseDate >= today && purchaseDate < tomorrow;
                    })
                    .OrderByDescending(p => DateTime.Parse(p.purchase_date))
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
                    purchase_date = DateTime.Now.ToString(),
                    variation_list = varList,
                    payment_mode = paymentMode,
                    status = status,
                    purchase_method = purchaseMethod,
                    total_price = totalPrice,
                    employee_incharge = 1 
                };

                
                _context.Sales.Add(newSales);
                _context.SaveChanges();

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
