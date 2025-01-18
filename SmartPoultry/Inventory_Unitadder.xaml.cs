using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_Unitadder.xaml
    /// </summary>
    public partial class Inventory_Unitadder : Window
    {
        public Inventory_AddingForm? inventoraddingWindow = Application.Current.Windows.OfType<Inventory_AddingForm>().FirstOrDefault();
        

        public static String? actionDeclaration; // add or edit
        public static String? agenda; // base_unit or sub_unit
        public static String? editname;
        public static String? editprice;
        public static String? editconversion;
        public static String? editstocks;
        public static String? baseunit;
        public static int positionlist;


        public static inventoryAdd_variationscontrol? unitcontrol;

        public Inventory_AddingForm inventoryAddForm { get; set; }


        public Inventory_Unitadder(String mode, String baseUnitValue, String action, int position, Inventory_AddingForm form)
        {
            InitializeComponent();
            SetRoundedCorners();
           
            InitializeUnitAdder(mode, action, null, null, null, baseUnitValue, null, null, position, form);
            NotifPopup.Visibility = Visibility.Hidden;
        }

        public Inventory_Unitadder(string mode, string action, string name, string price, string conversion, string baseUnitvalue, string stocks, inventoryAdd_variationscontrol control, int position, Inventory_AddingForm form)
        {
            InitializeComponent();
            SetRoundedCorners();

            
            InitializeUnitAdder(mode, action, name, price, conversion, baseUnitvalue, stocks, control, position, form);
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
        private void InitializeUnitAdder(string? mode, string? action, string? name, string? price, string? conversion, string? baseUnitvalue, string? stocks, inventoryAdd_variationscontrol control, int position, Inventory_AddingForm form)
        {
            inventoryAddForm = form;
            actionDeclaration = action; 
            agenda = mode;

            unitcontrol = control;

            positionlist = position;

            baseunit = baseUnitvalue;
           
            if (agenda == "base_unit")
            {
                if (actionDeclaration == "add")
                {
                    modeLabel.Content = "Add Base Unit";
                }
                else {
                    modeLabel.Content = "Edit Base Unit";
                }
                

                
                conversionName.Visibility = Visibility.Collapsed;
                conversionLabel.Visibility = Visibility.Collapsed;
                conversionBorder.Visibility = Visibility.Collapsed;
                conversionTextbox.Visibility = Visibility.Collapsed;

                
                if (name != null) UnitCB.Text = name;
                if (price != null) priceTextBox.Text = price;
                if (stocks != null) stocksTextBox.Text = stocks;
            }
            else if (agenda == "sub_unit")
            {
                if (actionDeclaration == "add")
                {
                    modeLabel.Content = "Add Sub Unit";
                }
                else
                {
                    modeLabel.Content = "Edit Sub Unit";
                }


                stocksBorder.Visibility = Visibility.Collapsed;
                stocksName.Visibility = Visibility.Collapsed;
                stocksTextBox.Visibility = Visibility.Collapsed;

                
                conversionName.Visibility = Visibility.Visible;

                if (name != null) UnitCB.Text = name;
                if (baseUnitvalue != null) conversionName.Content = "/ " + baseUnitvalue;
                if (price != null) priceTextBox.Text = price;
                if (conversion != null) conversionTextbox.Text = conversion;
            }
        }



        public void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (UnitCB.Text == "Select Unit Below..." || priceTextBox.Text == "Price...")
            {
                PopUpNotif("alert", "Incomplete Details.");
                return;
            }

            if (inventoryAddForm.unitlist.Contains(UnitCB.Text) && actionDeclaration == "add")
            {
                PopUpNotif("alert", "Unit has been added! Choose another one.");
                return;
            }
            if (actionDeclaration == "add")
            {
                if (agenda == "base_unit")
                {
                    inventoraddingWindow.AddUnit(UnitCB.Text, priceTextBox.Text, "1", stocksTextBox.Text, "base", positionlist);
                }
                else
                {
                    inventoraddingWindow.AddUnit(UnitCB.Text, priceTextBox.Text, conversionTextbox.Text, null, "sub", positionlist);
                }
                this.Close();
                inventoryAddForm.ActiveOverlay(false);
            }
            else 
            {
                if (agenda == "base_unit")
                {
                    HandleEditBaseUnit();
                }
                else
                {
                    HandleEditSubUnit();
                }
            }
        }

        private void HandleEditBaseUnit()
        {
            if (inventoryAddForm.unitlist[positionlist] == UnitCB.Text)
            {
                unitcontrol.EditUnit(UnitCB.Text, priceTextBox.Text, "1", stocksTextBox.Text, baseunit, positionlist);
                UpdateConversionForAllUserControls();
            }
            else if (inventoryAddForm.unitlist.Contains(UnitCB.Text))
            {
                PopUpNotif("alert", "Unit has been added! Choose another one.");
                return;
            }
            else
            {
                inventoryAddForm.unitlist[positionlist] = UnitCB.Text;
                unitcontrol.EditUnit(UnitCB.Text, priceTextBox.Text, "1", stocksTextBox.Text, baseunit, positionlist);
            }

            this.Close();
            inventoryAddForm.stockunit.Content = UnitCB.Text;
            inventoryAddForm.ActiveOverlay(false);
        }

        public void UpdateConversionForAllUserControls()
        {
            if (inventoryAddForm.unitsWPanel != null)
            {
                foreach (UIElement element in inventoryAddForm.unitsWPanel.Children)
                {
                    if (element is inventoryAdd_variationscontrol control)
                    {
                        control.UpdateBaseUnit(UnitCB.Text);
                    }
                }
            }
        }
        private void HandleEditSubUnit()
        {
            if (inventoryAddForm.unitlist[positionlist] == UnitCB.Text)
            {
                unitcontrol.EditUnit(UnitCB.Text, priceTextBox.Text, conversionTextbox.Text, null, baseunit, positionlist);
            }
            else if (inventoryAddForm.unitlist.Contains(UnitCB.Text))
            {
                PopUpNotif("alert", "Unit has been added! Choose another one.");
                return;
            }
            else
            {
                inventoryAddForm.unitlist[positionlist] = UnitCB.Text;
                unitcontrol.EditUnit(UnitCB.Text, priceTextBox.Text, conversionTextbox.Text, null, baseunit, positionlist);
            }

            this.Close();
            inventoryAddForm.ActiveOverlay(false);
        }


        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            inventoryAddForm.ActiveOverlay(false);
        }

        private void PriceTB_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(priceTextBox, "Price...", true);
        }

        private void PriceTB_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(priceTextBox, "Price...", false);
        }

        private void StocksTB_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(stocksTextBox, "Stocks...", true);
        }

        private void StocksTB_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(stocksTextBox, "Stocks...", false);
        }

        private void ConversionTB_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(conversionTextbox, "Conversion...", true);
        }

        private void ConversionTB_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(conversionTextbox, "Conversion...", false);
        }

        private void StocksTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (stocksTextBox.IsFocused) // Only filter when the user is typing
            {
                HandleNumericInput(stocksTextBox, true);
            }
        }

        private void PriceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (priceTextBox.IsFocused) // Only filter when the user is typing
            {
                HandleNumericInput(priceTextBox, true);
            }
        }

        private void ConversionTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (conversionTextbox.IsFocused) // Only filter when the user is typing
            {
                HandleNumericInput(conversionTextbox, false);
            }
        }


        private void HandleNumericInput(TextBox textBox, bool allowDecimal)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

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

        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }
    }
}
