using System;
using System.Linq;
using System.Windows;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
using static SmartPoultry.App;

namespace SmartPoultry
{
    public partial class SupplierInfoWindow : Window
    {
        private readonly AppDbContext _context;

        SupplierServices supplierServices;
        public SupplierList Supplier { get; private set; }
        public bool IsUpdated { get; private set; }
        MainWindow mainWindow = UserContext.mainWindow;
        public SupplierInfoWindow(SupplierList supplier)
        {
            InitializeComponent();
            _context = new AppDbContext();
            Supplier = supplier;
            supplierServices = new SupplierServices(_context);

            // Populate fields with the supplier's existing data
            SupplierName.Text = supplier.Name;
            ContactPerson.Text = supplier.Contact_Person;
            Phone.Text = supplier.Contact;
            Email.Text = supplier.Email;
            Address.Text = supplier.Location;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool isUpdated = supplierServices.UpdateSupplier(Supplier.Id, SupplierName.Text, ContactPerson.Text, Phone.Text, Address.Text, Email.Text);
            if (!isUpdated)
            {
                MessageBox.Show("Suuplier not updated!");
                return;
            }
            MessageBox.Show("Suuplier updated!");
            DynamicUpdate();
        }
        public void DynamicUpdate()
        {
            mainWindow.supplierControl.SupplierListPanel.Children.Clear();
            List<SupplierList> suppliers = supplierServices.ListSuppliers();

            for (int i = 0; i < suppliers.Count; i++)
            {
                var control = new Supplier_SupplierControl(suppliers[i]);
                mainWindow.supplierControl.SupplierListPanel.Children.Add(control);

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
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string deleteResult = supplierServices.DeleteSupplier(Supplier.Id);

                MessageBox.Show(deleteResult);

                if (deleteResult == "Supplier deleted successfully.")
                {
                    DynamicUpdate();
                    this.Close();
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
