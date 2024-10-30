using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPoultry
{
    public partial class Inventory_AddingForm : Window
    {
        
        public Inventory_AddingForm()
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            InitializeComponent();
            SetRoundedCorners();
            mainWindow.Opacity = 0.5;
            SetRoundedCorners();

            this.Closed += (s, e) => mainWindow.Opacity = 1.0;
        }

        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;

            Border border = new Border
            {
                CornerRadius = new CornerRadius(10),
                Background = Brushes.White,
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF077C5E"),
                BorderThickness = new Thickness(2)
            };

            this.Content = border;
        }
    }
}
