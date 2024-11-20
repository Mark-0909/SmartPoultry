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
        public Add_FinancialLiabilities()
        {
            InitializeComponent();
            AppDbContext context = new AppDbContext();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            datePicker.SelectedDate = DateTime.Now;
        }
        public void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ToPay_IsChecked(object sender, RoutedEventArgs e)
        {
            type = "To Pay";
        }

        private void ToReceive_IsChecked(object sender, RoutedEventArgs e)
        {
            type = "To Receieve";
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

            bool createNewSched = financialLiabilitiesServices.Create(name, price, type, mode, dueDate, contacts);
            if (!createNewSched) {
                MessageBox.Show("Not Created");
            }
            MessageBox.Show("Success");
            this.Close();
        }
    }
}
