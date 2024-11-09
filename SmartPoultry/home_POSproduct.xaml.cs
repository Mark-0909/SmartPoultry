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
        public home_POSproduct(string product_name, List<ProductVariations> var_list, string imagepath)
        {
            InitializeComponent();
            Productname.Content = product_name;
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;

            double buttonSize = (vartypesPanel.Width - (5 * var_list.Count)) / var_list.Count;

            foreach (var variation in var_list)
            {
                
                Border animalAllBorder = new Border
                {
                    Name = $"{variation.variant_type}Border", 
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
                    Name = $"{variation.variant_type}_{variation.id}",  
                    Content = variation.variant_type, 
                    Margin = new Thickness(9, 5, 9, 5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0E4BE")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC0E4BE")),
                    FontSize = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF66C265")),
                    Style = (Style)FindResource("NoHoverButton") 
                };

             
                animalAllBorder.Child = animalAllBtn;

           
                vartypesPanel.Children.Add(animalAllBorder);
            }
        }

    }
}
