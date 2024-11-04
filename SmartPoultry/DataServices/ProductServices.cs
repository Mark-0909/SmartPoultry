using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;


namespace SmartPoultry.DataServices
{
    public class ProductServices
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }
        public int Create(string product_name, string animal_type, string product_type, int employee_incharge, int supplierId, int stocks, string image)
        {
            try
            {
                var newProduct = new Products
                {
                    product_name = product_name,
                    animal_type = animal_type,
                    product_type = product_type,
                    employee_incharge = employee_incharge,
                    supplier_id = supplierId,
                    stocks = stocks,
                    image = image,
                    status = "active",
                    added_date = DateTime.Now.ToString("MM-dd-yyyy")
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();

                
                return newProduct.product_id; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return 0; 
            }
        }

    }

}
