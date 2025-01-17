using System.Collections.Generic;
using System.Windows.Controls;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;

namespace SmartPoultry
{
    public partial class Supplier : UserControl
    {
        private readonly SupplierServices _supplierServices;

        public Supplier()
        {
            InitializeComponent();
            _supplierServices = new SupplierServices(new AppDbContext());
            RetrieveSupplierList();
        }

        private void AddSupplier_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool success = _supplierServices.Create(
                SupplierName.Text,
                ContactPerson.Text,
                Phone.Text,
                Email.Text,
                Address.Text
            );

            if (success)
            {
                RetrieveSupplierList();
                ClearInputFields();
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to add the supplier.");
            }
        }

        private void ClearInputFields()
        {
            SupplierName.Clear();
            ContactPerson.Clear();
            Phone.Clear();
            Email.Clear();
            Address.Clear();
        }

        private void RetrieveSupplierList()
        {
            List<SupplierList> supplierLists = _supplierServices.ListSuppliers();

            SupplierListPanel.Children.Clear();

            foreach (var supplier in supplierLists)
            {
                var control = new Supplier_SupplierControl(supplier);
                SupplierListPanel.Children.Add(control);

                control.SupplierClicked += Supplier_SupplierControl_SupplierClicked;
            }
        }

        private void Supplier_SupplierControl_SupplierClicked(SupplierList supplier)
        {
            SupplierName.Text = supplier.Name;
            ContactPerson.Text = supplier.Contact_Person;
            Phone.Text = supplier.Contact;
            Email.Text = supplier.Email;
            Address.Text = supplier.Location;
        }
    }
}
