using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SmartPoultry.Models;
using static SmartPoultry.App;

namespace SmartPoultry
{
    public partial class Supplier_SupplierControl : UserControl
    {
        public bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateVisualState();
            }
        }

        public event Action<SupplierList> SupplierClicked;
        public SupplierList Supplier { get; set; }

        public Supplier_SupplierControl(SupplierList supplier)
        {
            InitializeComponent();
            Name.Content = supplier.Name;
            ContactPerson.Content = supplier.Contact_Person;
            ContactInfo.Text = supplier.Contact; // Use Text property for TextBox

            Supplier = supplier;

            MouseDown += Supplier_SupplierControl_MouseDown;

            // Apply alternating background colors
            ApplyAlternatingBackgroundColors();
        }

        private void ApplyAlternatingBackgroundColors()
        {
            // Get the parent container (e.g., WrapPanel or StackPanel)
            var parent = Parent as Panel;
            if (parent == null) return;

            int count = 0;

            // Loop through all children of the parent container
            foreach (UIElement child in parent.Children)
            {
                if (child is Supplier_SupplierControl)
                {
                    count++;
                }
            }

            // Apply alternating colors based on the count
            HighlightBorder.Background = new SolidColorBrush(count % 2 == 0 ? Colors.LightGray : Colors.White);
        }

        private void Supplier_SupplierControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Set the current control as selected
            IsSelected = true;
            SupplierClicked?.Invoke(Supplier);

            // Get the main window and update the visual states for all controls
            MainWindow mainWindow = UserContext.mainWindow;

            // Helper function to reset all `Supplier_SupplierControl` instances
            ResetAllSupplierControls(mainWindow, this);
        }

        private void ResetAllSupplierControls(MainWindow mainWindow, Supplier_SupplierControl selectedControl)
        {
            // Loop through all panels or areas that might contain Supplier_SupplierControl
            var allContainers = new List<Panel>
            {
                mainWindow.supplierControl.SupplierListPanel,
            };

            foreach (Panel container in allContainers)
            {
                foreach (UIElement element in container.Children)
                {
                    if (element is Supplier_SupplierControl control)
                    {
                        // Update the IsSelected state: true for the current, false for all others
                        control.IsSelected = control == selectedControl;

                        // Update the visual state for every control
                        control.UpdateVisualState();
                    }
                }
            }
        }

        public void UpdateVisualState()
        {
            HighlightBorder.Background = IsSelected
                ? new SolidColorBrush(Colors.LightGreen)
                : new SolidColorBrush(Colors.White);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new SupplierInfoWindow(Supplier);
            infoWindow.ShowDialog(); // Open as modal window

            // Update UI if supplier was modified
            if (infoWindow.IsUpdated)
            {
                Name.Content = infoWindow.Supplier.Name;
                ContactPerson.Content = infoWindow.Supplier.Contact_Person;
                ContactInfo.Text = infoWindow.Supplier.Contact; // Use Text property for TextBox
            }
        }

        // Restrict ContactInfo to only allow integers
        private void ContactInfo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the input is a digit
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Block non-digit input
            }
        }

        private void ContactInfo_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            // Check if the pasted text is a valid integer
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!int.TryParse(text, out _))
                {
                    e.CancelCommand(); // Block pasting non-integer text
                }
            }
            else
            {
                e.CancelCommand(); // Block non-text data
            }
        }
    }
}