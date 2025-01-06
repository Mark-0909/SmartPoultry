using System;
using System.Collections.Generic;
using System.Linq;
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
using SmartPoultry.Models;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Supplier_InfoUserControl.xaml
    /// </summary>
    public partial class Supplier_InfoUserControl : UserControl
    {
        public Supplier_InfoUserControl()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is SupplierList supplier)
            {
                // Populate fields with supplier data
                SupplierName.Text = supplier.Name;
                ContactPerson.Text = supplier.Contact_Person;
                Phone.Text = supplier.Contact;
                Email.Text = supplier.Email;
                Address.Text = supplier.Location;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
                    if (DataContext is SupplierList supplier)
        {
            // Update supplier data
            supplier.Name = SupplierName.Text;
            supplier.Contact_Person = ContactPerson.Text;
            supplier.Contact = Phone.Text;
            supplier.Email = Email.Text;
            supplier.Location = Address.Text;

            // Save changes to the database
            MessageBox.Show("Supplier details updated successfully.");
        }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

    }
}
