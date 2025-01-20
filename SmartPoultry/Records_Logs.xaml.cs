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
    /// Interaction logic for Records_Logs.xaml
    /// </summary>
    public partial class Records_Logs : UserControl
    {
        AppDbContext context = new AppDbContext();
        UserLogsServices userLogsServices;
        public Records_Logs()
        {
            InitializeComponent();
            userLogsServices = new UserLogsServices(context);
            FetchUserLogs();
        }

        public void FetchUserLogs()
        {
            List<UserLogs> logs = userLogsServices.GetList();
            int evenodd = 0;
            for (int i = 0; i < logs.Count; i++)
            {

                Records_LogsControl control = new Records_LogsControl(logs[i].user_id, logs[i].action, logs[i].timestamp, evenodd);

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
