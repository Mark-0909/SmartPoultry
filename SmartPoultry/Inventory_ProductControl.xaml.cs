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
using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_ProductControl.xaml
    /// </summary>
    public partial class Inventory_ProductControl : UserControl
    {
        Inventory_AddingForm viewproduct;
        ProductServices productservices;
        int id;
        public Inventory_ProductControl(int productid, string name, decimal stocks, string imagepath)
        {
            InitializeComponent();
            Productname.Content = name;
            Productstock.Content = stocks.ToString();
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;

            AppDbContext context = new AppDbContext();
            productservices = new ProductServices(context);
            id = productid;

        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {

                mainWindow.ActiveOverlay(true);

            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.");
            }
            Products product = productservices.FetchProduct(id);

            viewproduct = new Inventory_AddingForm(product, mainWindow, this);

            viewproduct.ShowDialog();

            
            
        }
    }
}
