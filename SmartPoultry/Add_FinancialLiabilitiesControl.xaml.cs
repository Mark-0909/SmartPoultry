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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Add_FinancialLiabilitiesControl.xaml
    /// </summary>
    public partial class Add_FinancialLiabilitiesControl : UserControl
    {
        public Add_FinancialLiabilitiesControl(int id, string name, string duedate, string amount, int evenodd)
        {
            InitializeComponent();
            Namelabel.Content = name;
            DueDateLabel.Content = duedate;
            AmountLabel.Content = amount;

            DateTime dueDateValue;
            bool isValidDate = DateTime.TryParse(duedate, out dueDateValue);

            if (isValidDate)
            {
                
                DateTime today = DateTime.Now.Date;
                DateTime yellowDate = today.AddDays(1);

                
                if (dueDateValue.Date <= today) 
                {
                    thisBorder1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3A8A8")); // Red
                }
                else if (dueDateValue.Date == yellowDate)
                {
                    thisBorder1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EEAF")); // yellow
                }
            }
            else
            {
                thisBorder1.BorderBrush = new SolidColorBrush(Colors.Gray); 
            }

            if (evenodd == 1)
            {
                thisBorder1.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                thisBorder1.Background = new SolidColorBrush(Colors.LightGray); 
            }
        }


    }
}
