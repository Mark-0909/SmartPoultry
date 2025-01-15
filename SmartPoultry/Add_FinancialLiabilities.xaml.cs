using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
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
using System.Windows.Shapes;
using static SmartPoultry.App;
namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Add_FinancialLiabilities.xaml
    /// </summary>
    public partial class Add_FinancialLiabilities : Window
    {
        FinancialLiabilitiesServices financialLiabilitiesServices;
        SalesServices salesServices;
        DeliveriesServices deliveriesServices;
        SupplierOrdersServices supplierOrdersServices;
        AppDbContext context = new AppDbContext();

        FinancialLiabilities finance;
        public string mode;
        public string type;
        public long orderid;

        public MainWindow mainWindow;

        string Agenda = "Update";

        public Add_FinancialLiabilities(MainWindow window)
        {
            InitializeComponent();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            datePicker.SelectedDate = DateTime.Now.AddDays(14);
            orderid = 0;
            EditBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;

            orderIdLabel.Visibility = Visibility.Collapsed;
            OrderDetailsBtn.Visibility = Visibility.Collapsed;
            OrderId.Visibility = Visibility.Collapsed;

            mainWindow = UserContext.mainWindow;
        }
        public Add_FinancialLiabilities(FinancialLiabilities itemrow, MainWindow mainwindow)
        {
            InitializeComponent();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            salesServices = new SalesServices(context);
            deliveriesServices = new DeliveriesServices(context);
            supplierOrdersServices = new SupplierOrdersServices(context);

            if (itemrow.order_id != 0) 
            {
                OrderId.Content = itemrow.order_id.ToString();
            }
            else
            {
                orderIdLabel.Visibility = Visibility.Collapsed;
                OrderDetailsBtn.Visibility = Visibility.Collapsed;
                OrderId.Visibility = Visibility.Collapsed;
            }

            NameTextBox.Text = itemrow.name;
            finance = itemrow;

            PriceTextBox.Text = itemrow.amount.ToString("N2");
            if (itemrow.type == "To Receive")
            {
                ToReceiveRBtn.IsChecked = true;
            }
            if (itemrow.payment_mode == "GCash")
            {
                GCashRBtn.IsChecked = true;
            }
            datePicker.SelectedDate = itemrow.due_date;
            ContactsTextBox.Text = itemrow.contacts;
            ConfirmBtn.Content = "PAID";
            mainWindow = UserContext.mainWindow;

            EnabledForm(false);
        }

        public void EnabledForm(bool isEnabled)
        {
            NameTextBox.IsEnabled = isEnabled;
            PriceTextBox.IsEnabled = isEnabled;
            datePicker.IsEnabled = isEnabled;
            ContactsTextBox.IsEnabled = isEnabled;
            ToPayRBtn.IsEnabled = isEnabled;
            ToReceiveRBtn.IsEnabled = isEnabled;
            CashRBtn.IsEnabled = isEnabled;
            GCashRBtn.IsEnabled = isEnabled;
        }

        public void DatePicker_Loaded(object sender, RoutedEventArgs e)
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
            if(ConfirmBtn.Content.ToString() == "PAID")
            {
                MarkAsPaid();
            }
            else if(ConfirmBtn.Content.ToString() == "CONFIRM")
            {
                AddScheduledPayment();
            }else if (ConfirmBtn.Content.ToString() == "UPDATE")
            {
                UpdatePayment();
            }
        }
        public void UpdatePayment()
        {
            string name = NameTextBox.Text;
            decimal price = decimal.Parse(PriceTextBox.Text);
            string type = "To Receive";
            if(ToPayRBtn.IsChecked == true)
            {
                type = "To Pay";
            }
            string paymode = "Cash";
            if(GCashRBtn.IsChecked == true)
            {
                paymode = "GCash";
            }
            DateTime date = datePicker.SelectedDate.Value;
            string contacts = ContactsTextBox.Text;

            bool isUpdated = financialLiabilitiesServices.EditPayment(finance.Id, name, price, type, paymode, date, contacts);
            if (!isUpdated) 
            {
                MessageBox.Show("Update Unsuccessfull");
            }
            MessageBox.Show("Update Successfull");
            Agenda = "Update";
            ConfirmBtn.Content = "PAID";
            EnabledForm(false);
            mainWindow.ScheduleUpdateReload();
        }
        public void MarkAsPaid()
        {
            bool financeupdate = financialLiabilitiesServices.MarkAsPaid(finance.Id);
            if (finance.order_id != 0)
            {

                bool salesupdate = salesServices.MarkAsPaid(finance.order_id);
                bool deliveryupdate = deliveriesServices.MarkAsPaid(finance.order_id);
                if (!financeupdate || !salesupdate || !deliveryupdate)
                {
                    MessageBox.Show("Error");
                    return;
                }
                this.Close();
                mainWindow.ActiveOverlay(false);
                mainWindow.ScheduleUpdateReload();
            }
        }

        public void AddScheduledPayment()
        {
            string name = NameTextBox.Text;
            decimal price = Decimal.Parse(PriceTextBox.Text);
            DateTime dueDate = DateTime.Parse(datePicker.Text);
            string contacts = ContactsTextBox.Text;

            bool createNewSched = financialLiabilitiesServices.Create(name, orderid, price, type, mode, dueDate, contacts);
            if (!createNewSched)
            {
                MessageBox.Show("Not Created");
            }
            MessageBox.Show("Success");

            if (mainWindow != null)
            {
                mainWindow.DynamicAddFinance();
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow. add financial");
            }
            this.Close();
            mainWindow.ActiveOverlay(false);
        }


        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Agenda == "Update")
            {
                Agenda = "Edit";
                ConfirmBtn.Content = "UPDATE";
                EnabledForm(true);
            }
            else
            {
                Agenda = "Update";
                ConfirmBtn.Content = "PAID";
                EnabledForm(false);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
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

        private void OrderDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = UserContext.mainWindow;
            if (finance.type == "To Receive")
            {
                Sales sale = salesServices.GetSales(long.Parse(OrderId.Content.ToString()));
                Sales_OrderInfo window = new Sales_OrderInfo(sale, main, "payment", this);
                ActiveOverlay(true);
                window.ShowDialog();
            }
            else 
            {
                SupplierOrders supp = supplierOrdersServices.GetById(int.Parse(OrderId.Content.ToString()));
                Supplier_OrderInfo window = new Supplier_OrderInfo(supp, this);
                ActiveOverlay(true);
                window.ShowDialog();
            }
        }
        public void ActiveOverlay(bool isActive)
        {
            if (isActive)
            {
                Overlay.Visibility = Visibility.Visible;

                Panel.SetZIndex(Overlay, 99);
            }
            else
            {
                Overlay.Visibility = Visibility.Collapsed;
                Panel.SetZIndex(Overlay, 0);
            }
        }
    }
}
