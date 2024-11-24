using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Add_Delivery.xaml
    /// </summary>
    public partial class Add_Delivery : Window
    {
        DeliveriesServices deliveriesServices;
        public AppDbContext context = new AppDbContext();

        public MainWindow? mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public long orderId = 0;
        public string type;
        public string mode;
        public string status;

        public Add_Delivery()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;
            OrderIdTextBox.IsEnabled = false;
            toDeliverRadio.IsEnabled = false;
            toReceiveRadio.IsChecked = true;
            deliveriesServices = new DeliveriesServices(context);
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string price = PriceTextBox.Text;
            string address = AddressTextBox.Text;
            string contacts = ContactsTextBox.Text;
            string deliveryman = DeliveryManTextBox.Text;
            DateTime? selectedDate = datePicker.SelectedDate;
            DateTime dateTime;


            if (selectedDate.HasValue)
            {
                dateTime = selectedDate.Value;
            }
            else
            {

                MessageBox.Show("Please select a date.");
                return;
            }

            decimal charge;
            if (!decimal.TryParse(ChargeTextBox.Text, out charge))
            {
                MessageBox.Show("Please enter a valid charge.");
                return;
            }



            bool added = deliveriesServices.Create(orderId, name, type, decimal.Parse(price), address, status, contacts, dateTime, deliveryman, charge);
            if (added)
            {
                MessageBox.Show("Successful!");
                mainWindow.DynamicAddDeliveries();
                this.Close();
            }
            else {
                MessageBox.Show("Unsuccessful!");
            }
        
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }

        private void ToDeliver_IsChecked(object sender, RoutedEventArgs e)
        {
            if (OrderIdTextBox != null && !OrderIdTextBox.IsEnabled)
            {
                OrderIdTextBox.IsReadOnly = false;  // Make it editable
                OrderIdTextBox.IsEnabled = true;    // Enable the control
            }
            type = "To Deliver";
        }

        private void ToReceive_IsChecked(object sender, RoutedEventArgs e)
        {
            if (OrderIdTextBox != null)
            {
                OrderIdTextBox.IsEnabled = false;
            }
            type = "To Receive";
        }

        private void CashRadio_IsChecked(object sender, RoutedEventArgs e)
        {
            mode = "Cash";
        }

        private void GCashRadio_IsChecked(object sender, RoutedEventArgs e)
        {
            mode = "GCash";
        }

        private void PaidRadio_IsChecked(object sender, RoutedEventArgs e)
        {
            status = "Paid";
        }

        private void UnpaidRadio_IsChecked(object sender, RoutedEventArgs e)
        {
            status = "Nnpaid";
        }
    }
}
