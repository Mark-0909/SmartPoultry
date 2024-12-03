using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Add_Delivery.xaml
    /// </summary>
    public partial class Add_Delivery : Window
    {
        DeliveriesServices deliveriesServices;
        public AppDbContext context = new AppDbContext();

        public MainWindow mainWindow { get; set; }

        public long orderId = 0;
        public string type;
        public string mode;
        public string status;

        public Add_Delivery(MainWindow window)
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;
            OrderIdTextBox.IsEnabled = false;
            toDeliverRadio.IsEnabled = false;
            toReceiveRadio.IsChecked = true;
            deliveriesServices = new DeliveriesServices(context);
            
            mainWindow = window;
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
                
                this.Close();
                if (mainWindow != null)
                {
                    mainWindow.DynamicAddDeliveries();
                    mainWindow.ActiveOverlay(false);
                }
                else
                {
                    MessageBox.Show("Unable to access the MainWindow.");
                }
            }
            else {
                MessageBox.Show("Unsuccessful!");
            }
        
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ActiveOverlay(false);
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

        private void ContactsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            HandleTextBoxPlaceholder(OrderIdTextBox, "Order ID...", true);
        }
        private void OrderTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(OrderIdTextBox, "Order ID...", false);
        }
        private void AddressTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(AddressTextBox, "Address...", true);
        }
        private void AddressTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(AddressTextBox, "Address...", false);
        }
        private void PriceTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(PriceTextBox, "Price...", true);
        }
        private void PriceTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(PriceTextBox, "Price...", false);
        }
        private void DeliveryManTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(DeliveryManTextBox, "Delivery Man...", true);
        }
        private void DeliveryManTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(DeliveryManTextBox, "Delivery Man...", false);
        }
        private void ChargeTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ChargeTextBox, "Charge fee...", true);
        }
        private void ChargeTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ChargeTextBox, "Charge fee...", false);
        }
        private void ContactsTB_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ContactsTextBox, "Contact...", true);
        }
        private void ContactsTB_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(ContactsTextBox, "Contact...", false);
        }
        private void HandleNumericInput(TextBox textBox, bool allowDecimal)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            string input = textBox.Text;

            // Filter input based on whether decimals are allowed
            string filteredInput = allowDecimal
                ? new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray())
                : new string(input.Where(char.IsDigit).ToArray());

            // Allow only one decimal point
            if (allowDecimal)
            {
                int firstDecimalIndex = filteredInput.IndexOf('.');
                if (firstDecimalIndex != -1)
                {
                    filteredInput = filteredInput.Substring(0, firstDecimalIndex + 1) +
                                    filteredInput.Substring(firstDecimalIndex + 1).Replace(".", "");
                }
            }

            // Update the TextBox only if input has changed
            if (input != filteredInput)
            {
                textBox.Text = filteredInput;
                textBox.CaretIndex = filteredInput.Length;
            }
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
