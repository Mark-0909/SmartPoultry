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
using Microsoft.VisualBasic;
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
        readonly DeliveriesServices deliveryServices;
        Add_FinancialLiabilities add_FinancialLiabilities;
        Add_Delivery Add_Delivery;

        

        public dashboard()
        {
            InitializeComponent();

            var context = new AppDbContext();
            salesServices = new SalesServices(context);
            financialLiabilities = new FinancialLiabilitiesServices(context);
            deliveryServices = new DeliveriesServices(context);


            
            DisplaySales();
            CountDeliveries();
            CountPayments();
            CountOrders();

            FinancialLiabilitiesCbox.SelectionChanged += FinancialCB_SelectionChanged;
            DeliveryCBox.SelectionChanged += DeliveryCB_SelectionChanged;


            FinancialLiabilitiesCbox.SelectedIndex = 0;
            DeliveryCBox.SelectedIndex = 0;


            if (FinancialLiabilitiesCbox.SelectedItem != null)
            {
                DisplayFinancialLiabilities(FinancialLiabilitiesCbox.Text);
            }

            if (DeliveryCBox.SelectedItem != null)
            {
                DisplayDeliveries(DeliveryCBox.Text);
            }
        }

        

        private void FinancialCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = FinancialLiabilitiesCbox.Text;
            string filter;

            if(selectedText == "To Pay")
            {
                filter = "To Receive";
            }
            else
            {
                filter = "To Pay";
            }
            
            
            if (FinancilaLiabilitiesPanel != null)
            {
                FinancilaLiabilitiesPanel.Children.Clear();
                
                DisplayFinancialLiabilities(filter);
            }
        }


        private void DeliveryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = DeliveryCBox.Text;
            string filter;

            if (selectedText == "To Deliver")
            {
                filter = "To Receive";
            }
            else
            {
                filter = "To Deliver";
            }
            if (DeliveriesPanel1 != null && DeliveryCBox.SelectedItem != null)
            {
                DeliveriesPanel1.Children.Clear();
                DisplayDeliveries(filter);
                
            }
        }

        public void DynamicOrderDisplay()
        {
            OrderListPanel.Children.Clear();
            DisplaySales();
            CountOrders();
        }

        public void DynamicReloadDeliveries()
        {
            DeliveriesPanel1.Children.Clear();
            DisplayDeliveries(DeliveryCBox.Text);
            CountDeliveries();
        }
        public void DynamicReloadFinancialLiabilities()
        {
            FinancilaLiabilitiesPanel.Children.Clear();
            DisplayFinancialLiabilities(FinancialLiabilitiesCbox.Text);
            CountPayments();
        }
        public void CountOrders()
        {
            int orderscount = OrderListPanel.Children.Count;
            OrdersLabel.Content = orderscount.ToString();
        }
        public void CountDeliveries()
        {
            int deliverycount = deliveryServices.CountDeliveries();
            ToDeliverLabel.Content = deliverycount.ToString();
        }
        public void CountPayments()
        {
            int PaymentCount = financialLiabilities.CountPayments();
            ToPayLabel.Content = PaymentCount.ToString();
        }
        public void DisplayDeliveries(string filter)
        {
            int evenodd = 0;
            List<Deliveries> deliveries = deliveryServices.GetList(filter);

            foreach (Deliveries deliver in deliveries) {
                int id = deliver.Id;
                string name = deliver.name;
                string date = deliver.delivery_date.ToString("MM-dd-yyyy");
                string status = deliver.delivery_status;

                Add_DeliveriesControl control;
                if (evenodd == 0)
                {
                    control = new Add_DeliveriesControl(id, name, date, status, 0);
                    evenodd = 1;
                }
                else
                {
                    control = new Add_DeliveriesControl(id, name, date, status, 1);
                    evenodd = 0;
                }
                DeliveriesPanel1.Children.Add(control);
            }
        }
        public void DisplayFinancialLiabilities(string filter)
        {
            int evenodd = 0;
            List<FinancialLiabilities> finance = financialLiabilities.GetList(filter);
            
            foreach (FinancialLiabilities list in finance) { 
                int id = list.Id;
                string name = list.name;
                string duedate = list.due_date.ToString("MM-dd-yyyy");
                string amount = list.amount.ToString("N2");
                Add_FinancialLiabilitiesControl control;
                if (evenodd == 0)
                {
                    control = new Add_FinancialLiabilitiesControl(id, name, duedate, amount, 0);
                    evenodd = 1;
                }
                else {
                    control = new Add_FinancialLiabilitiesControl(id, name, duedate, amount, 1);
                    evenodd = 0;
                }
                
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
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                add_FinancialLiabilities = new Add_FinancialLiabilities(mainWindow);
                mainWindow.ActiveOverlay(true);
                add_FinancialLiabilities.ShowDialog();       
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.");
            }
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                Add_Delivery = new Add_Delivery(mainWindow);
                mainWindow.ActiveOverlay(true);
                Add_Delivery.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.");
            }
            
            
        }
        

    }
}
