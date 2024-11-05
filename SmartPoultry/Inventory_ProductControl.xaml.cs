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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_ProductControl.xaml
    /// </summary>
    public partial class Inventory_ProductControl : UserControl
    {
        public Inventory_ProductControl(string name, int stocks, string imagepath)
        {
            InitializeComponent();
            Productname.Content = name;
            Productstock.Content = stocks.ToString();
            BitmapImage bitmap = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute));
            Productimage.Source = bitmap;
        }
    }
}
