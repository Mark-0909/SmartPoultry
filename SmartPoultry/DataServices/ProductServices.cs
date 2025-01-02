using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;
using System.Windows.Controls;
using System.IO;


namespace SmartPoultry.DataServices
{
    public class ProductServices
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }
        public bool EditProduct(int id, string name, string animaltype, string producttype, int supplierid, decimal stocks, string image)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.product_id == id);
                if (product == null)
                {
                    return false; 
                }

                product.product_name = name;
                product.product_type = producttype;
                product.animal_type = animaltype;
                product.supplier_id = supplierid;
                product.stocks = stocks;

                
                if (image != null && image.Length > 0)
                {
                    byte[] imageData = File.ReadAllBytes(image);
                    product.image = imageData;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }

        public List<Products> GetLowStockProducts(string animal, string type, string searchterm)
        {
            var lowStockProducts = _context.Products
                .Where(p =>
                    (p.product_type.Contains("feeds") && p.stocks <= 3) ||
                    (p.product_type.Contains("vaccine") && p.stocks <= 2) ||
                    (p.product_type.Contains("accessories") && p.stocks <= 3) ||
                    (p.product_type.Contains("medicine") && p.product_name.ToLower().Contains("powder") && p.stocks <= 10) ||
                    (p.product_type.Contains("medicine") && p.product_name.ToLower().Contains("liquid") && p.stocks <= 1) ||
                    (p.product_type.Contains("medicine") && p.product_name.ToLower().Contains("capsules") && p.stocks <= 10) ||
                    (p.product_type.Contains("medicine") && p.product_name.ToLower().Contains("tablets") && p.stocks <= 10) ||
                    (p.product_type.Contains("vitamins") && p.product_name.ToLower().Contains("powder") && p.stocks <= 10) ||
                    (p.product_type.Contains("vitamins") && p.product_name.ToLower().Contains("liquid") && p.stocks <= 1) ||
                    (p.product_type.Contains("vitamins") && p.product_name.ToLower().Contains("tablets") && p.stocks <= 1) ||
                    (p.product_type.Contains("vitamins") && p.product_name.ToLower().Contains("capsules") && p.stocks <= 10))
                .ToList();

            if (!string.IsNullOrEmpty(type))
            {
                lowStockProducts = lowStockProducts.Where(p => p.product_type.Contains(type, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(animal))
            {
                lowStockProducts = lowStockProducts.Where(p => p.animal_type.Contains(animal, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (string.IsNullOrEmpty(searchterm))
            {
                return lowStockProducts;
            }
            lowStockProducts = lowStockProducts.Where(p =>
                    p.product_id.ToString().Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.product_name.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.animal_type.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.product_type.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.employee_incharge.ToString().Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.supplier_id.ToString().Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                    p.status.Contains(searchterm, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            return lowStockProducts;
        }




        public void AdjustStocks(string agenda, decimal newstocks, int productid)
        {
            try
            {
                if (agenda == "subtract")
                {
                    var product = _context.Products.FirstOrDefault(p => p.product_id == productid);

                    if (product == null)
                    {
                        Console.WriteLine("Product not found.");
                        return;
                    }


                    product.stocks = newstocks;

                    _context.SaveChanges();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            {
                var products = _context.Products.ToList();

                if (!string.IsNullOrEmpty(type))
                {
                    products = products.Where(p => p.product_type.Contains(type, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(animal))
                {
                    products = products.Where(p => p.animal_type.Contains(animal, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (string.IsNullOrEmpty(searchTerm))
                {
                    return products; 
                }

                products = products.Where(p =>
                    p.product_id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.product_name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.animal_type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.product_type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.employee_incharge.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.supplier_id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.status.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();

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
                _context.ChangeTracker.Clear();
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
                if (!File.Exists(image))
                {
                    MessageBox.Show("Image file not found at the specified path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }

                // Read the image as binary data
                byte[] imageData = File.ReadAllBytes(image);

                var newProduct = new Products
                {
                    product_name = product_name,
                    animal_type = animal_type,
                    product_type = product_type,
                    employee_incharge = employee_incharge,
                    supplier_id = supplierId,
                    stocks = stocks,
                    image = imageData,
                    status = "active",
                    added_date = DateTime.Now.ToString("MM-dd-yyyy")
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();

                return newProduct.product_id;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Image not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }




    }

}
