using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
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
    /// Interaction logic for Login_CreateAccount.xaml
    /// </summary>
    public partial class Login_CreateAccount : UserControl
    {
        public AppDbContext context = new AppDbContext();
        public UserServices userServices;
        public Login_CreateAccount()
        {
            InitializeComponent();
            userServices = new UserServices(context);
        }

        private void Submit_Clicked(object sender, RoutedEventArgs e)
        {
            string role = "admin";
            if (userServices.IsThereAdmin())
            {
                role = "employee";
            }
            string username = usernameTB.Text;
            string password;
            if (passwordTB.Password == confirmpassTB.Password)
            {
                password = confirmpassTB.Password;
            }
            else {
                MessageBox.Show("Password does not match!");
                return;
            }
            string q1 = q1TB.Text;
            string q2 = q2TB.Text;
            string q3 = q3TB.Text;

            userServices.CreateAccount(username, password, q1, q2, q3, role);
        }
    }
}
