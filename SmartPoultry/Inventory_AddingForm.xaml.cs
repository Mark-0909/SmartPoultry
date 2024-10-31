using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPoultry
{
    public partial class Inventory_AddingForm : Window
    {
        public MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        public Inventory_AddingForm()
        {
            InitializeComponent();
            SetRoundedCorners();
            mainWindow.Opacity = 0.5;
            SetRoundedCorners();

            this.Closed += (s, e) => mainWindow.Opacity = 1.0;
        }
        private void ProductName_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (ProductNameTextBox.Text == "Enter text here...")
            {
                ProductNameTextBox.Text = "";
                ProductNameTextBox.Foreground = Brushes.Black;
            }
        }

        private void ProductName_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
            {
                ProductNameTextBox.Text = "Enter text here...";
                ProductNameTextBox.Foreground = Brushes.Gray;
            }
        }


        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;

        
        }

        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
        }

        public void ClosePopUp()
        {
            this.Close();
        }

        private void ChickenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AccessoriesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FeedsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
