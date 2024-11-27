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
    /// Interaction logic for Login_LoginControl.xaml
    /// </summary>
    public partial class Login_LoginControl : UserControl
    {
        //database
        public AppDbContext context = new AppDbContext();
        UserServices userServices;

        public LoginPage? loginWindow = Application.Current.Windows.OfType<LoginPage>().FirstOrDefault();
        MainWindow window = new MainWindow();
        public Login_LoginControl()
        {
            InitializeComponent();
            userServices = new UserServices(context);
        }

        private void Submit_Clicked(object sender, RoutedEventArgs e)
        {

            string username = usernameTB.Text;
            string password = passwordTB.Password;

            try
            {
                bool isVerified = userServices.LoginVerification(username, password);

                if (isVerified)
                {
                    
                    

                    MainWindow window = new MainWindow();
                    Application.Current.MainWindow = window;
                    window.Show();
                    loginWindow.SuccessLogin();
                }
                else
                {
                    MessageBox.Show("Incorrect username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show("An error occurred while processing your login. Please try again later.");
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
    }
}
