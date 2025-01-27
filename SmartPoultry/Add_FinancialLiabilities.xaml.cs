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
        UserLogsServices userLogsServices;
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
            userLogsServices = new UserLogsServices(context);
            datePicker.SelectedDate = DateTime.Now.AddDays(14);
            orderid = 0;
            EditBtn.Visibility = Visibility.Collapsed;

            orderIdLabel.Visibility = Visibility.Collapsed;
            OrderDetailsBtn.Visibility = Visibility.Collapsed;
            OrderId.Visibility = Visibility.Collapsed;

            mainWindow = UserContext.mainWindow;
            NotifPopup.Visibility = Visibility.Hidden;
            RemarksBtn.Visibility = Visibility.Collapsed;
        }
        public Add_FinancialLiabilities(FinancialLiabilities itemrow, MainWindow mainwindow)
        {
            InitializeComponent();
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            userLogsServices = new UserLogsServices(context);
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
            ToPayRBtn.IsEnabled = false;
            ToReceiveRBtn.IsEnabled = false;
            NotifPopup.Visibility = Visibility.Hidden;
            if(finance.status == "paid")
            {
                Paid();
            }
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


        public void Paid()
        {
            NameTextBox.IsReadOnly = true;
            PriceTextBox.IsReadOnly = true;
            datePicker.IsEnabled = false;
            ContactsTextBox.IsReadOnly = true;

            ToPayRBtn.IsEnabled = false;
            ToReceiveRBtn.IsEnabled = false;
            
            ConfirmBtn.Visibility = Visibility.Collapsed;

            CashRBtn.IsEnabled = false;
            GCashRBtn.IsEnabled = false;
            EditBtn.Visibility = Visibility.Hidden;
        }
        public void EnabledForm(bool isEnabled)
        {
            NameTextBox.IsEnabled = isEnabled;
            PriceTextBox.IsEnabled = isEnabled;
            datePicker.IsEnabled = isEnabled;
            ContactsTextBox.IsEnabled = isEnabled;
            
            CashRBtn.IsEnabled = isEnabled;
            GCashRBtn.IsEnabled = isEnabled;

            if (finance.order_id != 0) 
            {
                PriceTextBox.IsEnabled = false;
            }
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

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RemarksTextBox.Text = finance.Remarks.ToString();
            RemarksTextBox.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RemarksTextBox.Visibility = Visibility.Collapsed;
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
            Remarks_Popup remarksPopup = new Remarks_Popup();
            ActiveOverlay(true);
            string remarksInput = null;

            if (remarksPopup.ShowDialog() == true)
            {
                remarksInput = remarksPopup.Remarks;
            }
            else
            {

                ActiveOverlay(false);
                return;
            }


            ActiveOverlay(false);

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

            bool isUpdated = financialLiabilitiesServices.EditPayment(finance.Id, name, price, type, paymode, date, contacts, remarksInput);
            bool isCreated = userLogsServices.Create(UserContext.CurrentUserId, "PAYMENT", $"Edit {finance.name}: {remarksInput}");
            if (!isUpdated || !isCreated) 
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
            bool financeupdate = financialLiabilitiesServices.MarkAsPaid(finance.Id, "Mark as Paid");
            if (!financeupdate)
            {
                PopUpNotif("alert", "Failed to update financial liabilities.");
                return;
            }

            bool salesupdate = true;
            bool deliveryupdate = true;
            bool CreateExpense = true;


            if (!decimal.TryParse(finance.amount.ToString(), out var amount)) 
            {
                PopUpNotif("alert", "Invalid finance amount.");
                return;
            }


            if (finance.type == "To Pay" && finance.order_id != 0)
            {
                CreateExpense = expensesServices.Create(
                    NameTextBox.Text, "SUPPLY", "DONE", UserContext.CurrentUserId,
                    "Payment done.", amount, finance.order_id, finance.added_date);
            }
            else if (finance.order_id != 0)
            {
                salesupdate = salesServices.MarkAsPaid(finance.order_id, "Mark as Paid");
                deliveryupdate = deliveriesServices.MarkAsPaid(finance.order_id, "Mark as Paid");



                
            }
            else if (finance.order_id == 0) 
            {
                CreateExpense = expensesServices.Create(
                    NameTextBox.Text, "BILL", "DONE", UserContext.CurrentUserId,
                    "Payment done.", amount, long.Parse(finance.order_id.ToString()), finance.added_date);
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

            bool isCreated = userLogsServices.Create(UserContext.CurrentUserId, "PAYMENT", $"Edit {finance.name}: Mark as Paid");

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
            else 
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

        private void ContactsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleNumericInput(ContactsTextBox, false, 11);
        }
        private void HandleNumericInput(TextBox textBox, bool allowDecimal, int NumberLimit)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            string input = textBox.Text;

            string filteredInput = allowDecimal
                ? new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray())
                : new string(input.Where(char.IsDigit).ToArray());

            if (allowDecimal)
            {
                int decimalIndex = filteredInput.IndexOf('.');

                if (decimalIndex == -1)
                {
                    if (filteredInput.Length > NumberLimit)
                    {
                        filteredInput = filteredInput.Substring(0, NumberLimit);
                    }
                }
                else
                {
                    string wholePart = filteredInput.Substring(0, decimalIndex);
                    string decimalPart = filteredInput.Substring(decimalIndex + 1);
                    if (wholePart.Length > NumberLimit)
                    {
                        wholePart = wholePart.Substring(0, NumberLimit);
                    }

                    if (decimalPart.Length > 2)
                    {
                        decimalPart = decimalPart.Substring(0, 2);
                    }

                    filteredInput = $"{wholePart}.{decimalPart}";
                }

                int firstDecimalIndex = filteredInput.IndexOf('.');
                if (firstDecimalIndex != -1)
                {
                    filteredInput = filteredInput.Substring(0, firstDecimalIndex + 1) +
                                    filteredInput.Substring(firstDecimalIndex + 1).Replace(".", "");
                }
            }
            else
            {

                if (filteredInput.Length > NumberLimit)
                {
                    filteredInput = filteredInput.Substring(0, NumberLimit);
                }
            }

            if (input != filteredInput)
            {
                textBox.Text = filteredInput;
                textBox.CaretIndex = filteredInput.Length;
            }
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleNumericInput(PriceTextBox, true, 7);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
