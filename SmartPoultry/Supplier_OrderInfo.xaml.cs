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
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Supplier_OrderInfo.xaml
    /// </summary>
    public partial class Supplier_OrderInfo : Window
    {
        public AppDbContext context = new AppDbContext();
        public SupplierServices supplierServices;
        public UserServices userServices;
        public ProductServices productServices;
        public ProductVariationServices productVariationServices;
        SupplierOrders supplierOrders;

        Add_Delivery delivery;
        Add_FinancialLiabilities liabilities;
        
        public Supplier_OrderInfo(SupplierOrders order, Add_Delivery control)
        {
            InitializeComponent();
            supplierServices = new SupplierServices(context);
            userServices = new UserServices(context);
            productServices = new ProductServices(context);
            productVariationServices = new ProductVariationServices(context);
            supplierOrders = order;
            delivery = control;

            Initialize(order);
        }
        public Supplier_OrderInfo(SupplierOrders order, Add_FinancialLiabilities control)
        {
            InitializeComponent();
            supplierServices = new SupplierServices(context);
            userServices = new UserServices(context);
            productServices = new ProductServices(context);
            productVariationServices = new ProductVariationServices(context);
            supplierOrders = order;
            liabilities = control;

            Initialize(order);
        }
        public void Initialize(SupplierOrders suppOrder)
        {
            SupplierList supplier = supplierServices.FindSupplier(suppOrder.supplierID);

            NameLabel.Content = supplier.Name;
            PurchaseDatelabel.Content = suppOrder.Added_Date.ToString("MM-dd-yyyy");

            AmountLabel.Content = suppOrder.price.ToString("N2");

            EmployeeLabel.Content = userServices.GetUser(suppOrder.employee_incharge).Username;

            List<string> prodids = suppOrder.productList.Split(',').ToList();
            List<string> qty = suppOrder.orderQty.Split(",").ToList();

            GenerateList(prodids, qty);
        }

        public void GenerateList(List<string> prodid, List<string> qty)
        {
            OrderWPanel.Children.Clear();

            for (int i = 0; i < prodid.Count; i++)
            {
                Products products = productServices.FetchProduct(int.Parse(prodid[i]));
                string prodvar = productVariationServices.GetBaseUnit(int.Parse(prodid[i]));
                

                Border orderBorder = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Height = 35,
                    Width = 255
                };

                WrapPanel wrapPanel = new WrapPanel();

                Label itemNameLabel = new Label
                {
                    Content = $"{products.product_name}",
                    Height = 33,
                    Width = 126,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label qtyLabel = new Label
                {
                    Content = qty[i],
                    Height = 33,
                    Width = 43,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label itemFormLabel = new Label
                {
                    Content = $"{prodvar}",
                    Height = 33,
                    Width = 83,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };


                wrapPanel.Children.Add(itemNameLabel);
                wrapPanel.Children.Add(qtyLabel);
                wrapPanel.Children.Add(itemFormLabel);

                orderBorder.Child = wrapPanel;

                OrderWPanel.Children.Add(orderBorder);

                
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (delivery != null)
            {
                delivery.ActiveOverlay(false);
            }
            else 
            {
                liabilities.ActiveOverlay(false);
            }
        }
    }
}
