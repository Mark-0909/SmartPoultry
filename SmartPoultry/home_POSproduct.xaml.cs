using SmartPoultry.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartPoultry
{
    public partial class home_POSproduct : UserControl
    {
        public home? homeControl;
        public home_POSproduct(string product_name, List<ProductVariations> var_list, string imagepath, home homecontrol)
        {
            InitializeComponent();

            homeControl = homecontrol;

            // Set product name and image
            Productname.Content = product_name;
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;

            // Calculate button size based on the number of variations
            double buttonSize = (vartypesPanel.Width - (5 * var_list.Count)) / var_list.Count;

            foreach (var variation in var_list)
            {
                
                Border animalAllBorder = new Border
                {
                    Name = $"thisBorder",
                    Margin = new Thickness(0, 0, 5, 0),
                    CornerRadius = new CornerRadius(10),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF66C265")),
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = buttonSize,
                    Height = vartypesPanel.Height,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0E4BE"))
                };

                
                Button animalAllBtn = new Button
                {
                    Name = $"thisButton",
                    Content = variation.variant_type,
                    Margin = new Thickness(9, 5, 9, 5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0E4BE")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0E4BE")),
                    FontSize = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF66C265")),
                    Style = (Style)FindResource("NoHoverButton")
                };

                
                animalAllBtn.Click += (sender, e) => VarButton_Click(sender, e, variation.id, product_name);

                
                animalAllBorder.Child = animalAllBtn;

                
                vartypesPanel.Children.Add(animalAllBorder);
            }
        }

        
        private void VarButton_Click(object sender, RoutedEventArgs e, int variationId, string name)
        {
            homeControl.DisplayOrder(variationId, name);
        }


    }
}
