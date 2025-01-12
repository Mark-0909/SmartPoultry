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
        public SupplierOrdersServices supplierOrdersServices;
        public Inventory_OrderToSupplier orderForm;
        public Inventory_OrderSupplyControl(int supplierid, Products product, Inventory_OrderToSupplier OrderForm)
        {
            InitializeComponent();
            supplierServices = new SupplierServices(context); 
            supplierOrdersServices = new SupplierOrdersServices(context);
            supplierID = supplierid;

            GetSupplier(supplierid);
            AddProduct(product);

            orderForm = OrderForm;
        }

        public void ConfirmOrder()
        {
            List<string> productid = new List<string>();
            List<string> qty = new List<string>();
            foreach (UIElement element in Wpanel.Children)
            {
                if(element is Inventory_OrderSupplyProductControl control)
                {
                    productid.Add(control.ProductId);
                    qty.Add(control.QTYLabel.ToString());
                }
            }
            

        }
        public void CheckPresentProducts()
        {
            bool hasProductControl = Wpanel.Children.OfType<Inventory_OrderSupplyProductControl>().Any();

            if (hasProductControl)
            {
                return;
            }
            else
            {
                if (this.Parent is Panel parentPanel)
                {
                    parentPanel.Children.Remove(this);
                }
                else
                {
                    throw new InvalidOperationException("This control does not have a valid parent container.");
                }
            }
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
            Inventory_OrderSupplyProductControl control = new Inventory_OrderSupplyProductControl(product, this);

            if (Wpanel != null)
            {
                Wpanel.Children.Add(control);
            }
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this); 
            }
            else
            {
                throw new InvalidOperationException("This control does not have a valid parent container.");
            }

        }
        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.BlackoutDates.Clear();
            DateTime today = DateTime.Today;
            DateTime? specificPastDate = datePicker.SelectedDate;

            if (specificPastDate.HasValue)
            {
                DateTime pastDate = specificPastDate.Value;

                if (pastDate < today)
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, pastDate.AddDays(-1)));
                    datePicker.BlackoutDates.Add(new CalendarDateRange(pastDate.AddDays(1), today.AddDays(-1)));
                }
                else
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, today.AddDays(-1)));
                }
            }
            else
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, today.AddDays(-1)));
            }
        }
    }
}
