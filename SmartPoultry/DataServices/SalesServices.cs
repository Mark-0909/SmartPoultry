using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;

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
                
                var newSales = new Sales()
                {
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
    }
}
