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
using SmartPoultry.DataServices;
using SmartPoultry.DataAccess;
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Sales_OrderInfo.xaml
    /// </summary>
    public partial class Sales_OrderInfo : Window
    {
        public AppDbContext context = new AppDbContext();
        public ProductServices productServices;
        public ProductVariationServices productVariationServices;
        MainWindow mainWindow;
        public Sales_OrderInfo(Sales sales, MainWindow window)
        {
            InitializeComponent();
            
            productServices = new ProductServices(context);
            productVariationServices = new ProductVariationServices(context);

            List<string> productvarids = sales.product_list.Split(',').ToList();
            List<string> pricelist = sales.price_list.Split(',').ToList();
            List<string> qtylist = sales.quantity_list.Split(',').ToList();
            List<string> varlist = sales.variation_list.Split(',').ToList();
            List<string> prodname = new List<string>();
            for(int i = 0; i < productvarids.Count; i++)
            {
                int prodid = productVariationServices.GetProductVariationById(int.Parse(productvarids[i])).product_id;
                string name = productServices.FetchProduct(prodid).product_name;
                prodname.Add(name);
            }
            GenerateList(productvarids, qtylist, varlist, pricelist, prodname);

            mainWindow = UserContext.mainWindow;
        }

        public void GenerateList(List<string> prodvarid, List<string> qty, List<string> varSpec, List<string> priceList, List<string> prodname)
        {
            OrderWPanel.Children.Clear();

            for (int i = 0; i < prodvarid.Count; i++)
            {
                Border orderBorder = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Height = 35,
                    Width = 255
                };

                WrapPanel wrapPanel = new WrapPanel();

                Label itemNameLabel = new Label
                {
                    Content = $"({varSpec[i]}) {prodname[i]}",
                    Height = 33,
                    Width = 126,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label qtyLabel = new Label
                {
                    Content = qty[i],
                    Height = 33,
                    Width = 43,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                string formattedPrice = decimal.TryParse(priceList[i], out decimal price)
                    ? price.ToString("N2")
                    : "Invalid";

                Label priceLabel = new Label
                {
                    Content = formattedPrice,
                    Height = 33,
                    Width = 83,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };
                wrapPanel.Children.Add(itemNameLabel);
                wrapPanel.Children.Add(qtyLabel);
                wrapPanel.Children.Add(priceLabel);

                orderBorder.Child = wrapPanel;

                OrderWPanel.Children.Add(orderBorder);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ActiveOverlay(false);
            this.Close();
        }
    }
}
