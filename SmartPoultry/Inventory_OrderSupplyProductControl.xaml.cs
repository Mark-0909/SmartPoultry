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
    /// Interaction logic for Inventory_OrderSupplyProductControl.xaml
    /// </summary>
    public partial class Inventory_OrderSupplyProductControl : UserControl
    {
        public Inventory_OrderSupplyProductControl(Products products)
        {
            InitializeComponent();
            NameLabel.Content = products.product_name;
            StocksLabel.Content = products.stocks.ToString();
        }
    }
}
