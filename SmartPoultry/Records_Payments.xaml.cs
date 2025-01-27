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
    /// Interaction logic for Records_Payments.xaml
    /// </summary>
    public partial class Records_Payments : UserControl
    {
        AppDbContext context = new AppDbContext();
        FinancialLiabilitiesServices FinancialLiabilitiesServices;
        public Records_Payments()
        {
            InitializeComponent();
            FinancialLiabilitiesServices = new FinancialLiabilitiesServices(context);
            DisplayPayments("");
        }
        public void DisplayPayments( string searchTerm)
        {
            if(SalesPanel.Children.Count != 0)
            {
                SalesPanel.Children.Clear();
            }
            List<FinancialLiabilities> finances = FinancialLiabilitiesServices.GetAllPayments(); 


            finances = finances.Where(x =>
                (x.name != null && x.name.ToLower().Contains(searchTerm.ToLower())) ||             
                x.order_id.ToString().Contains(searchTerm) ||                                      
                x.amount.ToString().Contains(searchTerm) ||                                      
                (x.type != null && x.type.ToLower().Contains(searchTerm.ToLower())) ||             
                (x.status != null && x.status.ToLower().Contains(searchTerm.ToLower())) ||       
                x.added_date.ToString("yyyy-MM-dd").Contains(searchTerm) ||                         
                x.due_date.ToString("yyyy-MM-dd").Contains(searchTerm) ||                          
                x.updated_date.ToString("yyyy-MM-dd").Contains(searchTerm) ||                     
                (x.contacts != null && x.contacts.ToLower().Contains(searchTerm.ToLower())) ||      
                (x.payment_mode != null && x.payment_mode.ToLower().Contains(searchTerm.ToLower())) || 
                (x.Remarks != null && x.Remarks.ToLower().Contains(searchTerm.ToLower()))        
            ).ToList();

            int evenOdd = 0;

            for(int i = 0; i < finances.Count; i++)
            {
                Records_PaymentsControl control = new Records_PaymentsControl(finances[i], evenOdd);

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
