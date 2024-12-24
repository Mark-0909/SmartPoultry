using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartPoultry
{
    public partial class home_POSproduct : UserControl
    {
        public home? homeControl;
        public decimal origstock { get; set; }

        public List<ProductVariations> prodVarList;

        public AppDbContext context = new AppDbContext();

        public ProductServices productServices;

        public string prodname;

        public int productId;
        public home_POSproduct(string product_name, List<ProductVariations> var_list, string imagepath, home homecontrol, decimal stocks, Products prod)
        {
            InitializeComponent();

            productServices = new ProductServices(context);

            homeControl = homecontrol;

            StocksLabel.Content = stocks;

            origstock = stocks;
            prodVarList = var_list;
            prodname = product_name;

            Productname.Content = product_name;
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;

            Initialize(var_list, origstock);

            productId = prod.product_id;
        }

        public void Initialize(List<ProductVariations> var_list, decimal adjustedstock)
        {
            double totalMargin = 1 * (var_list.Count - 1);
            double buttonSize = (vartypesPanel.Width - totalMargin) / var_list.Count;

            for (int i = 0; i < var_list.Count; i++)
            {
                var variation = var_list[i];
                decimal stockvalue = 1m / variation.conversion_rate;

                Border animalAllBorder = new Border
                {
                    Name = $"thisBorder_{SanitizeName(variation.id.ToString())}_border",
                    Margin = i == var_list.Count - 1
                        ? new Thickness(0, 0, 0, 0)
                        : new Thickness(0, 0, 1, 0),
                    CornerRadius = new CornerRadius(5),
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
                    FontSize = 10.5,
                    Padding = new Thickness(10, 5, 10, 5),
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Style = (Style)FindResource("NoHoverButton"),
                    Width = 70,
                    MaxWidth = 90,
                    FontWeight = FontWeights.Bold,
                };


                animalAllBtn.IsEnabled = stockvalue <= adjustedstock;

                if (!animalAllBtn.IsEnabled)
                {
                    animalAllBorder.Opacity = 0.5;
                    animalAllBtn.Opacity = 0.5;
                }

                animalAllBtn.Click += (sender, e) => VarButton_Click(sender, e, variation.id, prodname);

                animalAllBorder.Child = animalAllBtn;

                animalAllBorder.MouseLeftButtonDown += (sender, e) => VarButton_Click(sender, e, variation.id, prodname);

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
            homeControl.EnableDropBtn();
            homeControl.IsOrderConfirmed();
        }

        public void AdjustStocks(decimal amount)
        {
            vartypesPanel.Children.Clear();
            origstock = origstock + amount;

            StocksLabel.Content = origstock.ToString("0.######");
            Initialize(prodVarList, origstock);
        }

        public void StockRevertBack()
        {
            Products products = productServices.FetchProduct(productId);

            this.StocksLabel.Content = products.stocks.ToString();

            this.vartypesPanel.Children.Clear();
            this.Initialize(prodVarList, products.stocks);

            this.origstock = products.stocks;
        }

    }
}