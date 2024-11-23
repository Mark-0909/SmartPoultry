using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Home_OrdersControl.xaml
    /// </summary>
    public partial class Home_OrdersControl : UserControl
    {
       
        public decimal pricevar;
        public int productId;
        public int positionList;

        
        private home homeControl;

       
        private int previousQuantity = 1;


        public Home_OrdersControl(int prodId, string variant, decimal price, string productName, home homeCtrl, int position, string quantity)
        {
            InitializeComponent();

            prodName.Content = productName;
            pricelabel.Content = price.ToString("N2");
            varName.Content = variant;
            quantitylabel.Content = quantity;
            pricevar = price;
            productId = prodId;
            positionList = position;
            homeControl = homeCtrl;

            
            if (position % 2 != 0)
            {
                this.controlBorder.Background = new SolidColorBrush(Colors.White); 
            }
        }




        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);
            initialQuantity++;
            quantitylabel.Content = initialQuantity.ToString();

           
            decimal priceDifference = pricevar * (initialQuantity - previousQuantity);
            homeControl?.DisplayTotalPrice(priceDifference);

            homeControl?.EditQuantityPriceList(positionList, (pricevar * initialQuantity).ToString(), initialQuantity.ToString());

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

               
                decimal priceDifference = pricevar * (initialQuantity - previousQuantity);
                homeControl?.DisplayTotalPrice(priceDifference);

                homeControl?.EditQuantityPriceList(positionList, (pricevar * initialQuantity).ToString(), initialQuantity.ToString());

                pricelabel.Content = (pricevar * initialQuantity).ToString();
                previousQuantity = initialQuantity;
                

            }
        }

        
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; 

           
            decimal totalItemPrice = pricevar * previousQuantity;
            homeControl?.DisplayTotalPrice(-totalItemPrice);
            homeControl?.RemoverFromList(positionList);
        }

       
        public void AddToListCheckOut()
        {
            homeControl?.CheckOutList(productId.ToString(), quantitylabel.Content.ToString(), varName.Content.ToString(), pricelabel.Content.ToString());
        }
    }
}
