using Microsoft.EntityFrameworkCore;
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
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
        //database
        public AppDbContext context = new AppDbContext();
        UserServices userServices;

        public LoginPage()
        {
            InitializeComponent();
            CreateProductImagesFolder();

            using (var context = new AppDbContext())
            {
                // Apply any pending migrations
                context.Database.Migrate();
            }

            userServices = new UserServices(new AppDbContext());
            Initialization();
        }




        public void Initialization()
        {
            
            bool isPresent = userServices.IsThereAdmin();

            if (isPresent)
            {
                createAccountControl.Visibility = Visibility.Hidden;
                loginControl.Visibility = Visibility.Visible;
                forgotControl.Visibility = Visibility.Hidden;
                

                Panel.SetZIndex(createAccountControl, 0);
                Panel.SetZIndex(loginControl, 1);
                Panel.SetZIndex(forgotControl, 0);

            }
            else
            {
                createAccountControl.Visibility = Visibility.Visible;
                loginControl.Visibility = Visibility.Hidden;
                forgotControl.Visibility = Visibility.Hidden;
                

                Panel.SetZIndex(createAccountControl, 1);
                Panel.SetZIndex(loginControl, 0);
                Panel.SetZIndex(forgotControl, 0);

            }
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

        public void SuccessLogin()
        {
            this.Close();
        }
        
        public void ChangeControl(string control)
        {
            if(control == "create")
            {
                createAccountControl.Visibility = Visibility.Visible;
                loginControl.Visibility = Visibility.Hidden;
                forgotControl.Visibility = Visibility.Hidden;

                createAccountControl.Changelabel();
                Panel.SetZIndex(createAccountControl, 1);
                Panel.SetZIndex(loginControl, 0);
                Panel.SetZIndex(forgotControl, 0);
            }
            else if(control == "forgot")
            {
                createAccountControl.Visibility = Visibility.Hidden;
                loginControl.Visibility = Visibility.Hidden;
                forgotControl.Visibility = Visibility.Visible;

                Panel.SetZIndex(createAccountControl, 0);
                Panel.SetZIndex(loginControl, 0);
                Panel.SetZIndex(forgotControl, 1);
            }
            else
            {
                createAccountControl.Visibility = Visibility.Hidden;
                loginControl.Visibility = Visibility.Visible;
                forgotControl.Visibility = Visibility.Hidden;

                Panel.SetZIndex(createAccountControl, 0);
                Panel.SetZIndex(loginControl, 1);
                Panel.SetZIndex(forgotControl, 0);
            }
        }

        

    }
}
