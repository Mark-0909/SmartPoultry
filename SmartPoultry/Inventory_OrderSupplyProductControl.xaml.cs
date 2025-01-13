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
        public string ProductId { get; set; }


        Inventory_OrderSupplyControl form;
        public Inventory_OrderSupplyProductControl(Products products, Inventory_OrderSupplyControl formControl)
        {
            InitializeComponent();
            NameLabel.Content = products.product_name;
            StocksLabel.Content = products.stocks.ToString();

            ProductId = products.product_id.ToString();
            form = formControl;
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
                form.CheckPresentProducts();
            }
            else
            {
                MessageBox.Show("Parent container not found or is not a valid panel.");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int qtyvalue = int.Parse(QTYLabel.Content.ToString());
            int todisplay = qtyvalue + 1;

            QTYLabel.Content = todisplay.ToString();
        }

        private void minusBtn_Click(object sender, RoutedEventArgs e)
        {
            int qtyvalue = int.Parse(QTYLabel.Content.ToString());

            if (qtyvalue == 1) 
            {
                return;
            }
            int todisplay = qtyvalue - 1;

            QTYLabel.Content = todisplay.ToString();

        }
    }
}
