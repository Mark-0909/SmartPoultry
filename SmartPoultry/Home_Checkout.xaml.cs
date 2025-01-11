using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        decimal priceamount;
        
        public Home_Checkout(string price, home homeControl, MainWindow window, List<string> provvarid, List<string> quantity, List<string> varspec, List<string> pricelist, List<string> Prodname)
        {
            InitializeComponent();
            totalPricelabel.Content = price;
            homeController = homeControl;
            mainWindow = window;

            GenerateList(provvarid, quantity, varspec, pricelist, Prodname);

            if (OrderWPanel.Children.Count < 10)
            {
                OrderScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            datePickerPayment.SelectedDate = DateTime.Now.AddDays(14);
            datePickerDelivery.SelectedDate = DateTime.Now;
            DisableTextBoxes();

            priceamount = decimal.Parse(price);
            NotifPopup.Visibility = Visibility.Hidden;
        }
        public void DisableTextBoxes()
        {
            void SetControlState(Control control, Border border, bool isEnabled, double opacity)
            {
                if (control != null)
                {
                    control.IsEnabled = isEnabled;
                }

                if (border != null)
                {
                    border.Opacity = opacity;
                }
            }

            SetControlState(datePickerDelivery, DPdeliverborder, false, 0.5);
            SetControlState(datePickerPayment, DPpaymentborder, false, 0.5);
            SetControlState(NameTB, NameBorder, false, 0.5);
            SetControlState(ContactsTB, ContactsBorder, false, 0.5);
            SetControlState(AddressTB, AddressBorder, false, 0.5);
            SetControlState(ChargeTB, ChargeBorder, false, 0.5);

            if (status == "unpaid" && purchasemethod == "to deliver")
            {
                SetControlState(datePickerDelivery, DPdeliverborder, true, 1);
                SetControlState(datePickerPayment, DPpaymentborder, true, 1);
                SetControlState(NameTB, NameBorder, true, 1);
                SetControlState(ContactsTB, ContactsBorder, true, 1);
                SetControlState(AddressTB, AddressBorder, true, 1);
                SetControlState(ChargeTB, ChargeBorder, true, 1);
            }
            else if (status == "unpaid")
            {
                SetControlState(datePickerPayment, DPpaymentborder, true, 1);
                SetControlState(NameTB, NameBorder, true, 1);
                SetControlState(ContactsTB, ContactsBorder, true, 1);
            }
            else if (purchasemethod == "to deliver")
            {
                SetControlState(datePickerDelivery, DPdeliverborder, true, 1);
                SetControlState(NameTB, NameBorder, true, 1);
                SetControlState(ContactsTB, ContactsBorder, true, 1);
                SetControlState(AddressTB, AddressBorder, true, 1);
                SetControlState(ChargeTB, ChargeBorder, true, 1);
            }
        }


        public void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerDelivery.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
            datePickerPayment.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }

        public void GenerateList(List<string> prodvarid, List<string> qty, List<string> varSpec, List<string> priceList, List<string> prodname)
        {
            OrderWPanel.Children.Clear();

            for (int i = 0; i < prodvarid.Count; i++)
            {
                Border orderBorder = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Height = 35,
                    Width = 255
                };

                WrapPanel wrapPanel = new WrapPanel();

                Label itemNameLabel = new Label
                {
                    Content = $"({varSpec[i]}) {prodname[i]}",
                    Height = 33,
                    Width = 126,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label qtyLabel = new Label
                {
                    Content = qty[i],
                    Height = 33,
                    Width = 43,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                string formattedPrice = decimal.TryParse(priceList[i], out decimal price)
                    ? price.ToString("N2")
                    : "Invalid";

                Label priceLabel = new Label
                {
                    Content = formattedPrice,
                    Height = 33,
                    Width = 83,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };
                wrapPanel.Children.Add(itemNameLabel);
                wrapPanel.Children.Add(qtyLabel);
                wrapPanel.Children.Add(priceLabel);

                orderBorder.Child = wrapPanel;

                OrderWPanel.Children.Add(orderBorder);
            }
        }



        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ActiveOverlay(false);
        }

        public bool Validation()
        {
            if (purchasemethod == "to deliver" && status == "unpaid" &&
                (NameTB.Text == NameTB.Tag.ToString() || ContactsTB.Text == ContactsTB.Tag.ToString() || ChargeTB.Text == ChargeTB.Tag.ToString() || AddressTB.Text == AddressTB.Tag.ToString()))
            {
                PopUpNotif("alert", "Incomplete Details");
                return false;
            }

            if (status == "unpaid" &&
                (NameTB.Text == NameTB.Tag.ToString() || ContactsTB.Text == ContactsTB.Tag.ToString()))
            {
                PopUpNotif("alert", "Incomplete Details");
                return false;
            }

            if (purchasemethod == "to deliver" &&
                (NameTB.Text == NameTB.Tag.ToString() || ContactsTB.Text == ContactsTB.Tag.ToString() || ChargeTB.Text == ChargeTB.Tag.ToString() || AddressTB.Text == AddressTB.Tag.ToString()))
            {
                PopUpNotif("alert", "Incomplete Details");
                return false;
            }
            return true;
        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            decimal price = decimal.Parse(totalPricelabel.Content.ToString());
            if (Validation())
            {
                if (purchasemethod == "to deliver" && status == "unpaid")
                {
                    homeController.ConfirmOrder(paymentmethod, status, purchasemethod, datePickerDelivery.SelectedDate.Value, datePickerPayment.SelectedDate.Value, NameTB.Text, ContactsTB.Text, ChargeTB.Text, AddressTB.Text, price);
                } else if (purchasemethod == "to deliver")
                {
                    homeController.ConfirmOrder(paymentmethod, status, purchasemethod, datePickerDelivery.SelectedDate.Value, null, NameTB.Text, ContactsTB.Text, ChargeTB.Text, AddressTB.Text, price);
                } else if (status == "unpaid")
                {
                    homeController.ConfirmOrder(paymentmethod, status, purchasemethod, null, datePickerPayment.SelectedDate.Value, NameTB.Text, ContactsTB.Text, null, null, price);
                } else
                {
                    homeController.ConfirmOrder(paymentmethod, status, purchasemethod, null, null, null, null, null, null, price);
                }
                homeController.EnableDropBtn();

                this.Close();
                mainWindow.ActiveOverlay(false);
            }
            
        }

        private void CashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paymentmethod = "Cash";
            DisableTextBoxes();
        }

        private void GCashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paymentmethod = "GCash";
            DisableTextBoxes();
        }

        private void PaidRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            status = "paid";
            DisableTextBoxes();
        }

        private void UnpaidRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            status = "unpaid";
            DisableTextBoxes();

        }

        private void UpfrontRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            purchasemethod = "upfront";
            DisableTextBoxes();
        }

        private void ToDeliverRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            purchasemethod = "to deliver";
            DisableTextBoxes();
        }

        private void ChargeFee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChargeTB.Text == "Charge Fee...")
            {
                return;
            }

            HandleNumericInput(ChargeTB, true);

            if (string.IsNullOrWhiteSpace(ChargeTB.Text) || !decimal.TryParse(ChargeTB.Text, out decimal charge))
            {
                totalPricelabel.Content = priceamount.ToString("N2");
                RemoveExistingChargeBorder();
                return;
            }

            if (ChargeTB.Tag != null && ChargeTB.Text == ChargeTB.Tag.ToString())
            {
                totalPricelabel.Content = priceamount.ToString("N2");
                return;
            }

            decimal total = priceamount + charge;
            totalPricelabel.Content = total.ToString("N2");

            Border existingBorder = OrderWPanel.Children.OfType<Border>().FirstOrDefault(b => b.Name == "ChargeBorderControlList");

            if (existingBorder != null)
            {
                WrapPanel existingWrapPanel = existingBorder.Child as WrapPanel;
                if (existingWrapPanel != null)
                {
                    Label priceLabel = existingWrapPanel.Children.OfType<Label>().LastOrDefault(); 
                    if (priceLabel != null)
                    {
                        priceLabel.Content = charge.ToString("N2");
                    }
                }
            }
            else
            {
                Border orderBorder = new Border
                {
                    Name = "ChargeBorderControlList",
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Height = 35,
                    Width = 255
                };

                WrapPanel wrapPanel = new WrapPanel();

                Label itemNameLabel = new Label
                {
                    Content = "Delivery Fee:",
                    Height = 33,
                    Width = 126,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label qtyLabel = new Label
                {
                    Content = "",
                    Height = 33,
                    Width = 43,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label priceLabel = new Label
                {
                    Content = charge.ToString("N2"),
                    Height = 33,
                    Width = 83,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                wrapPanel.Children.Add(itemNameLabel);
                wrapPanel.Children.Add(qtyLabel);
                wrapPanel.Children.Add(priceLabel);

                orderBorder.Child = wrapPanel;

                OrderWPanel.Children.Add(orderBorder);
            }
        }


        private void RemoveExistingChargeBorder()
        {
            Border existingBorder = OrderWPanel.Children.OfType<Border>().FirstOrDefault(b => b.Name == "ChargeBorderControlList");
            if (existingBorder != null)
            {
                OrderWPanel.Children.Remove(existingBorder);
            }
        }




        private void HandleNumericInput(TextBox textBox, bool allowDecimal)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            if (textBox.Text == "Charge Fee...") return;

            string input = textBox.Text;

            string filteredInput = allowDecimal
                ? new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray())
                : new string(input.Where(char.IsDigit).ToArray());

            if (allowDecimal)
            {
                int firstDecimalIndex = filteredInput.IndexOf('.');
                if (firstDecimalIndex != -1)
                {
                    filteredInput = filteredInput.Substring(0, firstDecimalIndex + 1) +
                                    filteredInput.Substring(firstDecimalIndex + 1).Replace(".", "");
                }
            }

            if (input != filteredInput)
            {
                textBox.Text = filteredInput;
                textBox.CaretIndex = filteredInput.Length;
            }
        }

        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholder = textBox.Tag?.ToString(); 
                if (!string.IsNullOrEmpty(placeholder) && textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black; 
                }
            }
        }

        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholder = textBox.Tag?.ToString(); 
                if (!string.IsNullOrEmpty(placeholder) && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder; 
                    textBox.Foreground = Brushes.Gray; 
                }
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
        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }
    }
}
