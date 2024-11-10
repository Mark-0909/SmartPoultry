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
        // Fields for storing price, product ID, and position in list
        public int pricevar;
        public int productId;
        public int positionList;

        // Reference to the home control
        private home homeControl;

        // Track the previous quantity for calculating price differences
        private int previousQuantity = 1;

        // Constructor to initialize control with product details and home control reference
        public Home_OrdersControl(int prodId, string variant, int price, string productName, home homeCtrl, int position)
        {
            InitializeComponent();

            // Set product details
            prodName.Content = productName;
            pricelabel.Content = price.ToString();
            varName.Content = variant;

            // Set fields for use in event handlers
            pricevar = price;
            productId = prodId;
            positionList = position;
            homeControl = homeCtrl;
        }

        // Event handler for the plus button click
        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);
            initialQuantity++; // Increment quantity
            quantitylabel.Content = initialQuantity.ToString();

            // Calculate price difference and update total price in homeControl
            int priceDifference = pricevar * (initialQuantity - previousQuantity);
            homeControl?.DisplayTotalPrice(priceDifference);

            // Update item price display
            pricelabel.Content = (pricevar * initialQuantity).ToString();
            previousQuantity = initialQuantity;
        }

        // Event handler for the minus button click
        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            int initialQuantity = Convert.ToInt32(quantitylabel.Content);

            // Ensure quantity does not go below 1
            if (initialQuantity > 1)
            {
                initialQuantity--; // Decrement quantity
                quantitylabel.Content = initialQuantity.ToString();

                // Calculate price difference and update total price in homeControl
                int priceDifference = pricevar * (initialQuantity - previousQuantity);
                homeControl?.DisplayTotalPrice(priceDifference);

                // Update item price display
                pricelabel.Content = (pricevar * initialQuantity).ToString();
                previousQuantity = initialQuantity;
            }
        }

        // Event handler for the remove button click
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; // Hide the control

            // Calculate total item price and update the home control with a negative value to adjust the total
            int totalItemPrice = pricevar * previousQuantity;
            homeControl?.DisplayTotalPrice(-totalItemPrice);
        }

        // Method to add item to checkout list
        public void AddToListCheckOut()
        {
            homeControl?.CheckOutList(productId.ToString(), quantitylabel.Content.ToString(), varName.Content.ToString(), pricelabel.Content.ToString());
        }
    }
}
