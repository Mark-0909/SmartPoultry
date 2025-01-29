using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for Inventory_AddSupplier.xaml
    /// </summary>
    public partial class Inventory_AddSupplier : Window
    {
        public Inventory_AddingForm addingForm;
        public AppDbContext context = new AppDbContext();
        SupplierServices supplierServices;
        MainWindow mainWindow = UserContext.mainWindow;
        public Inventory_AddSupplier(Inventory_AddingForm addringform)
        {
            InitializeComponent();
            addingForm = addringform;
            supplierServices = new SupplierServices(context);
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

        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        public void CloseWindow()
        {
            this.Close();
            addingForm.ActiveOverlay(false);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            
            bool validated = Validation();
            if (!validated) 
            {
                PopUpNotif("alert", "Incomplete Details");
                return;
            }
            if (ContactTB.Text.Length < 11)
            {
                PopUpNotif("error", "Contact number must be 11 digits.");
                return;
            }
            if (!EmailTB.Text.Contains("@") || !EmailTB.Text.Contains("."))
            {
                PopUpNotif("error", "Invalid email address.");
                return;
            }
            bool isCreate = supplierServices.Create(SupplierNameTB.Text, ContactPersonTB.Text, ContactTB.Text, EmailTB.Text, LocationTB.Text);
            if (!isCreate) 
            {
                PopUpNotif("alert", "Unsuccessful.");
                return;
            }

            PopUpNotif("alert", "Successful.");
            addingForm.PopulateSupplierList("add");
            string supplierName = SupplierNameTB.Text;
            if (!string.IsNullOrEmpty(supplierName) && addingForm.SupplierCBox.Items.Contains(supplierName))
            {
                addingForm.SupplierCBox.SelectedItem = supplierName;
            }
            else
            {
                addingForm.SupplierCBox.SelectedIndex = 0;
            }
            CloseWindow();
            mainWindow.supplierControl.RetrieveSupplierList();
        }

        public bool Validation()
        {
            if (SupplierNameTB.Text == SupplierNameTB.Tag.ToString() || ContactPersonTB.Text == ContactPersonTB.Tag.ToString() || ContactTB.Text == ContactTB.Tag.ToString() || EmailTB.Text == EmailTB.Tag.ToString() || LocationTB.Text == LocationTB.Tag.ToString())
            {
                return false;
            }
            return true;
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

        private void ContactTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleNumericInput(ContactTB, false, 11);
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

        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }
    }
}
