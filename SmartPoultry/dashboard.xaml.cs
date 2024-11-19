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
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for dashboard.xaml
    /// </summary>
    public partial class dashboard : UserControl
    {
        SalesServices salesServices;
        public dashboard()
        {
            InitializeComponent();
            var context = new AppDbContext();
            salesServices = new SalesServices(context);
            DisplaySales();
        }
        public void DisplaySales() {
            int evenodd = 0;
            List<Sales> salesList = salesServices.GetSales();

            foreach (Sales sales in salesList) { 
                string refid = sales.receipt_id.ToString();
                string mode = sales.payment_mode.ToString();
                string status = sales.status.ToString();
                string price = sales.total_price.ToString();

                if (evenodd == 0)
                {
                    Dashboard_OrdersControl control = new Dashboard_OrdersControl(refid, mode, status, price, 0);
                    evenodd = 1;
                    OrderListPanel.Children.Add(control);
                }
                else {
                    Dashboard_OrdersControl control = new Dashboard_OrdersControl(refid, mode, status, price, 1);
                    evenodd = 0;
                    OrderListPanel.Children.Add(control);
                }

                
                
            }
        }
    }
}
