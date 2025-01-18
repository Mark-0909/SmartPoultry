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

            FetchSales();
        }
        public void FetchSales()
        {
            List<Sales> sales = salesServices.GetAllSales();
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
