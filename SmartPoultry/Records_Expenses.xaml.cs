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

            DisplayExpenses("");
        }
        public void DisplayExpenses(string searchTerm)
        {
            if(SalesPanel.Children.Count != 0)
            {
                SalesPanel.Children.Clear();
            }

            List<Expenses> expenses = expensesServices.GeAllExpenses(); 

           
            expenses = expenses.Where(x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm.ToLower())) ||
                (x.Category != null && x.Category.ToLower().Contains(searchTerm.ToLower())) ||
                (x.Status != null && x.Status.ToLower().Contains(searchTerm.ToLower())) ||
                (x.Remarks != null && x.Remarks.ToLower().Contains(searchTerm.ToLower())) ||
                x.Order_ID.ToString().Contains(searchTerm) ||
                x.price.ToString("0.00").Contains(searchTerm) ||
                x.Added_Date.ToString("yyyy-MM-dd").Contains(searchTerm) ||
                x.Updated_Time.ToString("yyyy-MM-dd").Contains(searchTerm) ||
                x.Employee_Incharge.ToString().Contains(searchTerm)
            ).ToList();


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

                SalesPanel.Children.Add(control);   

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
