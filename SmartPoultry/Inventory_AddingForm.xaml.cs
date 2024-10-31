using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32; // Add this at the top if not already present
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace SmartPoultry
{
    public partial class Inventory_AddingForm : Window
    {
        
        public List<String> AnimalList = new List<String>();
        public List<String> ProductTypeList = new List<String>();


        public MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        public Inventory_AddingForm()
        {
            InitializeComponent();
            SetRoundedCorners();
            mainWindow.Opacity = 0.5;
            SetRoundedCorners();

            this.Closed += (s, e) => mainWindow.Opacity = 1.0;
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
        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;

        
        }
        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string message = "Here are your animal list:\n" + string.Join("\n", AnimalList) + "\nHere are your Product Type List:\n" + string.Join("\n", ProductTypeList);
            

            MessageBox.Show(message, "Items List", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ClosePopUp()
        {
            this.Close();
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog 
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                
                string selectedFilePath = openFileDialog.FileName;
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
            Button button = FindName(stringbutton) as Button;
            Border border = FindName(stringborder) as Border;

            if (button != null && border != null)
            {
                // Set the styles for the Border and Button
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
            Button button = FindName(stringbutton) as Button;
            Border border = FindName(stringborder) as Border;


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

        
    }
}
