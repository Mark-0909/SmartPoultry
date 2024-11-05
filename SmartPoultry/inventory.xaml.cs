using SmartPoultry.DataServices;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartPoultry.DataServices;
using SmartPoultry.DataAccess;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for inventory.xaml
    /// </summary>
    public partial class inventory : UserControl
    {
        private readonly ProductServices productService;
        public inventory()
        {
            InitializeComponent();
            var context = new AppDbContext();
            productService = new ProductServices(context);

            LoadProducts();
        }
        private void LoadProducts()
        {
            
            List<Products> products = productService.GetAllProducts(); 

           
            foreach (var product in products)
            {
               
                Inventory_ProductControl productControl = new Inventory_ProductControl(
                    product.product_name,
                    product.stocks,
                    product.image 
                );

                
                ProductListWPanel.Children.Add(productControl);
            }
        }
        private void OpenAddForm_Click(object sender, RoutedEventArgs e)
        {
            Inventory_AddingForm addForm = new Inventory_AddingForm();
            addForm.ShowDialog();
        }
    }
}
