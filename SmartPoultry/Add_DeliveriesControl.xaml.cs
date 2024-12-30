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
    /// Interaction logic for Add_DeliveriesControl.xaml
    /// </summary>
    public partial class Add_DeliveriesControl : UserControl
    {
        public AppDbContext context = new AppDbContext();
        public DeliveriesServices deliveriesServices;
        public int ID;
        public Add_DeliveriesControl(Deliveries deliver, int evenodd)
        {
            InitializeComponent();
            NameLabel.Content = deliver.name;
            Datelabel.Content = deliver.delivery_date;
            StatusLabel.Content = deliver.payment_status;
            ID = deliver.Id;
            deliveriesServices = new DeliveriesServices(context);

            DateTime dateValue;
            bool isValidDate = DateTime.TryParse(deliver.delivery_date.ToString(), out dateValue);

            if (isValidDate)
            {
                DateTime today = DateTime.Now.Date;
                DateTime yellowDate = today.AddDays(1);

                if (dateValue.Date <= today) 
                {
                    thisBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3A8A8")); // Red

                }
                else if (dateValue.Date == yellowDate) 
                {
                    thisBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EEAF")); // yellow
                }
            }
            else
            {
                thisBorder.BorderBrush = new SolidColorBrush(Colors.Gray); 
            }

            if (evenodd == 1)
            {
                thisBorder.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                thisBorder.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void DisplayDetail_Clicked(object sender, RoutedEventArgs e)
        {
            Deliveries var = deliveriesServices.GetById(ID);


            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;

            Add_Delivery window = new Add_Delivery(var, mainWindow);
            if (mainWindow != null)
            {

                mainWindow.ActiveOverlay(true);
                window.ShowDialog();

            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.");
            }
        }
    }
}
