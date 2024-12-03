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
    /// Interaction logic for Dashboard_OrderDetailsControl.xaml
    /// </summary>
    public partial class Dashboard_OrderDetailsControl : UserControl
    {
        public Dashboard_OrderDetailsControl(string name, string var, string qty, string price, string total, int evenodd)
        {
            InitializeComponent();
            nameLabel.Content = name;
            varLabel.Content = var;
            qtyLabel.Content = qty;
            priceLabel.Content = price;
            totalLabel.Content = total;

            if (evenodd == 1) { 
                thisBorder.Background = Brushes.White;
            }
        }
    }
}
