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
    /// Interaction logic for Records_Delivery.xaml
    /// </summary>
    public partial class Records_Delivery : UserControl
    {
        AppDbContext context = new AppDbContext();
        DeliveriesServices deliveriesServices;
        public Records_Delivery()
        {
            InitializeComponent();
            deliveriesServices = new DeliveriesServices(context);
            DisplayDeliveries();
        }
        public void DisplayDeliveries()
        {
            if(SalesPanel.Children.Count != 0)
            {
                SalesPanel.Children.Clear();
            }
            List<Deliveries> deliveries = deliveriesServices.GetDeliveriesList();

            int evenOdd = 0;
            for(int i = 0; i < deliveries.Count; i++)
            {
                Records_DeliveryControl control = new Records_DeliveryControl(deliveries[i], evenOdd);
                SalesPanel.Children.Add(control);
                if(evenOdd == 0)
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
