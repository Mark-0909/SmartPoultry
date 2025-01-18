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
            FetchInventoryLogs();
        }
        public void FetchInventoryLogs()
        {

            List<InventoryLogs> inventory = inventoryLogsServices.GetList();
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
