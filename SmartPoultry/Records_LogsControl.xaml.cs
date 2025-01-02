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
using SmartPoultry.Models;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Records_LogsControl.xaml
    /// </summary>
    public partial class Records_LogsControl : UserControl
    {
        public AppDbContext context = new AppDbContext();
        public UserServices userServices;
        public Records_LogsControl(int id, string action, DateTime date, int evenodd)
        {
            InitializeComponent();
            userServices = new UserServices(context);
            User name = userServices.GetUser(id);

            NameLabel.Content = name.Username;
            ActionLabel.Content = action;
            DateLabel.Content = date.ToString("MM-dd-yyyy HH:mm");


            if (evenodd == 1)
            {
                ThisBorder.Background = Brushes.White;
                ThisBorder.BorderBrush = Brushes.White;
            }
        }
    }
}
