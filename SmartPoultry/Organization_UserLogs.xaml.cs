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
    /// Interaction logic for Organization_UserLogs.xaml
    /// </summary>
    public partial class Organization_UserLogs : UserControl
    {
        public Organization_UserLogs(UserLogs log, int evenodd)
        {
            InitializeComponent();

            DateLabel.Content = log.timestamp.ToString("MM/dd/yyyy HH:mm");

            ActionLabel.Content = log.action.ToString();

            if (evenodd == 1) 
            {
                ControlBorder.Background = Brushes.White;
            }
        }
    }
}
