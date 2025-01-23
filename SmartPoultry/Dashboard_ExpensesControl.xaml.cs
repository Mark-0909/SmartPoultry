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
    /// Interaction logic for Dashboard_ExpensesControl.xaml
    /// </summary>
    public partial class Dashboard_ExpensesControl : UserControl
    {
        public Dashboard_ExpensesControl(Expenses expenses, int evenOdd)
        {
            InitializeComponent();

            Namelabel.Content = expenses.Name;
            DueDateLabel.Content = expenses.Category;
            AmountLabel.Content = expenses.price.ToString("N2");

            if(evenOdd == 1)
            {
                thisBorder.Background = new SolidColorBrush(Colors.White);
            }
            

        }

        private void Info_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
