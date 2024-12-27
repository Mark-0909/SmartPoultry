using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
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
        public int VariantID { get; set; }

        public decimal pricevar;
        public int productId;
        public int positionList;


        private home homeControl;


        private int previousQuantity = 1;
        decimal CVRate;
        public home_POSproduct poscontrol;



        public Home_OrdersControl(int prodId, string variant, decimal price, string productName, home homeCtrl, int position, string quantity, home_POSproduct pos, int conversion)
        {
            InitializeComponent();

            CVRate = 1m / conversion;

            VariantID = prodId;

            prodName.Content = productName;
            pricelabel.Content = price.ToString("N2");
            varName.Content = variant;
            quantitylabel.Content = quantity;
            pricevar = price;
            productId = prodId;
            positionList = position;
            homeControl = homeCtrl;

            poscontrol = pos;

            if (position % 2 != 0)
            {
                this.controlBorder.Background = new SolidColorBrush(Colors.White);
            }
        }

        public void UpdateColorCoding(int position)
        {
            if (position % 2 != 0)
            {
                this.controlBorder.Background = new SolidColorBrush(Colors.White);
            } else
            {
                this.controlBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD4, 0xD4, 0xD4));

            }
        }
        public void AddQuantity()
        {
            decimal stocks = poscontrol.origstock;
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);

            if (stocks >= CVRate)
            {
                initialQuantity++;
                quantitylabel.Content = initialQuantity.ToString();

                decimal priceDifference = pricevar * (initialQuantity - previousQuantity);
                homeControl?.DisplayTotalPrice(priceDifference);


                homeControl?.EditQuantityPriceList(positionList, (pricevar * initialQuantity).ToString(), initialQuantity.ToString());

                poscontrol.AdjustStocks(-CVRate);

                pricelabel.Content = (pricevar * initialQuantity).ToString("N2");
                previousQuantity = initialQuantity;
            }
        }



        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            AddQuantity();
        }


        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            decimal stocks = poscontrol.origstock;
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);


            if (initialQuantity > 1)
            {
                initialQuantity--;
                quantitylabel.Content = initialQuantity.ToString();


                decimal priceDifference = pricevar * (initialQuantity - previousQuantity);
                homeControl?.DisplayTotalPrice(priceDifference);

                homeControl?.EditQuantityPriceList(positionList, (pricevar * initialQuantity).ToString(), initialQuantity.ToString());
                poscontrol.AdjustStocks(CVRate);

                pricelabel.Content = (pricevar * initialQuantity).ToString("N2");
                previousQuantity = initialQuantity;


            }
        }


        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {

            decimal totalItemPrice = pricevar * previousQuantity;

            int initialQuantity = Convert.ToInt32(quantitylabel.Content);

            decimal stocksback = initialQuantity * CVRate;


            homeControl?.DisplayTotalPrice(-totalItemPrice);
            homeControl?.RemoverFromList(positionList, poscontrol);
            homeControl?.EnableDropBtn();

            poscontrol.AdjustStocks(stocksback);

            this.Visibility = Visibility.Collapsed;
        }




    }
}