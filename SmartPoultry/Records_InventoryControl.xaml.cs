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
using SmartPoultry.Models;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Records_InventoryControl.xaml
    /// </summary>
    public partial class Records_InventoryControl : UserControl
    {
        public AppDbContext context = new AppDbContext();
        public UserServices UserServices;
        public ProductServices ProductServices;
        public Records_InventoryControl(int productid, int userid, string action, DateTime date, string remarks, int evenodd)
        {
            InitializeComponent();
            UserServices = new UserServices(context);
            ProductServices = new ProductServices(context);

            string productname = ProductServices.FetchProduct(productid).product_name.ToString();
            string name = UserServices.GetUser(userid).Username.ToString();

            ProductName.Content = name;
            DateLabel.Content = date.ToString("MM-dd-yyyy");
            ActionLabel.Content = action;
            RemarksLabel.Content = remarks;

            if (evenodd == 1)
            {
                ThisBorder.Background = Brushes.White;
                ThisBorder.BorderBrush = Brushes.White;
            }
        }
    }
}
