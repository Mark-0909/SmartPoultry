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
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : UserControl
    {
        private SupplierList _selectedSupplier;
        SupplierServices SupplierServices;
        public Supplier()
        {
            InitializeComponent();
            AppDbContext context = new AppDbContext();
            SupplierServices = new SupplierServices(context);
            RetrieveSupplierList(SupplierServices);
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            string name = SupplierName.Text;
            string contactperson = ContactPerson.Text;
            string phone = Phone.Text;
            string email = Email.Text;
            string address = Address.Text;

            bool success = SupplierServices.Create(name,contactperson, phone, email, address);

            if (success)
            {
                RetrieveSupplierList(SupplierServices); //this will refresh the list after adding
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            string name = SupplierName.Text;
            string contactperson = ContactPerson.Text;
            string phone = Phone.Text;
            string email = Email.Text;
            string address = Address.Text;
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            string name = SupplierName.Text;
        }

        public void RetrieveSupplierList(SupplierServices supplierServices)
        {
            
                List<SupplierList> supplierLists = supplierServices.ListSuppliers();
            
            foreach (SupplierList list in supplierLists)
            {
                Supplier_SupplierControl control = new Supplier_SupplierControl(list);
                SupplierListPanel.Children.Add(control);

                control.SupplierClicked += Supplier_SupplierControl_SupplierClicked;
            }
        }

        public void Supplier_SupplierControl_SupplierClicked(SupplierList supplier)
        {


            // Populate the form with the selected supplier's data
            SupplierName.Text = supplier.Name;
            ContactPerson.Text = supplier.Contact_Person;
            Phone.Text = supplier.Contact;
            Email.Text = supplier.Contact;
            Address.Text = supplier.Location;
        }
    }
}
