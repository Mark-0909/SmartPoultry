using SmartPoultry.DataAccess;
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
using System.Windows.Shapes;
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_OrderToSupplier.xaml
    /// </summary>
    public partial class Inventory_OrderToSupplier : Window
    {
        public AppDbContext context = new AppDbContext();
        public ProductServices productServices;
        public Inventory_OrderToSupplier()
        {
            InitializeComponent();
            productServices = new ProductServices(context);
            DisplayOutOfStocks();
        }

        public void DisplayOutOfStocks()
        {
            List<Products> products = GetOutOfStockProducts();
            if (products == null || products.Count == 0)
            {
                return;
            }

            Dictionary<int, Inventory_OrderSupplyControl> existingControls = new Dictionary<int, Inventory_OrderSupplyControl>();

            foreach (UIElement element in Wpanel.Children)
            {
                if (element is Inventory_OrderSupplyControl control)
                {
                    existingControls[control.supplierID] = control;
                }
            }

            foreach (Products product in products)
            {
                if (existingControls.TryGetValue(product.supplier_id, out Inventory_OrderSupplyControl control))
                {
                    control.AddProduct(product);
                }
                else
                {
                    Inventory_OrderSupplyControl newControl = new Inventory_OrderSupplyControl(product.supplier_id, product, this);
                    Wpanel.Children.Add(newControl);
                    existingControls[product.supplier_id] = newControl;
                }
            }
        }
        
        public List<Products> GetOutOfStockProducts()
        {
            return productServices.GetLowStockProducts("", "", "");
        }



        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = UserContext.mainWindow;
            this.Close();
            mainWindow.ActiveOverlay(false);
        }
    }
}
