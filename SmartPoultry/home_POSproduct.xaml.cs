using SmartPoultry.Models;
using System.Diagnostics;
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
        public decimal origstock {  get; set; }

        public List<ProductVariations> prodVarList;

        public string prodname;
        public home_POSproduct(string product_name, List<ProductVariations> var_list, string imagepath, home homecontrol, decimal stocks)
        {
            InitializeComponent();

            homeControl = homecontrol;

            StocksLabel.Content = stocks;

            origstock = stocks;
            prodVarList = var_list;
            prodname = product_name;

            Productname.Content = product_name;
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;

            Initialize(var_list, origstock);
            
        }

        public void Initialize(List<ProductVariations> var_list, decimal adjustedstock)
        {
            double buttonSize = (vartypesPanel.Width - (5 * var_list.Count)) / var_list.Count;

            foreach (var variation in var_list)
            {
                decimal stockvalue = 1m / variation.conversion_rate;

                Border animalAllBorder = new Border
                {
                    Name = $"thisBorder_{SanitizeName(variation.id.ToString())}_border",

                    Margin = new Thickness(0, 0, 5, 0),
                    CornerRadius = new CornerRadius(10),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D")),
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = buttonSize,
                    Height = vartypesPanel.Height,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D"))
                };


                Button animalAllBtn = new Button
                {
                    Name = $"thisButton_{SanitizeName(variation.id.ToString())}_button",

                    Content = variation.variant_type,
                    Margin = new Thickness(9, 5, 9, 5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D")),
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Colors.White),
                    Style = (Style)FindResource("NoHoverButton")

                };
                animalAllBtn.IsEnabled = stockvalue <= adjustedstock;

                if (!animalAllBtn.IsEnabled)
                {
                    animalAllBorder.Opacity = 0.5;
                    animalAllBtn.Opacity = 0.5;
                }

                animalAllBtn.Click += (sender, e) => VarButton_Click(sender, e, variation.id, prodname);


                animalAllBorder.Child = animalAllBtn;


                vartypesPanel.Children.Add(animalAllBorder);
            }
        }

        string SanitizeName(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;


            return string.Concat(input.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_'));
        }

        private void VarButton_Click(object sender, RoutedEventArgs e, int variationId, string name)
        {
            homeControl.DisplayOrder(variationId, name, this);
        }

        public void AdjustStocks(decimal amount)
        {
            vartypesPanel.Children.Clear();
            origstock = origstock + amount;

            StocksLabel.Content = origstock.ToString("0.######");
            Initialize(prodVarList, origstock);
        }


    }
}