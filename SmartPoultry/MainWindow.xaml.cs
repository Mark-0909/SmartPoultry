using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowOverlay.Visibility = Visibility.Hidden;
        }

        public void ActiveOverlay(bool isActive)
        {
            if (isActive)
            {
                MainWindowOverlay.Visibility = Visibility.Visible;

                Panel.SetZIndex(MainWindowOverlay, 99);
            }
            else
            {
                MainWindowOverlay.Visibility = Visibility.Collapsed;
                Panel.SetZIndex(MainWindowOverlay, 0);
            }
        }

        public void DynamicAddDeliveries()
        {
            dashboardControl.DynamicReloadDeliveries();
        }
        public void DynamicAddFinance()
        {
            dashboardControl.DynamicReloadFinancialLiabilities();
        }
        public void DynamicAddOrder()
        {
            dashboardControl.DynamicOrderDisplay();
        }
        public void DynamicReload()
        {
            homeControl.DynamicReload();
            inventoryControl.DynamicReload();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //active inactive buttons
            ActiveButton(homeButton, "Images/homeicongreen.png", "homeBorder", homeIcon);
            InactiveButton(dashboardButton, "Images/dashboardgrey.png", "dashboardBorder", dashboardIcon);
            InactiveButton(inventoryButton, "Images/inventorygrey.png", "inventoryBorder", inventoryIcon);
            InactiveButton(recordsButton, "Images/recordsgrey.png", "recordsBorder", recordsIcon);
            InactiveButton(organizationButton, "Images/organizationgrey.png", "organizationBorder", organizationIcon);
            InactiveButton(supplierButton, "Images/suppliergrey.png", "supplierBorder", supplierIcon);


            //front usercontrol
            Control[] controls = { dashboardControl, inventoryControl, recordsControl, organizationControl, supplierControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(homeControl, 10);

        }
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            //active inactive buttons
            InactiveButton(homeButton, "Images/homeicongrey.png", "homeBorder", homeIcon);
            ActiveButton(dashboardButton, "Images/dashboardgreen.png", "dashboardBorder", dashboardIcon);
            InactiveButton(inventoryButton, "Images/inventorygrey.png", "inventoryBorder", inventoryIcon);
            InactiveButton(recordsButton, "Images/recordsgrey.png", "recordsBorder", recordsIcon);
            InactiveButton(organizationButton, "Images/organizationgrey.png", "organizationBorder", organizationIcon);
            InactiveButton(supplierButton, "Images/suppliergrey.png", "supplierBorder", supplierIcon);

            Control[] controls = { homeControl, inventoryControl, recordsControl, organizationControl, supplierControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(dashboardControl, 10);
        }
        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            //active inactive buttons
            InactiveButton(homeButton, "Images/homeicongrey.png", "homeBorder", homeIcon);
            InactiveButton(dashboardButton, "Images/dashboardgrey.png", "dashboardBorder", dashboardIcon);
            ActiveButton(inventoryButton, "Images/inventorygreen.png", "inventoryBorder", inventoryIcon);
            InactiveButton(recordsButton, "Images/recordsgrey.png", "recordsBorder", recordsIcon);
            InactiveButton(organizationButton, "Images/organizationgrey.png", "organizationBorder", organizationIcon);
            InactiveButton(supplierButton, "Images/suppliergrey.png", "supplierBorder", supplierIcon);

            //front usercontrol
            Control[] controls = { dashboardControl, homeControl, recordsControl, organizationControl, supplierControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(inventoryControl, 10);
        }
        private void RecordsButton_Click(object sender, RoutedEventArgs e)
        {
            //active inactive buttons
            InactiveButton(homeButton, "Images/homeicongrey.png", "homeBorder", homeIcon);
            InactiveButton(dashboardButton, "Images/dashboardgrey.png", "dashboardBorder", dashboardIcon);
            InactiveButton(inventoryButton, "Images/inventorygrey.png", "inventoryBorder", inventoryIcon);
            ActiveButton(recordsButton, "Images/recordsgreen.png", "recordsBorder", recordsIcon);
            InactiveButton(organizationButton, "Images/organizationgrey.png", "organizationBorder", organizationIcon);
            InactiveButton(supplierButton, "Images/suppliergrey.png", "supplierBorder", supplierIcon);


            //front usercontrol
            Control[] controls = { dashboardControl, inventoryControl, homeControl, organizationControl, supplierControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(recordsControl, 10);

        }
        private void OrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(homeButton, "Images/homeicongrey.png", "homeBorder", homeIcon);
            InactiveButton(dashboardButton, "Images/dashboardgrey.png", "dashboardBorder", dashboardIcon);
            InactiveButton(inventoryButton, "Images/inventorygrey.png", "inventoryBorder", inventoryIcon);
            InactiveButton(recordsButton, "Images/recordsgrey.png", "recordsBorder", recordsIcon);
            ActiveButton(organizationButton, "Images/organizationgreen.png", "organizationBorder", organizationIcon);
            InactiveButton(supplierButton, "Images/suppliergrey.png", "supplierBorder", supplierIcon);

            //front usecontrol
            Control[] controls = { dashboardControl, inventoryControl, recordsControl, homeControl, supplierControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(organizationControl, 10);
        }

        private void SupplierButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(homeButton, "Images/homeicongrey.png", "homeBorder", homeIcon);
            InactiveButton(dashboardButton, "Images/dashboardgrey.png", "dashboardBorder", dashboardIcon);
            InactiveButton(inventoryButton, "Images/inventorygrey.png", "inventoryBorder", inventoryIcon);
            InactiveButton(recordsButton, "Images/recordsgrey.png", "recordsBorder", recordsIcon);
            InactiveButton(organizationButton, "Images/organizationgrey.png", "organizationBorder", organizationIcon);
            ActiveButton(supplierButton, "Images/suppliergreen.png", "supplierBorder", supplierIcon);

            //front usecontrol
            Control[] controls = { dashboardControl, inventoryControl, recordsControl, organizationControl, homeControl };

            foreach (var control in controls)
            {
                Panel.SetZIndex(control, 0);
            }
            Panel.SetZIndex(supplierControl, 10);
        }

        private void ActiveButton(Button button, string imagesource, string buttonborder, Image icon)
        {
            var border = (Border)button.Template.FindName(buttonborder, button);
            if (border != null)
            {
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 194, 101));
            }
            button.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
            icon.Source = new BitmapImage(new Uri(imagesource, UriKind.RelativeOrAbsolute));
        }

        private void InactiveButton(Button button, string imagesource, string buttonborder, Image icon)
        {
            var border = (Border)button.Template.FindName(buttonborder, button);
            if (border != null)
            {
                border.BorderThickness = new Thickness(0);
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(244, 247, 252));
            }
            button.Background = new SolidColorBrush(Color.FromRgb(244, 247, 252));
            icon.Source = new BitmapImage(new Uri(imagesource, UriKind.RelativeOrAbsolute));
        }

        private void LogoutBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                LoginPage loginWindow = new LoginPage();
                Application.Current.MainWindow = loginWindow;
                loginWindow.Show();
                this.Close();
            }
        }
    }
}