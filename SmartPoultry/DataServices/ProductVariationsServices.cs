using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SmartPoultry.DataServices
{
    public class ProductVariationServices
    {
        private readonly AppDbContext _context;

        public ProductVariationServices(AppDbContext context)
        {
            _context = context;
        }

        public string GetBaseUnit(int id)
        {
            var variation = _context.ProductVariations
                                    .FirstOrDefault(p => p.product_id == id && p.isBaseUnit);
            if (variation == null)
            {
                return "Base unit not found"; 
            }
            return variation.variant_type.ToString();
        }
        
        public bool EditUnitVar(int id, string name, decimal price, int conversion)
        {
            try
            {
                var variation = _context.ProductVariations.FirstOrDefault(p => p.id == id);

                variation.variant_type = name;
                variation.price = price;
                variation.conversion_rate = conversion;
                
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        public ProductVariations GetProductVariationById(int id)
        {
            var var_row = _context.ProductVariations.FirstOrDefault(p => p.id == id);
            return var_row;
        }

        public List<ProductVariations> GetAllProductVariations(int productId)
        {
            try
            {
                _context.ChangeTracker.Clear();
                var productVariations = _context.ProductVariations
                                                 .Where(pv => pv.product_id == productId)
                                                 .OrderByDescending(pv => pv.isBaseUnit) 
                                                 .ToList();

                if (!productVariations.Any())
                {
                    MessageBox.Show("No variations found for this product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return productVariations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<ProductVariations>();
            }
        }




        public bool Create(int productid, string variantname, bool unittype, decimal price, int conversionrate)
        {
            try
            {

                var newvariations = new ProductVariations() { 
                    product_id = productid,
                    variant_type = variantname,
                    isBaseUnit = unittype,
                    price = price,
                    conversion_rate = conversionrate
                };

                _context.ProductVariations.Add(newvariations);
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex) {

                MessageBox.Show($"Error creating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
        
        
        }
    }
}
