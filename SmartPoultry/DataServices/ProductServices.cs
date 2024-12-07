using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;
using System.Windows.Controls;


namespace SmartPoultry.DataServices
{
    public class ProductServices
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }
        public Products FetchProduct(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.product_id == id);

                if (product == null)
                    throw new Exception($"Product with ID {id} not found");

                return product;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching product: {ex.Message}");
                throw; 
            }
        }

        public List<Products> FilterProducts(string productType, string animalType)
        {
            try
            {

                var products = _context.Products.ToList();

                if (!string.IsNullOrEmpty(productType))
                {
                    products = products.Where(p => p.product_type.Contains(productType)).ToList();
                }

                if (!string.IsNullOrEmpty(animalType))
                {
                    products = products.Where(p => p.animal_type.Contains(animalType)).ToList();
                }

                return products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching products: {ex.Message}");
                throw;
            }
        }


        public List<Products> SearchProducts(string searchTerm, string type, string animal)
        {
            try
            {   var products = _context.Products.ToList();

                if (!string.IsNullOrEmpty(type))
                {
                    products = products.Where(p => p.product_type.Contains(type)).ToList();
                }

                if (!string.IsNullOrEmpty(animal))
                {
                    products = products.Where(p => p.animal_type.Contains(animal)).ToList();
                }

                if (string.IsNullOrEmpty(searchTerm))
                {
                    List<Products> products1 = new List<Products>();
                    return products1;
                }
                products = products.Where(p => p.product_id.ToString().Contains(searchTerm.ToLower()) || p.product_name.ToLower().Contains(searchTerm.ToLower()) || p.animal_type.Contains(searchTerm.ToLower()) || p.product_type.Contains(searchTerm.ToLower()) || p.employee_incharge.ToString().Contains(searchTerm.ToLower()) || p.supplier_id.ToString().Contains(searchTerm.ToLower()) || p.status.Contains(searchTerm.ToLower())).ToList();
                return products;
            }
            catch (Exception e)
            {
                List<Products> products = new List<Products>();
                return products;
            }
        }

        public List<Products> GetAllProducts()
        {
            try
            {
                return _context.Products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Products>();
            }
        }

        public int Create(string product_name, string animal_type, string product_type, int employee_incharge, int supplierId, decimal stocks, string image)
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
        public void UpdateImagePath(int id, string imagePath)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.product_id == id);

                if (product != null)
                {
                    product.image = imagePath;

                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating image path: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }

}
