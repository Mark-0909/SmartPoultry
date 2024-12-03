using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using SmartPoultry.DataServices;
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
    /// Interaction logic for Dashboard_OrdersControl.xaml
    /// </summary>
    public partial class Dashboard_OrdersControl : UserControl
    {
        public SalesServices salesServices;
        ProductServices productServices;
        public AppDbContext context = new AppDbContext();
        public long id;
        public Dashboard_OrdersControl(string refid, string mode, string status, string price, int position)
        {
            InitializeComponent();
            ReferenceIdlabel.Content = refid;
            Modelabel.Content = mode;
            Statuslabel.Content = status;
            TotalpriceLabel.Content = price;
            salesServices = new SalesServices(context);
            productServices = new ProductServices(context);
            if (position == 1) {
                thisBorder.Background = new SolidColorBrush(Colors.White);
            }
            id = long.Parse(refid);
        }

        private void DropDown_Clicked(object sender, RoutedEventArgs e)
        {
            //Dashboard_DisplayOrderDetails orderWindow = new Dashboard_DisplayOrderDetails(id);
            //orderWindow.ShowDialog();

        }

        
        


    }
}
