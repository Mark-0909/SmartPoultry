using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using static SmartPoultry.App;
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
    /// Interaction logic for Login_LoginControl.xaml
    /// </summary>
    public partial class Login_LoginControl : UserControl
    {
        //database
        public AppDbContext context = new AppDbContext();
        UserServices userServices;
        public UserLogsServices userLogsServices;

        public LoginPage? loginWindow = Application.Current.Windows.OfType<LoginPage>().FirstOrDefault();
        MainWindow window = new MainWindow();
        public Login_LoginControl()
        {
            InitializeComponent();
            userServices = new UserServices(context);
            userLogsServices = new UserLogsServices(context);
        }

        private void Submit_Clicked(object sender, RoutedEventArgs e)
        {
            Submit();
        }
        public void Submit()
        {
            string username = usernameTB.Text;
            string password = passwordTB.Password;

            if (string.IsNullOrWhiteSpace(username) || username == "Enter Username...")
            {
                loginWindow.PopUpNotif("alert", "Please enter a username.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                loginWindow.PopUpNotif("alert", "Please enter a password.");
                return;
            }
            else if (username == "Enter Username..." && string.IsNullOrWhiteSpace(password))
            {
                loginWindow.PopUpNotif("alert", "Form is empty.");
                return;
            }

            try
            {
                bool isVerified = userServices.LoginVerification(username, password);

                if (isVerified)
                {
                    MainWindow window = new MainWindow();
                    Application.Current.MainWindow = window;
                    window.Show();
                    loginWindow.SuccessLogin();
                    UserContext.mainWindow = window;
                    int user_id = UserContext.CurrentUserId;
                    bool isRecorded = userLogsServices.Create(user_id, "LOGIN");
                    if (!isRecorded) 
                    {
                        loginWindow.PopUpNotif("alert", "Not Recorded");
                    }
                }
                else
                {
                    loginWindow.PopUpNotif("alert", "Incorrect username or password");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                loginWindow.PopUpNotif("alert", "An error occurred while processing your login.");
            }
        }
        private void TB_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && sender == usernameTB)
            {
                passwordoverTB.Visibility = Visibility.Hidden;
                passwordTB.Focus();
            }
            if (e.Key == Key.Enter && sender == passwordTB)
            {
                Submit();
            }
            if (e.Key == Key.Enter && sender == usernameTB)
            {
                Submit();
            }
        }
        private void CreateAccount_Clicked(object sender, RoutedEventArgs e)
        {
            loginWindow.ChangeControl("create");
        }

        private void ForgotPass_Clicked(object sender, RoutedEventArgs e)
        {
            loginWindow.ChangeControl("forgot");
        }

        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Enter Username...", true);
        }
        private void UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Enter Username...", false);
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
            else 
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        public void PassOverTB_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordoverTB.Visibility = Visibility.Hidden;
            passwordTB.Focus();
        }

        public void PassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordTB.Password))
            {
                passwordoverTB.Visibility = Visibility.Visible;
            }
            else
            {
                passwordoverTB.Visibility = Visibility.Hidden;
            }
        }

    }
}
