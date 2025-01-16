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
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_ProductControl.xaml
    /// </summary>
    public partial class Inventory_ProductControl : UserControl
    {
        Inventory_AddingForm viewproduct;
        ProductServices productservices;
        public int prodid {  get; set; }
        public Inventory_ProductControl(int productid, string name, decimal stocks, byte[] imagepath)
        {
            InitializeComponent();
            Productname.Content = name;
            Productstock.Content = stocks.ToString();

            DisplayProductImage(imagepath);

            AppDbContext context = new AppDbContext();
            productservices = new ProductServices(context);
            prodid = productid;

        }
        private void DisplayProductImage(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                try
                {
                    using (var memoryStream = new System.IO.MemoryStream(imageData))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = memoryStream;
                        bitmap.EndInit();
                        Productimage.Source = bitmap;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Image Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Productimage.Source = null;
                }
            }
            else
            {
                Productimage.Source = null;
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow? mainWindow = UserContext.mainWindow;
            if (mainWindow != null)
            {

                mainWindow.ActiveOverlay(true);

            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow.  inventory product control");
            }
            Products product = productservices.FetchProduct(prodid);

            viewproduct = new Inventory_AddingForm(product, mainWindow, this);

            viewproduct.ShowDialog();   
        }
        public void AdustStocksAfterSupplierorder(decimal stocks)
        {
            decimal origStocks = decimal.Parse(Productstock.ToString());
            decimal newStocks = origStocks + stocks;
            Productstock.Content = newStocks;
        }
    }
}
