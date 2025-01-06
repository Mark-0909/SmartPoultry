using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SmartPoultry.Models;

namespace SmartPoultry
{
    public partial class Supplier_SupplierControl : UserControl
    {
        private bool _isSelected;

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
            ContactInfo.Content = supplier.Contact;

            Supplier = supplier;

            // Add MouseDown event for row selection
            MouseDown += Supplier_SupplierControl_MouseDown;
        }

        private void Supplier_SupplierControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsSelected = true; // Mark this row as selected
            SupplierClicked?.Invoke(Supplier);
        }

        private void UpdateVisualState()
        {
            if (IsSelected)
            {
                HighlightBorder.Background = new SolidColorBrush(Colors.LightBlue); // Selected color
            }
            else
            {
                HighlightBorder.Background = new SolidColorBrush(Colors.White); // Default color
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of Supplier_InfoUserControl
            Supplier_InfoUserControl supplierInfoControl = new Supplier_InfoUserControl();

            // Set the DataContext to the current supplier so the form can auto-fill
            supplierInfoControl.DataContext = Supplier;

            // Display the control in a new window or popup
            Window editWindow = new Window
            {
                Title = "Edit Supplier Details",
                Content = supplierInfoControl,
                Width = 400,
                Height = 300,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Window.GetWindow(this) // Associate with the current window
            };

            // Show the window modally to block interaction with other UI elements
            editWindow.ShowDialog();
        }
    }
}
