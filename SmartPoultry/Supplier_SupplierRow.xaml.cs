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

            MouseDown += Supplier_SupplierControl_MouseDown;
        }

        private void Supplier_SupplierControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsSelected = true;
            SupplierClicked?.Invoke(Supplier);
        }

        private void UpdateVisualState()
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
                ContactInfo.Content = infoWindow.Supplier.Contact;
            }
        }
    }
}
