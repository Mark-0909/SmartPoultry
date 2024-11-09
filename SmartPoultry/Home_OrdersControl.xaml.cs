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
    /// Interaction logic for Home_OrdersControl.xaml
    /// </summary>
    public partial class Home_OrdersControl : UserControl
    {
        public int pricevar;

        home homeControl;
        private int previousQuantity = 1;
        public Home_OrdersControl(string var, int price, string name, home homecontrol)
        {
            InitializeComponent();
            productName.Content = name;
            pricelabel.Content = price.ToString();
            varName.Content = var;
            pricevar = price;

            homeControl = homecontrol;
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);
            initialQuantity++; 
            quantitylabel.Content = initialQuantity.ToString();

           
            int priceDifference = pricevar * (initialQuantity - previousQuantity);
            homeControl?.DisplayTotalPrice(priceDifference);

         
            pricelabel.Content = (pricevar * initialQuantity).ToString();
            previousQuantity = initialQuantity;
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);

           
            if (initialQuantity > 1)
            {
                initialQuantity--; 
                quantitylabel.Content = initialQuantity.ToString();

      
                int priceDifference = pricevar * (initialQuantity - previousQuantity);
                homeControl?.DisplayTotalPrice(priceDifference);

                pricelabel.Content = (pricevar * initialQuantity).ToString();
                previousQuantity = initialQuantity;
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

            int totalItemPrice = pricevar * previousQuantity;
            homeControl?.DisplayTotalPrice(-totalItemPrice);
        }
    }

    }
