using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Home_OrdersControl.xaml
    /// </summary>
    public partial class Home_OrdersControl : UserControl
    {
       
        public int pricevar;
        public int productId;
        public int positionList;

        
        private home homeControl;

       
        private int previousQuantity = 1;

       
        public Home_OrdersControl(int prodId, string variant, int price, string productName, home homeCtrl, int position)
        {
            InitializeComponent();

          
            prodName.Content = productName;
            pricelabel.Content = price.ToString();
            varName.Content = variant;

        
            pricevar = price;
            productId = prodId;
            positionList = position;
            homeControl = homeCtrl;
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

       
        public void AddToListCheckOut()
        {
            homeControl?.CheckOutList(productId.ToString(), quantitylabel.Content.ToString(), varName.Content.ToString(), pricelabel.Content.ToString());
        }
    }
}
