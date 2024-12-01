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
    /// Interaction logic for Login_ForgotPassword.xaml
    /// </summary>
    public partial class Login_ForgotPassword : UserControl
    {
        UserServices UserServices;
        public AppDbContext context = new AppDbContext();
        public int randomindex;
        string[] questions = {"First Pet's Name?","Favorite Color?","Favorite Book or Movie?"};
        public LoginPage? loginWindow = Application.Current.Windows.OfType<LoginPage>().FirstOrDefault();
        public Login_ForgotPassword()
        {
            InitializeComponent();
            UserServices = new UserServices(context);
            Random random = new Random();
            randomindex = random.Next(questions.Length);
            QuestionLabel.Content = questions[randomindex];
        }

        private void Submit_Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(createNewPassTB.Password) || string.IsNullOrWhiteSpace(confirmNewPassTB.Password))
            {
                MessageBox.Show("Please fill all required fields.");
                return;
            }

            if (createNewPassTB.Password != confirmNewPassTB.Password)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                bool passwordChanged = UserServices.UpdatePassword(usernameTB.Text, confirmNewPassTB.Password);
                if (!passwordChanged)
                {
                    MessageBox.Show("Password change failed. User not found.");
                    return;
                }

                MessageBox.Show("Password changed successfully.");
                ClearTextBoxes();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

       
        private void Verify_Clicked(object sender, RoutedEventArgs e)
        {
            string username = usernameTB.Text;
            int question = randomindex + 1;
            string answer = QuestionTB.Text;

            if (string.IsNullOrWhiteSpace(usernameTB.Text) || string.IsNullOrWhiteSpace(QuestionTB.Text))
            {
                MessageBox.Show("Please fill all required field.");
                return;
            } 

            bool isverified = UserServices.ForgotPassVerification(username, question, answer);
            if (isverified) {
                createNewPassTB.IsEnabled = true;
                confirmNewPassTB.IsEnabled = true;
                ChangePassBtn.IsEnabled = true;
                confirmNewPassOverTB.IsEnabled = true;
                CreateNewPassOverTB.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Not Verified");
                return;
            }
        }
        private void CreateOverTB_GotFocused(object sender, RoutedEventArgs e)
        {
            CreateNewPassOverTB.Visibility = Visibility.Hidden;
            createNewPassTB.Focus();
        }
        private void ConfirmOverTB_GotFocused(object sender, RoutedEventArgs e)
        {
            confirmNewPassOverTB.Visibility= Visibility.Hidden;
            confirmNewPassTB.Focus();
        }
        private void CreateNewPass_LostFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(createNewPassTB.Password))
            {
                CreateNewPassOverTB.Visibility = Visibility.Visible;
            }
            else
            {
                CreateNewPassOverTB.Visibility = Visibility.Hidden;
            }
        }
        private void ConfirmNewPass_LostFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(confirmNewPassTB.Password))
            {
                confirmNewPassOverTB.Visibility = Visibility.Visible;
            }
            else
            {
                confirmNewPassOverTB.Visibility = Visibility.Hidden;
            }
        }

        public void ClearTextBoxes()
        {
            usernameTB.Text = "Username...";
            usernameTB.Foreground = Brushes.Gray;
            QuestionTB.Text = "Your answer...";
            QuestionTB.Foreground = Brushes.Gray;
            
            createNewPassTB.Clear();
            CreateNewPassOverTB.Visibility = Visibility.Visible;

            confirmNewPassTB.Clear();
            confirmNewPassOverTB.Visibility = Visibility.Visible;

            createNewPassTB.IsEnabled = false;
            CreateNewPassOverTB.IsEnabled = false;
            confirmNewPassTB.IsEnabled = false;
            confirmNewPassOverTB.IsEnabled = false;

            loginWindow.ChangeControl("login");


        }
        private void UsernameTB_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Username...", true);
        }
        private void UsernameTB_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(usernameTB, "Username...", false);
        }
        private void QuestionTB_GotFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(QuestionTB, "Your answer...", true);
        }
        private void QuestionTB_LostFocused(object sender, RoutedEventArgs e)
        {
            HandleTextBoxPlaceholder(QuestionTB, "Your answer...", false);
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

        private void ChangeControl_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
            loginWindow.ChangeControl("login");
        }
    }
}
