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
        public void MainWindowClick()
        {
            
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
