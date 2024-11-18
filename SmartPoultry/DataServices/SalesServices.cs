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

        public bool Create(string productList, string priceList, string quantityList, string varList, string paymentMode, string status, decimal totalPrice, string purchaseMethod)
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

                return true;
            }
            catch (Exception e)
            {
                
                Console.WriteLine($"Error in SalesServices.Create: {e.Message}");
                return false;
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
