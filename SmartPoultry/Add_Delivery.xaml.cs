using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static SmartPoultry.App;

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
        SupplierOrdersServices supplierOrdersServices;
        ProductServices productServices;
        ExpensesServices expensesServices;
        UserLogsServices userLogsServices;
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
            productServices = new ProductServices(context);
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            userLogsServices = new UserLogsServices(context);
            expensesServices = new ExpensesServices(context);
            
            EditBtn.Visibility = Visibility.Hidden;

            
            mainWindow = UserContext.mainWindow;
            NotifPopup.Visibility = Visibility.Hidden;
        }

        public Add_Delivery(Deliveries itemrow, MainWindow window)
        {
            InitializeComponent();
            deliveriesServices = new DeliveriesServices(context);
            salesServices = new SalesServices(context);
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            supplierOrdersServices = new SupplierOrdersServices(context);
            productServices = new ProductServices(context);
            userLogsServices = new UserLogsServices(context);
            expensesServices = new ExpensesServices(context);

            deliveries = itemrow;

            NameTextBox.Text = itemrow.name;
            AddressTextBox.Text = itemrow.address;
            PriceTextBox.Text = itemrow.price.ToString("N2");
            datePicker.SelectedDate = itemrow.delivery_date;
            ContactsTextBox.Text = itemrow.contact_no;
            ChargeTextBox.Text = itemrow.charges.ToString("N2");

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

            Status.Visibility = Visibility.Hidden;
            PaidRadio.Visibility = Visibility.Hidden;
            UnpaidRadio.Visibility = Visibility.Hidden;

            if (itemrow.payment_status == "unpaid")
            {
                UnpaidRadio.IsChecked = true;
            }
            if(itemrow.type == "To Receive")
            {
                toReceiveRadio.IsChecked = true;
                Status.Visibility = Visibility.Visible;
                PaidRadio.Visibility = Visibility.Visible;
                UnpaidRadio.Visibility = Visibility.Visible;
            }

            mainWindow = UserContext.mainWindow;
            confirmBtn.Content = "DELIVERED";

            if(itemrow.type == "To Receive")
            {
                DeliveryManTextBox.Text = itemrow.name;
            }

            
            EnableForm(false);
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


            if (deliveries.type == "To Receive")
            {
                PriceTextBox.IsEnabled = true;
                datePicker.IsEnabled = true;
            }
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
                PopUpNotif("alert", "Unexpected button action. Please check the button state.");
            }
        }

        public void MarkAsDelivered()
        {
            if (string.IsNullOrWhiteSpace(DeliveryManTextBox.Text) ||
                DeliveryManTextBox.Text == DeliveryManTextBox.Tag?.ToString())
            {
                PopUpNotif("alert", "Add Delivery Man");
                return;
            }

            if (deliveries.payment_status == "pending")
            {
                if (string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    PriceTextBox.Text == PriceTextBox.Tag?.ToString() ||
                    PriceTextBox.Text == "0.00")
                {
                    PopUpNotif("alert", "Fill up the price.");
                    return;
                }

                if (status == "unpaid")
                {
                    bool isCreated = financialLiabilitiesServices.Create(
                        NameTextBox.Text,
                        deliveries.order_id,
                        decimal.TryParse(PriceTextBox.Text, out var price) ? price : 0,
                        "To Pay",
                        "Cash",
                        datePicker.SelectedDate ?? DateTime.Now,
                        ContactsTextBox.Text, 
                        "Added Payement schedule."
                    );

                    if (!isCreated)
                    {
                        PopUpNotif("alert", "Finance add unsuccessful");
                        return;
                    }
                }
            }

            bool updatedDelivery = deliveriesServices.UpdateDelivered(deliveries.Id, DeliveryManTextBox.Text, "Update Delivery");
            bool updateSale = deliveries.type != "To Deliver" || salesServices.UpdateDelivered(deliveries.order_id, "Update Delivery");
            bool IsPriceUpdated = supplierOrdersServices.UpdatePrice(int.Parse(OrderId.Content.ToString()), decimal.Parse(PriceTextBox.Text.ToString()));
           

            if (!updatedDelivery || !updateSale || !IsPriceUpdated)
            {
                PopUpNotif("alert", "Unsuccessful.");
                return;
            }

            if (deliveries.order_id != 0 && deliveries.payment_status == "unpaid")
            {
                FinancialLiabilities finance = financialLiabilitiesServices.GetByReceipt(deliveries.order_id);
                if (finance != null)
                {
                    var payment = new Add_FinancialLiabilities(finance, mainWindow);
                    payment.Show();
                }

                this.Close();
                mainWindow.ScheduleUpdateReload();
                return;
            }

            if (!long.TryParse(deliveries.order_id.ToString(), out var orderId))
            {
                PopUpNotif("alert", "Invalid Order ID.");
                return;
            }

            if(status == "paid")
            {
                bool isCreatedExpense = expensesServices.Create(
                    NameTextBox.Text,
                    "SUPPLY",
                    "DONE",
                    UserContext.CurrentUserId,
                    "Order supply delivered.",
                    decimal.Parse(PriceTextBox.Text),
                    int.Parse(deliveries.order_id.ToString()),
                    deliveries.added_date == DateTime.MinValue ? DateTime.Now : deliveries.added_date
                );

                if (!isCreatedExpense)
                {
                    PopUpNotif("alert", "Creating expense row unsuccessful.");
                    return;
                }
            }

            SupplierOrders order = supplierOrdersServices.GetById(int.Parse(orderId.ToString()));
            if (order == null)
            {
                PopUpNotif("alert", "Order not found.");
                return;
            }

            List<string> ids = order.productList.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> qty = order.orderQty.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < ids.Count; i++)
            {
                if (!int.TryParse(ids[i], out var productId) ||
                    !decimal.TryParse(qty[i], out var quantity))
                {
                    PopUpNotif("alert", $"Invalid product or quantity for ID: {ids[i]}.");
                    continue;
                }

                decimal newStock = productServices.UpdateStockAfterDelivery(productId, quantity);

                if (newStock == -1)
                {
                    PopUpNotif("alert", $"Stock update failed for Product ID: {productId}");
                    continue;
                }

                mainWindow.inventoryControl.UpdateStocksAfterSupplierDeliver(productId, newStock);
            }
            mainWindow.homeControl.DynamicReload();


            bool isCreatedUserlogs = userLogsServices.Create(UserContext.CurrentUserId, "DELIVERIES", $"Edit {deliveries.name}: Mark as Delivered");
            this.Close();
            mainWindow.ActiveOverlay(false);
            mainWindow.ScheduleUpdateReload();
            mainWindow.PopUpNotif("notif", "Order Delivered!");
            mainWindow.dashboardControl.DynamicUpdateCharts();



        }


        public void EditDelivery()
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

            bool UpdateDelivery = deliveriesServices.UpdateDelivery(deliveries.Id, name, address, type, date, price, contacts, charge, remarksInput);
            bool isCreated = userLogsServices.Create(UserContext.CurrentUserId, "DELIVERIES", $"Edit {deliveries.name}: {remarksInput}");

            if (!UpdateDelivery || !isCreated) 
            {
                PopUpNotif("alert", "Update Unsuccessful");
                return;
            }
            PopUpNotif("notif", "Update Successful");
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

                PopUpNotif("alert", "Please select a date.");
                return;
            }

            decimal charge;
            if (!decimal.TryParse(ChargeTextBox.Text, out charge))
            {
                PopUpNotif("alert", "Please enter a valid charge.");
                return;
            }



            bool added = deliveriesServices.Create(orderId, name, type, decimal.Parse(price), address, status, contacts, dateTime, deliveryman, charge);
            if (added)
            {
                PopUpNotif("notif", "Successful!");

                this.Close();
                if (mainWindow != null)
                {
                    mainWindow.DynamicAddDeliveries();
                    mainWindow.ActiveOverlay(false);
                }
                else
                {
                    PopUpNotif("alert", "Unable to access the MainWindow. add delivery");
                }
            }
            else
            {
                PopUpNotif("alert", "Unsuccessful!");
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
            status = "paid";
        }

        private void UnpaidRadio_IsChecked(object sender, RoutedEventArgs e)
        {
            status = "unpaid";
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

        private void OrderInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = UserContext.mainWindow;
            if (deliveries.type == "To Deliver")
            {
                Sales sale = salesServices.GetSales(long.Parse(OrderId.Content.ToString()));
                Sales_OrderInfo window = new Sales_OrderInfo(sale, main, "delivery", this);
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
