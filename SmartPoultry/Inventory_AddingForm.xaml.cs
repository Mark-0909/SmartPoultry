using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32; 
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.Xml.Linq;
using SmartPoultry.DataServices;
using SmartPoultry.DataAccess;
using System.IO;
using SmartPoultry.Models;
using static SmartPoultry.App;

namespace SmartPoultry
{
    public partial class Inventory_AddingForm : Window
    {
        //variables
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;


        public static string baseUnitValue = "";
        public static string? stocksvar;
        public bool baseUnit = false;
        private string? selectedFilePath;
        public bool isEditing = false;

        //lists
        public List<String> AnimalList = new List<String>();
        public List<String> ProductTypeList = new List<String>();
        public List<String> unitlist = new List<String>();
        public List<String> pricelist = new List<String>();
        public List<String> conversionlist = new List<String>();

        //database
        public AppDbContext context;
        private readonly ProductServices productService;
        private readonly ProductVariationServices productVariationService;
        readonly SupplierServices supplierServices;
        readonly UserServices userServices;
        public Inventory_AddingForm()
        {
            InitializeComponent();
            SetRoundedCorners();
            editBtn.Visibility = Visibility.Collapsed;
            phaseoutBtn.Visibility = Visibility.Collapsed;
            AddingFormOverlay.Visibility = Visibility.Hidden;
            

            //set mainwindow in dim mode
            stockunit.Visibility = Visibility.Collapsed;
            stocklisting.Visibility = Visibility.Collapsed;


            context = new AppDbContext();
            productService = new ProductServices(context);
            productVariationService = new ProductVariationServices(context);
            supplierServices = new SupplierServices(context);
            userServices = new UserServices(context);

            
            PopulateSupplierList("add");
            SupplierCBox.SelectedItem = "-- Select a Supplier --";
            windowName.Content = "ADD PRODUCT";
        }

        public Inventory_AddingForm(Products product)
        {
            InitializeComponent();
            windowName.Content = "PRODUCT DETAILS";
            isEditing = true;
            AddingFormOverlay.Visibility = Visibility.Hidden;

            context = new AppDbContext();
            productService = new ProductServices(context);
            productVariationService = new ProductVariationServices(context);
            supplierServices = new SupplierServices(context);

            PopulateSupplierList("edit");

            SelectedImage.Height = SelectImageBtn.Height;
            SelectedImage.Width = SelectImageBtn.Width;
            BitmapImage bitmap = new BitmapImage(new Uri(product.image, UriKind.RelativeOrAbsolute));
            SelectedImage.Source = bitmap;
            stocklisting.Content = product.stocks.ToString();

            stockunit.Content = productVariationService.GetBaseUnit(product.product_id);

            
            ProductNameTextBox.Text = product.product_name;
            
            var supplier = supplierServices.FindSupplier(product.supplier_id);
            if (supplier != null)
            {
                SupplierCBox.SelectedItem = supplier.Name.ToString(); 
            }
            else
            {
                MessageBox.Show("Supplier not found for the given ID.");
            }
        }

        public void ActiveOverlay(bool isActive)
        {
            if (isActive == true)
            {
                AddingFormOverlay.Visibility = Visibility.Visible;
                Panel.SetZIndex(AddingFormOverlay, 99);
            }
            else
            {
                AddingFormOverlay.Visibility = Visibility.Hidden;
                Panel.SetZIndex(AddingFormOverlay, 0);
            }
        }
        public void PopulateSupplierList(string mode)
        {
            
            SupplierCBox.Items.Clear();

            if (mode != "edit")
            {
                SupplierCBox.Items.Add("-- Select a Supplier --");
            }
            List <SupplierList> suppliers = supplierServices.ListSuppliers();

            foreach (var supplier in suppliers)
            {
                SupplierCBox.Items.Add(supplier.Name); 
            }
        }



        public void UpdateBaseValueForAllInstances(string name, string price, string conversion, string stocks, int position)
        {

            unitsWPanel.Children.Clear();


            unitlist[position] = name;
            pricelist[position] = price;
            conversionlist[position] = conversion;

            string stockupdate = stocks;
            if (stocks == null)
            {
                stockupdate = stocklisting.Content.ToString();
            }
            
            stocklisting.Content = stockupdate;

            string type;

            for (int i = 0; i < unitlist.Count; i++) {
                if (i != 0)
                {
                    type = "sub";

                }
                else {
                    type = "base";
                }

                inventoryAdd_variationscontrol? control = new inventoryAdd_variationscontrol(unitlist[i], pricelist[i], conversionlist[i], type, stockupdate, unitlist[0], i, this)
                {
                    Height = 166,
                    Width = 60,
                    VerticalAlignment = VerticalAlignment.Center 
                };

                unitsWPanel.Children.Add(control);

            }

            unitsWPanel.Children.Add(addUnitBtn);
        }

