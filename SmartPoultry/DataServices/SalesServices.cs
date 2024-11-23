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
        public List<Sales> GetSales()
        {
            try
            {
                string today = DateTime.Now.ToString("MM/dd/yyyy");

                return _context.Sales
                    .Where(p => p.purchase_date.StartsWith(today)) // Filter by today's date
                    .OrderByDescending(p => p.purchase_date)       // Sort by newest first
                    .ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Fetching Sales: " + e.Message);
                return null;
            }
        }


        public Sales GetSales(int id)
        {
            try
            {
                return _context.Sales.FirstOrDefault(p => p.id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching sales record: {e.Message}");
                return null; 
            }
        }


        public int Create(string productList, string priceList, string quantityList, string varList, string paymentMode, string status, decimal totalPrice, string purchaseMethod)
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

                return newSales.id;
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
