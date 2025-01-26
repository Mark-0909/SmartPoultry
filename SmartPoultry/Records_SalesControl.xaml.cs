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
    /// Interaction logic for Records_SalesControl.xaml
    /// </summary>
    public partial class Records_SalesControl : UserControl
    {
        public AppDbContext context = new AppDbContext();
        SalesServices salesServices;
        public Records_SalesControl(long receiptid, int oddeven)
        {
            InitializeComponent();
            salesServices = new SalesServices(context);
            displayDetails(receiptid);

            if(oddeven == 1)
            {
                ThisBorder.Background = Brushes.White;
                ThisBorder.BorderBrush = Brushes.White;
            }
        }
        public void displayDetails(long receiptid) 
        {
            Sales sale = salesServices.GetSales(receiptid);
            DateTime dateTime = sale.purchase_date;
            IdLabel.Content = sale.receipt_id.ToString();
            DateLabel.Content = dateTime.ToString("MM-dd-yyyy");
            StatusLabel.Content = sale.status;
            MethodLabel.Content = sale.payment_mode;
            PriceLabel.Content = sale.total_price.ToString("N2");
        }
    }
}
