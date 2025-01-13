using SmartPoultry.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SmartPoultry.DataAccess;

namespace SmartPoultry
{
    public partial class Supplier_InfoUserControl : UserControl
    {
        public event EventHandler Closed;
        private AppDbContext _context;

        public Supplier_InfoUserControl()
        {
            InitializeComponent();
            _context = new AppDbContext(); // Create a new instance of your DbContext
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
                // Ensure that the supplier is tracked by the DbContext
                var existingSupplier = _context.SupplierLists.FirstOrDefault(p => p.Id == supplier.Id);
                if (existingSupplier != null)
                {
                    // Update the supplier's properties with the new values from the UI
                    existingSupplier.Name = SupplierName.Text;
                    existingSupplier.Contact_Person = ContactPerson.Text;
                    existingSupplier.Contact = Phone.Text;
                    existingSupplier.Email = Email.Text;
                    existingSupplier.Location = Address.Text;

                    try
                    {
                        _context.SaveChanges(); // Commit the changes to the database
                        MessageBox.Show("Supplier details successfully updated.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating supplier: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Supplier not found.");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Confirm before deleting
            var result = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (DataContext is SupplierList supplier)
                {
                    var supplierToDelete = _context.SupplierLists.FirstOrDefault(p => p.Id == supplier.Id);
                    if (supplierToDelete != null)
                    {
                        try
                        {
                            _context.SupplierLists.Remove(supplierToDelete);
                            _context.SaveChanges(); // Commit the deletion to the database
                            MessageBox.Show("Supplier successfully deleted.");

                            // Notify the parent control to remove this supplier's row from the UI
                            Closed?.Invoke(this, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting supplier: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the control (make it invisible or remove it from the parent)
            this.Visibility = Visibility.Collapsed;
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
