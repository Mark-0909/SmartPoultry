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
using iTextSharp.xmp.impl;
using Org.BouncyCastle.Math;
using static iTextSharp.text.pdf.XfaForm;
using System.Text;

namespace SmartPoultry
{
    public partial class Inventory_AddingForm : Window
    {
        //variables
        MainWindow mainWindow;
        Inventory_ProductControl inventoryControl;

        public static string baseUnitValue = "";
        public static string? stocksvar;
        public bool baseUnit = false;
        private string? selectedFilePath;
        public bool isEditing = false;
        public ImageSource imagePath;

        //lists
        public List<String> AnimalList = new List<String>();
        public List<String> ProductTypeList = new List<String>();
        public List<String> unitlist = new List<String>();
        public List<String> pricelist = new List<String>();
        public List<String> conversionlist = new List<String>();

        public List<int> variationIDlist = new List<int>();

        //database
        public AppDbContext context;
        public readonly ProductServices productService;
        public ProductVariationServices productVariationService;
        public SupplierServices supplierServices;
        public InventoryLogsServices InventoryLogsServices;

        string Agenda = "Add";

        Products prod;
        public Inventory_AddingForm(MainWindow mainwindow)
        {
            InitializeComponent();
            SetRoundedCorners();
            editBtn.Visibility = Visibility.Collapsed;
            phaseoutBtn.Visibility = Visibility.Collapsed;
            AddingFormOverlay.Visibility = Visibility.Hidden;
            


            stockunit.Visibility = Visibility.Collapsed;
            stocklisting.Visibility = Visibility.Collapsed;


            context = new AppDbContext();
            productService = new ProductServices(context);
            productVariationService = new ProductVariationServices(context);
            supplierServices = new SupplierServices(context);
            InventoryLogsServices = new InventoryLogsServices(context);

            
            PopulateSupplierList("add");
            SupplierCBox.SelectedItem = "-- Select a Supplier --";
            windowName.Content = "ADD PRODUCT";

            mainWindow = mainwindow;
        }

        public Inventory_AddingForm(Products product, MainWindow window, Inventory_ProductControl productcontrol)
        {
            InitializeComponent();
            mainWindow = window;
            inventoryControl = productcontrol;

            prod = product;
            windowName.Content = "PRODUCT DETAILS";
            isEditing = true;
            AddingFormOverlay.Visibility = Visibility.Hidden;
            imagePath = SelectedImage.Source;

            context = new AppDbContext();
            productService = new ProductServices(context);
            productVariationService = new ProductVariationServices(context);
            supplierServices = new SupplierServices(context);

            PopulateSupplierList("edit");

            SelectedImage.Height = SelectImageBtn.Height;
            SelectedImage.Width = SelectImageBtn.Width;

            DisplayProductImage(product.image);

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

            AnimalList = product.animal_type.Split(',').Select(animal => animal.Trim()).ToList();
            ProductTypeList = product.product_type.Split(',').Select(type => type.Trim()).ToList();

            List<ProductVariations> productvarlist = productVariationService.GetAllProductVariations(product.product_id);

            var baseUnit = productvarlist.FirstOrDefault(pv => pv.isBaseUnit == true);
            if (baseUnit != null)
            {
                unitlist.Add(baseUnit.variant_type.ToString());
                pricelist.Add(baseUnit.price.ToString());
                conversionlist.Add(baseUnit.conversion_rate.ToString());
                variationIDlist.Add(baseUnit.id);
            }

            for (int i = 0; i < productvarlist.Count; i++)
            {
                if (productvarlist[i] != baseUnit)
                {
                    unitlist.Add(productvarlist[i].variant_type.ToString());
                    pricelist.Add(productvarlist[i].price.ToString());
                    conversionlist.Add(productvarlist[i].conversion_rate.ToString());
                    variationIDlist.Add(productvarlist[i].id);
                }
            }

            DisplayVariation();
            AdjustAnimalButtons();
            AdjustTypeButtons();
            DisableForm(false);
        }

