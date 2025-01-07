using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SmartPoultry.Models;
using static SmartPoultry.App;

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

            Supplier_InfoUserControl supplierInfoControl = new Supplier_InfoUserControl();


            supplierInfoControl.DataContext = Supplier;


            Window editWindow = new Window  // gawa ka nalang bagong window sa solution explorer
            {
                Title = "Edit Supplier Details",
                Content = supplierInfoControl,
                Width = 400,
                Height = 300,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Window.GetWindow(this) 
            };

            MainWindow mainWindow = UserContext.mainWindow;
            mainWindow.ActiveOverlay(true);
            editWindow.ShowDialog();
        }
    }
}
