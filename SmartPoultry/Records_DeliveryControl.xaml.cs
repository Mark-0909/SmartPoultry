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
    /// Interaction logic for Records_DeliveryControl.xaml
    /// </summary>
    public partial class Records_DeliveryControl : UserControl
    {
        AppDbContext context = new AppDbContext();
        UserServices userServices;
        Deliveries deliveries;
        public Records_DeliveryControl(Deliveries deliveries, int evenOdd)
        {
            InitializeComponent();
            userServices = new UserServices(context);

            this.deliveries = deliveries;
            NameLabel.Content = deliveries.name;
            DateLabel.Content = deliveries.added_date.ToString("MM-dd-yyyy");
            EmployeeLabel.Content = userServices.GetUser(deliveries.employee_incharge).Username;
            PurposeLabel.Content = deliveries.delivery_status.ToString();
            AmountLabel.Content = deliveries.price.ToString("N2");

            if(evenOdd == 1)
            {
                ThisBorder.Background = new SolidColorBrush(Colors.White);  
            }
        }

        private void DeliveryInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = UserContext.mainWindow;
            Add_Delivery add_Deliveries = new Add_Delivery(deliveries, mainWindow);
            mainWindow.ActiveOverlay(true);
            add_Deliveries.ShowDialog();
        }
    }
}
