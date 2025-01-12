using Microsoft.EntityFrameworkCore.Migrations.Internal;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
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

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_OrderSupplyControl.xaml
    /// </summary>
    public partial class Inventory_OrderSupplyControl : UserControl
    {
        public int supplierID { get; set; }
        public AppDbContext context = new AppDbContext();
        public SupplierServices supplierServices;
        public Inventory_OrderToSupplier orderForm;
        public Inventory_OrderSupplyControl(int supplierid, Products product, Inventory_OrderToSupplier OrderForm)
        {
            InitializeComponent();
            supplierServices = new SupplierServices(context); 
            supplierID = supplierid;

            GetSupplier(supplierid);
            AddProduct(product);

            orderForm = OrderForm;
        }

        public void GetSupplier(int id)
        {
            SupplierList supplier = supplierServices.FindSupplier(id);

            SupplierNameLabel.Content = supplier.Name;
            ContactPersonLabel.Content = supplier.Contact_Person;
            ContactLabel.Content = $"{supplier.Contact} / {supplier.Email}";
        }

        public void AddProduct(Products product)
        {
            Inventory_OrderSupplyProductControl control = new Inventory_OrderSupplyProductControl(product);

            if (Wpanel != null)
            {
                Wpanel.Children.Add(control);
            }
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

        }
    }
}
