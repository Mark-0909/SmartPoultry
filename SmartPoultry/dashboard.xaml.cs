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
            FromDatePicker.SelectedDate = DateTime.Now.AddDays(-6);
            ToDatePicker.SelectedDate = DateTime.Now;
            
            DisplaySales();
            CountDeliveries();
            CountPayments();
            DisplayTodaysExpenses();
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

            

            GenerateRevenueAndCostComboChart(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
            OverAllSalesAndCostPieGraph(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);

            //SalesAndCostPieChart.Visibility = Visibility.Hidden;
            GrowthAndTrendsAnalysis.Visibility = Visibility.Hidden;
        }

        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromDatePicker.SelectedDate.HasValue)
            {
                DateTime fromDate = FromDatePicker.SelectedDate.Value;

                ToDatePicker.BlackoutDates.Clear();
                ToDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, fromDate.AddDays(-1))); 
                ToDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
            }
            else
            {
                ToDatePicker.BlackoutDates.Clear();
                ToDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
            }

            if (FromDatePicker.SelectedDate.HasValue && ToDatePicker.SelectedDate.HasValue)
            {
                GenerateRevenueAndCostComboChart(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
                OverAllSalesAndCostPieGraph(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
            }
        }

        private void ToDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToDatePicker.SelectedDate.HasValue)
            {
                DateTime toDate = ToDatePicker.SelectedDate.Value;

                FromDatePicker.BlackoutDates.Clear();
                FromDatePicker.BlackoutDates.Add(new CalendarDateRange(toDate.AddDays(1), DateTime.MaxValue)); 
            }
            else
            {
                FromDatePicker.BlackoutDates.Clear();
            }


            if (FromDatePicker.SelectedDate.HasValue && ToDatePicker.SelectedDate.HasValue)
            {
                GenerateRevenueAndCostComboChart(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
                OverAllSalesAndCostPieGraph(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
            }
        }



        public void DisplayTodaysExpenses()
        {
            if(ExpensesListPanel.Children.Count != 0)
            {
                ExpensesListPanel.Children.Clear();
            }

            List<Expenses> expenses = expensesServices.GetTodaysExpenses();
            int evenOdd = 0;
            for(int i = 0; i < expenses.Count; i++)
            {
                Dashboard_ExpensesControl control = new Dashboard_ExpensesControl(expenses[i], evenOdd);

                if (evenOdd == 0)
                {
                    evenOdd = 1;
                }
                else 
                {
                    evenOdd = 0;
                }

                ExpensesListPanel.Children.Add(control);
            }

            
        }


        public void GenerateRevenueAndCostComboChart(DateTime FromDate, DateTime ToDate)
        {
            GrowthAndTrendsAnalysis.Series.Clear();

            List<Sales> sales = salesServices.GetAllSales();
            List<Expenses> expenses = expensesServices.GeAllExpenses();

            List<DateTime> labels = new List<DateTime>();
            List<decimal> revenueValues = new List<decimal>();
            List<decimal> costValues = new List<decimal>();


            bool isMoreThanOneMonth = (ToDate - FromDate).Days > 30;

            if (isMoreThanOneMonth)
            {
                DateTime currentStartOfWeek = FromDate.Date.AddDays(-(int)FromDate.DayOfWeek);  

                while (currentStartOfWeek <= ToDate)
                {
                    DateTime currentEndOfWeek = currentStartOfWeek.AddDays(6); 
                    if (currentEndOfWeek > ToDate)
                    {
                        currentEndOfWeek = ToDate;
                    }

                    labels.Add(currentStartOfWeek);

                    decimal totalSales = sales
                        .Where(s => s.status == "paid" && s.purchase_date >= currentStartOfWeek && s.purchase_date <= currentEndOfWeek)
                        .Sum(s => s.total_price);

                    decimal totalExpenses = expenses
                        .Where(e => e.Added_Date >= currentStartOfWeek && e.Added_Date <= currentEndOfWeek)
                        .Sum(e => e.price);

                    revenueValues.Add(totalSales);
                    costValues.Add(totalExpenses);

                   
                    currentStartOfWeek = currentStartOfWeek.AddDays(7);
                }
            }
            else
            {
                
                DateTime currentDate = FromDate.Date;
                while (currentDate <= ToDate)
                {
                    labels.Add(currentDate);

                    decimal totalSales = sales
                        .Where(s => s.status == "paid" && s.purchase_date.Date == currentDate)
                        .Sum(s => s.total_price);

                    decimal totalExpenses = expenses
                        .Where(e => e.Added_Date.Date == currentDate)
                        .Sum(e => e.price);

                    revenueValues.Add(totalSales);
                    costValues.Add(totalExpenses);

                    currentDate = currentDate.AddDays(1);
                }
            }

            
            GrowthAndTrendsAnalysis.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Revenue",
                        Values = new ChartValues<decimal>(revenueValues),
                        Fill = Brushes.SteelBlue
                    },
                    new ColumnSeries
                    {
                        Title = "Cost",
                        Values = new ChartValues<decimal>(costValues),
                        Fill = Brushes.Tomato
                    }
                };

            
            GrowthAndTrendsAnalysis.AxisX.Clear();
            GrowthAndTrendsAnalysis.AxisX.Add(new Axis
            {
                Labels = labels.Select(d => isMoreThanOneMonth ? $"{d:MM/dd} - {d.AddDays(6):MM/dd}" : $"{d:MM/dd/yyyy}").ToArray(),
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
                LabelsRotation = -45  
            });

            
            GrowthAndTrendsAnalysis.AxisY.Clear();
            GrowthAndTrendsAnalysis.AxisY.Add(new Axis
            {
                Title = "Amount",
                LabelFormatter = value => $"₱{value:N2}"
            });
        }


        public void OverAllSalesAndCostPieGraph(DateTime FromDate, DateTime ToDate)
        {
            
            List<Sales> sales = salesServices.GetAllSales();
            List<Expenses> expenses = expensesServices.GeAllExpenses();
            List<FinancialLiabilities> finances = financialLiabilities.GetAllPayments();

            
            List<FinancialLiabilities> unpaid = finances
                .Where(p => p.status.Trim().Equals("Unpaid", StringComparison.OrdinalIgnoreCase) &&
                            p.type.Trim().Equals("To Receive", StringComparison.OrdinalIgnoreCase) &&
                            p.updated_date.Date >= FromDate.Date &&
                            p.updated_date.Date <= ToDate.Date)
                .ToList();

            List<Sales> paid = sales
                .Where(p => p.purchase_date.Date >= FromDate.Date &&
                            p.purchase_date.Date <= ToDate.Date &&
                            p.status.Trim().Equals("paid", StringComparison.OrdinalIgnoreCase))
                .ToList();

            expenses = expenses
                .Where(p => p.Added_Date.Date >= FromDate.Date &&
                            p.Added_Date.Date <= ToDate.Date)
                .ToList();

            finances = finances
                .Where(p => p.status.Trim().Equals("Unpaid", StringComparison.OrdinalIgnoreCase) &&
                            p.type.Trim().Equals("To Pay", StringComparison.OrdinalIgnoreCase) &&
                            p.updated_date.Date >= FromDate.Date &&
                            p.updated_date.Date <= ToDate.Date)
                .ToList();

            
            decimal totalUnpaidSales = unpaid.Any() ? unpaid.Sum(p => p.amount) : 0;
            decimal totalPaidSales = paid.Any() ? paid.Sum(p => p.total_price) : 0;
            decimal totalExpenses = expenses.Any() ? expenses.Sum(e => e.price) : 0;
            decimal totalLiabilities = finances.Any() ? finances.Sum(f => f.amount) : 0;

            
            SalesAndCostPieChart.Series = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Unpaid Sales",
            Values = new ChartValues<decimal> { totalUnpaidSales },
            Fill = Brushes.OrangeRed
        },
        new PieSeries
        {
            Title = "Paid Sales",
            Values = new ChartValues<decimal> { totalPaidSales },
            Fill = Brushes.Green
        },
        new PieSeries
        {
            Title = "Expenses",
            Values = new ChartValues<decimal> { totalExpenses },
            Fill = Brushes.CornflowerBlue
        },
        new PieSeries
        {
            Title = "Liabilities",
            Values = new ChartValues<decimal> { totalLiabilities },
            Fill = Brushes.Indigo
        }
    };

            
            RightSidePanel.Children.Clear(); 

            AddLegendEntry("Unpaid Sales", totalUnpaidSales, Brushes.OrangeRed);
            AddLegendEntry("Paid Sales", totalPaidSales, Brushes.Green);
            AddLegendEntry("Expenses", totalExpenses, Brushes.CornflowerBlue);
            AddLegendEntry("Liabilities", totalLiabilities, Brushes.Indigo);
        }

        
        private void AddLegendEntry(string title, decimal amount, Brush color)
        {
            
            StackPanel legendEntry = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

            
            Rectangle colorBox = new Rectangle
            {
                Width = 15,
                Height = 15,
                Fill = color,
                Margin = new Thickness(0, 0, 5, 0)
            };

            
            TextBlock label = new TextBlock
            {
                Text = $"{title}: ₱{amount:N2}",
                VerticalAlignment = VerticalAlignment.Center
            };

            
            legendEntry.Children.Add(colorBox);
            legendEntry.Children.Add(label);

            
            RightSidePanel.Children.Add(legendEntry);
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

        public void DynamicUpdateCharts()
        {
            GenerateRevenueAndCostComboChart(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
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
