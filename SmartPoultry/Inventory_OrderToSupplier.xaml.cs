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
using System.Windows.Media.Animation;

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

            InitializeComboBoxItems();
            NotifPopup.Visibility = Visibility.Hidden;
        }

        public void InitializeComboBoxItems()
        {
            SearchCBox.Items.Clear();
            SearchCBox.Items.Add("Search Product...");

            List<Products> products = productServices.GetAllProducts();

            foreach (var product in products)
            {
                SearchCBox.Items.Add(product.product_name);
            }

            SearchCBox.SelectedIndex = 0;
        }

        public string PopUpNotif(string type, string message)
        {
            NotifPopup.Visibility = Visibility.Visible;

            
            Panel.SetZIndex(NotifPopup, int.MaxValue);
            if (type == "notif")
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
            }
            else
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
            }

            NotifMessage.Content = message;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromSeconds(4.5),
                Duration = TimeSpan.FromMilliseconds(500)
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            storyboard.Children.Add(fadeOut);

            Storyboard.SetTarget(fadeIn, NotifPopup);
            Storyboard.SetTarget(fadeOut, NotifPopup);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));

            storyboard.Completed += (sender, args) =>
            {
                NotifPopup.Visibility = Visibility.Collapsed;
            };
            storyboard.Begin();
            return message;
        }

        private void SearchCBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string searchText = SearchCBox.Text + e.Text;

            SearchCBox.Items.Clear();
            SearchCBox.Items.Add("Search Product...");

            
            List<Products> products = productServices.GetAllProducts();
            foreach (var product in products)
            {
                if (product.product_name.ToLower().Contains(searchText.ToLower()))
                {
                    SearchCBox.Items.Add(product.product_name);
                }
            }

            
            SearchCBox.IsDropDownOpen = SearchCBox.Items.Count > 1;

            
            SearchCBox.SelectedIndex = -1; 
        }

        private void SearchCBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (SearchCBox.SelectedIndex != -1)
            {
                SearchCBox.SelectedIndex = -1;
                SearchCBox.Text = string.Empty;  
            }
        }

        private void SearchCBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddProductToControl(SearchCBox.Text);

                RoutedEventArgs routedEventArgs = new RoutedEventArgs();
                SearchCBox_LostFocus(SearchCBox, routedEventArgs);
            }
        }

        private void SearchCBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchCBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(SearchCBox.Text))
            {
                SearchCBox.SelectedIndex = 0;
                SearchCBox.Text = "Search Product...";
            }
        }

        public void AddProductToControl(string item)
        {
            Products product = productServices.GetProductByName(item);
            if (product == null) 
            {
                PopUpNotif("alert", "Product not found");
                return;
            }

            

            foreach(UIElement element in Wpanel.Children)
            {
                if(element is Inventory_OrderSupplyControl control && control.supplierID == product.supplier_id)
                {
                    bool isPresent = control.IsProductIdPresent(product.product_id);
                    if (isPresent)
                    {
                        PopUpNotif("alert", "Product is already listed.");
                        return;
                    }

                    control.AddProduct(product);
                    return;
                }
            }

            Inventory_OrderSupplyControl orderControl = new Inventory_OrderSupplyControl(product.supplier_id, product, this);

            Wpanel.Children.Add(orderControl);


        }






        public void DisplayOutOfStocks()
        {
            List<Products> products = productServices.GetLowStockProducts("", "", "");
            products = products.Where(p => !p.hasOrder).ToList();

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
                PopUpNotif("alert", "There are no orders to confirm.");
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
                        PopUpNotif("alert", $"Invalid date.");
                        return; 
                    }
                }
            }
            foreach (var control in controlsToRemove)
            {
                RemoveControl(control);  
            }
            MainWindow main = UserContext.mainWindow;
            main.ScheduleUpdateReload();
            PopUpNotif("notif", "All orders have been confirmed successfully.");
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

        private void SearchBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchCBox.Text))
            {
                PopUpNotif("alert", "Empty search box");
                return;
            }
            AddProductToControl(SearchCBox.Text);

            RoutedEventArgs routedEventArgs = new RoutedEventArgs();
            SearchCBox_LostFocus(SearchCBox, routedEventArgs);
        }

        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }
    }
}
