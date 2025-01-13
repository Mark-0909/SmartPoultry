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
using System.Linq;

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

            ProductSuggestionsListBox = new ListBox
            {
                Visibility = Visibility.Collapsed,
                Width = 408
            };


            Wpanel.Children.Add(ProductSuggestionsListBox);
        }

        private void AddOrderProductTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProductSuggestionsListBox == null)
            {
                return; 
            }


            if (string.IsNullOrWhiteSpace(AddOrderProductTB.Text) ||
                (AddOrderProductTB.Tag != null && AddOrderProductTB.Tag.ToString() == AddOrderProductTB.Text))
            {
                ProductSuggestionsListBox.ItemsSource = null;
                ProductSuggestionsListBox.Visibility = Visibility.Collapsed;
                return;
            }

            productServices ??= new ProductServices(context);
            List<Products> products = productServices.SearchProducts(AddOrderProductTB.Text, "", "");

            if (products != null && products.Any())
            {
                ProductSuggestionsListBox.ItemsSource = products;
                ProductSuggestionsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                ProductSuggestionsListBox.ItemsSource = null;
                ProductSuggestionsListBox.Visibility = Visibility.Collapsed;
            }
        }




        private void ProductSuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductSuggestionsListBox.SelectedItem is Products selectedProduct)
            {
                AddOrderProductTB.Text = selectedProduct.product_name;
                ProductSuggestionsListBox.Visibility = Visibility.Collapsed;
            }
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
            var lowStockProducts = productServices.GetLowStockProducts("", "", "");
            var productsWithoutOrder = lowStockProducts.Where(p => !p.hasOrder).ToList();

            return productsWithoutOrder;
        }




        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Wpanel.Children.Count == 0)
            {
                MessageBox.Show("There are no orders to confirm.", "No Orders", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<UIElement> childrenList = Wpanel.Children.Cast<UIElement>().ToList();

            List<Inventory_OrderSupplyControl> controlsToRemove = new List<Inventory_OrderSupplyControl>();

            foreach (UIElement element in childrenList)
            {
                if (element is Inventory_OrderSupplyControl control)
                {
                    try
                    {
                        control.ConfirmOrder();
                        controlsToRemove.Add(control);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while confirming an order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }
            }
            foreach (var control in controlsToRemove)
            {
                RemoveControl(control);  
            }

            MessageBox.Show("All orders have been confirmed successfully.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void RemoveControl(Inventory_OrderSupplyControl control)
        {
            Wpanel.Children.Remove(control);
        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = UserContext.mainWindow;
            this.Close();
            mainWindow.ActiveOverlay(false);
        }

        
    }
}
