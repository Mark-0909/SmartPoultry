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
using static SmartPoultry.App;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

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
        readonly ProductServices productServices;
        Add_FinancialLiabilities add_FinancialLiabilities;
        ExpensesServices expensesServices;
        Add_Delivery Add_Delivery;

        MainWindow mainWindow;

        
        public dashboard()
        {
            InitializeComponent();

            mainWindow = UserContext.mainWindow;

            var context = new AppDbContext();
            salesServices = new SalesServices(context);
            financialLiabilities = new FinancialLiabilitiesServices(context);
            deliveryServices = new DeliveriesServices(context);
            productServices = new ProductServices(context);
            expensesServices = new ExpensesServices(context);

            
            DisplaySales();
            CountDeliveries();
            CountPayments();
            CountOrders();
            CountOutOfStocks();

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
            GenerateRevenueAndCostComboChart();
        }

        public ChartValues<decimal> RevenueValues { get; set; }
        public ChartValues<decimal> ExpenseValues { get; set; }
        public List<string> DateLabels { get; set; }

        public void GenerateRevenueAndCostComboChart()
        {
            GrowthAndTrendsAnalysis.Series.Clear();

            List<Sales> sales = salesServices.GetSalesList();
            List<Expenses> expenses = expensesServices.GetTodaysExpenses();

            DateTime today = DateTime.Now.Date; 


            decimal totalSales = sales
                .Where(s => s.status == "paid")
                .Sum(s => s.total_price);

            decimal totalExpenses = expenses
                .Where(e => e.Added_Date.Date == today)
                .Sum(e => e.price);

            GrowthAndTrendsAnalysis.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Revenue",
                    Values = new ChartValues<decimal> { totalSales },
                    Fill = Brushes.SteelBlue, 
                },

               
                new ColumnSeries
                {
                    Title = "Cost",
                    Values = new ChartValues<decimal> { totalExpenses },
                    Fill = Brushes.Tomato, 
                }
            };

            // Set axis labels
            GrowthAndTrendsAnalysis.AxisX.Clear();
            GrowthAndTrendsAnalysis.AxisX.Add(new Axis
            {
                Labels = new[] { "Today" }, 
                Separator = new LiveCharts.Wpf.Separator { Step = 1 }
            });

            GrowthAndTrendsAnalysis.AxisY.Clear();
            GrowthAndTrendsAnalysis.AxisY.Add(new Axis
            {
                Title = "Amount",
                LabelFormatter = value => $"₱{value:N2}" 
            });
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
                DisplayDeliveries(filter);
                
            }
        }

        public void DynamicOrderDisplay()
        {
            DisplaySales();
            CountOrders();
        }

        public void DynamicReloadDeliveries()
        {
            DisplayDeliveries(DeliveryCBox.Text);
            CountDeliveries();
        }
        public void DynamicReloadFinancialLiabilities()
        {
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
        public void CountOutOfStocks()
        {
            List<Products> lowonstocks = productServices.GetLowStockProducts("", "", "");
            OutOfStockLabel.Content = lowonstocks.Count.ToString();
        }

        public void DisplayDeliveries(string filter)
        {
            DeliveriesPanel1.Children.Clear();
            int evenodd = 0;
            List<Deliveries> deliveries = deliveryServices.GetList(filter);

            foreach (Deliveries deliver in deliveries) {
                Add_DeliveriesControl control;
                if (evenodd == 0)
                {
                    control = new Add_DeliveriesControl(deliver, 0);
                    evenodd = 1;
                }
                else
                {
                    control = new Add_DeliveriesControl(deliver, 1);
                    evenodd = 0;
                }
                DeliveriesPanel1.Children.Add(control);
            }
        }
        public void DisplayFinancialLiabilities(string filter)
        {
            FinancilaLiabilitiesPanel.Children.Clear();
            int evenodd = 0;
            List<FinancialLiabilities> finance = financialLiabilities.GetList(filter);
            
            foreach (FinancialLiabilities list in finance) { 
                Add_FinancialLiabilitiesControl control;
                if (evenodd == 0)
                {
                    control = new Add_FinancialLiabilitiesControl(list, 0);
                    evenodd = 1;
                }
                else {
                    control = new Add_FinancialLiabilitiesControl(list, 1);
                    evenodd = 0;
                }
                
                FinancilaLiabilitiesPanel.Children.Add(control);
            }
        }
        public void DisplaySales() {
            OrderListPanel.Children.Clear();
            int evenodd = 0;
            List<Sales> salesList = salesServices.GetSalesList();
       
            foreach (Sales sales in salesList) { 
                string refid = sales.receipt_id.ToString();
                string mode = sales.payment_mode.ToString();
                string status = sales.status.ToString();
                string price = sales.total_price.ToString("N2");
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

            if (mainWindow != null)
            {
                add_FinancialLiabilities = new Add_FinancialLiabilities(mainWindow);
                mainWindow.ActiveOverlay(true);
                add_FinancialLiabilities.ShowDialog();       
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow. dashboard3");
            }
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {

            if (mainWindow != null)
            {
                Add_Delivery = new Add_Delivery(mainWindow);
                mainWindow.ActiveOverlay(true);
                Add_Delivery.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow. dashboard1");
            }
            
            
        }
        

    }
}
