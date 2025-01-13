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
        public Supplier_InfoUserControl infoUserControl;

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
                HighlightBorder.Background = new SolidColorBrush(Colors.LightGreen); // Selected color
            }
            else
            {
                HighlightBorder.Background = new SolidColorBrush(Colors.White); // Default color
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the overlay (semi-transparent background)
            var overlay = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)), // 50% black opacity
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Create the popup container
            var popupContainer = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Width = 400, // Set your desired popup width
                Height = 350, // Set your desired popup height
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Create the Supplier_InfoUserControl and set its DataContext
            var supplierInfoControl = new Supplier_InfoUserControl
            {
                DataContext = Supplier
            };

            // Add the control to the popup container
            popupContainer.Child = supplierInfoControl;

            // Add the popup container to the overlay
            overlay.Children.Add(popupContainer);

            // Find the parent container (e.g., Grid)
            var parentGrid = FindParent<Grid>(this);
            if (parentGrid == null)
            {
                MessageBox.Show("Parent container not found.");
                return;
            }

            // Add the overlay to the parent container
            parentGrid.Children.Add(overlay);

            // Handle the close action from Supplier_InfoUserControl
            supplierInfoControl.Closed += (s, args) =>
            {
                // Remove the overlay from the parent container
                parentGrid.Children.Remove(overlay);
            };
        }


        // Helper method to find the parent Grid
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}