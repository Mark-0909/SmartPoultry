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
    /// Interaction logic for Records_Sales.xaml
    /// </summary>
    public partial class Records_Sales : UserControl
    {
        AppDbContext context = new AppDbContext();
        SalesServices salesServices;
        public Records_Sales()
        {
            InitializeComponent();
            salesServices = new SalesServices(context);

            FetchSales("");
        }
        public void FetchSales(string searchTerm)
        {
            if (SalesPanel.Children.Count > 0)
            {
                SalesPanel.Children.Clear();
            }
            List<Sales> sales = salesServices.GetAllSales(); 

 
            sales = sales.Where(x =>
                x.receipt_id.ToString().Contains(searchTerm) ||                                   
                (x.product_list != null && x.product_list.ToLower().Contains(searchTerm.ToLower())) || 
                (x.price_list != null && x.price_list.ToLower().Contains(searchTerm.ToLower())) ||      
                (x.quantity_list != null && x.quantity_list.ToLower().Contains(searchTerm.ToLower())) || 
                x.purchase_date.ToString("yyyy-MM-dd").Contains(searchTerm) ||                    
                (x.variation_list != null && x.variation_list.ToLower().Contains(searchTerm.ToLower())) || 
                (x.payment_mode != null && x.payment_mode.ToLower().Contains(searchTerm.ToLower())) ||
                (x.status != null && x.status.ToLower().Contains(searchTerm.ToLower())) ||              
                (x.purchase_method != null && x.purchase_method.ToLower().Contains(searchTerm.ToLower())) || 
                x.total_price.ToString().Contains(searchTerm) ||                                   
                (x.Remarks != null && x.Remarks.ToLower().Contains(searchTerm.ToLower()))            
            ).ToList();

            int evenodd = 0;
            for (int i = 0; i < sales.Count; i++)
            {

                Records_SalesControl control = new Records_SalesControl(sales[i].receipt_id, evenodd);

                SalesPanel.Children.Add(control);
                if (evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }

    }
}
