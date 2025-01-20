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
using System.Windows.Media.Animation;
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
        ExpensesServices expensesServices;
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
            NotifPopup.Visibility = Visibility.Hidden;
        }
        public Add_FinancialLiabilities(FinancialLiabilities itemrow, MainWindow mainwindow)
        {
            InitializeComponent();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            salesServices = new SalesServices(context);
            deliveriesServices = new DeliveriesServices(context);
            supplierOrdersServices = new SupplierOrdersServices(context);
            expensesServices = new ExpensesServices(context);

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
            NotifPopup.Visibility = Visibility.Hidden;
        }
        public void PopUpNotif(string type, string message)
        {
            NotifPopup.Visibility = Visibility.Visible;
            Panel.SetZIndex(NotifPopup, int.MaxValue);
            if (type == "notif")
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
            }
            else
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
            }

            NotifMessage.Content = message;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromSeconds(4.5),
                Duration = TimeSpan.FromMilliseconds(500)
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            storyboard.Children.Add(fadeOut);

            Storyboard.SetTarget(fadeIn, NotifPopup);
            Storyboard.SetTarget(fadeOut, NotifPopup);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));

            storyboard.Completed += (sender, args) =>
            {
                NotifPopup.Visibility = Visibility.Collapsed;
            };
            storyboard.Begin();
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

            bool isUpdated = financialLiabilitiesServices.EditPayment(finance.Id, name, price, type, paymode, date, contacts, "");
            if (!isUpdated) 
            {
                PopUpNotif("alert", "Update Unsuccessfull");
            }
            PopUpNotif("notif", "Update Successfull");
            Agenda = "Update";
            ConfirmBtn.Content = "PAID";
            EnabledForm(false);
            mainWindow.ScheduleUpdateReload();
        }
        public void MarkAsPaid()
        {
            // Update financial liabilities
            bool financeupdate = financialLiabilitiesServices.MarkAsPaid(finance.Id, "Mark as Paid");
            if (!financeupdate)
            {
                PopUpNotif("alert", "Failed to update financial liabilities.");
                return;
            }

            bool salesupdate = true;
            bool deliveryupdate = true;
            bool CreateExpense = true;

            // Parse finance.amount safely
            if (!decimal.TryParse(finance.amount.ToString(), out var amount))
            {
                PopUpNotif("alert", "Invalid finance amount.");
                return;
            }

            // Handle 'To Pay' type or orders with IDs
            if (finance.type == "To Pay")
            {
                CreateExpense = expensesServices.Create(
                    NameTextBox.Text, "BILL", "DONE", UserContext.CurrentUserId,
                    "Payment done.", amount, 0);
            }
            else if (finance.order_id != 0)
            {
                salesupdate = salesServices.MarkAsPaid(finance.order_id, "Mark as Paid");
                deliveryupdate = deliveriesServices.MarkAsPaid(finance.order_id, "Mark as Paid");

                if (!int.TryParse(finance.order_id.ToString(), out var orderId))
                {
                    PopUpNotif("alert", "Invalid order ID.");
                    return;
                }

                CreateExpense = expensesServices.Create(
                    NameTextBox.Text, "BILL", "DONE", UserContext.CurrentUserId,
                    "Payment done.", amount, orderId);
            }

            // Check for any failures
            if (!salesupdate)
            {
                PopUpNotif("alert", "Failed to update sales.");
                return;
            }

            if (!deliveryupdate)
            {
                PopUpNotif("alert", "Failed to update deliveries.");
                return;
            }

            if (!CreateExpense)
            {
                PopUpNotif("alert", "Failed to create expense.");
                return;
            }

            // Success actions
            this.Close();
            mainWindow.ActiveOverlay(false);
            mainWindow.ScheduleUpdateReload();
            mainWindow.PopUpNotif("notif", "Marked as paid successfully.");
        }


        public void AddScheduledPayment()
        {
            string name = NameTextBox.Text;
            decimal price = Decimal.Parse(PriceTextBox.Text);
            DateTime dueDate = DateTime.Parse(datePicker.Text);
            string contacts = ContactsTextBox.Text;

            bool createNewSched = financialLiabilitiesServices.Create(name, orderid, price, type, mode, dueDate, contacts, "Added payemnt schedule.");
            if (!createNewSched)
            {
                PopUpNotif("alert", "Not Created");
            }
            

            if (mainWindow != null)
            {
                mainWindow.DynamicAddFinance();
            }
            else
            {
                PopUpNotif("alert", "Unable to access the MainWindow. add financial");
            }
            this.Close();
            mainWindow.ActiveOverlay(false);
            mainWindow.PopUpNotif("notif", "Payment schedule added successfully.");
            mainWindow.ScheduleUpdateReload();
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

        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }
    }
}
