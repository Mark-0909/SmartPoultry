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
    /// Interaction logic for Add_FinancialLiabilitiesControl.xaml
    /// </summary>
    public partial class Add_FinancialLiabilitiesControl : UserControl
    {
        public Add_FinancialLiabilitiesControl(int id, string name, string duedate, string amount)
        {
            InitializeComponent();
            Namelabel.Content = name;
            DueDateLabel.Content = duedate;
            AmountLabel.Content = amount;
        }
    }
}
