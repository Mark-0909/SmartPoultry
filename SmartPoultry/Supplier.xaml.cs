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

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : UserControl
    {
        SupplierServices SupplierServices;
        public Supplier()
        {
            InitializeComponent();
            AppDbContext context = new AppDbContext();
            SupplierServices = new SupplierServices(context);
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            string name = SupplierName.Text;
            string contactperson = ContactPerson.Text;
            string phone = Phone.Text;
            string email = Email.Text;
            string address = Address.Text;

            SupplierServices.Create(name,contactperson, phone, email, address);
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
