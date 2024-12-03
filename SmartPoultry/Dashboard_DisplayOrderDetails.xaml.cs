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
using System.Windows.Shapes;
using SmartPoultry.Models;
using SmartPoultry.DataAccess;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Dashboard_DisplayOrderDetails.xaml
    /// </summary>
    public partial class Dashboard_DisplayOrderDetails : Window
    {
        ProductServices productServices;
        SalesServices salesServices;
        AppDbContext context;
        public Dashboard_DisplayOrderDetails(long id)
        {
            InitializeComponent();
            context = new AppDbContext();
            salesServices = new SalesServices(context);
            productServices = new ProductServices(context);
            DisplayOrderDetails(id);
        }
        public void DisplayOrderDetails(long id)
        {
            Sales sale = salesServices.GetSales(id);

            if (sale == null ||
                string.IsNullOrWhiteSpace(sale.product_list) ||
                string.IsNullOrWhiteSpace(sale.price_list) ||
                string.IsNullOrWhiteSpace(sale.quantity_list) ||
                string.IsNullOrWhiteSpace(sale.variation_list))
            {
                MessageBox.Show("No products found for this sale.");
                return;
            }
            List<int> productIds = sale.product_list.Split(',').Select(int.Parse).ToList();
            List<decimal> productPrices = sale.price_list.Split(',').Select(decimal.Parse).ToList();
            List<int> productQty = sale.quantity_list.Split(',').Select(int.Parse).ToList();
            List<string> productVar = sale.variation_list.Split(',').ToList();

            int evenodd = 0;
            for (int i = 0; i < productIds.Count; i++) { 
                Products products = productServices.FetchProduct(productIds[i]);
                string name = products.product_name;
                string var = productVar[i];

                decimal orig = productPrices[i]/productQty[i];
                string qty = productQty[i].ToString();
                string price = orig.ToString("N2");

                string total = productPrices[i].ToString("N2");

                Dashboard_OrderDetailsControl control = new Dashboard_OrderDetailsControl(name, var, qty, price, total, evenodd);

                ListPanel.Children.Add(control);
                if (evenodd == 0) { 
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
