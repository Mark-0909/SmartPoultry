using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Migrations;
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
    /// Interaction logic for Add_FinancialLiabilities.xaml
    /// </summary>
    public partial class Add_FinancialLiabilities : Window
    {
        FinancialLiabilitiesServices financialLiabilitiesServices;
        public string mode;
        public string type;
        public long orderid;

        MainWindow mainWindow { get; set; }
        public Add_FinancialLiabilities(MainWindow window)
        {
            InitializeComponent();
            AppDbContext context = new AppDbContext();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            datePicker.SelectedDate = DateTime.Now.AddDays(14);
            OrderIDTextBox.IsEnabled = false;
            orderid = 0;

            mainWindow = window;
        }
        public void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ActiveOverlay(false);
        }
        private void ToPay_IsChecked(object sender, RoutedEventArgs e)
        {
            type = "To Pay";
        }

        private void ToReceive_IsChecked(object sender, RoutedEventArgs e)
        {
            type = "To Receive";
        }

        private void Cash_IsChecked(object sender, RoutedEventArgs e)
        {
            mode = "Cash";
        }

        private void GCash_IsChecked(object sender, RoutedEventArgs e)
        {
            mode = "GCash";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            decimal price = Decimal.Parse(PriceTextBox.Text);
            DateTime dueDate = DateTime.Parse(datePicker.Text);
            string contacts = ContactsTextBox.Text;

            bool createNewSched = financialLiabilitiesServices.Create(name, orderid, price, type, mode, dueDate, contacts);
            if (!createNewSched) {
                MessageBox.Show("Not Created");
            }
            MessageBox.Show("Success");

            if (mainWindow != null)
            {
                mainWindow.DynamicAddFinance();
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.");
            }
            this.Close();
            mainWindow.ActiveOverlay(false);
        }
        private void NameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(NameTextBox, "Name...", true);
        }
        private void NameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(NameTextBox, "Name...", false);
        }
        private void OrderTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(OrderIDTextBox, "Order ID...", true);
        }
        private void OrderTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(OrderIDTextBox, "Order ID...", false);
        }
        private void PriceTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(PriceTextBox, "Price...", true);
        }
        private void PriceTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(PriceTextBox, "Price...", false);
        }
        private void ContactTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ContactsTextBox, "Contacts...", true);
        }
        private void ContactTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ContactsTextBox, "Contacts...", false);
        }
        public void HandleTextBoxPlaceholder(TextBox tb, string placeholder, bool isFocused)
        {
            if (isFocused)
            {
                if (tb.Text == placeholder)
                {
                    tb.Text = string.Empty;
                    tb.Foreground = Brushes.Black;
                }
            }
            else // When the TextBox loses focus
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        
    }
}
