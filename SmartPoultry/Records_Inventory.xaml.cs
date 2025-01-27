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
    /// Interaction logic for Records_Inventory.xaml
    /// </summary>
    public partial class Records_Inventory : UserControl
    {
        AppDbContext context = new AppDbContext();
        InventoryLogsServices inventoryLogsServices;
        public Records_Inventory()
        {
            InitializeComponent();
            inventoryLogsServices = new InventoryLogsServices(context);
            FetchInventoryLogs("");
        }
        public void FetchInventoryLogs(string searchTerm)
        {
            if(SalesPanel.Children.Count > 0)
            {
                SalesPanel.Children.Clear();
            }

            List<InventoryLogs> inventory = inventoryLogsServices.GetList(); 

            
            inventory = inventory.Where(x =>
                (x.action != null && x.action.ToLower().Contains(searchTerm.ToLower())) ||    
                (x.reason != null && x.reason.ToLower().Contains(searchTerm.ToLower())) ||    
                x.product_id.ToString().Contains(searchTerm) ||                             
                x.employee_incharge.ToString().Contains(searchTerm) ||                      
                x.quatity.ToString().Contains(searchTerm) ||                                
                x.timestamp.ToString("yyyy-MM-dd").Contains(searchTerm)                     
            ).ToList();

            int evenodd = 0;
            for (int i = 0; i < inventory.Count; i++)
            {

                Records_InventoryControl control = new Records_InventoryControl(inventory[i].product_id, inventory[i].employee_incharge, inventory[i].action, inventory[i].timestamp, inventory[i].reason, evenodd);

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
