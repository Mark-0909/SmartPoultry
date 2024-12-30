using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
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
        SalesServices salesServices;
        FinancialLiabilitiesServices financialLiabilitiesServices;
        public AppDbContext context = new AppDbContext();

        Deliveries deliveries;

        public MainWindow mainWindow { get; set; }

        public long orderId = 0;
        public string type;
        public string mode;
        public string status;

        string Agenda = "Update";

        public Add_Delivery(MainWindow window)
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;

            toDeliverRadio.IsEnabled = false;
            toReceiveRadio.IsChecked = true;
            deliveriesServices = new DeliveriesServices(context);
            salesServices = new SalesServices(context);
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            
            mainWindow = window;
        }

        public Add_Delivery(Deliveries itemrow, MainWindow window)
        {
            InitializeComponent();
            deliveriesServices = new DeliveriesServices(context);
            salesServices = new SalesServices(context);
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            deliveries = itemrow;

            NameTextBox.Text = itemrow.name;
            AddressTextBox.Text = itemrow.address;
            PriceTextBox.Text = itemrow.price.ToString("N2");
            datePicker.SelectedDate = itemrow.delivery_date;
            ContactsTextBox.Text = itemrow.contact_no;
            ChargeTextBox.Text = itemrow.charges.ToString("N2");

            if(itemrow.payment_status == "unpaid")
            {
                UnpaidRadio.IsChecked = true;
            }
            if(itemrow.type == "To Receive")
            {
                toReceiveRadio.IsChecked = true;
            }

            mainWindow = window;
            confirmBtn.Content = "DELIVERED";

            if(itemrow.type == "To Receive")
            {
                DeliveryManTextBox.Text = itemrow.name;
            }

            PaidRadio.IsEnabled = false;
            UnpaidRadio.IsEnabled = false;
            EnableForm(false);
        }
        public void EnableForm(bool isEnabled)
        {
            NameTextBox.IsEnabled = isEnabled;
            AddressTextBox.IsEnabled = isEnabled;
            PriceTextBox.IsEnabled = isEnabled;
            toDeliverRadio.IsEnabled = isEnabled;
            toReceiveRadio.IsEnabled = isEnabled;
            datePicker.IsEnabled = isEnabled;
            PriceTextBox.IsEnabled = isEnabled;
            ContactsTextBox.IsEnabled = isEnabled;
            ChargeTextBox.IsEnabled = isEnabled;
            DeliveryManTextBox.IsEnabled = !isEnabled;
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (confirmBtn.Content.ToString() == "CONFIRM")
            {
                AddScheduledDelivery();
            }
            else if (confirmBtn.Content.ToString() == "DELIVERED")
            {
                MarkAsDelivered();
            } 
            else if (confirmBtn.Content.ToString() == "UPDATE")
            {
                EditDelivery();
            }
            else
            {
                MessageBox.Show("Unexpected button action. Please check the button state.");
            }
        }

        public void MarkAsDelivered()
        {
            if(DeliveryManTextBox.Text == DeliveryManTextBox.Tag.ToString())
            {
                MessageBox.Show("Add Delivery Man");
                return;
            }
            bool updatedelivery = deliveriesServices.UpdateDelivered(deliveries.Id);
            bool updateSale = salesServices.UpdateDelivered(deliveries.order_id);
            if (!updatedelivery || !updateSale) 
            {
                MessageBox.Show("Unsuccessfull.");
                return;
            }
            if(deliveries.order_id != 0 && deliveries.payment_status == "unpaid")
            {
                FinancialLiabilities finance = financialLiabilitiesServices.GetByReceipt(deliveries.order_id);

                Add_FinancialLiabilities payment = new Add_FinancialLiabilities(finance, mainWindow);
                payment.Show();
                this.Close();
                mainWindow.ScheduleUpdateReload();
                return;
            }
            this.Close();
            mainWindow.ActiveOverlay(false);
            mainWindow.ScheduleUpdateReload();

        }
        public void EditDelivery()
        {
            string name = NameTextBox.Text;
            string address = AddressTextBox.Text;
            string type = "To Deliver";
            if (toReceiveRadio.IsChecked == true)
            {
                type = "To Receive";
            }
            DateTime date = datePicker.SelectedDate.Value;
            decimal price = decimal.Parse(PriceTextBox.Text);
            string contacts = ContactsTextBox.Text;
            decimal charge = decimal.Parse(ChargeTextBox.Text);

            bool UpdateDelivery = deliveriesServices.UpdateDelivery(deliveries.Id, name, address, type, date, price, contacts, charge);

            if (!UpdateDelivery) 
            {
                MessageBox.Show("Update Unsuccessfull");
                return;
            }
            MessageBox.Show("Update Successfull");
            mainWindow.ScheduleUpdateReload();
            EnableForm(false);
            Agenda = "Update";
            confirmBtn.Content = "DELIVERED";
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Agenda == "Update")
            {
                Agenda = "Edit";
                confirmBtn.Content = "UPDATE";
                EnableForm(true);
            }
            else
            {
                Agenda = "Update";
                confirmBtn.Content = "DELIVERED";
                EnableForm(false);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        public void AddScheduledDelivery()
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
            else
            {
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
            datePicker.BlackoutDates.Clear();
            DateTime today = DateTime.Today;
            DateTime? specificPastDate = datePicker.SelectedDate;

            if (specificPastDate.HasValue)
            {
                DateTime pastDate = specificPastDate.Value;

                if (pastDate < today)
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, pastDate.AddDays(-1)));
                    datePicker.BlackoutDates.Add(new CalendarDateRange(pastDate.AddDays(1), today.AddDays(-1)));
                }
                else
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, today.AddDays(-1)));
                }
            }
            else
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, today.AddDays(-1)));
            }
        }

        private void ToDeliver_IsChecked(object sender, RoutedEventArgs e)
        {
            type = "To Deliver";
        }

        private void ToReceive_IsChecked(object sender, RoutedEventArgs e)
        {
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
