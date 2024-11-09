using SmartPoultry.Models;
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
    /// Interaction logic for home_POSproduct.xaml
    /// </summary>
    public partial class home_POSproduct : UserControl
    {
        public home_POSproduct(string product_name, List<ProductVariations> var_list)
        {
            InitializeComponent();
            //int buttonsize = vartypesPanel.Width;
        }
    }
}
