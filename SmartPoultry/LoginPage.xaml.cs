using SmartPoultry.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {

        public LoginPage()
        {
            InitializeComponent();
            CreateProductImagesFolder();
            var context = new AppDbContext();

            context.Database.EnsureCreated();            
        }
        private void CreateProductImagesFolder()
        {
            
            string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product_Images");

            
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
                MessageBox.Show("Product_Images folder created.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

           
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Display a confirmation message box
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to close this?",
                "Closed",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Check the user's response
            if (result == MessageBoxResult.Yes)
            {
                // Log out logic here
                // For example, closing the current window or navigating to a login page
                Application.Current.Shutdown(); // This will close the application
            }
            else
            {
                // Optionally handle the case where the user clicked No (e.g., do nothing)
            }
        }

    }
}
