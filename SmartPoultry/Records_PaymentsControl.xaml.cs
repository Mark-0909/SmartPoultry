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
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Records_PaymentsControl.xaml
    /// </summary>
    public partial class Records_PaymentsControl : UserControl
    {
        AppDbContext context = new AppDbContext();
        UserServices userServices;
        FinancialLiabilities financialLiabilities;
        public Records_PaymentsControl(FinancialLiabilities finance, int evenOdd)
        {
            InitializeComponent();
            userServices = new UserServices(context);  
            NameLabel.Content = finance.name;
            DateLabel.Content = finance.added_date.ToString("MM-dd-yyyy");
            EmployeeLabel.Content = userServices.GetUser(finance.employee_incharge).Username;
            PurposeLabel.Content = finance.status;
            AmountLabel.Content = finance.amount.ToString("N2");
            financialLiabilities = finance;

            if (evenOdd == 1)
            {
                ThisBorder.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void PaymentInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = UserContext.mainWindow;
            Add_FinancialLiabilities add_FinancialLiabilities = new Add_FinancialLiabilities(financialLiabilities, mainWindow);
            mainWindow.ActiveOverlay(true);
            add_FinancialLiabilities.ShowDialog();
        }
    }
}
