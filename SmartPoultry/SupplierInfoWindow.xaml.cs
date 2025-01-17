using System;
using System.Linq;
using System.Windows;
using SmartPoultry.DataAccess;
using SmartPoultry.Models;

namespace SmartPoultry
{
    public partial class SupplierInfoWindow : Window
    {
        private readonly AppDbContext _context;
        public SupplierList Supplier { get; private set; }
        public bool IsUpdated { get; private set; }

        public SupplierInfoWindow(SupplierList supplier)
        {
            InitializeComponent();
            _context = new AppDbContext();
            Supplier = supplier;

            // Populate fields with the supplier's existing data
            SupplierName.Text = supplier.Name;
            ContactPerson.Text = supplier.Contact_Person;
            Phone.Text = supplier.Contact;
            Email.Text = supplier.Email;
            Address.Text = supplier.Location;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update supplier details
            var existingSupplier = _context.SupplierLists.FirstOrDefault(s => s.Id == Supplier.Id);
            if (existingSupplier != null)
            {
                existingSupplier.Name = SupplierName.Text;
                existingSupplier.Contact_Person = ContactPerson.Text;
                existingSupplier.Contact = Phone.Text;
                existingSupplier.Email = Email.Text;
                existingSupplier.Location = Address.Text;

                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Supplier details updated successfully.");
                    IsUpdated = true; // Indicate changes were made
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating supplier: {ex.Message}");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Confirm before deleting
            var result = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var existingSupplier = _context.SupplierLists.FirstOrDefault(s => s.Id == Supplier.Id);
                if (existingSupplier != null)
                {
                    try
                    {
                        _context.SupplierLists.Remove(existingSupplier);
                        _context.SaveChanges();
                        MessageBox.Show("Supplier deleted successfully.");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting supplier: {ex.Message}");
                    }
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
