using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
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

        public SupplierList Supplier { get; set; }

        public void Supplier_SupplierControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsSelected = true; // Mark this row as selected

            SupplierClicked?.Invoke(Supplier);
        }

        public void UpdateVisualState()
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
            // Instantiate the Supplier_InfoUserControl
            Supplier_InfoUserControl supplierInfoControl = new Supplier_InfoUserControl();

            // Optionally, pass supplier data to the popup here
            // For example: supplierInfoControl.FillData(supplierName, contactPerson, etc.);

            // Show the popup (e.g., in a parent window or as a dialog)
            Window.GetWindow(this).Content = supplierInfoControl;
            supplierInfoControl.Visibility = Visibility.Visible;
        }
    }
}