        private void DisplayProductImage(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                try
                {
                    using (var memoryStream = new System.IO.MemoryStream(imageData))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = memoryStream;
                        bitmap.EndInit();
                        SelectedImage.Source = bitmap;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Image Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    SelectedImage.Source = null;
                }
            }
            else
            {
                SelectedImage.Source = null;
            }
        }


        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Agenda == "Add")
            {
                Agenda = "Edit";
                DisableForm(true);
                SubmitBtn.Content = "Update";
            }
            else
            {
                Agenda = "Add";
                DisableForm(false);
                SubmitBtn.Content = "Submit";
            }
        }

        private void PhaseOutBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        public void DisableForm(bool isEnabled)
        {
            SelectImageBtn.IsEnabled = isEnabled;
            ProductNameTextBox.IsEnabled = isEnabled;
            SupplierCBox.IsEnabled = isEnabled;
            animalChickenBorder.IsEnabled = isEnabled;
            animalDogBorder.IsEnabled = isEnabled;
            animalCatBorder.IsEnabled = isEnabled;
            animalPigBorder.IsEnabled = isEnabled;
            animalGuineaBorder.IsEnabled = isEnabled;
            animalDuckBorder.IsEnabled = isEnabled;
            animalCowBorder.IsEnabled = isEnabled;
            animalHorseBorder.IsEnabled = isEnabled;
            animalRabbitBorder.IsEnabled = isEnabled;
            animalBirdBorder.IsEnabled = isEnabled;
            animalFishBorder.IsEnabled = isEnabled;
            SubmitBtn.IsEnabled = isEnabled;

            typeFeedsBorder.IsEnabled = isEnabled;
            typeAccessoriesBorder.IsEnabled = isEnabled;
            typeMedicineBorder.IsEnabled = isEnabled;
            typeVaccinesBorder.IsEnabled = isEnabled;
            typeVitaminsBorder.IsEnabled = isEnabled;
            unitsWPanel.IsEnabled = isEnabled;
        }
        public void AdjustAnimalButtons()
        {
            
            for (int i = 0; i < AnimalList.Count; i++)
            {
                if (AnimalList[i] == "guinea pig") 
                {
                    ActiveButton("animalGuineaBtn", "animalGuineaBorder", "animal", AnimalList[i]);
                }
                else
                {
                    string animalButton = $"animal{char.ToUpper(AnimalList[i][0])}{AnimalList[i].Substring(1)}Btn";
                    string animalBorder = $"animal{char.ToUpper(AnimalList[i][0])}{AnimalList[i].Substring(1)}Border";

                    ActiveButton(animalButton, animalBorder, "animal", AnimalList[i]);
                }
            }
        }
        public void AdjustTypeButtons()
        {
            for (int i = 0; i < ProductTypeList.Count; i++)
            {
                string typeButton = $"type{char.ToUpper(ProductTypeList[i][0])}{ProductTypeList[i].Substring(1)}Btn";
                string typeBorder = $"type{char.ToUpper(ProductTypeList[i][0])}{ProductTypeList[i].Substring(1)}Border";

                ActiveButton(typeButton, typeBorder, "product-type", ProductTypeList[i]);
            }
        }



        public void DisplayVariation()
        {
            unitsWPanel.Children.Clear();
            string type;
            for (int i = 0; i < unitlist.Count; i++)
            {
                if (i != 0)
                {
                    type = "sub";

                }
                else
                {
                    type = "base";
                }

                inventoryAdd_variationscontrol? control = new inventoryAdd_variationscontrol(unitlist[i], pricelist[i], conversionlist[i], type, stocklisting.Content.ToString(), unitlist[0], i, this)
                {
                    Height = 166,
                    Width = 60,
                    VerticalAlignment = VerticalAlignment.Center
                };

                unitsWPanel.Children.Add(control);

            }

            unitsWPanel.Children.Add(addUnitBtn);
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

        public void ClearVariation(int position)
        {
            unitlist.RemoveAt(position);
            pricelist.RemoveAt(position);
            conversionlist.RemoveAt(position);

            unitsWPanel.Children.Clear();

            if(position == 0)
            {
                unitlist.Clear();
                pricelist.Clear(); 
                conversionlist.Clear();
                stocklisting.Visibility = Visibility.Hidden;
                stockunit.Visibility = Visibility.Hidden;
                unitsWPanel.Children.Add(addUnitBtn);
                return;
            }

            string type;

            string stockupdate = stocklisting.Content.ToString();

            for (int i = 0; i < unitlist.Count; i++)
            {
                if (i != 0)
                {
                    type = "sub";

                }
                else
                {
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

        public void UpdateBaseValueForAllInstances(string name, string price, string conversion, string stocks, int position)
        {
            unitsWPanel.Children.Clear();

            unitlist[position] = name;
            pricelist[position] = price;
            conversionlist[position] = conversion;

            string stockupdate = stocks ?? stocklisting.Content.ToString();
            stocklisting.Content = stockupdate;

            string type;
            for (int i = 0; i < unitlist.Count; i++)
            {
                type = i == 0 ? "base" : "sub";

                inventoryAdd_variationscontrol? control = new inventoryAdd_variationscontrol(
                    unitlist[i], pricelist[i], conversionlist[i], type, stockupdate, unitlist[0], i, this)
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

            
            stocklisting.Visibility = Visibility.Visible;
            stockunit.Visibility = Visibility.Visible;
            stockunit.Content = unitlist[0];
            if (stocks == null) 
            { 
                return; 
            }
            stocklisting.Content = stocks;
        }

        public void AddUnitPopup_Click(object sender, RoutedEventArgs s)
        {
            int position = unitlist.Count;
            if (unitsWPanel.Children.Count == 1)
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
            HandleTextBoxPlaceholder(ProductNameTextBox, "Enter text here...", true);
        }
        private void ProductName_LostFocus(object sender, RoutedEventArgs e)
        {

            HandleTextBoxPlaceholder(ProductNameTextBox, "Enter text here...", false);
        }
        
        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
            mainWindow.ActiveOverlay(false);
        }

        //database - save product
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitBtn.Content.ToString() == "Submit")
            {
                SubmitAddProduct();
            }else if(SubmitBtn.Content.ToString() == "Update")
            {
                EditProduct();
            }
            
        }
        public void EditProduct()
        {
            try
            {
                string name = ProductNameTextBox.Text;
                int supplierid = supplierServices.FindSupplierByName(SupplierCBox.Text);
                string animallist = string.Join(",", AnimalList);
                string typelist = string.Join(",", ProductTypeList);
                decimal stocks = decimal.Parse(stocklisting.Content.ToString());

                for (int i = 0; i < variationIDlist.Count; i++)
                {
                    decimal price = decimal.Parse(pricelist[i].ToString());
                    int conversion = int.Parse(conversionlist[i].ToString());
                    bool isUpdated = productVariationService.EditUnitVar(variationIDlist[i], unitlist[i], price, conversion);

                    if (!isUpdated)
                    {
                        MessageBox.Show("Failed to update product variation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                bool isProductUpdated = productService.EditProduct(prod.product_id, name, animallist, typelist, supplierid, stocks, selectedFilePath);
                if (!isProductUpdated)
                {
                    MessageBox.Show("Failed to update product details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisableForm(false);
                inventoryControl.Productname.Content = ProductNameTextBox.Text;
                inventoryControl.Productstock.Content = stocklisting.Content.ToString();
                inventoryControl.Productimage.Source = SelectedImage.Source;

                home_POSproduct posprod = GetPOSControlById(prod.product_id);
                if (posprod != null) 
                {
                    posprod.Productimage.Source = SelectedImage.Source;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public home_POSproduct GetPOSControlById(int id)
        {
            foreach (UIElement element in mainWindow.homeControl.posPrdocutsPanel.Children)
            {
                if (element is home_POSproduct control && control.productId == id)
                {
                    return control;
                }
            }
            return null; 
        }
        public void SubmitAddProduct()
        {
            if (imagePath == SelectedImage.Source || ProductNameTextBox.Text == "Enter text here..." || unitsWPanel.Children.Count == 1 || AnimalList.Count == 0 || ProductTypeList.Count == 0 || SupplierCBox.Text == "-- Select a Supplier --")
            {
                MessageBox.Show("Incomplete Details.");
                return;
            }

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

                string imagePath = new Uri(SelectedImage.Source.ToString()).LocalPath;

                int id = productService.Create(ProductNameTextBox.Text, animaltypelist, producttypelist, employeeId, supplierid, stocks, imagePath);

                if (id == 0)
                {
                    MessageBox.Show("Failed to save product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    AddVariations(id);

                    bool isRecorded = InventoryLogsServices.Create(id, employeeId, "ADD", "Added new product.");

                    if (!isRecorded)
                    {
                        MessageBox.Show("Not Recorded");
                        return;
                    }

                    mainWindow.DynamicReload();
                    mainWindow.ActiveOverlay(false);
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



        public void HandleTextBoxPlaceholder(TextBox tb, string placeholder, bool isFocused)
        {
            if (isFocused)
            {
                if (tb.Text == placeholder)
                {
                    tb.Text = string.Empty;
                    tb.Foreground = Brushes.Black;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        public void ClosePopUp()
        {
            this.Close();
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
                if (category == "animal" && !AnimalList.Contains(toSave))
                {
                    AnimalList.Add(toSave);
                }
                else if (category != "animal" && !ProductTypeList.Contains(toSave))
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

                if (category == "animal" && AnimalList.Contains(toSave))
                {
                    AnimalList.Remove(toSave);
                }
                else if (category != "animal" && ProductTypeList.Contains(toSave))
                {
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

        

        
    }
}
