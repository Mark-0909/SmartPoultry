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
    /// Interaction logic for Records_Orders.xaml
    /// </summary>
    public partial class Records_Orders : UserControl
    {
        AppDbContext context = new AppDbContext();
        SupplierOrdersServices SupplierOrdersServices;
        public Records_Orders()
        {
            InitializeComponent();
            SupplierOrdersServices = new SupplierOrdersServices(context);
            DisplaySupplierOrders();
        }
        public void DisplaySupplierOrders()
        {
            if (SalesPanel.Children.Count != 0)
            {
                SalesPanel.Children.Clear();
            }
            List<SupplierOrders> orders = SupplierOrdersServices.GetAllSupplierOrders();
            int evenOdd = 0;
            for(int i = 0; i < orders.Count; i++)
            {
                Records_OrdersControl control = new Records_OrdersControl(orders[i], evenOdd);

                SalesPanel.Children.Add(control);
                if(evenOdd == 1)
                {
                    evenOdd = 0;
                }
                else
                {
                    evenOdd = 1;
                }
            }
        }
    }
}
