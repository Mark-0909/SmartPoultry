using SmartPoultry.Migrations;
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
    /// Interaction logic for Add_DeliveriesControl.xaml
    /// </summary>
    public partial class Add_DeliveriesControl : UserControl
    {
        public Add_DeliveriesControl(int id, string name, string date, string status, int evenodd)
        {
            InitializeComponent();
            NameLabel.Content = name;
            Datelabel.Content = date;
            StatusLabel.Content = status;

            DateTime yellowdate = DateTime.Now.AddDays(1);

            string red = DateTime.Now.ToString("MM-dd-yyyy");
            string yellow = yellowdate.ToString("MM-dd-yyyy");


            if (date == red)
            {
                thisBorder.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (date == yellow) {
                thisBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
            }
            

            if (evenodd == 1)
            {
                this.thisBorder.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
