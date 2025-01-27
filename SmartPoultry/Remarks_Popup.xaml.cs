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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Remarks_Popup.xaml
    /// </summary>
    public partial class Remarks_Popup : Window
    {
        public string Remarks { get; private set; }

        public Remarks_Popup()
        {
            InitializeComponent();
            RemarksTB.Focus();
            NotifPopup.Visibility = Visibility.Hidden;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RemarksTB.Text))
            {
                PopUpNotif("alert", "Please fill in the remarks field.");
                return;
            }
            else
            {
                Remarks = RemarksTB.Text; 
                DialogResult = true;     
                this.Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            
            DialogResult = false; 
            this.Close();
        }



        public void PopUpNotif(string type, string message)
        {
            NotifPopup.Visibility = Visibility.Visible;
            Panel.SetZIndex(NotifPopup, int.MaxValue);
            if (type == "notif")
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCE6D3"));
            }
            else
            {
                NotifPopup.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
                NotifPopup.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD2D2"));
            }

            NotifMessage.Content = message;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromSeconds(4.5),
                Duration = TimeSpan.FromMilliseconds(500)
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            storyboard.Children.Add(fadeOut);

            Storyboard.SetTarget(fadeIn, NotifPopup);
            Storyboard.SetTarget(fadeOut, NotifPopup);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));

            storyboard.Completed += (sender, args) =>
            {
                NotifPopup.Visibility = Visibility.Collapsed;
            };
            storyboard.Begin();
        }
        private void NotifCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NotifPopup.Visibility = Visibility.Hidden;
        }

        
    }
}
