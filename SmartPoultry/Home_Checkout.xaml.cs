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

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Home_Checkout.xaml
    /// </summary>
    public partial class Home_Checkout : Window
    {
        public MainWindow mainWindow { get; set; }


        public string paymentmethod;
        public string status;
        public string purchasemethod;
        readonly home homeController;
        
        public Home_Checkout(string price, home homeControl, MainWindow window, List<string> provvarid, List<string> quantity, List<string> varspec, List<string> pricelist, List<string> Prodname)
        {
            InitializeComponent();
            totalPricelabel.Content = price;
            homeController = homeControl;
            BackBtnBorder.Visibility = Visibility.Hidden;
            mainWindow = window;

            GenerateList(provvarid, quantity, varspec, pricelist, Prodname);

            if (OrderWPanel.Children.Count < 10)
            {
                OrderScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
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



        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ActiveOverlay(false);
        }

        

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            homeController.ConfirmOrder(paymentmethod, status, purchasemethod);
            homeController.EnableDropBtn();
            this.Close();
            mainWindow.ActiveOverlay(false);
        }

        private void CashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paymentmethod = "cash";
        }

        private void GCashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paymentmethod = "gcash";
        }

        private void PaidRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            status = "paid";
        }

        private void UnpaidRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            status = "unpaid";
        }

        private void UpfrontRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            purchasemethod = "upfront";
        }

        private void ToDeliverRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            purchasemethod = "to deliver";
        }
    }
}
