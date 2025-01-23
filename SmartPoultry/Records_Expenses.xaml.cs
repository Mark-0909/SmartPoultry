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
    /// Interaction logic for Records_Expenses.xaml
    /// </summary>
    public partial class Records_Expenses : UserControl
    {
        AppDbContext context = new AppDbContext();
        ExpensesServices expensesServices;
        public Records_Expenses()
        {
            InitializeComponent();

            expensesServices = new ExpensesServices(context);

            DisplayExpenses();
        }
        public void DisplayExpenses()
        {
            if(ExpensesPanel.Children.Count != 0)
            {
                ExpensesPanel.Children.Clear();
            }

            List<Expenses> expenses = new List<Expenses>();

            try
            {
                expenses = expensesServices.GeAllExpenses();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }


            int evenOdd = 0;
            for(int i = 0; i < expenses.Count; i++)
            {

                Records_ExpensesControl control = new Records_ExpensesControl(expenses[i], evenOdd);

                ExpensesPanel.Children.Add(control);   

                if (evenOdd == 0)
                {
                    evenOdd = 1;
                }
                else 
                {
                    evenOdd = 0;
                }
            }
        }
    }
}
