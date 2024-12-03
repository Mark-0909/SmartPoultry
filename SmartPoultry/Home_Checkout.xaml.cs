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
        public Home_Checkout(string price, home homeControl, MainWindow window)
        {
            InitializeComponent();
            totalPricelabel.Content = price;
            homeController = homeControl;

            mainWindow = window;
        }


        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ActiveOverlay(false);
        }

        

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            homeController.ConfirmOrder(paymentmethod, status, purchasemethod);
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
