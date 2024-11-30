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
        public LoginPage? loginWindow = Application.Current.Windows.OfType<LoginPage>().FirstOrDefault();
        public AppDbContext context = new AppDbContext();
        public UserServices userServices;
        public Login_CreateAccount()
        {
            InitializeComponent();
            userServices = new UserServices(context);
        }
        public void Changelabel()
        {
            Controllabel.Content = "CREATE ACCOUNT";
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

        private void UsernameTb_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Username...", true);
        }
        private void UsernameTb_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Username...", false);
        }

        private void Q1Tb_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(q1TB, "Pet's name...", true);
        }
        private void Q1Tb_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(q1TB, "Pet's name...", false);
        }
        private void Q2Tb_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(q2TB, "Favorite color...", true);
        }
        private void Q2Tb_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(q2TB, "Favorite color...", false);
        }
        private void Q3Tb_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(q3TB, "Book or movie...", true);
        }
        private void Q3Tb_LostFocused(object sender, RoutedEventArgs e)
        {
            
            HandleTextBoxPlaceholder(q3TB, "Book or movie...", false);
        }
        private void PassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordTB.Password))
            {
                createpassoverTB.Visibility = Visibility.Visible;
            }
            else
            {
                createpassoverTB.Visibility = Visibility.Hidden;
            }
            
        }
        private void ConfirmPassTB_LostFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(confirmpassTB.Password))
            {
                confirmpassoverTB.Visibility = Visibility.Visible;
            }
            else
            {
                confirmpassoverTB.Visibility = Visibility.Hidden;
            }
        }
        private void ConfirmOverPass_GotFocused(object sender, RoutedEventArgs e)
        {
            confirmpassoverTB.Visibility = Visibility.Hidden;
            confirmpassTB.Focus();
            
        }
        private void CreatePass_GotFocused(object sender, RoutedEventArgs e)
        {
            createpassoverTB.Visibility = Visibility.Hidden;
            createpassoverTB.Focus();
        }
        public void HandleTextBoxPlaceholder(TextBox tb, string placeholder, bool isFocused)
        {
            if (isFocused)
            {
                if (tb.Text == placeholder)
                {
                    tb.Text = string.Empty;
                    tb.Foreground = Brushes.Black;
                }
            }
            else // When the TextBox loses focus
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginWindow.ChangeControl("login");
        }
    }
}
