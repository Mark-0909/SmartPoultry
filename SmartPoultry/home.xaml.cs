using SmartPoultry.DataServices;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using SmartPoultry.DataAccess;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;



namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for home.xaml
    /// </summary>
    public partial class home : UserControl
    {
        SalesServices salesServices;
        ProductServices productServices;
        ProductVariationServices productvariationsServices;

        public List<string> Productvaridlist = new List<string>();
        public List<string> QuantityList = new List<string>();
        public List<string> VarSpecification = new List<string>();
        public List<string> PriceList = new List<string>();
        public List<string> ProductnameList = new List<string>();

        public static String[] buttonAnimalArray = { "animalAllBtn", "animalChickenBtn", "animalDogBtn", "animalCatBtn", "animalPigBtn", "animalDuckBtn", "animalCowBtn", "animalHorseBtn", "animalRabbitBtn", "animalBirdBtn", "animalFishBtn", "animalGuineaBtn" };
        public static String[] borderAnimalArray = { "animalAllBorder", "animalChickenBorder", "animalDogBorder", "animalCatBorder", "animalPigBorder", "animalDuckBorder", "animalCowBorder", "animalHorseBorder", "animalRabbitBorder", "animalBirdBorder", "animalFishBorder", "animalGuineaBorder" };

        public static String[] buttonTypeArray = { "typeAllBtn", "typeFeedsBtn", "typeMedicineBtn", "typeVitaminsBtn", "typeAccessoriesBtn", "typeVaccinesBtn"};
        public static String[] borderTypeArray = { "typeAllBorder", "typeFeedsBorder", "typeMedicineBorder", "typeVitaminsBorder", "typeAccessoriesBorder", "typeVaccinesBorder" };
        public home()
        {
            InitializeComponent();
            var context = new AppDbContext();
            productServices = new ProductServices(context);
            productvariationsServices = new ProductVariationServices(context);
            salesServices = new SalesServices(new AppDbContext());
            totalPiceLabel.Visibility = Visibility.Collapsed;
            displayProducts();
        }
        private void DropOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            orderPanel.Children.Clear();
            totalPiceLabel.Content = "0";
            totalPiceLabel.Visibility = Visibility.Collapsed;
        }

        public void ConfirmOrder(string paymentMode, string status, string purchasemethod)
        {
            
            string StringProductList = string.Join(",", Productvaridlist);
            string StringPriceList = string.Join(",", PriceList);
            string StringQuantityList = string.Join(",", QuantityList);
            string StringVarSpecification = string.Join(",", VarSpecification);

            
            decimal totalPrice = decimal.Parse(totalPiceLabel.Content.ToString());

            
            bool addingSales = salesServices.Create(
                StringProductList,
                StringPriceList,
                StringQuantityList,
                StringVarSpecification,
                paymentMode,
                status,
                totalPrice,
                purchasemethod
            );

            if (addingSales)
            {
                MessageBox.Show("Order confirmed successfully!");
                orderPanel.Children.Clear();
                totalPiceLabel.Content = "0";
                totalPiceLabel.Visibility= Visibility.Collapsed;
                Productvaridlist.Clear();
                PriceList.Clear();
                QuantityList.Clear();
                VarSpecification.Clear();
                ProductnameList.Clear();

                DisplayReceipt(paymentMode, purchasemethod, status);
            }
            else
            {
                MessageBox.Show("Failed to confirm the order.");
            }
        }

        public void DisplayReceipt(string customerName, string paymentMethod, string orderDetails)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                writer.CloseStream = false; 
                doc.Open();

                
                doc.Add(new iTextSharp.text.Paragraph("Receipt"));
                doc.Add(new iTextSharp.text.Paragraph($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"));
                doc.Add(new iTextSharp.text.Paragraph($"Customer Name: {customerName}"));
                doc.Add(new iTextSharp.text.Paragraph($"Payment Method: {paymentMethod}"));
                doc.Add(new iTextSharp.text.Paragraph("Order Details:"));
                doc.Add(new iTextSharp.text.Paragraph(orderDetails));

                doc.Close(); 
                memoryStream.Position = 0; 

                
                string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{DateTime.Now:yyyyMMddHHmmss}.pdf");
                File.WriteAllBytes(tempFilePath, memoryStream.ToArray());

                
                Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });

                
                Task.Run(() =>
                {
                    Thread.Sleep(5000); 
                    File.Delete(tempFilePath);
                });
            }
        }




        private void CheckOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (orderPanel.Children.Count > 0)
            {
                Home_Checkout checkout = new Home_Checkout(totalPiceLabel.Content.ToString(), this);
                checkout.ShowDialog();
            }
            else
            {
                MessageBox.Show("Empty order!");
            }


        }
        public void CheckOutList(string prodid, string quantity, string varspec, string price)
        {
            Productvaridlist.Add(prodid);
            QuantityList.Add(quantity);
            VarSpecification.Add(varspec);
            PriceList.Add(price);
        }
        public void RemoverFromList(int position)
        {
            Productvaridlist.RemoveAt(position);
            PriceList.RemoveAt(position);
            VarSpecification.RemoveAt(position);
            QuantityList.RemoveAt(position);
            ProductnameList.RemoveAt(position);

            orderPanel.Children.Clear();

            for (int i = 0; i < Productvaridlist.Count; i++)
            {
                int id = int.Parse(Productvaridlist[i]);
                decimal price = decimal.Parse(PriceList[i]);
                int quantity = int.Parse(QuantityList[i]);

                Home_OrdersControl ordersControl = new Home_OrdersControl(id, VarSpecification[i], price, ProductnameList[i], this, i, quantity.ToString());
                orderPanel.Children.Add(ordersControl);
            }

        }

        public void DisplayOrder(int id, string productname)
        {
            var productvar = productvariationsServices.GetProductVariationById(id);
            string? var = productvar.variant_type;
            decimal price = productvar.price;
            int position = Productvaridlist.Count;
            
            ProductnameList.Add(productname);
            Productvaridlist.Add(id.ToString());
            QuantityList.Add("1");
            VarSpecification.Add(var);
            PriceList.Add(price.ToString());
            
            Home_OrdersControl orderControl = new Home_OrdersControl(id, var, price, productname, this, position, "1");

            orderPanel.Children.Add(orderControl);
            scroller.ScrollToVerticalOffset(scroller.ExtentHeight);

            DisplayTotalPrice(price);
        }

        public void DisplayTotalPrice(decimal toadd)
        {
            totalPiceLabel.Visibility = Visibility.Visible;
            decimal initialPrice = Convert.ToDecimal(totalPiceLabel.Content); 
            decimal finalprice = initialPrice + toadd;

            totalPiceLabel.Content = finalprice.ToString("F2"); 
        }

        public void EditQuantityPriceList(int position, string price, string quantity)
        {
            PriceList[position] = price;
            QuantityList[position] = quantity;
        }
        public void displayProducts()
        {
            
            List<Products> products = productServices.GetAllProducts();

            
            foreach (Products product in products)
            {
                
                string productname = product.product_name;
                int id = product.product_id;
                string imagepath = product.image;

                
                List<ProductVariations> var = productvariationsServices.GetAllProductVariations(id);

               
                home_POSproduct productControl = new home_POSproduct(productname, var, imagepath, this);

                
                posPrdocutsPanel.Children.Add(productControl);
            }
        }

        //Search function
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //POS Buttons Click Functions (Animal type)
        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalAllBtn");
            borderAnimalList.Remove("animalAllBorder");

            ActiveButton("animalAllBtn", "animalAllBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }

        private void ChickenButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalChickenBtn");
            borderAnimalList.Remove("animalChickenBorder");

            ActiveButton("animalChickenBtn", "animalChickenBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalDogBtn");
            borderAnimalList.Remove("animalDogBorder");

            ActiveButton("animalDogBtn", "animalDogBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalCatBtn");
            borderAnimalList.Remove("animalCatBorder");

            ActiveButton("animalCatBtn", "animalCatBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void PigButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalPigBtn");
            borderAnimalList.Remove("animalPigBorder");

            ActiveButton("animalPigBtn", "animalPigBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void DuckButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalDuckBtn");
            borderAnimalList.Remove("animalDuckBorder");

            ActiveButton("animalDuckBtn", "animalDuckBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void CowButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalCowBtn");
            borderAnimalList.Remove("animalCowBorder");

            ActiveButton("animalCowBtn", "animalCowBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void HorseButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalHorseBtn");
            borderAnimalList.Remove("animalHorseBorder");

            ActiveButton("animalHorseBtn", "animalHorseBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void RabbitButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalRabbitBtn");
            borderAnimalList.Remove("animalRabbitBorder");

            ActiveButton("animalRabbitBtn", "animalRabbitBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void BirdButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalBirdBtn");
            borderAnimalList.Remove("animalBirdBorder");

            ActiveButton("animalBirdBtn", "animalBirdBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void FishButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalFishBtn");
            borderAnimalList.Remove("animalFishBorder");

            ActiveButton("animalFishBtn", "animalFishBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }
        private void GuineaButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonAnimalList = new List<String>(buttonAnimalArray);
            List<String> borderAnimalList = new List<String>(borderAnimalArray);

            buttonAnimalList.Remove("animalGuineaBtn");
            borderAnimalList.Remove("animalGuineaBorder");

            ActiveButton("animalGuineaBtn", "animalGuineaBorder");

            for (int i = 0; i < buttonAnimalList.Count; i++)
            {
                InactiveButton(buttonAnimalList[i], borderAnimalList[i]);
            }
        }


        //POS Buttons Click Functions (Product type)
        private void TypeAllButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeAllBtn");
            borderTypeList.Remove("typeAllBorder");

            ActiveButton("typeAllBtn", "typeAllBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }
        private void FeedsButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeFeedsBtn");
            borderTypeList.Remove("typeFeedsBorder");

            ActiveButton("typeFeedsBtn", "typeFeedsBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }
        private void MedicineButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeMedicineBtn");
            borderTypeList.Remove("typeMedicineBorder");

            ActiveButton("typeMedicineBtn", "typeMedicineBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }
        private void VitaminsButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeVitaminsBtn");
            borderTypeList.Remove("typeVitaminsBorder");

            ActiveButton("typeVitaminsBtn", "typeVitaminsBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }
        private void AccessoriesButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeAccessoriesBtn");
            borderTypeList.Remove("typeAccessoriesBorder");

            ActiveButton("typeAccessoriesBtn", "typeAccessoriesBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }
        private void VaccinesButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> buttonTypeList = new List<String>(buttonTypeArray);
            List<String> borderTypeList = new List<String>(borderTypeArray);

            buttonTypeList.Remove("typeVaccinesBtn");
            borderTypeList.Remove("typeVaccinesBorder");

            ActiveButton("typeVaccinesBtn", "typeVaccinesBorder");

            for (int i = 0; i < buttonTypeList.Count; i++)
            {
                InactiveButton(buttonTypeList[i], borderTypeList[i]);
            }
        }


        //Active and inactive button properties change
        private void ActiveButton(String stringbutton, String stringborder)
        {
            Button? button = FindName(stringbutton) as Button;
            Border? border = FindName(stringborder) as Border;

            if (button != null && border != null)
            {
                // Set the styles for the Border and Button
                border.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 194, 101));

                button.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                button.Foreground = new SolidColorBrush(Color.FromRgb(102, 194, 101));
            }
            else
            {
                MessageBox.Show("Button or Border not found.");
            }
        }

        private void InactiveButton(String stringbutton, String stringborder)
        {
            Button? button = FindName(stringbutton) as Button;
            Border? border = FindName(stringborder) as Border;


            if (button != null && border != null)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(243, 243, 243));
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                button.Foreground = new SolidColorBrush(Color.FromRgb(185, 185, 185));
            }
            else
            {
                MessageBox.Show("Button or Border not found.");
            }

            
        }

        
    }
}
