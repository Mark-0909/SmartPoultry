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
            FetchSales();
        }
        public void FetchSales()
        {
            if(SalesPanel.Children != null)
            {
                SalesPanel.Children.Clear();
            }
            List<Sales> sales = salesServices.GetAllSales();
            int evenodd = 0;
            for (int i = 0; i < sales.Count; i++) { 
                
                Records_SalesControl control = new Records_SalesControl(sales[i].receipt_id, evenodd);
                
                SalesPanel.Children.Add(control);
                if(evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }
        public void FetchInventoryLogs()
        {
            if (SalesPanel.Children != null)
            {
                SalesPanel.Children.Clear();
            }
            List<InventoryLogs> inventory = inventoryLogsServices.GetList();
            int evenodd = 0;
            for (int i = 0; i < inventory.Count; i++)
            {

                Records_InventoryControl control = new Records_InventoryControl(inventory[i].product_id, inventory[i].employee_incharge, inventory[i].action, inventory[i].timestamp, inventory[i].reason, evenodd);

                SalesPanel.Children.Add(control);
                if (evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }
        public void FetchUserLogs()
        {
            if (SalesPanel.Children != null)
            {
                SalesPanel.Children.Clear();
            }
            List<UserLogs> logs = userLogsServices.GetList();
            int evenodd = 0;
            for (int i = 0; i < logs.Count; i++)
            {

                Records_LogsControl control = new Records_LogsControl(logs[i].user_id, logs[i].action, logs[i].timestamp, evenodd);

                SalesPanel.Children.Add(control);
                if (evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Sales_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(SalesBtn);
            FetchSales();
        }

        private void Inventory_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(InventoryBtn);
            FetchInventoryLogs();
        }

        private void Logs_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(LogsBtn);
            FetchUserLogs();
        }
        public void HandleButtonDesign(Button button)
        {
            SolidColorBrush activecolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D"));
            SolidColorBrush inactivecolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDCEDD5"));
            if (button == SalesBtn)
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                InventoryBtn.Foreground = Brushes.Gray;
                LogsBtn.Foreground = Brushes.Gray;

                InventoryBtn.Background = inactivecolor;
                LogsBtn.Background = inactivecolor;
                InventoryBtn.BorderBrush = inactivecolor;
                LogsBtn.BorderBrush = inactivecolor;
            }
            else if(button == InventoryBtn)
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                SalesBtn.Foreground = Brushes.Gray;
                LogsBtn.Foreground = Brushes.Gray;

                SalesBtn.Background = inactivecolor;
                LogsBtn.Background = inactivecolor;
                SalesBtn.BorderBrush = inactivecolor;
                LogsBtn.BorderBrush = inactivecolor;
            }else
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                SalesBtn.Foreground = Brushes.Gray;
                InventoryBtn.Foreground = Brushes.Gray;

                SalesBtn.Background = inactivecolor;
                InventoryBtn.Background = inactivecolor;
                SalesBtn.BorderBrush = inactivecolor;
                InventoryBtn.BorderBrush = inactivecolor;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