        public void AddUnit(string name, string price, string conversion, string stocks, string type, int position)
        {
            

            if (baseUnit == false)
            {
                baseUnit = true;
                baseUnitValue = name;
            }
          

            inventoryAdd_variationscontrol? control = new inventoryAdd_variationscontrol(name, price, conversion, type, stocks, baseUnitValue, position, this)
            {
                Height = 166,
                Width = 60,
                VerticalAlignment = VerticalAlignment.Center
            };

            unitsWPanel.Children.Remove(addUnitBtn);
            unitsWPanel.Children.Add(control);
            unitsWPanel.Children.Add(addUnitBtn);

            unitlist.Add(name); 
            pricelist.Add(price);
            conversionlist.Add(conversion);

            if (stocks != null)
            {
                stocklisting.Visibility = Visibility.Visible;
                stocklisting.Content = stocks;
                stockunit.Visibility = Visibility.Visible;
                stockunit.Content = unitlist[0];
            }
            else { 
            
            
            }
        }

        public void AddUnitPopup_Click(object sender, RoutedEventArgs s)
        {
            int position = unitlist.Count;
            if (!baseUnit)
            {
                Inventory_Unitadder? popup = new Inventory_Unitadder("base_unit", baseUnitValue, "add", position, this);
                ActiveOverlay(true);
                popup.ShowDialog();
            }
            else {
                Inventory_Unitadder? popup = new Inventory_Unitadder("sub_unit", baseUnitValue, "add", position, this);
                ActiveOverlay(true);
                popup.ShowDialog();
            }
            
        }
        private void ProductName_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (ProductNameTextBox.Text == "Enter text here...")
            {
                ProductNameTextBox.Text = "";
                ProductNameTextBox.Foreground = Brushes.Black;
            }
        }
        private void ProductName_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
            {
                ProductNameTextBox.Text = "Enter text here...";
                ProductNameTextBox.Foreground = Brushes.Gray;
            }
        }
        
        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
            mainWindow.ActiveOverlay(false);
        }

        //database - save product
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                string animaltypelist = string.Join(",", AnimalList);
                string producttypelist = string.Join(",", ProductTypeList);
                decimal stocks;

                if (!decimal.TryParse(stocklisting.Content.ToString(), out stocks))
                {
                    MessageBox.Show("Invalid stock value. Please enter a numeric value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int supplierid = supplierServices.FindSupplierByName(SupplierCBox.Text);
                int employeeId = UserContext.CurrentUserId;





                int id = productService.Create(ProductNameTextBox.Text, animaltypelist, producttypelist, employeeId, supplierid, stocks, "");

                if (id == 0)
                {
                    MessageBox.Show("Failed to save product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    AddVariations(id);


                    if (!string.IsNullOrEmpty(selectedFilePath))
                    {

                        string destinationDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product_Images");

                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }


                        string destinationPath = System.IO.Path.Combine(destinationDirectory, $"{id}.jpg");

                        try
                        {

                            File.Copy(selectedFilePath, destinationPath, overwrite: true);


                            productService.UpdateImagePath(id, destinationPath);

                            mainWindow.DynamicReload();
                            mainWindow.ActiveOverlay(false);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to copy image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else 
            {
                MessageBox.Show("Submit for edit.");
            }
            
        }


        public void AddVariations(int id) 
        {
            try
            {
                for (int i = 0; i < unitlist.Count; i++)
                {
                    decimal price = Convert.ToDecimal(pricelist[i]);
                    int conversion = Convert.ToInt32(conversionlist[i]);
                    if (i == 0)
                    {
                        productVariationService.Create(id, unitlist[i], true, price, conversion);
                    }
                    else
                    {
                        productVariationService.Create(id, unitlist[i], false, price, conversion);
                    }

                }
                
                
                this.Close();
            }
            catch (Exception ex) {
                MessageBox.Show($"Error creating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }



        public void ClosePopUp()
        {
            this.Close();
        }

        public void SaveProductDB()
        {
            

        }


        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog? openFileDialog = new OpenFileDialog 
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(selectedFilePath));
                SelectedImage.Source = bitmap;
                SelectedImage.Height = SelectImageBtn.Height;
                SelectedImage.Width = SelectImageBtn.Width;
            }
        }

        private void ChickenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("chicken"))
            {
                ActiveButton("animalChickenBtn", "animalChickenBorder", "animal", "chicken");
            }
            else
            {
                InactiveButton("animalChickenBtn", "animalChickenBorder", "animal", "chicken");
            }
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("dog"))
            {
                ActiveButton("animalDogBtn", "animalDogBorder", "animal", "dog");
            }
            else
            {
                InactiveButton("animalDogBtn", "animalDogBorder", "animal", "dog");
            }
        }
        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("cat"))
            {
                ActiveButton("animalCatBtn", "animalCatBorder", "animal", "cat");
            }
            else
            {
                InactiveButton("animalCatBtn", "animalCatBorder", "animal", "cat");
            }
        }
        private void PigButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("pig"))
            {
                ActiveButton("animalPigBtn", "animalPigBorder", "animal", "pig");
            }
            else
            {
                InactiveButton("animalPigBtn", "animalPigBorder", "animal", "pig");
            }
        }
        private void GuineaButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("guinea pig"))
            {
                ActiveButton("animalGuineaBtn", "animalGuineaBorder", "animal", "guinea pig");
            }
            else
            {
                InactiveButton("animalGuineaBtn", "animalGuineaBorder", "animal", "guinea pig");
            }
        }
        private void DuckButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("duck"))
            {
                ActiveButton("animalDuckBtn", "animalDuckBorder", "animal", "duck");
            }
            else
            {
                InactiveButton("animalDuckBtn", "animalDuckBorder", "animal", "duck");
            }
        }
        private void CowButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("cow"))
            {
                ActiveButton("animalCowBtn", "animalCowBorder", "animal", "cow");
            }
            else
            {
                InactiveButton("animalCowBtn", "animalCowBorder", "animal", "cow");
            }
        }
        private void HorseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("horse"))
            {
                ActiveButton("animalHorseBtn", "animalHorseBorder", "animal", "horse");
            }
            else
            {
                InactiveButton("animalHorseBtn", "animalHorseBorder", "animal", "horse");
            }
        }
        private void RabbitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("rabbit"))
            {
                ActiveButton("animalRabbitBtn", "animalRabbitBorder", "animal", "rabbit");
            }
            else
            {
                InactiveButton("animalRabbitBtn", "animalRabbitBorder", "animal", "rabbit");
            }
        }
        private void BirdButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("bird"))
            {
                ActiveButton("animalBirdBtn", "animalBirdBorder", "animal", "bird");
            }
            else
            {
                InactiveButton("animalBirdBtn", "animalBirdBorder", "animal", "bird");
            }
        }
        private void FishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AnimalList.Contains("fish"))
            {
                ActiveButton("animalFishBtn", "animalFishBorder", "animal", "fish");
            }
            else
            {
                InactiveButton("animalFishBtn", "animalFishBorder", "animal", "fish");
            }
        }

        private void AccessoriesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProductTypeList.Contains("accessories"))
            {
                ActiveButton("typeAccessoriesBtn", "typeAccessoriesBorder", "product-type", "accessories");
            }
            else
            {
                InactiveButton("typeAccessoriesBtn", "typeAccessoriesBorder", "product-type", "accessories");
            }
        }

        private void FeedsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProductTypeList.Contains("feeds"))
            {
                ActiveButton("typeFeedsBtn", "typeFeedsBorder", "product-type", "feeds");

            }
            else
            {
                InactiveButton("typeFeedsBtn", "typeFeedsBorder", "product-type", "feeds");
            }
        }
        private void VitaminsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProductTypeList.Contains("vitamins"))
            {
                ActiveButton("typeVitaminsBtn", "typeVitaminsBorder", "product-type", "vitamins");
            }
            else
            {
                InactiveButton("typeVitaminsBtn", "typeVitaminsBorder", "product-type", "vitamins");
            }
        }
        private void VaccinesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProductTypeList.Contains("vaccines"))
            {
                ActiveButton("typeVaccinesBtn", "typeVaccinesBorder", "product-type", "vaccines");
            }
            else
            {
                InactiveButton("typeVaccinesBtn", "typeVaccinesBorder", "product-type", "vaccines");
            }
        }
        private void MedicineButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ProductTypeList.Contains("medicine"))
            {
                ActiveButton("typeMedicineBtn", "typeMedicineBorder", "product-type", "medicine");
            }
            else
            {
                InactiveButton("typeMedicineBtn", "typeMedicineBorder", "product-type", "medicine");
            }
        }


        private void ActiveButton(String stringbutton, String stringborder, String category, String toSave)
        {
            Button? button = FindName(stringbutton) as Button;
            Border? border = FindName(stringborder) as Border;

            if (button != null && border != null)
            {
                
                border.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 194, 101));
                button.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(192, 228, 190));
                button.Foreground = new SolidColorBrush(Color.FromRgb(102, 194, 101));
                if (category == "animal")
                {
                    AnimalList.Add(toSave);
                }
                else
                {
                    ProductTypeList.Add(toSave);
                }
            }
            else
            {
                MessageBox.Show("Button or Border not found.");
            }
        }

        private void InactiveButton(String stringbutton, String stringborder, String category, String toSave)
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
                if (category == "animal")
                {
                    AnimalList.Remove(toSave);
                }
                else {
                    ProductTypeList.Remove(toSave);
                }
            }
            else
            {
                MessageBox.Show("Button or Border not found.");
            }


        }
        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
