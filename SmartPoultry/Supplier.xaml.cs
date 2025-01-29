using System.Collections.Generic;
using System.Windows.Controls;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
using static SmartPoultry.App;

namespace SmartPoultry
{
    public partial class Supplier : UserControl
    {
        private readonly SupplierServices _supplierServices;
        MainWindow mainWindow = UserContext.mainWindow;

        public Supplier()
        {
            InitializeComponent();
            _supplierServices = new SupplierServices(new AppDbContext());
            RetrieveSupplierList();
        }

        private void AddSupplier_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Phone.Text.Length < 11)
            {
                mainWindow.PopUpNotif("error", "Contact number must be 11 digits.");
                return;
            }
            if (!Email.Text.Contains("@") || !Email.Text.Contains("."))
            {
                mainWindow.PopUpNotif("error", "Invalid email address.");
                return;
            }


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
                mainWindow.PopUpNotif("alert", "Failed to add the supplier.");
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

        public void RetrieveSupplierList()
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

        public void Supplier_SupplierControl_SupplierClicked(SupplierList supplier)
        {
            SupplierName.Text = supplier.Name;
            ContactPerson.Text = supplier.Contact_Person;
            Phone.Text = supplier.Contact;
            Email.Text = supplier.Email;
            Address.Text = supplier.Location;
        }
    }
}
