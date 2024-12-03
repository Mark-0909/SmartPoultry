using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartPoultry
{
    public partial class MainWindow : Window
    {
        private Dictionary<Button, Control> navigationMap;

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
            // Initialize the dictionary to map buttons to their respective controls
            navigationMap = new Dictionary<Button, Control>
            {
                { homeButton, homeControl },
                { dashboardButton, dashboardControl },
                { inventoryButton, inventoryControl },
                { recordsButton, recordsControl },
                { organizationButton, organizationControl },
                { supplierButton, supplierControl }
            };
        }

        // General method to handle navigation and visibility
        private void NavigateTo(Button activeButton)
        {
            foreach (var pair in navigationMap)
            {
                var button = pair.Key;
                var control = pair.Value;

                if (button == activeButton)
                {
                    ActiveButton(button, GetActiveImageSource(button), GetBorderName(button), GetButtonIcon(button));
                    control.Visibility = Visibility.Visible;
                    Panel.SetZIndex(control, 10); // Bring this control to the front
                }
                else
                {
                    InactiveButton(button, GetInactiveImageSource(button), GetBorderName(button), GetButtonIcon(button));
                    control.Visibility = Visibility.Collapsed;
                    Panel.SetZIndex(control, 0); // Send other controls to the back
                }
            }
        }

        private string GetActiveImageSource(Button button)
        {
            // Define logic for active image source (could be dynamic depending on the button)
            return button == homeButton ? "Images/homeicongreen.png" :
                   button == dashboardButton ? "Images/dashboardgreen.png" :
                   button == inventoryButton ? "Images/inventorygreen.png" :
                   button == recordsButton ? "Images/recordsgreen.png" :
                   button == organizationButton ? "Images/organizationgreen.png" :
                   "Images/suppliergreen.png";
        }

        private string GetInactiveImageSource(Button button)
        {
            // Define logic for inactive image source
            return button == homeButton ? "Images/homeicongrey.png" :
                   button == dashboardButton ? "Images/dashboardgrey.png" :
                   button == inventoryButton ? "Images/inventorygrey.png" :
                   button == recordsButton ? "Images/recordsgrey.png" :
                   button == organizationButton ? "Images/organizationgrey.png" :
                   "Images/suppliergrey.png";
        }

        private string GetBorderName(Button button)
        {
            // Return the appropriate border name based on the button
            return button == homeButton ? "homeBorder" :
                   button == dashboardButton ? "dashboardBorder" :
                   button == inventoryButton ? "inventoryBorder" :
                   button == recordsButton ? "recordsBorder" :
                   button == organizationButton ? "organizationBorder" :
                   "supplierBorder";
        }

        private Image GetButtonIcon(Button button)
        {
            // Return the button icon based on the button clicked
            return button == homeButton ? homeIcon :
                   button == dashboardButton ? dashboardIcon :
                   button == inventoryButton ? inventoryIcon :
                   button == recordsButton ? recordsIcon :
                   button == organizationButton ? organizationIcon :
                   supplierIcon;
        }

        // Active button appearance logic
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

        // Inactive button appearance logic
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

        // Button click events simplified
        private void HomeButton_Click(object sender, RoutedEventArgs e) => NavigateTo(homeButton);
        private void DashboardButton_Click(object sender, RoutedEventArgs e) => NavigateTo(dashboardButton);
        private void InventoryButton_Click(object sender, RoutedEventArgs e) => NavigateTo(inventoryButton);
        private void RecordsButton_Click(object sender, RoutedEventArgs e) => NavigateTo(recordsButton);
        private void OrganizationButton_Click(object sender, RoutedEventArgs e) => NavigateTo(organizationButton);
        private void SupplierButton_Click(object sender, RoutedEventArgs e) => NavigateTo(supplierButton);

        // Logout functionality
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

        // Methods for dynamically adding content (if necessary)
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
    }
}
