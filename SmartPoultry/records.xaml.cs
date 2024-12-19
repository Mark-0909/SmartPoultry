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
        public records()
        {
            InitializeComponent();
            salesServices = new SalesServices(context);
            FetchSales();
        }
        public void FetchSales()
        {
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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Sales_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(SalesBtn);
        }

        private void Inventory_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(InventoryBtn);
        }

        private void Logs_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(LogsBtn);
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

    }
}
