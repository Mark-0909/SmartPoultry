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
    /// Interaction logic for dashboard.xaml
    /// </summary>
    public partial class dashboard : UserControl
    {
        readonly SalesServices salesServices;
        readonly FinancialLiabilitiesServices financialLiabilities;
        Add_FinancialLiabilities add_FinancialLiabilities;
        Add_Delivery Add_Delivery;
        public dashboard()
        {
            InitializeComponent();
            var context = new AppDbContext();
            salesServices = new SalesServices(context);
            financialLiabilities = new FinancialLiabilitiesServices(context);
            DisplaySales();
            DisplayFinancialLiabilities();
        }
        public void DynamicOrderDisplay()
        {
            OrderListPanel.Children.Clear();
            DisplaySales();
        }
        public void DisplayFinancialLiabilities()
        {
            int evenodd = 0;
            List<FinancialLiabilities> finance = financialLiabilities.GetList();
            
            foreach (FinancialLiabilities list in finance) { 
                int id = list.Id;
                string name = list.name;
                string duedate = list.due_date.ToString("MM-dd-yyyy");
                string amount = list.amount.ToString("N2");
                Add_FinancialLiabilitiesControl control = new Add_FinancialLiabilitiesControl(id, name, duedate, amount);
                FinancilaLiabilitiesPanel.Children.Add(control);
            }
        }
        public void DisplaySales() {
            int evenodd = 0;
            List<Sales> salesList = salesServices.GetSales();

            foreach (Sales sales in salesList) { 
                string refid = sales.receipt_id.ToString();
                string mode = sales.payment_mode.ToString();
                string status = sales.status.ToString();
                string price = sales.total_price.ToString();
                Dashboard_OrdersControl control;
                if (evenodd == 0)
                {
                    control = new Dashboard_OrdersControl(refid, mode, status, price, 0);
                    evenodd = 1;
                }
                else {
                    control = new Dashboard_OrdersControl(refid, mode, status, price, 1);
                    evenodd = 0;
                    
                }
                OrderListPanel.Children.Add(control);


            }
        }
        private void AddFinancialLiabilities_Click(object sender, RoutedEventArgs e)
        {
            add_FinancialLiabilities = new Add_FinancialLiabilities();
            add_FinancialLiabilities.ShowDialog();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            Add_Delivery = new Add_Delivery();
            Add_Delivery.ShowDialog();
        }
    }
}
