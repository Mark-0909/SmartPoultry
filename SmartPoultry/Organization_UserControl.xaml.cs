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

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Organization_UserControl.xaml
    /// </summary>
    public partial class Organization_UserControl : UserControl
    {
        Organization organization;
        User useinfo;
        public Organization_UserControl(User user, Organization org)
        {
            InitializeComponent();
            NameLabel.Content = user.Username;
            RoleLabel.Content = user.Role;

            organization = org;
            useinfo = user;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            organization.ViewUser(useinfo);
        }
    }
}
