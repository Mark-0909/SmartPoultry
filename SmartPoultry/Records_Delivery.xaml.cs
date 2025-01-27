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
            DisplayDeliveries("");
        }
        public void DisplayDeliveries(string searchTerm)
        {
            if(SalesPanel.Children.Count != 0)
            {
                SalesPanel.Children.Clear();
            }
            List<Deliveries> deliveries = deliveriesServices.GetDeliveriesList();

            // Filter deliveries based on the search term across multiple properties
            deliveries = deliveries.Where(x =>
                (x.type != null && x.type.ToLower().Contains(searchTerm.ToLower())) ||
                (x.name != null && x.name.ToLower().Contains(searchTerm.ToLower())) ||
                (x.address != null && x.address.ToLower().Contains(searchTerm.ToLower())) ||
                (x.payment_status != null && x.payment_status.ToLower().Contains(searchTerm.ToLower())) ||
                (x.delivery_status != null && x.delivery_status.ToLower().Contains(searchTerm.ToLower())) ||
                (x.contact_no != null && x.contact_no.ToLower().Contains(searchTerm.ToLower())) ||
                (x.delivery_man != null && x.delivery_man.ToLower().Contains(searchTerm.ToLower())) ||
                (x.Remarks != null && x.Remarks.ToLower().Contains(searchTerm.ToLower())) ||
                x.order_id.ToString().Contains(searchTerm) ||
                x.price.ToString("0.00").Contains(searchTerm) ||
                x.charges.ToString("0.00").Contains(searchTerm) ||
                x.added_date.ToString("yyyy-MM-dd").Contains(searchTerm) ||
                x.delivery_date.ToString("yyyy-MM-dd").Contains(searchTerm)
            ).ToList();


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
