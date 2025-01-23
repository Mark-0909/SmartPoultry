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
    /// Interaction logic for Records_ExpensesControl.xaml
    /// </summary>
    public partial class Records_ExpensesControl : UserControl
    {
        AppDbContext context = new AppDbContext();
        UserServices userServices;
        public Records_ExpensesControl(Expenses expenses, int evenOdd)
        {
            InitializeComponent();
            userServices = new UserServices(context);

            NameLabel.Content = expenses.Name;
            DateLabel.Content = expenses.Added_Date.ToString("MM-dd-yyyy");
            EmployeeLabel.Content = userServices.GetUser(expenses.Employee_Incharge).Username;
            PurposeLabel.Content = expenses.Category;
            AmountLabel.Content = expenses.price.ToString("N2");

            if(evenOdd == 1)
            {
                ThisBorder.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
