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
    /// Interaction logic for Records_OrdersControl.xaml
    /// </summary>
    public partial class Records_OrdersControl : UserControl
    {
        AppDbContext context = new AppDbContext();
        UserServices userServices;
        SupplierServices supplierServices;
        public Records_OrdersControl(SupplierOrders supplierOrders, int evenOdd)
        {
            InitializeComponent();
            userServices = new UserServices(context);
            supplierServices = new SupplierServices(context);

            NameLabel.Content = supplierOrders.id;
            DateLabel.Content = supplierServices.FindSupplier(supplierOrders.supplierID).Name;
            EmployeeLabel.Content = supplierOrders.Delivery_Date;
            PurposeLabel.Content = supplierOrders.Added_Date.ToString("MM-dd-yyyy");
            AmountLabel.Content = userServices.GetUser(supplierOrders.employee_incharge).Username;

            if (evenOdd == 1)
            {
                ThisBorder.Background = new SolidColorBrush(Colors.White);  
            }
        }
    }
}
