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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for records.xaml
    /// </summary>
    public partial class records : UserControl
    {
        public AppDbContext context = new AppDbContext();
        SalesServices salesServices;
        UserLogsServices userLogsServices;
        InventoryLogsServices inventoryLogsServices;
        public records()
        {
            InitializeComponent();
            salesServices = new SalesServices(context);
            userLogsServices = new UserLogsServices(context);
            inventoryLogsServices = new InventoryLogsServices(context);
            
            HideAllControls();
            SalesControl.Visibility = Visibility.Visible;
        }
        private void HideAllControls()
        {
            LogsControl.Visibility = Visibility.Collapsed;
            ExpensesControl.Visibility = Visibility.Collapsed;
            OrdersControl.Visibility = Visibility.Collapsed;
            DeliveryControl.Visibility = Visibility.Collapsed;
            PaymentsControl.Visibility = Visibility.Collapsed;
            InventoryControl.Visibility = Visibility.Collapsed;
            SalesControl.Visibility = Visibility.Collapsed;
        }

        
        private void Sales_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(SalesBtn);
            HideAllControls();
            SalesControl.Visibility = Visibility.Visible;
        }

        private void Inventory_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(InventoryBtn);
            HideAllControls();
            InventoryControl.Visibility = Visibility.Visible;
        }

        private void Logs_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(LogsBtn);
            HideAllControls();
            LogsControl.Visibility = Visibility.Visible;
        }
        private void Payments_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(PaymentsBtn);
            HideAllControls();
            PaymentsControl.Visibility = Visibility.Visible;
        }

        private void Delivery_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(DeliveryBtn);
            HideAllControls();
            DeliveryControl.Visibility = Visibility.Visible;
        }

        private void Orders_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(OrdersBtn);
            HideAllControls();
            OrdersControl.Visibility = Visibility.Visible;
        }

        private void Expenses_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(ExpensesBtn);
            HideAllControls();
            ExpensesControl.Visibility = Visibility.Visible;
        }

        public void SearchRecords()
        {
            // Search for records
        }
        public void HandleButtonDesign(Button activeButton)
        {
            // Define active and inactive colors
            SolidColorBrush activeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D"));
            SolidColorBrush inactiveColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDCEDD5"));

            // List of all buttons to manage
            var buttons = new List<Button> { SalesBtn, InventoryBtn, LogsBtn, PaymentsBtn, DeliveryBtn, OrdersBtn, ExpensesBtn};

            // Reset all buttons to inactive state
            foreach (var button in buttons)
            {
                button.Background = inactiveColor;
                button.BorderBrush = inactiveColor;
                button.Foreground = Brushes.Gray;
            }

            // Set the clicked button to active state
            activeButton.Background = activeColor;
            activeButton.BorderBrush = activeColor;
            activeButton.Foreground = Brushes.White;
        }


        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                string searchTerm = SearchTB.Text.Trim(); 
                                                          
                SearchRecords(searchTerm);
            }
        }

        public void SearchRecords(string searchTerm)
        {
            SalesControl.FetchSales(searchTerm);
            InventoryControl.FetchInventoryLogs(searchTerm);
            PaymentsControl.DisplayPayments(searchTerm);
            DeliveryControl.DisplayDeliveries(searchTerm);
            OrdersControl.DisplaySupplierOrders(searchTerm);
            ExpensesControl.DisplayExpenses(searchTerm);
            LogsControl.FetchUserLogs(searchTerm);
        }
        private void ReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            SalesControl.FetchSales("");
            InventoryControl.FetchInventoryLogs("");
            PaymentsControl.DisplayPayments("");
            DeliveryControl.DisplayDeliveries("");
            OrdersControl.DisplaySupplierOrders("");
            ExpensesControl.DisplayExpenses("");
            LogsControl.FetchUserLogs("");
        }

        private void SearchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(SearchTB, "Search Product...", true);
        }
        private void SearchTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(SearchTB, "Search Product...", false);
        }
        public void HandleTextBoxPlaceholder(TextBox tb, string placeholder, bool isFocused)
        {
            if (isFocused)
            {
                if (tb.Text == placeholder)
                {
                    tb.Text = string.Empty;
                    tb.Foreground = Brushes.Black;
                }
            }
            else // When the TextBox loses focus
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        
    }
}
